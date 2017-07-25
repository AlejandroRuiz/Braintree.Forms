using System;
using Android.Content;
using Android.Preferences;
using Android.Util;

namespace Xamarin.Braintree.Android.DropIn.Test.Views
{
	public class SummaryEditTestPreference : EditTextPreference
	{

		private string mSummaryString;

		public SummaryEditTestPreference(Context context) : base(context)
		{
			init();
		}

		public SummaryEditTestPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			init();
		}

		public SummaryEditTestPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			init();
		}

		private void init()
		{
			var text = Text;
			Summary = text ?? "";
			mSummaryString = Summary;
		}
	}
}
