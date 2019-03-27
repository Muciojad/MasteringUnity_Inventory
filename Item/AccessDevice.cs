using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AccessDevice item class, base class - ItemClass.
/// Used for customization AccessDevices properties.
/// </summary>

[System.Serializable]
public class AccessDevice : ItemClass
{

    /// <summary>
    /// Overriden Use() method. 
    /// Marks item as already used and sends notification OnItemUsed to listeners.
    /// </summary>
    public override void Use()
    {
        MarkUsed();
        SendNotificationToAll(new OnItemUsed() { ItemName = ItemName });
        //UsageNotifier.instance.Notify(new OnItemUsed() { ItemName = ItemName });               
    }

}
