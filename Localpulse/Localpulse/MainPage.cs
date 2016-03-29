using Xamarin.Forms;

namespace Localpulse
{
	public class MainPage : TabbedPage
	{
		public MainPage ()
		{
			var list = new IssuesListPage();
			var nav = new NavigationPage(list);
			nav.Title = list.Title;
			BackgroundColor = new Color(0.0, 166.0 / 255.0, 233.0 / 255.0);

			Children.Add(nav);
		}
	}
}
