using Android;
using Android.App;
using Android.Speech;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace MauiApp1;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public SmsReceiver SmsReceiver { get; private set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var grant = ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReceiveSms);
        if (grant != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this, new[]
            {
                Manifest.Permission.ReceiveSms
            }, 1);
        }

        SmsReceiver = MauiApplication.Current.Services.GetService<SmsReceiver>();
    }

    protected override void OnResume()
    {
        base.OnResume();
        RegisterReceiver(SmsReceiver, new(Android.Provider.Telephony.Sms.Intents.SmsReceivedAction));
        // Code omitted for clarity
    }

    protected override void OnPause()
    {
        UnregisterReceiver(SmsReceiver);
        // Code omitted for clarity
        base.OnPause();
    }
}

