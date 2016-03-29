using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;
using Windows.UI.ViewManagement;

namespace Localpulse.WinPhone
{
	public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
	{
		public MainPage ()
		{
			InitializeComponent ();
			SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

			var container = new SimpleContainer();
			container.Register<IDevice>(t => WindowsPhoneDevice.CurrentDevice);
			container.Register<IGeolocator, Geolocator>();
			container.Register<IMediaPicker, MediaPicker>();
			Resolver.SetResolver(container.GetResolver());

			global::Xamarin.Forms.Forms.Init ();
			LoadApplication (new Localpulse.App ());
		}
	}
}
