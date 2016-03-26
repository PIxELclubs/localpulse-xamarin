using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
			List<IssueDetail> localItems = await Task.Run(() => JsonConvert.DeserializeObject<List<IssueDetail>>(output));
			foreach (IssueDetail issue in localItems) {
				Issues.Add(issue);
			}
		}

		public static async Task<IssueDetail> GetIssueDetailAsync(string objectId)
		{
			HttpResponseMessage response = await client.GetAsync(new Uri("https://localpulse.org/api/1.0/get/" + objectId));
			string output = await response.Content.ReadAsStringAsync();
			return await Task.Run(() => JsonConvert.DeserializeObject<IssueDetail>(output));
		}
	}
}
