using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Localpulse
{
	public partial class IssueDetailPage : ContentPage
	{
		public IssueDetail Issue { get; set; }

		public IssueDetailPage(int id)
		{
			InitializeComponent();
			Issue = RestService.Issues[id];
			layout.BindingContext = Issue;
			Issue.Description = "abc";
			Issue.Votes = 2;
		}

		async void GetDetailAsync(string objectId)
		{
			try {
				Issue = await RestService.GetIssueDetailAsync(objectId);
			} catch (Exception e) {
				await DisplayAlert("Error", "Failed to fetch issue details.\n" + e.Message, "OK");
				await Navigation.PopAsync();
			}
		}
	}
}
