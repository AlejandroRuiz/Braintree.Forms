using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Text;
using Android.Widget;
using Java.Lang;
using Java.Util.Logging;
using Xamarin.Braintree.Api;
using Xamarin.Braintree.Api.Interfaces;
using Xamarin.Braintree.Api.Internal;
using Xamarin.Braintree.Api.Models;
using Xamarin.PayPal.Android.OneTouch.Core;

namespace Xamarin.Braintree.Android.DropIn.Test.Activities
{
	public abstract class BaseActivity : AppCompatActivity,
								global::Android.Support.V4.App.ActivityCompat.IOnRequestPermissionsResultCallback,
								IPaymentMethodNonceCreatedListener,
								IBraintreeCancelListener,
								IBraintreeErrorListener,
								ActionBar.IOnNavigationListener
	{

		const string KEY_AUTHORIZATION = "com.braintreepayments.demo.KEY_AUTHORIZATION";

		protected string mAuthorization;
		protected string mCustomerId;
		protected BraintreeFragment mBraintreeFragment;

		private bool mActionBarSetup;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (savedInstanceState != null && savedInstanceState.ContainsKey(KEY_AUTHORIZATION))
			{
				mAuthorization = savedInstanceState.GetString(KEY_AUTHORIZATION);
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (!mActionBarSetup)
			{
				SetupActionBar();
				mActionBarSetup = true;
			}

			SignatureVerificationOverrides.disableAppSwitchSignatureVerification(Settings.IsPayPalSignatureVerificationDisabled(this));
			PayPalOneTouchCore.UseHardcodedConfig(this, Settings.UseHardcodedPayPalConfiguration(this));

			if (Api.BuildConfig.Debug && ContextCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.WriteExternalStorage)
				!= global::Android.Content.PM.Permission.Granted)
			{
				ActivityCompat.RequestPermissions(this, new string[] { global::Android.Manifest.Permission.WriteExternalStorage }, 1);
			}
			else
			{
				HandleAuthorizationState();
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, global::Android.Content.PM.Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			HandleAuthorizationState();
		}

		private void HandleAuthorizationState()
		{
			if (mAuthorization == null ||
					(Settings.UseTokenizationKey(this) && mAuthorization != Settings.GetEnvironmentTokenizationKey(this)) ||
					!TextUtils.Equals(mCustomerId, Settings.GetCustomerId(this)))
			{
				PerformReset();
			}
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
			if (mAuthorization != null)
			{
				outState.PutString(KEY_AUTHORIZATION, mAuthorization);
			}
		}

		#region IPaymentMethodNonceCreatedListener

		public void OnPaymentMethodNonceCreated(PaymentMethodNonce p0)
		{
			System.Console.WriteLine("Payment Method Nonce received: " + p0.TypeLabel);
		}

		#endregion

		#region IBraintreeCancelListener

		public void OnCancel(int p0)
		{
			System.Console.WriteLine("Cancel received: " + p0);
		}

		#endregion

		#region IBraintreeErrorListener

		public void OnError(Java.Lang.Exception p0)
		{
			System.Console.WriteLine("Error received (" + p0.GetType().ToString() + "): " + p0.Message);
			System.Console.WriteLine(p0.ToString());

			ShowDialog("An error occurred (" + p0.GetType().ToString() + "): " + p0.Message);
		}

		#endregion

		#region ActionBar.IOnNavigationListener

		public bool OnNavigationItemSelected(int itemPosition, long itemId)
		{
			if (Settings.GetEnvironment(this) != itemPosition)
			{
				Settings.SetEnvironment(this, itemPosition);
				performReset();
			}
			return true;
		}

		#endregion

		private void performReset()
		{
			mAuthorization = null;
			mCustomerId = Settings.GetCustomerId(this);

			if (mBraintreeFragment == null)
			{
				mBraintreeFragment = (BraintreeFragment)FragmentManager.FindFragmentByTag(mBraintreeFragment.Tag);
			}

			if (mBraintreeFragment != null)
			{
				if (Build.VERSION.SdkInt >= Build.VERSION_CODES.N)
				{
					FragmentManager.BeginTransaction().Remove(mBraintreeFragment).CommitNow();
				}
				else
				{
					FragmentManager.BeginTransaction().Remove(mBraintreeFragment).Commit();
					FragmentManager.ExecutePendingTransactions();
				}

				mBraintreeFragment = null;
			}

			Reset();
			FetchAuthorization();
		}

		protected abstract void Reset();

		protected abstract void OnAuthorizationFetched();

		protected void FetchAuthorization()
		{
			if (mAuthorization != null)
			{
				OnAuthorizationFetched();
			}
			else if (Settings.UseTokenizationKey(this))
			{
				mAuthorization = Settings.GetEnvironmentTokenizationKey(this);
				OnAuthorizationFetched();
			}
			else
			{
				//			DemoApplication.getApiClient(this).getClientToken(Settings.getCustomerId(this),
				//					Settings.getMerchantAccountId(this), new Callback<ClientToken>() {
				//					@Override

				//					public void success(ClientToken clientToken, Response response)
				//	{
				//		if (TextUtils.isEmpty(clientToken.getClientToken()))
				//		{
				//			showDialog("Client token was empty");
				//		}
				//		else
				//		{
				//			mAuthorization = clientToken.getClientToken();
				//			onAuthorizationFetched();
				//		}
				//	}

				//	@Override
				//					public void failure(RetrofitError error)
				//	{
				//		showDialog("Unable to get a client token. Response Code: " +
				//				error.getResponse().getStatus() + " Response body: " +
				//				error.getResponse().getBody());
				//	}
				//});
			}
		}

		protected void ShowDialog(string message)
		{
			new AlertDialog.Builder(this)
						   .SetMessage(message)
						   .SetPositiveButton(global::Android.Resource.String.Ok,
											  (sender, e) => (sender as AlertDialog).Dismiss())
						   .Show();
		}

		protected void setUpAsBack()
		{
			if (ActionBar != null)
			{
				ActionBar.SetDisplayHomeAsUpEnabled(true);
			}
		}

		private void setupActionBar()
		{
			ActionBar actionBar = SupportActionBar;
			actionBar.SetDisplayShowTitleEnabled(false);
			actionBar.NavigationMode = (int)global::Android.App.ActionBarNavigationMode.List;

			var adapter = ArrayAdapter.CreateFromResource(this,
														  Resource.Array.Environments, global::Android.Resource.Layout.simple_spinner_dropdown_item);
			actionBar.SetListNavigationCallbacks(adapter, this);
			actionBar.SetSelectedNavigationItem(Settings.GetEnvironment(this));
		}

		public override bool OnCreateOptionsMenu(global::Android.Views.IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(global::Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case global::Android.Resource.Id.Home:
					Finish();
					return true;
				case Resource.Id.reset:
					performReset();
					return true;
				case Resource.Id.settings:
					StartActivity(new Intent(this, typeof(SettingsActivity)));
					return true;
				default:
					return false;
			}
		}
	}
}
