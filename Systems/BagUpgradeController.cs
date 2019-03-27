using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When player enters the trigger, try to upgrade bag and send notification to listeners.
/// </summary>
public class BagUpgradeController : MonoBehaviour
{
    [SerializeField] private string UpgradeShortMessage;
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.CompareTo("Player") == 0)
        {
            Inventory.instance.UpgradeBag();
            Ui_InfoLine.instance.ShowMessage(UpgradeShortMessage);
            gameObject.SetActive(false);
        }
    }
}
