using System;
using System.Threading.Tasks;
using UnityEngine;

public class TestClass
{
    public event EventHandler<DeferredEventArgs> TestEvent;

    public Task RaiseTestEvent() => TestEvent.InvokeAsync(this, new DeferredEventArgs());
}