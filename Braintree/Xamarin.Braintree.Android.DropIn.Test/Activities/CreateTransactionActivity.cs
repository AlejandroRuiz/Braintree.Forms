using System;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Braintree.Api.Models;

namespace Xamarin.Braintree.Android.DropIn.Test.Activities
{
	public class CreateTransactionActivity : AppCompatActivity
	{
		const string EXTRA_PAYMENT_METHOD_NONCE = "nonce";

		private ProgressBar mLoadingSpinner;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.create_transaction_activity);
			mLoadingSpinner = FindViewById<ProgressBar>(Resource.Id.loading_spinner);

			SetTitle(Resource.String.processing_transaction);

			SendNonceToServer((PaymentMethodNonce)Intent.GetParcelableExtra(EXTRA_PAYMENT_METHOD_NONCE));
		}

		private void sendNonceToServer(PaymentMethodNonce nonce)
		{
			Callback<Transaction> callback = null;
		}
	}
}
