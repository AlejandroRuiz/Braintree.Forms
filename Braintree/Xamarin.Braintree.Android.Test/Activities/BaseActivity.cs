
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util.Logging;
using Xamarin.Braintree.Api;
using Xamarin.Braintree.Api.Interfaces;
using Xamarin.Braintree.Api.Models;

namespace Xamarin.Braintree.Android.Test.Activities
{
	public class BaseActivity : AppCompatActivity,
								global::Android.Support.V4.App.ActivityCompat.IOnRequestPermissionsResultCallback,
								IPaymentMethodNonceCreatedListener,
								IBraintreeCancelListener,
								IBraintreeErrorListener,
								global::Android.Support.V7.App.ActionBar.IOnNavigationListener
	{

		const string EXTRA_AUTHORIZATION = "com.braintreepayments.demo.EXTRA_AUTHORIZATION";
		const string EXTRA_CUSTOMER_ID = "com.braintreepayments.demo.EXTRA_CUSTOMER_ID";

		protected string mAuthorization;
		protected string mCustomerId;
		protected BraintreeFragment mBraintreeFragment;
		protected Logger mLogger;

		private bool mActionBarSetup;

		#region global::Android.Support.V7.App.ActionBar.IOnNavigationListener

		public bool OnNavigationItemSelected(int itemPosition, long itemId)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IBraintreeErrorListener

		public void OnError(Java.Lang.Exception p0)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IBraintreeCancelListener

		public void OnCancel(int p0)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IPaymentMethodNonceCreatedListener

		public void OnPaymentMethodNonceCreated(PaymentMethodNonce p0)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
