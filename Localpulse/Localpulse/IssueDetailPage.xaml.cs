using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Xamarin.Forms;

namespace Localpulse
{
	public partial class IssueDetailPage : ContentPage
	{
		IssueDetail issue;
		ObservableCollection<IssueComment> comments = new ObservableCollection<IssueComment>();
		bool loading = true;

		public IssueDetailPage(string objectId)
		{
			InitializeComponent();
			issue = DbService.Issues[objectId];
			IssueLayout.BindingContext = issue;

			comments.CollectionChanged += CommentsChanged;

			GetCommentsAsync();
		}

		void CommentsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
				case NotifyCollectionChangedAction.Add:
					foreach (var comment in e.NewItems) {
						CommentsView.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
						var tmpl = new Views.CommentView();
						tmpl.BindingContext = (IssueComment)comment;
						CommentsView.Children.Add(tmpl, 0, CommentsView.Children.Count);
					}
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
				loading = true;
				var tmp = await RestService.GetIssueCommentsAsync(issue.ObjectId);
				DbService.MergeCollection<IssueComment>(comments, tmp);
				loading = false;
			} catch (Exception e) {
				loading = false;
				await DisplayAlert("Error", "Failed to fetch issue comments.\n" + e.Message, "OK");
				await Navigation.PopAsync();
			}
		}

		void RefreshHandler(object sender, EventArgs e)
		{
			GetCommentsAsync();
		}
	}
}
