using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Localpulse
{
    public class RestService
    {
		public static ObservableCollection<IssueDetail> Issues { get; private set; } = new ObservableCollection<IssueDetail>();

		static string CacheBuster()
		{
			return "c=" + DateTime.Now.Ticks;
		}

		static async Task<string> GetFromUri(Uri uri)
		{
			var request = WebRequest.Create(uri);
			var response = await request.GetResponseAsync();
			var stream = response.GetResponseStream();
			var streamReader = new StreamReader(stream);
			return await streamReader.ReadToEndAsync();
		}

		static async Task<T> ParseJson<T>(string input)
		{
			return await Task.Run(() => JsonConvert.DeserializeObject<T>(input, new IsoDateTimeConverter()));
		}

		public static async Task RefreshIssuesAsync()
		{
			var fetched = await GetFromUri(new Uri("https://localpulse.org/api/1.2/getAllJSON?" + CacheBuster()));
			var parsed = await ParseJson<List<IssueDetail>>(fetched);
			foreach (var issue in parsed) {
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
			var fetched = await GetFromUri(new Uri("https://localpulse.org/api/1.2/getComments/" + objectId + "?" + CacheBuster()));
			var parsed = await ParseJson<List<IssueComment>>(fetched);
			return DbService.IssueComments[objectId] = new ObservableCollection<IssueComment>(parsed);
		}
	}
}
