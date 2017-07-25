using System;
using Android.Content;
using Android.Preferences;
using Xamarin.Braintree.Api;

namespace Xamarin.Braintree.Android.DropIn.Test.Activities
{
	public class Settings
	{
		protected const string ENVIRONMENT = "environment";

		const string VERSION = "version";

		const string SANDBOX_BASE_SERVER_URL = "https://braintree-sample-merchant.herokuapp.com";
		const string PRODUCTION_BASE_SERVER_URL = "https://executive-sample-merchant.herokuapp.com";

		const string SANDBOX_TOKENIZATION_KEY = "sandbox_tmxhyf7d_dcpspy2brwdjr3qn";
		const string PRODUCTION_TOKENIZATION_KEY = "production_t2wns2y2_dfy45jdj3dxkmz5m";

		private static ISharedPreferences sSharedPreferences;

		public static ISharedPreferences GetPreferences(Context context)
		{
			if (sSharedPreferences == null)
			{
				sSharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context.ApplicationContext);
			}

			return sSharedPreferences;
		}

		public static int GetVersion(Context context)
		{
			return GetPreferences(context).GetInt(VERSION, 0);
		}

		public static void SetVersion(Context context)
		{
			GetPreferences(context).Edit().PutInt(VERSION, BuildConfig.VersionCode).Apply();
		}

		public static int GetEnvironment(Context context)
		{
			return GetPreferences(context).GetInt(ENVIRONMENT, 0);
		}

		public static void SetEnvironment(Context context, int environment)
		{
			GetPreferences(context)
								.Edit()
								.PutInt(ENVIRONMENT, environment)
							.Apply();

			DemoApplication.ResetApiClient();
		}

		public static string GetSandboxUrl()
		{
			return SANDBOX_BASE_SERVER_URL;
		}

		public static string GetEnvironmentUrl(Context context)
		{
			int environment = getEnvironment(context);
			if (environment == 0)
			{
				return SANDBOX_BASE_SERVER_URL;
			}
			else if (environment == 1)
			{
				return PRODUCTION_BASE_SERVER_URL;
			}
			else
			{
				return "";
			}
		}

		public static string GetCustomerId(Context context)
		{
			return GetPreferences(context).GetString("customer", null);
		}

		public static string GetMerchantAccountId(Context context)
		{
			return GetPreferences(context).GetString("merchant_account", null);
		}

		public static bool ShouldCollectDeviceData(Context context)
		{
			return GetPreferences(context).GetBoolean("collect_device_data", false);
		}

		public static string GetThreeDSecureMerchantAccountId(Context context)
		{
			if (IsThreeDSecureEnabled(context) && getEnvironment(context) == 1)
			{
				return "test_AIB";
			}
			else
			{
				return null;
			}
		}

		public static string GetUnionPayMerchantAccountId(Context context)
		{
			if (GetEnvironment(context) == 0)
			{
				return "fake_switch_usd";
			}
			else
			{
				return null;
			}
		}

		public static bool UseTokenizationKey(Context context)
		{
			return GetPreferences(context).GetBoolean("tokenization_key", false);
		}

		public static string GetEnvironmentTokenizationKey(Context context)
		{
			int environment = getEnvironment(context);
			if (environment == 0)
			{
				return SANDBOX_TOKENIZATION_KEY;
			}
			else if (environment == 1)
			{
				return PRODUCTION_TOKENIZATION_KEY;
			}
			else
			{
				return "";
			}
		}

		public static bool IsAndroidPayShippingAddressRequired(Context context)
		{
			return GetPreferences(context).GetBoolean("android_pay_require_shipping_address", false);
		}

		public static bool IsAndroidPayPhoneNumberRequired(Context context)
		{
			return GetPreferences(context).GetBoolean("android_pay_require_phone_number", false);
		}

		public static string GetAndroidPayCurrency(Context context)
		{
			return GetPreferences(context).GetString("android_pay_currency", "USD");
		}

		public static string[] GetAndroidPayAllowedCountriesForShipping(Context context)
		{
			string[] countries = GetPreferences(context).GetString("android_pay_allowed_countries_for_shipping", "US").Split(',');
			for (int i = 0; i < countries.Length; i++)
			{
				countries[i] = countries[i].Trim();
			}

			return countries;
		}

		public static string GetPayPalPaymentType(Context context)
		{
			return GetPreferences(context).GetString("paypal_payment_type", context.GetString(Resource.String.paypal_billing_agreement));
		}

		public static bool IsPayPalAddressScopeRequested(Context context)
		{
			return GetPreferences(context).GetBoolean("paypal_request_address_scope", false);
		}

		public static bool IsPayPalSignatureVerificationDisabled(Context context)
		{
			return GetPreferences(context).GetBoolean("paypal_disable_signature_verification", true);
		}

		public static bool UseHardcodedPayPalConfiguration(Context context)
		{
			return GetPreferences(context).GetBoolean("paypal_use_hardcoded_configuration", false);
		}

		public static bool IsThreeDSecureEnabled(Context context)
		{
			return GetPreferences(context).GetBoolean("enable_three_d_secure", false);
		}

		public static bool IsThreeDSecureRequired(Context context)
		{
			return GetPreferences(context).GetBoolean("require_three_d_secure", true);
		}
	}
}
