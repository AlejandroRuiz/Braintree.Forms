using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Braintree.Api;

namespace Xamarin.Braintree.Android.DropIn.Test.Internal
{
	public class ApiClient
	{
		HttpClient _client;

		public ApiClient()
		{
			_client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("User-Agent", "braintree/android-demo-app/" + BuildConfig.VersionName));
		}

		void getClientToken(@Query("customer_id") String customerId, @Query("merchant_account_id") String merchantAccountId, Callback < ClientToken > callback)
		{
			
		}
	}
}
