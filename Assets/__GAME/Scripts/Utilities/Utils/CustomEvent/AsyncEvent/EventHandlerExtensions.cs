using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public static class EventHandlerExtensions
{
    private static readonly Task CompletedTask = Task.FromResult(0);

    public static Task InvokeAsync<T>(this EventHandler<T> eventHandler, object sender, T eventArgs)
        where T :DeferredEventArgs
        {

        return InvokeAsync(eventHandler,sender,eventArgs,CancellationToken.None);

    }


    public static Task InvokeAsync<T>(this EventHandler<T> eventHandler,object sender,T eventArgs,CancellationToken token)
        where T:DeferredEventArgs
    {
        if(eventHandler==null)
        {
            return CompletedTask;
        }


        var Tasks = eventHandler.GetInvocationList()
            .OfType<EventHandler<T>>().Select(invocationDelegate =>
            {
                token.ThrowIfCancellationRequested();
                invocationDelegate(sender, eventArgs);
                var deferral = eventArgs.GetCurrentDeferralAndReset();
                return deferral?.WaitForCompletion(token) ?? CompletedTask;
            }).ToArray();

        return Task.WhenAll(Tasks);
    }
}
