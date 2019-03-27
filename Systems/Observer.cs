using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for every single observer.
/// </summary>
[System.Serializable]
public abstract class Observer
{
    public abstract void OnNotify(object @event);
}
