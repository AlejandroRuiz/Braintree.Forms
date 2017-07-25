using System;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Xamarin.Braintree.Android.DropIn.Test.Views;

namespace Xamarin.Braintree.Android.DropIn.Test.Fragments
{
	public class SettingsFragment : PreferenceFragment, global::Android.Content.ISharedPreferencesOnSharedPreferenceChangeListener
	{
		public SettingsFragment()
		{
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
             AddPreferencesFromResource(Resource.Xml.settings);

			ISharedPreferences preferences = PreferenceManager.SharedPreferences;

			OnSharedPreferenceChanged(preferences, "paypal_payment_type");

			OnSharedPreferenceChanged(preferences, "android_pay_currency");

			OnSharedPreferenceChanged(preferences, "android_pay_allowed_countries_for_shipping");
			preferences.RegisterOnSharedPreferenceChangeListener(this);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			PreferenceManager.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
		}

		#region global::Android.Content.ISharedPreferencesOnSharedPreferenceChangeListener

		public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
		{
			Preference preference = FindPreference(key);
			if (preference is ListPreference)
			{
				preference.Summary = (((ListPreference)preference).Entry);
			}
			else if (preference is SummaryEditTestPreference)
			{
				preference.Summary = preference.Summary;
			}
		}

		#endregion
	}
}
