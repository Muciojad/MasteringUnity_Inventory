using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waits for usage of specific item.
/// When item is ready to be used, displays instruction on the screen.
/// </summary>
public class UI_UsageWaiter : MonoBehaviour
{
    private class UiWaitForUseObserver : Observer
    {
        public string WaitingForItemName;

        public delegate void ShowText(bool show, string itemName);
        public ShowText showTextDelegate;
        

        public override void OnNotify(object @event)
        {
           if(@event is OnItemUsed)
            {

                var data = (OnItemUsed)@event;
                if(WaitingForItemName != null)
                {
                    if (WaitingForItemName.CompareTo(data.ItemName) == 0)
                    {
                        showTextDelegate(false, data.ItemName);
                    }
                }
                
            }

            if (@event is OnItemReadyToBeUsed)
            {

                var data = (OnItemReadyToBeUsed)@event;
               
                WaitingForItemName = data.ItemName;
                showTextDelegate(true, data.ItemName);                
            }
        }
    }private UiWaitForUseObserver useObserver = new UiWaitForUseObserver();

    [SerializeField] private string DisplayText;
    private TMPro.TMP_Text displayText;

    private string waitingForItem;
    public Observer GetObserver()
    {
        return useObserver;
    }
    void Start()
    {
        displayText = GetComponent<TMPro.TMP_Text>();
        useObserver.showTextDelegate += ShowText;
        
    }

    void ShowText(bool show, string itemName)
    {
        if (show)
        {
            displayText.text = $"{DisplayText} {itemName}";
            waitingForItem = itemName;
        }
        else displayText.text = DisplayText;


        displayText.enabled = show;        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && displayText.enabled)
        {
            var slotId = Inventory.instance.ContainsItemInSlot(waitingForItem);

            if(slotId != -1)
            {

                Inventory.instance.getSlotData(slotId).itemInfo.Use();
            }
        }
    }
}
