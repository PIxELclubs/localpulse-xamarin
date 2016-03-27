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
			
			Children.Add(nav);
		}
	}
}
