using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple controller for UI text label displaying short messages.
/// </summary>
public class Ui_InfoLine : MonoBehaviour
{
    public static Ui_InfoLine instance;

    private TMPro.TMP_Text renderText;
    private void Awake()
    {
        instance = this;
        renderText = GetComponent<TMPro.TMP_Text>();
        renderText.enabled = false;
    }
    public void ShowMessage( string message )
    {
        if(!renderText.enabled)
        {
            renderText.text = message;
            renderText.enabled = true;
            Invoke("HideLine", 5f);
        }
        else
        {
            renderText.text = "";
            renderText.enabled = false;
        }

    }

    void HideLine()
    {
        ShowMessage("");
    }

    
}
