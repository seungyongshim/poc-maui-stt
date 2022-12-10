using System.Threading.Channels;
using Android;
using Android.App;
using Android.Content;
using Android.Provider;
using AndroidX;
using Java.Security;
using SmsMessage = Android.Telephony.SmsMessage;

namespace MauiApp1;

//https://github.com/gbzarelli/sms-received-sample/blob/master/app/src/main/java/br/com/helpdev/smsreceiver/receiver/SMSReceiver.kt

[BroadcastReceiver(Enabled = true, Exported = true, Permission = Manifest.Permission.BroadcastSms)]
public class SmsReceiver : BroadcastReceiver
{
    public SmsReceiver()
    {
        
    }
    public Channel<SmsMessage> Channel1 { get; } = Channel.CreateUnbounded<SmsMessage>();

    public override void OnReceive(Context context, Intent intent)
    {
        if (intent?.Action is not "android.provider.Telephony.SMS_RECEIVED")
            return;

        var extractMessage = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

        foreach(var message in extractMessage)
        {
            Channel1.Writer.TryWrite(message);
        }
    }
}
