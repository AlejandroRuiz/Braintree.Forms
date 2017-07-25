using System;
using Android.App;
using Android.Content;
using Java.Lang;
using Square.Retrofit;
using Xamarin.Braintree.Android.DropIn.Test.Activities;
using Xamarin.Braintree.Android.DropIn.Test.Internal;

namespace Xamarin.Braintree.Android.DropIn.Test
{
	[Application]
	public class DemoApplication : Application
	{

		private static ApiClient sApiClient;

		public DemoApplication()
		{
		}

		static ApiClient getApiClient(Context context)
		{
			if (sApiClient == null)
			{
				sApiClient = new RestAdapter.Builder()
											.SetEndpoint(Settings.GetEnvironmentUrl(context))
											.SetRequestInterceptor(new ApiClientRequestInterceptor())
											.Build()
				                            .Create(Class.FromType(typeof(ApiClient));
			}

        	return sApiClient;
		}

		static void resetApiClient()
		{
			sApiClient = null;
		}
	}
}
