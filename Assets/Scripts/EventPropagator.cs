using System;
using Sirenix.OdinInspector;


public abstract class EventPropagator: SerializedScriptableObject
{
    public abstract void Invoke();
}