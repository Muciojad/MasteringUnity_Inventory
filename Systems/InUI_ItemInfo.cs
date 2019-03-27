using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the UI view of item details.
/// </summary>
public class InUI_ItemInfo : MonoBehaviour
{
    [SerializeField] TMP_Text[] itemTexts;
    [SerializeField] private string throwMessage = "[H] throw";
    [SerializeField] private string useMessage = "[F] use";
    public static ItemDetailsUpdater detailsUpdater = new ItemDetailsUpdater();

    private void Awake()
    {        
        detailsUpdater.clickEvent += SetTexts;
        detailsUpdater.clearEvent += ClearTexts;
        
    }
    private void OnEnable()
    {
        foreach (var item in itemTexts)
        {
            item.text = "";
        }
    }
    
    public void ClearTexts()
    {
        SetTexts(null);
    }

    /// <summary>
    /// Gets item data and displaying it's details on the UI.
    /// </summary>
    /// <param name="slotData"></param>
    private void SetTexts(InventorySlot slotData)
    {
        if(slotData != null && slotData.itemInfo != null)
        {
            if (slotData.itemInfo.StackingEnabled)
            {
                itemTexts[0].text = $"{slotData.itemInfo.ItemName} x{slotData.itemInfo.StackCount}";
            }
            else itemTexts[0].text = $"{slotData.itemInfo.ItemName} ";
            itemTexts[1].text = slotData.itemInfo.ItemDescription;
            itemTexts[2].text = throwMessage;
            itemTexts[3].text = useMessage;
            itemTexts[3].enabled = slotData.itemInfo.ManualUseAllowed;

            if(slotData.itemInfo.ItemUnnecessary)
            {
                itemTexts[3].text = "";
                itemTexts[1].text += "(useless)";
            }
        }
        else
        {
            foreach (var item in itemTexts)
            {
                item.text = "";
            }
        }
        
    }

    /// <summary>
    /// UI details view listener.
    /// Waits untill UI events(clicks on slots, throwing items).
    /// </summary>
    public class ItemDetailsUpdater : Observer
    {
        public delegate void ClickEvent(InventorySlot slotData);
        public ClickEvent clickEvent;

        public delegate void ClearEvent();
        public ClearEvent clearEvent;

        
        public void ForceTextUpdate(int slotId)
        {
            clickEvent(Inventory.instance.getSlotData(slotId));
        }

        public override void OnNotify(object @event)
        {
            if (@event is OnSlotClick)
            {
                var slotClick = (OnSlotClick)@event;                
                clickEvent(Inventory.instance.getSlotData(slotClick.slotId));
            }
            else if(@event is OnItemThrow)
            {
                clearEvent();
            }

        }
    }
}
