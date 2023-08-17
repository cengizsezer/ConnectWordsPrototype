using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EventManager
{
    public class EventHandlers<EventType> where EventType : CustomEvent
    {
        private List<Action<EventType>> handlers = new List<Action<EventType>>();
        private static EventHandlers<EventType> _instance = null;
        private static EventHandlers<EventType> instance 
        {

            get
            {

                if(_instance==null)
                {
                    _instance = new EventHandlers<EventType>();
                }

                


                return _instance;
            }

            

        }



        public static void Register(Action<EventType> handler)
        {
            if (instance.handlers.Contains(handler))
            {
                return;
            }

            
            
            instance.handlers.Add(handler);
        }

        public static void Unregister(Action<EventType> handler)
        {
            instance.handlers.Remove(handler);
        }

        public static void Handle(EventType eventData)
        {
            if (instance.handlers != null)
            {
                for (int i = instance.handlers.Count - 1; i >= 0; i--)
                {
                    instance.handlers[i](eventData);
                }
            }
        }
    }
    public static void RegisterHandler<EventType>(Action<EventType> handler) where EventType : CustomEvent
    {
        EventManager.EventHandlers<EventType>.Register(handler);
    }

    public static void UnregisterHandler<EventType>(Action<EventType> handler) where EventType : CustomEvent
    {
        EventManager.EventHandlers<EventType>.Unregister(handler);
    }

    public static void Send<T>(T eventData) where T : CustomEvent
    {
        EventManager.EventHandlers<T>.Handle(eventData);
    }


}


