using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MedPack item class. Base class - ItemClass.
/// </summary>
[System.Serializable]
public class MedPack : ItemClass
{
    /// <summary>
    /// Overriden Use() method.
    /// Deals with item usefullness and sends notifications.
    /// </summary>
    public override void Use()
    {
        if(!ItemUnnecessary)
        {
            StackCount--;
            SendNotificationToAll(new OnItemUsefullnessChange() { slotId = SlotIdInInventory });
            SendNotificationToAll(new OnSlotsMerge() { slotA = SlotIdInInventory });
            if (StackCount < 1)
            {
                MarkUsed();

                SendNotificationToAll(new OnItemThrow() { slotId = SlotIdInInventory });               
            }
        }        
    }
    /// <summary>
    /// Overriden MarkUsed() method.
    /// Invoking base method and then sending notifiaction about item usefullness change.
    /// </summary>
    public override void MarkUsed()
    {
        base.MarkUsed();
        if(SlotIdInInventory != -1)
        {
            SendNotificationToAll(new OnItemUsefullnessChange() { slotId = SlotIdInInventory });
        }
    }
}
