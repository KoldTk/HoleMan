using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event
{
    HoleClick,
    MoveCharacter,
    CaculatePoint,
    CountCharacter,
    SpawnUFO,
    SendCharToReserve,
    SpawnReserveArea,
    ReduceHoleCounter,
    FromReserveToHole,
}
public class EventDispatcher<T>
{
    private static Dictionary<string, Action<T>> eventTable = new();

    public static void AddListener(string eventName, Action<T> listener)
    {
        if (!eventTable.ContainsKey(eventName))
        {
            eventTable[eventName] = delegate { };
        }
        eventTable[eventName] += listener;
    }

    public static void RemoveListener(string eventName, Action<T> listener)
    {
        if (eventTable.ContainsKey(eventName))
        {
            eventTable[eventName] -= listener;
        }
    }

    public static void Dispatch(string eventName, T parameter)
    {
        if(eventTable.TryGetValue(eventName, out var action))
        {
            action.Invoke(parameter);
        }    
    }

    public static void ClearAll()
    {
        eventTable.Clear();
    }
}
