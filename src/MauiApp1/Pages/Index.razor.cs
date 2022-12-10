using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Proto;

namespace MauiApp1.Pages;

public partial class Index : IDisposable
{
    [Inject]
    private IRootContext Root { get; init; }

    private string State { get; set; }

    private EventStreamSubscription<object> EventStream { get; set; }

    public void Dispose()
    {
        EventStream.Unsubscribe();
    }

    public Task ButtonHandlerAsync(MouseEventArgs args)
    {
        Root.System.EventStream.Publish(new PressButton());

        return Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        EventStream = Root.System.EventStream.Subscribe<StringState>(s =>
        {
            State += "\n\n" + s.Value;
            StateHasChanged();
        });
    }
}

public record PressButton;

public record StringState(string Value);

