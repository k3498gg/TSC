using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventCenter
{
    public delegate void EventHandler<T>(object sender, T evt);

    public static EventCenter Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new EventCenter();
            }
            return instance;
        }
    }

    private static EventCenter instance;

    private Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();

    public void Register<T>(EventHandler<T> handler)
    {
        Register(typeof(T), handler);
    }

    private void Register<T>(Type eventType, EventHandler<T> handler)
    {
        if (!handlers.ContainsKey(eventType))
        {
            handlers.Add(eventType, new List<object>());
        }

        if (!handlers[eventType].Contains(handler))
        {
            handlers[eventType].Add(handler);
        }
    }

    public void Unregister<T>(EventHandler<T> handler)
    {
        Unregister(typeof(T), handler);
    }

    private void Unregister<T>(Type eventType, EventHandler<T> handler)
    {
        if (handlers.ContainsKey(eventType))
        {
            handlers[eventType].Remove(handler);

            if (0 == handlers[eventType].Count)
            {
                handlers.Remove(eventType);
            }
        }
    }

    public void Publish<T>(object sender, T evt)
    {
        Publish<T>(sender, typeof(T), evt);
        sender = null;
        evt = default(T);
    }

    public void Publish<T>(object sender, Type eventType, T evt)
    {
        if (handlers.ContainsKey(eventType))
        {
            handlers[eventType].RemoveAll(delegate(object handler) { return handler == null; });

            //foreach (object handler in handlers[eventType])
            //{
            //    MethodInfo method = handler.GetType().GetMethod("Invoke");
            //    method.Invoke(handler, new object[] { sender, evt });
            //}
            for (int i = 0; i < handlers[eventType].Count; ++i)
            {
                object handler = handlers[eventType][i];
                MethodInfo method = handler.GetType().GetMethod("Invoke");
                method.Invoke(handler, new object[] { sender, evt });
            }
        }
    }

    public void Clear()
    {
        handlers.Clear();
    }
}