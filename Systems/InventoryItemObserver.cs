using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listen for slot update and refresh UI view.
/// </summary>
[System.Serializable]
public class InventoryItemObserver : Observer
{
    public static InventoryItemObserver instance = new InventoryItemObserver();
    public override void OnNotify(object @event)
    {
        if(@event is OnInvSlotUpdate)
        {
            var eventData = (OnInvSlotUpdate)@event;
            if(eventData.slotID != -1) // temp
            {
                var inventoryMainController = GameObject.Find("InventoryUI").GetComponent<InventoryController>();
                inventoryMainController.FinalSlotUpdate();
            }
        }       
    }

}
