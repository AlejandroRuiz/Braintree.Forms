
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
using Xamarin.Braintree.CardForm;
using Xamarin.Braintree.CardForm.Utils;
using Xamarin.Braintree.CardForm.View;

namespace Xamarin.Braintree.Android.CardForm.Test
{
	public class BaseCardFormActivity : AppCompatActivity, IOnCardFormSubmitListener, CardEditText.IOnCardTypeChangedListener
	{
		public CardType[] SUPPORTED_CARD_TYPES { get; private set; } = new CardType[]{ CardType.Visa, CardType.Mastercard, CardType.Discover,
			CardType.Amex, CardType.DinersClub, CardType.Jcb, CardType.Maestro, CardType.Unionpay };

		SupportedCardTypesView mSupportedCardTypesView;

		protected Xamarin.Braintree.CardForm.View.CardForm mCardForm;

		#region CardEditText.IOnCardTypeChangedListener

		public void OnCardTypeChanged(CardType p0)
		{
			if (p0 == CardType.Empty)
			{
				mSupportedCardTypesView.SetSupportedCardTypes(SUPPORTED_CARD_TYPES);
			}
			else
			{
				mSupportedCardTypesView.SetSelected(p0);
			}
		}
  		
		#endregion

		#region IOnCardFormSubmitListener

		public void OnCardFormSubmit()
		{
			if (mCardForm.IsValid)
			{
				Toast.MakeText(this, Resource.String.valid, ToastLength.Short).Show();
			}
			else
			{
				mCardForm.Validate();
				Toast.MakeText(this, Resource.String.invalid, ToastLength.Short).Show();
			}
		}

		#endregion

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.card_form);

			mSupportedCardTypesView = FindViewById<SupportedCardTypesView>(Resource.Id.supported_card_types);
			mSupportedCardTypesView.SetSupportedCardTypes(SUPPORTED_CARD_TYPES);

			mCardForm = FindViewById<Xamarin.Braintree.CardForm.View.CardForm>(Resource.Id.card_form);
			mCardForm.CardRequired(true)
							.ExpirationRequired(true)
							.CvvRequired(true)
							.PostalCodeRequired(true)
							.MobileNumberRequired(true)
							.MobileNumberExplanation("Make sure SMS is enabled for this mobile number")
							.ActionLabel(GetString(Resource.String.purchase))
					 .Setup(this);
			mCardForm.SetOnCardFormSubmitListener(this);
			mCardForm.SetOnCardTypeChangedListener(this);

			// Warning: this is for development purposes only and should never be done outside of this example app.
			// Failure to set FLAG_SECURE exposes your app to screenshots allowing other apps to steal card information.
			Window.ClearFlags(WindowManagerFlags.Secure);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			base.OnOptionsItemSelected(item);
			if (item.ItemId == Resource.Id.card_io_item)
			{
				mCardForm.ScanCard(this);
				return true;
			}

			return false;
		}
	}
}
