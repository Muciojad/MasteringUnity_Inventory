using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton for observing slots changes events.
/// </summary>
[System.Serializable]
public class SlotsManagerObserver : Observer
{
    public static SlotsManagerObserver instance = new SlotsManagerObserver();


    public delegate void ManageSelectsDelegate(int slotId);
    public ManageSelectsDelegate selectsDelegate;
    public delegate void UpdateUsefullnesDelegate(int slotId);
    public UpdateUsefullnesDelegate usefullnesDelegate;
    public override void OnNotify(object @event)
    {
        if (@event is OnSlotClick)
        {
            var eventData = (OnSlotClick)@event;
            selectsDelegate(eventData.slotId);
        }
        else if (@event is OnItemUsefullnessChange)
        {
            var eventData = (OnItemUsefullnessChange)@event;
            usefullnesDelegate(@eventData.slotId);
        }
    }


}

[System.Serializable]
public class InvUI_SlotsManager : MonoBehaviour
{
    [SerializeField] private InvUI_SlotController[] slots;
    public int Length { get { return slots.Length; } }
    void Start()
    {
        SlotsManagerObserver.instance.selectsDelegate += ManageSelects;
        SlotsManagerObserver.instance.usefullnesDelegate += UpdateSlot;
    }
    private void OnDestroy()
    {
        SlotsManagerObserver.instance.selectsDelegate -= ManageSelects;
        SlotsManagerObserver.instance.usefullnesDelegate -= UpdateSlot;
    }

    public Observer GetObserver()
    {
        return SlotsManagerObserver.instance;
    }

    public void UpdateSlot(int slotId)
    {
        var slotData = Inventory.instance.getSlotData(slotId);


        if (slotData != null)
        {
            slots[slotId].CheckIfSlotAvailable(slotData.slotAvailable);
            
            if (slotData.itemInfo != null)
            {
                if(slotData.itemInfo.ItemUnnecessary)
                {
                    slots[slotId].MarkUseless(true);
                    InUI_ItemInfo.detailsUpdater.ForceTextUpdate(slotId);

                    return;
                }   
                else
                {
                    InUI_ItemInfo.detailsUpdater.ForceTextUpdate(slotId);
                }
                slots[slotId].UpdateIcon(slotData.itemInfo.ItemName);
            }
            else
            {
                slots[slotId].MarkUseless(false);
                slots[slotId].ResetIcon();
            }
        }
        
    }


    private void ManageSelects(int clickedId)
    {
        for(int i = 0;i< slots.Length; i++)
        {
            if(clickedId != i)
            {
                slots[i].isButtonClicked = false;
            }
        }
    }

   
}
