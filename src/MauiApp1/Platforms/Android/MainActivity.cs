using Android;
using Android.App;
using Android.Speech;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Speech.Tts;
using static Android.Provider.DocumentsContract;
using Proto;
using MauiApp1.Pages;

namespace MauiApp1;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private bool isRecording;
    private static readonly int VOICE = 10;
    private string RecordText { get; set; } = string.Empty;
    private EventStreamSubscription<object> EventStream { get; set; }

    private IRootContext RootContext { get; set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        isRecording = false;
        RootContext = MauiApplication.Current.Services.GetService<IRootContext>();

        EventStream = RootContext.System.EventStream.Subscribe<PressButton>(s =>
        {
            isRecording = !isRecording;
            if (isRecording)
            {
                SpeechToText();
            }
        });

        base.OnCreate(savedInstanceState);
    }

    public void SpeechToText()
    {
        var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
        voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now!");
        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
        voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
        StartActivityForResult(voiceIntent, VOICE);
    }

    protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
    {
        if (requestCode == VOICE)
        {
            if (resultVal == Result.Ok)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                if (matches.Count != 0)
                {
                    RootContext.System.EventStream.Publish(new StringState(matches[0]));
                }
            }
        }

        base.OnActivityResult(requestCode, resultVal, data);
    }
}

