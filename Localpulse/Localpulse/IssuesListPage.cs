using System;
using Xamarin.Forms;

namespace Localpulse
{
	public class IssuesListPage : ContentPage
	{
		public IssuesListPage ()
		{
			Title = "Localpulse";

			ToolbarItems.Add(new ToolbarItem {
				Icon = "Toolkit.Content\\ApplicationBar.Refresh.png",
				Text = "Refresh",
			});
			ToolbarItems.Add(new ToolbarItem {
				Icon = "Toolkit.Content\\ApplicationBar.Add.png",
				Text = "Post",
			});

			ListView lstView = new ListView();
			lstView.RowHeight = 120;

			lstView.ItemsSource = RestService.Issues;
			lstView.ItemTemplate = new DataTemplate(typeof(IssueCell));
			lstView.ItemSelected += OnSelected;
			Content = lstView;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			RefreshIssuesAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}
			// Prevent selection
			ListView lst = (ListView)sender;
			lst.SelectedItem = null;
			// Navigate
			IssueDetail issue = (IssueDetail)e.SelectedItem;
			Navigation.PushAsync(new IssueDetailPage(issue.ObjectId));
		}

		async void RefreshIssuesAsync()
		{
			try {
				await RestService.RefreshIssuesAsync();
			} catch (Exception e) {
				await DisplayAlert("Error", "Failed to fetch issues.\n" + e.Message, "Try again");
				RefreshIssuesAsync();
			}
		}
	}
}
