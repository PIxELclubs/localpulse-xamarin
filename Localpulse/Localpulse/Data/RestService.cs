using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XLabs.Platform.Services.Geolocation;
using System.Windows.Media.Imaging;

namespace Localpulse
{
    public class RestService
    {
		public static ObservableCollection<IssueDetail> Issues { get; private set; } = new ObservableCollection<IssueDetail>();

		static string CacheBuster()
		{
			return "c=" + DateTime.Now.Ticks;
		}

		static async Task<string> GetFromRequestAsync(WebRequest request)
		{
			var response = await request.GetResponseAsync();
			var stream = response.GetResponseStream();
			var streamReader = new StreamReader(stream);
			var output = await streamReader.ReadToEndAsync();
			streamReader.Close();
			response.Close();
			return output;
		}

		static async Task<string> GetFromUriAsync(Uri uri)
		{
			var request = WebRequest.Create(uri);
			return await GetFromRequestAsync(request);
		}

		static async Task<T> ParseJsonAsync<T>(string input)
		{
			return await Task.Run(() => JsonConvert.DeserializeObject<T>(input, new IsoDateTimeConverter()));
		}

		public static async Task RefreshIssuesAsync()
		{
			var fetched = await GetFromUriAsync(new Uri("https://localpulse.org/api/1.2/getAllJSON?" + CacheBuster()));
			var parsed = await ParseJsonAsync<List<IssueDetail>>(fetched);
			foreach (var issue in parsed) {
				Issues.Add(issue);
				if (DbService.Issues.ContainsKey(issue.ObjectId)) {
					DbService.Issues[issue.ObjectId].Update(issue);
				} else {
					DbService.Issues[issue.ObjectId] = issue;
				}
			}
		}

		public static async Task GetIssueCommentsAsync(string objectId)
		{
			var fetched = await GetFromUriAsync(new Uri("https://localpulse.org/api/1.2/getComments/" + objectId + "?" + CacheBuster()));
			var parsed = await ParseJsonAsync<List<IssueComment>>(fetched);
			var output = new ObservableCollection<IssueComment>(parsed);
			if (!DbService.IssueComments.ContainsKey(objectId)) {
				DbService.IssueComments[objectId] = new ObservableCollection<IssueComment>();
			}
			DbService.MergeCollection<IssueComment>(DbService.IssueComments[objectId], output);
		}

		public static async Task PostIssueAsync(Stream picture, string description, Position location)
		{
			var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			var boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
			
			var request = (HttpWebRequest)WebRequest.Create(new Uri("https://localpulse.org/api/1.0/upload"));
			request.Method = "POST";
			request.ContentType = "multipart/form-data; boundary=" + boundary;
			request.Credentials = CredentialCache.DefaultCredentials;

			var reqStream = await request.GetRequestStreamAsync();

			var formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

			reqStream.Write(boundarybytes, 0, boundarybytes.Length);
			var formattedDescription = string.Format(formdataTemplate, "description", description);
			var formattedDescriptionBytes = Encoding.UTF8.GetBytes(formattedDescription);
			reqStream.Write(formattedDescriptionBytes, 0, formattedDescriptionBytes.Length);

			if (location != null) {
				reqStream.Write(boundarybytes, 0, boundarybytes.Length);
				var formattedLatitude = string.Format(formdataTemplate, "latitude", location.Latitude.ToString());
				var formattedLatitudeBytes = Encoding.UTF8.GetBytes(formattedLatitude);
				reqStream.Write(formattedLatitudeBytes, 0, formattedLatitudeBytes.Length);
				reqStream.Write(boundarybytes, 0, boundarybytes.Length);
				var formattedLongitude = string.Format(formdataTemplate, "longitude", location.Longitude.ToString());
				var formattedLongitudeBytes = Encoding.UTF8.GetBytes(formattedLongitude);
				reqStream.Write(formattedLongitudeBytes, 0, formattedLongitudeBytes.Length);
			}

			if (picture != null) {
				reqStream.Write(boundarybytes, 0, boundarybytes.Length);

				var headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
				var header = string.Format(headerTemplate, "picture", DateTime.Now.Ticks.ToString("x") + ".jpg", "image/jpeg");
				var headerBytes = Encoding.UTF8.GetBytes(header);
				reqStream.Write(headerBytes, 0, headerBytes.Length);

				await picture.CopyToAsync(reqStream);
			}

			var trailerBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
			reqStream.Write(trailerBytes, 0, trailerBytes.Length);
			reqStream.Close();

			var fetched = await GetFromRequestAsync(request);
			var parsed = await ParseJsonAsync<RestResponse>(fetched);
			if (parsed.Response != 200) {
				throw new WebException("Server returned " + parsed.Response + ": " + parsed.Text);
			}
		}
	}
}
