using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessDeviceController : MonoBehaviour
{
    [SerializeField] private AccessDevice accessDeviceData;
    [SerializeField] private GameObject unlockingDoors;
    void Start()
    {
        var observer = unlockingDoors.GetComponent<LockedDoorController>().GetObserver();
        accessDeviceData.SubscribeTo(observer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            CollectItem();
        }
    }


    void CollectItem()
    {
        if (Inventory.instance.IsFreeSlotAvailable())
        {
            accessDeviceData.Collect();
            gameObject.SetActive(false);
        }

    }

}
