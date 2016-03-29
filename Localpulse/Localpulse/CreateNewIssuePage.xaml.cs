using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

#if WINDOWS_PHONE
using XLabs.Ioc;
using XLabs.Platform;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;
#endif

namespace Localpulse
{
	public partial class CreateNewIssuePage : ContentPage
	{
#if WINDOWS_PHONE
		List<ToolbarItem> backupToolbarItems;
		TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

		IGeolocator geolocator;
		Task<Position> positionTask;
		Position Position = null;

		IMediaPicker mediaPicker;
		ImageSource imageSource = null;
		Stream imageStream = null;
		Stream copiedImageStream = null;
#endif

		public CreateNewIssuePage()
		{
			InitializeComponent();
			Title = "Submit Report";

#if WINDOWS_PHONE
			backupToolbarItems = new List<ToolbarItem>(ToolbarItems);

			Description.Focused += (sender, e) => ToolbarItems.Clear();
			Description.Unfocused += (sender, e) => { foreach (var a in backupToolbarItems) ToolbarItems.Add(a); };
			
			geolocator = Resolver.Resolve<IGeolocator>();
			mediaPicker = Resolver.Resolve<IMediaPicker>();
			GetGeolocation();

			#region PhotoChooser show camera icon hack
			var _photoChooserField = typeof(MediaPicker).GetField("_photoChooser", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
			var _photoChooser = (Microsoft.Phone.Tasks.PhotoChooserTask)_photoChooserField.GetValue(mediaPicker);
			_photoChooser.ShowCamera = true;
			#endregion
#endif
		}

#if WINDOWS_PHONE
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			CleanupPicture();
			if (copiedImageStream != null) {
				copiedImageStream.Dispose();
			}
		}

		void GetGeolocation()
		{
			positionTask = geolocator.GetPositionAsync(10000);
			positionTask.ContinueWith(_pos => {
				Position = _pos.Result;
			}, scheduler);
		}

		async Task CleanupPicture()
		{
			if (imageSource != null) {
				await imageSource.Cancel();
				imageSource = null;
			}
			if (imageStream != null) {
				imageStream.Dispose();
				imageStream = null;
			}
		}

		async void TakePicture(object sender, EventArgs e)
		{
			try {
				var mediaFile = await mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions {
					DefaultCamera = CameraDevice.Rear,
					MaxPixelDimension = 1500,
					PercentQuality = 80,
				});
				var stream = mediaFile.Source;
				if (copiedImageStream != null) {
					copiedImageStream.Dispose();
				}
				// Eww
				copiedImageStream = new MemoryStream((int)stream.Length);
				stream.Seek(0L, SeekOrigin.Begin);
				await stream.CopyToAsync(copiedImageStream);
				stream.Seek(0L, SeekOrigin.Begin);
				copiedImageStream.Seek(0L, SeekOrigin.Begin);
				ImagePreview.Source = ImageSource.FromStream(() => stream);

				// Cleanup resources
				await CleanupPicture();
				imageSource = ImagePreview.Source;
				imageStream = stream;
			} catch (TaskCanceledException err) {
				// Do nothing
			} catch (Exception err) {
				await DisplayAlert("Error", "Failed to take photo.\n" + err.ToString(), "OK");
			}
		}

		async void SubmitReport(object sender, EventArgs e)
		{
			try {
				if (copiedImageStream == null) {
					await DisplayAlert("Error", "You have not taken a photo.", "OK");
					return;
				}
				if (Description.IsEmpty) {
					await DisplayAlert("Error", "Please write a short description of the issue.", "OK");
					return;
				}
				await positionTask;
				await RestService.PostIssueAsync(copiedImageStream, Description.Text, Position);
				await Navigation.PopAsync();
			} catch (Exception err) {
				await DisplayAlert("Error", "Failed to submit issue.\n" + err.ToString(), "OK");
			}
		}
#endif
	}
}
