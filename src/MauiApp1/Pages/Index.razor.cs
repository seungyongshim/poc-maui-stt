using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MauiApp1;
using MauiApp1.Shared;
using Android.SE.Omapi;
using System.Collections.ObjectModel;

namespace MauiApp1.Pages
{
    public partial class Index : IDisposable
    {
        [Inject]
        SmsReceiver SmsReceiver { get; set; }

        string Sms { get; set; }

        bool IsToggled { get; set; } = true;

        CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var reader = SmsReceiver.Channel1.Reader;

            Task.Run(async () =>
            {
                Sms = "Hello";

                while(!CancellationTokenSource.IsCancellationRequested)
                {
                    var sms = await reader.ReadAsync(CancellationTokenSource.Token);

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Sms = sms.MessageBody;
                        StateHasChanged();
                    });
                }
            });
        }

        async Task FlashlightSwitch_Toggled()
        {
            try
            {
                if (IsToggled)
                {
                    HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                    await Flashlight.Default.TurnOnAsync();
                    IsToggled = false;
                }
                else
                {
                    HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                    await Flashlight.Default.TurnOffAsync();
                    IsToggled = true;
                }
            }
            catch (FeatureNotSupportedException ex)
            {
            // Handle not supported on device exception
            }
            catch (PermissionException ex)
            {
            // Handle permission exception
            }
            catch (Exception ex)
            {
            // Unable to turn on/off flashlight
            }
        }
    }
}
