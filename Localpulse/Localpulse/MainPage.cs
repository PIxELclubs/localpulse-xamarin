using Xamarin.Forms;

namespace Localpulse
{
	public class MainPage : ContentPage
	{
		public MainPage ()
		{
			Label label = new Label {
				Text = "MainPage",
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Yellow,
				TextColor = Color.Black,
			};
			Content = label;
		}
	}
}
