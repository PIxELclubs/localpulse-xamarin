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

		public static ObservableCollection<IssueViewModel> Issues { get; private set; } = new ObservableCollection<IssueViewModel>();

		public static async Task RefreshIssuesAsync()
		{
			HttpResponseMessage response = await client.GetAsync(new Uri("https://localpulse.org/api/1.0/getAllJSON"));
			string output = await response.Content.ReadAsStringAsync();
			List<IssueViewModel> localItems = await Task.Run(() => JsonConvert.DeserializeObject<List<IssueViewModel>>(output));
			foreach (IssueViewModel issue in localItems) {
				Issues.Add(issue);
			}
		}

		public static async Task GetIssueDetail(string objectId)
		{
			HttpResponseMessage response = await client.GetAsync(new Uri("https://localpulse.org/api/1.0/get/" + objectId));
			string output = await response.Content.ReadAsStringAsync();

		}
	}
}
