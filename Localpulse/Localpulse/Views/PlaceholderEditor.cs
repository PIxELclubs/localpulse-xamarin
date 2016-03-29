using Xamarin.Forms;

namespace Localpulse.Views
{
    public class PlaceholderEditor : Editor
    {
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create<PlaceholderEditor, string>(p => p.Placeholder, string.Empty);

		public string Placeholder
		{
			get {
				return (string)GetValue(PlaceholderProperty);
			}
			set {
				SetValue(PlaceholderProperty, value);
			}
		}

		public static readonly BindableProperty IsEmptyProperty =
			BindableProperty.Create<PlaceholderEditor, bool>(p => p.IsEmpty, true);

		public bool IsEmpty
		{
			get {
				return Text == "" || Text == Placeholder;
			}
		}

		public PlaceholderEditor()
			: base()
		{
			// Doesn't work
			// Text = Placeholder;

			Focused += (sender, e) => Text = Text == Placeholder ? "" : Text;
			Unfocused += (sender, e) => Text = Text == "" ? Placeholder : Text;
		}
	}
}
