﻿using System;
using Xamarin.Forms;

namespace Localpulse
{
	public class IssuesListPage : ContentPage
	{
		public IssuesListPage ()
		{
			Title = "Localpulse";

			BackgroundColor = new Color(0.0, 166.0 / 255.0, 233.0 / 255.0);

			ToolbarItems.Add(new ToolbarItem {
				Icon = "Toolkit.Content\\ApplicationBar.Refresh.png",
				Text = "Refresh",
			});
			var postButton = new ToolbarItem {
				Icon = "Toolkit.Content\\ApplicationBar.Add.png",
				Text = "Post",
			};
			postButton.Clicked += CreateNewIssue;
			ToolbarItems.Add(postButton);

			var lstView = new ListView();
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
			var lst = (ListView)sender;
			lst.SelectedItem = null;
			// Navigate
			var issue = (IssueDetail)e.SelectedItem;
			Navigation.PushAsync(new IssueDetailPage(issue.ObjectId));
		}

		void CreateNewIssue(object sender, EventArgs e)
		{
			Navigation.PushAsync(new CreateNewIssuePage());
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
