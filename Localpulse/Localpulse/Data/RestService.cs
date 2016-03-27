using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Windows.Web.Http;

namespace Localpulse
{
    public class RestService
    {
		private static HttpClient client = new HttpClient();

		public static ObservableCollection<IssueDetail> Issues { get; private set; } = new ObservableCollection<IssueDetail>();

		public static async Task RefreshIssuesAsync()
		{
			HttpResponseMessage response = await client.GetAsync(new Uri("https://localpulse.org/api/1.2/getAllJSON"));
			string output = await response.Content.ReadAsStringAsync();
			List<IssueDetail> localItems = await Task.Run(() => JsonConvert.DeserializeObject<List<IssueDetail>>(output, new IsoDateTimeConverter()));
			foreach (IssueDetail issue in localItems) {
				Issues.Add(issue);
				if (DbService.Issues.ContainsKey(issue.ObjectId)) {
					DbService.Issues[issue.ObjectId].Update(issue);
				} else {
					DbService.Issues[issue.ObjectId] = issue;
				}
			}
		}

		public static async Task<ObservableCollection<IssueComment>> GetIssueCommentsAsync(string objectId)
		{
			HttpResponseMessage response = await client.GetAsync(new Uri("https://localpulse.org/api/1.2/getComments/" + objectId));
			string output = await response.Content.ReadAsStringAsync();
			List<IssueComment> parsed = await Task.Run(() => JsonConvert.DeserializeObject<List<IssueComment>>(output, new IsoDateTimeConverter()));
			return DbService.IssueComments[objectId] = new ObservableCollection<IssueComment>(parsed);
		}
	}
}
