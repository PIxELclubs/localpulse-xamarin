using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Xamarin.Forms;

namespace Localpulse
{
	public partial class IssueDetailPage : ContentPage
	{
		IssueDetail issue;
		ObservableCollection<IssueComment> comments;

		public IssueDetailPage(string objectId)
		{
			InitializeComponent();
			issue = DbService.Issues[objectId];
			IssueLayout.BindingContext = issue;
			LoadingLabel.IsVisible = false;

			// Init and point to db
			bool exists = DbService.IssueComments.TryGetValue(objectId, out comments);
			if (!exists) {
				comments = DbService.IssueComments[objectId] = new ObservableCollection<IssueComment>();
			}
			// Add db change handler
			comments.CollectionChanged += CommentsChanged;
			// Add existing db entries
			AddComments(comments);
			// Try to fetch new entries
			GetCommentsAsync();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			comments.CollectionChanged -= CommentsChanged;
		}

		void AddComments(IList incoming)
		{
			foreach (var comment in incoming) {
				CommentsView.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
				var tmpl = new Views.CommentView();
				tmpl.BindingContext = (IssueComment)comment;
				CommentsView.Children.Add(tmpl, 0, CommentsView.Children.Count);
			}
		}

		void CommentsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
				case NotifyCollectionChangedAction.Add:
					AddComments(e.NewItems);
					break;
				case NotifyCollectionChangedAction.Remove:
					DisplayAlert("Error", "Unimplemented comment removal. Please contact support.", "OK");
					break;
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Reset:
				default:
					break;
			}
		}

		async void GetCommentsAsync()
		{
			try {
				LoadingLabel.IsVisible = true;
				await RestService.GetIssueCommentsAsync(issue.ObjectId);
			} catch (Exception e) {
				await DisplayAlert("Error", "Failed to fetch issue comments.\n" + e.Message, "OK");
			} finally {
				LoadingLabel.IsVisible = false;
			}
		}

		void RefreshHandler(object sender, EventArgs e)
		{
			GetCommentsAsync();
		}
	}
}
