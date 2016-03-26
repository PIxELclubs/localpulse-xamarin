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
				Icon = "Toolkit.Content\\ApplicationBar.Add.png",
				Text = "Post",
			});

			ListView lstView = new ListView();
			lstView.RowHeight = 120;

			lstView.ItemsSource = RestService.Issues;
			lstView.ItemTemplate = new DataTemplate(typeof(IssueCell));
			lstView.ItemSelected += OnSelection;
			Content = lstView;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			RefreshIssuesAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		void OnSelection(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}
			Navigation.PushAsync(new IssueDetailPage(0));
			//comment out if you want to keep selections
			ListView lst = (ListView)sender;
			lst.SelectedItem = null;
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
