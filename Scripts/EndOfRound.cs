using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndOfRound : MonoBehaviour
{
    public static EndOfRound manager;
    public static Canvas     canva;
    
    private GameObject  shopDisplay;
    private GameObject  pauseDisplay;
    private GameObject  shopObj;
    private ShopHandler shop;

    public int costOfRepair { get; private set; }
    public bool isPaused    { get; set; }

    void Awake()
    {
        if ( manager == null) {
            manager = this;
        } else {
            Destroy( this );
        }

        canva = this.GetComponent<Canvas>();
        canva.enabled = false;

        shopDisplay  = GameObject.FindGameObjectWithTag("Shop");
        pauseDisplay = GameObject.FindGameObjectWithTag("Pause");
        shopObj      = GameObject.FindGameObjectWithTag("ShopHandler");
        shop         = shopObj.GetComponent<ShopHandler>();
        costOfRepair = 0;
        shopDisplay.GetComponentInParent<Canvas>().enabled = false;
        pauseDisplay.GetComponentInParent<Canvas>().enabled = false;
    }

    public void SetCanvas( bool en )
    {
        Time.timeScale = 0;
        canva.enabled  = en;
    }

    public void ToggleShop()
    {
        shop.UpdateCost();
        pauseDisplay.GetComponentInParent<Canvas>().enabled = false;
        shopDisplay.GetComponentInParent<Canvas>().enabled = true;
        isPaused = true;
    }

    public void TogglePause()
    {
        shopDisplay.GetComponentInParent<Canvas>().enabled = false;
        pauseDisplay.GetComponentInParent<Canvas>().enabled = true;
        isPaused = true;
    }

    public void AddRepairCost( int mny )
    {
        costOfRepair += mny;
    }
}
