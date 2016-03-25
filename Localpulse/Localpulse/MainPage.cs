using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

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
