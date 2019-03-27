using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Structs representing events.
/// </summary>
public struct EventSystem { }
public struct OnItemCollectedEvent { public ItemClass itemData; }
public struct OnInvSlotUpdate { public int slotID; }
public struct OnSlotClick { public int slotId; }
public struct OnItemThrow { public int slotId; }
public struct OnBagUpgrade { public int slotId; }
public struct OnItemUsefullnessChange { public int slotId; }
public struct OnSlotsMerge { public int slotA; public int slotB; }
public struct OnItemUsed { public string ItemName; }
public struct OnItemReadyToBeUsed { public string ItemName; }
