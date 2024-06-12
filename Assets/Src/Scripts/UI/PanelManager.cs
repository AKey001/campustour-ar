using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject parent;
    public GameObject background;
    public GameObject popup;
    

    public void Open()
    {
        parent.SetActive(true);
        background.LeanAlpha(1f, 0.05f).setEaseInSine();
        popup.LeanScale(Vector3.one, 0.25f).setEaseInQuart();
    }

    public void Close()
    {
        popup.LeanScale(Vector3.zero, 0.25f).setEaseInQuart().setOnComplete(o =>
        {
            parent.SetActive(false); 
        });
        background.LeanAlpha(0f, 0.05f).setEaseOutSine().setOnComplete(() =>
        {
            
        });
    }
}
