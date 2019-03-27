using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Slot controller.
/// Nofities about clicks on slot and throwing item from slot.
/// Deals with icon update.
/// </summary>
public class InvUI_SlotController : MonoBehaviour
{
        private class SlotClickNotifier :Subject
        {
            public SlotClickNotifier()
            {
                AddObserver(InUI_ItemInfo.detailsUpdater);
            
            }

         }private SlotClickNotifier clickNotifier;

        private class SlotThrowItem : Subject
        {
            public SlotThrowItem()
            {
                AddObserver(Inventory.InventoryInternal_ThrowRubbishObserver.instance);
                AddObserver(InUI_ItemInfo.detailsUpdater);
            }

        }private SlotThrowItem slotThrowItem;

    private Image itemIcon;
    private Image bgIcon;

    

    public bool isButtonClicked;

    [SerializeField] private int CorrespondingBagId;
    [SerializeField] private Color lockedSlotColor;
    private Color unlockedSlotColor;


    internal void ResetIcon()
    {       
       itemIcon.sprite = null;
       if (itemIcon.enabled) itemIcon.enabled = false;
    }
    internal void MarkUseless(bool isUseless)
    {
        if(isUseless) bgIcon.color = lockedSlotColor;
        else
        {
            bgIcon.color = unlockedSlotColor;
        }
    }
    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemIcon.enabled = false;
        bgIcon = GetComponent<Image>();
        unlockedSlotColor = bgIcon.color;
    }
    void Start()
    {
        clickNotifier = new SlotClickNotifier();
        clickNotifier.AddObserver(transform.parent.GetComponent<InvUI_SlotsManager>().GetObserver());

        slotThrowItem = new SlotThrowItem();
    }


    public void UpdateIcon(string itemName)
    {
        if(itemName != null)
        {
            itemIcon.sprite = InvUI_IconDispatcher.instance.GetIconFromList(InventoryUI_IconManager.instance.GetIconFromName(itemName));
            if (!itemIcon.enabled) itemIcon.enabled = true;
        }
        
        
    }

    public void CheckIfSlotAvailable(bool isAvailable)
    {
        if (isAvailable)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(itemIcon.sprite != null && isButtonClicked)
            {
                slotThrowItem.Notify(new OnItemThrow() { slotId = CorrespondingBagId });
            }
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (itemIcon.sprite != null && isButtonClicked)
            {
                if(Inventory.instance.getSlotData(CorrespondingBagId).itemInfo.ManualUseAllowed)
                {
                    Inventory.instance.getSlotData(CorrespondingBagId).itemInfo.Use();
                }
            }
        }
    }

    public void OnClick()
    {
        isButtonClicked = true;
        clickNotifier.Notify(new OnSlotClick() { slotId = CorrespondingBagId });
    }
    
}
