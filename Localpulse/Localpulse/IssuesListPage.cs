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

			RestService.RefreshIssuesAsync();
		}

		void OnSelection(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}
			DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
			//comment out if you want to keep selections
			ListView lst = (ListView)sender;
			lst.SelectedItem = null;
		}
	}
}
