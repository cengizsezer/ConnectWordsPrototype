using System;
using System.Threading.Tasks;
using UnityEngine;

public class TestAwaitableCode:MonoBehaviour
{
    
    public void OnMyEvent1(object sender, EventArgs eventArgs)
    {
        //code
    }

    public async void OnMyEvent2(object sender,DeferredEventArgs e)
    {
        var deferral = e.GetEventDeferral();

        await Task.Yield();

        deferral.Complate();
    }

    public async void OnMyEvent3(object sender,DeferredEventArgs e)
    {

        using (e.GetEventDeferral())
        {
            await Task.Yield();
        }
    }

    public void GettingDeferralCausesAwait()
    {
        var tsc = new TaskCompletionSource<bool>();

        var testClass = new TestClass();

        testClass.TestEvent += async (s, e) =>
        {
            var deferral = e.GetEventDeferral();

            await tsc.Task;

            deferral.Complate();
        };

        var handlersTask = testClass.RaiseTestEvent();

        UnityEngine.Assertions.Assert.IsFalse(handlersTask.IsCompleted);
        Debug.Log(handlersTask.IsCompleted);

        tsc.SetResult(true);

        UnityEngine.Assertions.Assert.IsTrue(handlersTask.IsCompleted);
        Debug.Log(handlersTask.IsCompleted);
    }

   
    public void NotGettingDeferralCausesNoAwait()
    {
        var tsc = new TaskCompletionSource<bool>();

        var testClass = new TestClass();

        testClass.TestEvent += async (s, e) =>
        {
            await tsc.Task;
        };

        var handlersTask = testClass.RaiseTestEvent();

        UnityEngine.Assertions.Assert.IsTrue(handlersTask.IsCompleted);

        tsc.SetResult(true);
    }

   
    public void UsingDeferralCausesAwait()
    {
        var tsc = new TaskCompletionSource<bool>();

        var testClass = new TestClass();

        testClass.TestEvent += async (s, e) =>
        {
            using (e.GetEventDeferral())
            {
                await tsc.Task;
            }
        };

        var handlersTask = testClass.RaiseTestEvent();

        UnityEngine.Assertions.Assert.IsFalse(handlersTask.IsCompleted);

        tsc.SetResult(true);

        UnityEngine.Assertions.Assert.IsTrue(handlersTask.IsCompleted);
    }

   
    public void MultipleHandlersCauseAwait(int firstToReleaseDeferral, int lastToReleaseDeferral)
    {
        var tsc = new[] {
                new TaskCompletionSource<bool>(),
                new TaskCompletionSource<bool>()
            };

        var testClass = new TestClass();

        testClass.TestEvent += async (s, e) =>
        {
            var deferral = e.GetEventDeferral();

            await tsc[0].Task;

            deferral.Complate();
        };

        testClass.TestEvent += async (s, e) =>
        {
            var deferral = e.GetEventDeferral();

            await tsc[1].Task;

            deferral.Complate();
        };

        var handlersTask = testClass.RaiseTestEvent();

        UnityEngine.Assertions.Assert.IsFalse(handlersTask.IsCompleted);

        tsc[firstToReleaseDeferral].SetResult(true);

        UnityEngine.Assertions.Assert.IsFalse(handlersTask.IsCompleted);

        tsc[lastToReleaseDeferral].SetResult(true);

        UnityEngine.Assertions.Assert.IsTrue(handlersTask.IsCompleted);
    }

   
}
