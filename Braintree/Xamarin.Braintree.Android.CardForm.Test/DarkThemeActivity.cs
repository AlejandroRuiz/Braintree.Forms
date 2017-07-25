
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xamarin.Braintree.Android.CardForm.Test
{
	[Activity(Label = "DarkThemeActivity", Theme="@style/DarkTheme")]
	public class DarkThemeActivity : BaseCardFormActivity
	{
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu(menu);
			if (mCardForm.IsCardScanningAvailable)
			{
				MenuInflater.Inflate(Resource.Menu.card_io_dark, menu);
			}

			return true;
		}
	}
}
