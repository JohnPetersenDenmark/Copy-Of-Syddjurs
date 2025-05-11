using Android.App;
using Android.Content;
using Android.OS;
using SMSmessage = Android.Telephony.SmsMessage;

namespace Syddjurs.Platforms.Android
{
    public  class SmsReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent.Action == "android.provider.Telephony.SMS_RECEIVED")
            {
                var pdus = (Java.Lang.Object[])intent.Extras.Get("pdus");
                foreach (var pdu in pdus)
                {
                    SMSmessage message = SMSmessage.CreateFromPdu((byte[])pdu, Build.VERSION.SdkInt >= BuildVersionCodes.M ? "3gpp" : null);
                    string sender = message.OriginatingAddress;
                    string body = message.MessageBody;

                    // You can raise an event or call back into shared MAUI code here
                    Console.WriteLine($"SMS from {sender}: {body}");
                }
            }
        }
    }
}
