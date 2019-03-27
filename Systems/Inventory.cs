using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main class of Inventory system.
/// Keeps List of InventorySlot inside with items data.
/// Deals with adding, removing and merging items.
/// </summary>

[System.Serializable]
public class Inventory :Observer
{
    public static Inventory instance = new Inventory();
    public int Capacity = 50;
    public int FreeSpace = 50;

    private List<InventorySlot> inventoryBag = new List<InventorySlot>();

    private Dictionary<int, int> bagUpgradeLevels = new Dictionary<int, int>();
    private int currentUpgradeLevel = 1;

    public Inventory()
    {
        bagUpgradeLevels.Add(1, 4);
        bagUpgradeLevels.Add(2, 6);
        bagUpgradeLevels.Add(3, 8);

        InventoryInternal_ThrowRubbishObserver.instance.throwItemDelegate += RemoveItemFromInventory;
        for(int i = 0; i< 8; i++)
        {
            inventoryBag.Add(new InventorySlot() { itemInfo = null});
        }
        UnlockSlots();

    }

    /// <summary>
    /// Return item in specified slot.
    /// </summary>
    /// <param name="slotId"></param>
    /// <returns></returns>
    public InventorySlot getSlotData(int slotId)
    {
        if (inventoryBag[slotId] is null) return null;
        return inventoryBag[slotId];
    }

    /// <summary>
    /// Check if item is already in inventory.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public bool ContainsItem(string itemName)
    {
        foreach(var slot in inventoryBag)
        {
            if(slot.itemInfo != null)
            {
                if(slot.itemInfo.ItemName.CompareTo(itemName) == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Checks if item is already in inventory and if so, at what index.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns>SlotId or -1</returns>
    public int ContainsItemInSlot(string itemName)
    {
        int slotId = 0;
        foreach (var slot in inventoryBag)
        {
            if (slot.itemInfo != null)
            {

                if (slot.itemInfo.ItemName.CompareTo(itemName) == 0)
                {
                    return slotId;
                }
            }
            slotId++;
        }
        return -1;
    }

    /// <summary>
    /// Listener for basic events.
    /// </summary>
    /// <param name="event"></param>
    public override void OnNotify(object @event)
    {
        if(@event is OnItemCollectedEvent)
        {

            var eventData = (OnItemCollectedEvent)@event;

            AddItemToInventory(eventData.itemData);

        }
        else if(@event is OnSlotsMerge)
        {
            var eventData = (OnSlotsMerge)@event;
            var mergeId = SearchForItemInBag(inventoryBag[eventData.slotA].itemInfo, eventData.slotA);
            if(mergeId != -1)
            {
                MergeSlots(eventData.slotA, mergeId);
            }
        }      
    }
    /// <summary>
    /// Merge slots with the same items if possible.
    /// </summary>
    /// <param name="slotA"></param>
    /// <param name="slotB"></param>
    private void MergeSlots(int slotA, int slotB)
    {
        var sum = inventoryBag[slotA].itemInfo.StackCount + inventoryBag[slotB].itemInfo.StackCount;
        var targetId = -1;
        var throwId = -1;
        if (sum <= inventoryBag[slotA].itemInfo.MaxStackCount)
        {
            targetId = slotA;
            throwId = slotB;
        }
        else if (sum <= inventoryBag[slotB].itemInfo.MaxStackCount)
        {
            targetId = slotB;
            throwId = slotA;
        }

        if (targetId != -1)
        {
            inventoryBag[targetId].itemInfo.StackCount = sum;
            RemoveItemFromInventory(throwId);
        }
        else throw new System.IndexOutOfRangeException("Slots overloaded");
    }

    /// <summary>
    /// Unlock locked slots and mark them as available to use.
    /// </summary>
    private void UnlockSlots()
    {
        for(int i = 0; i< inventoryBag.Count; i++)
        {
            if(i < bagUpgradeLevels[currentUpgradeLevel])
            {
                inventoryBag[i].MarkAvailable(true);
            }
            else inventoryBag[i].MarkAvailable(false);
        }
    }

    /// <summary>
    /// Upgrade capacity of bag.
    /// </summary>
    public void UpgradeBag()
    {
        if (currentUpgradeLevel < 3) currentUpgradeLevel++;
        UnlockSlots();
        InventoryUiNotifier.instance.Notify(new OnInvSlotUpdate() { slotID = 4 });
        InventoryUiNotifier.instance.Notify(new OnInvSlotUpdate() { slotID = 5 });
    }

    /// <summary>
    /// Search for specific item in inventory, excluding specific index.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="excludeId"></param>
    /// <returns></returns>
    private int SearchForItemInBag(ItemClass item, int excludeId)
    {
        int slotId = 0;
        foreach (var slot in inventoryBag)
        {
            if (slot.itemInfo != null)
            {
                if(slot.itemInfo.ItemName.CompareTo(item.ItemName) == 0 && slotId != excludeId)
                {
                    return slotId;
                }
            }
            slotId++;
        }
        return -1;
    }

    /// <summary>
    /// Stack the same items.
    /// </summary>
    /// <param name="slotId"></param>
    /// <param name="amount"></param>
    private void StackItemInSlot(int slotId, int amount)
    {
        inventoryBag[slotId].itemInfo.StackCount += amount;
    }

    /// <summary>
    /// Add item to inventory. If item has stacking enabled, stack item if possible.
    /// If no, find first free slot and put item inside.
    /// If there is no avaliable slot, nothing happens.
    /// </summary>
    /// <param name="item"></param>
    void AddItemToInventory(ItemClass item)
    {
        if(item.StackingEnabled)
        {
            var stackSlotId = SearchForItemInBag(item,-1);
            if(stackSlotId != -1)
            {
                if(inventoryBag[stackSlotId].itemInfo.StackCount < inventoryBag[stackSlotId].itemInfo.MaxStackCount)
                {
                    //can definitely stack there
                    StackItemInSlot(stackSlotId, item.StackCount);
                    return;
                }
                else
                {
                    //get free slot
                    PutItemIntoFreeSlot(item);
                    return;
                }
            }
            else
            {
                PutItemIntoFreeSlot(item);
                return;
            }
        }
        else
        {
            PutItemIntoFreeSlot(item);
            return;
        }
        
    }

    /// <summary>
    /// Put item inside first available slot.
    /// </summary>
    /// <param name="itemToPut"></param>
    private void PutItemIntoFreeSlot(ItemClass itemToPut)
    {
        int slotId = 0;

        foreach (var slot in inventoryBag)
        {
            if (slot.itemInfo == null)
            {
                itemToPut.SetIdInInventory(slotId);

                slot.itemInfo = ItemClass.DeepClone<ItemClass>(itemToPut); 

                InventoryUiNotifier.instance.Notify(new OnInvSlotUpdate() { slotID = slotId });
                return;
            }
            slotId++;
        }
    }
    /// <summary>
    /// Checks if there is any empty slot in inventory.
    /// </summary>
    /// <returns></returns>
   public bool IsFreeSlotAvailable()
    {
        foreach(var slot in inventoryBag)
        {
            if (slot.itemInfo is null && slot.slotAvailable) return true;
        }
        return false;
    }

    /// <summary>
    /// Remove item from specified slot.
    /// </summary>
    /// <param name="itemId"></param>
    void RemoveItemFromInventory(int itemId)
    {
        if(inventoryBag[itemId] == null)
        {
            return;
        }
        else
        {
            inventoryBag[itemId].itemInfo = null;
            inventoryBag[itemId].MarkAvailable(true);
            InventoryUiNotifier.instance.Notify(new OnInvSlotUpdate() { slotID = itemId });
        }
    }
    /// <summary>
    /// For future use - calculate free space in inventory.
    /// </summary>
    void CalculateSpace()
    {
        int weightSum = 0;
        foreach(var slot in inventoryBag)
        {
            if(slot != null)
            {
                weightSum += slot.itemInfo.ItemWeight;
            }
        }
        FreeSpace = Capacity - weightSum;
    }

    [System.Serializable]
    private class InventoryUiNotifier :Subject
    {
        public static InventoryUiNotifier instance = new InventoryUiNotifier();
        public InventoryUiNotifier()
        {
            AddObserver(InventoryItemObserver.instance);
        }
    }
    /// <summary>
    /// Listen for item throwing event and fire delegate.
    /// </summary>
    public class InventoryInternal_ThrowRubbishObserver : Observer
    {
        public static InventoryInternal_ThrowRubbishObserver instance = new InventoryInternal_ThrowRubbishObserver();
        public delegate void ThrowItemDelegate(int slotId);
        public ThrowItemDelegate throwItemDelegate;
        public override void OnNotify(object @event)
        {
            if(@event is OnItemThrow)
            {
                var eventData = (OnItemThrow)@event;
                throwItemDelegate(eventData.slotId);
            }
        }       

    }
}
