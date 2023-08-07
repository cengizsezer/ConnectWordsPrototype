using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EventDeferral : IDisposable
{
    private readonly TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();

    internal EventDeferral()
    {

    }

    public void Complate() => taskCompletionSource.TrySetResult(null);

    internal async Task WaitForCompletion(CancellationToken token)
    {
        using (token.Register(() => taskCompletionSource.TrySetCanceled()))
        {
            await taskCompletionSource.Task;
        }
    }


    public void Dispose()
    {
        Complate();
    }


}
