using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    /// <summary>
    /// Observer class. Waits for OnItemUsed event and then fires delegate
    /// </summary>
    private class GrantAccessObserver : Observer
    {
        private string _wantedItem;
        private string _currentItem;

        /// <summary>
        /// Delegate used for invoking specific functionality inside event handler
        /// </summary>
        public delegate void UnlockingDelegate();
        public UnlockingDelegate unlockingDelegate;

        public void SetUnlockingItem(string name)
        {
            _wantedItem = name;
        }
        /// <summary>
        /// Catch for event and perform some actions.
        /// </summary>
        /// <param name="event"></param>
        public override void OnNotify(object @event)
        {
            
            if(@event is OnItemUsed)
            {

                var eventData = (OnItemUsed)@event;
                _currentItem = eventData.ItemName;
                if(_currentItem.CompareTo(_wantedItem) == 0)
                {
                    unlockingDelegate();
                }
            }
        }

       
    }
    /// <summary>
    /// Internal subject class used for sending events to UI listeners.
    /// </summary>
    private class UiUsageWaiterNotifier : Subject
    {
        
    }private UiUsageWaiterNotifier waiterNotifier = new UiUsageWaiterNotifier();

    public bool DoorsLocked => doorsLocked;

    [SerializeField] private string unlockingItem;
    private bool doorsLocked = true;

    private GrantAccessObserver accessObserver = new GrantAccessObserver();
    private DoubleSlidingDoorController doorController;

    public Observer GetObserver()
    {
        return accessObserver;
    }

    void Start()
    {
        accessObserver.SetUnlockingItem(unlockingItem);
        accessObserver.unlockingDelegate += UnlockDoors;

        var uiObserverGO = GameObject.Find("WaitingForItemUse");
        waiterNotifier.AddObserver(uiObserverGO.GetComponent<UI_UsageWaiter>().GetObserver());

        doorController = GetComponent<DoubleSlidingDoorController>();
    }

    private void OnDestroy()
    {
        accessObserver.unlockingDelegate -= UnlockDoors;
    }


    private void UnlockDoors()
    {
        doorsLocked = false;
        waiterNotifier.Notify(new OnItemUsed() { ItemName = unlockingItem });

        doorController.ForceOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(doorsLocked && Inventory.instance.ContainsItem(unlockingItem))
        {
            waiterNotifier.Notify(new OnItemReadyToBeUsed() { ItemName = unlockingItem });
        }
    }
}
