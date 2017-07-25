using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Java.Interop;

namespace Xamarin.Braintree.Android.CardForm.Test
{
	[Activity(Label = "Xamarin.Braintree.Android.CardForm.Test", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
		}

		[Export("onClick")]
		public void OnClick(View v)
		{
			switch (v.Id)
			{
				case Resource.Id.material_light_theme_form:
					StartActivity(new Intent(this, typeof(LightThemeActivity)));
					break;
				case Resource.Id.material_dark_theme_form:
					StartActivity(new Intent(this, typeof(DarkThemeActivity)));
					break;
				default:
					break;
			}
		}
	}
}

