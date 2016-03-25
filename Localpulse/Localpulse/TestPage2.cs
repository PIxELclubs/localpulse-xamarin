using Xamarin.Forms;

namespace Localpulse
{
	public class TestPage2 : ContentPage
	{
		public TestPage2 ()
		{
			Title = "Localpulse2";
			Content = new StackLayout {
				Children = {
					new Label { Text = "Hello ContentPage2", VerticalOptions = LayoutOptions.CenterAndExpand }
				}
			};
		}
	}
}
