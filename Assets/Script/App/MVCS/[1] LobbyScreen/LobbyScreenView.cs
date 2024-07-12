using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class LobbyScreenView : AView
{
    // Start is called before the first frame update
    void Start()
    {
        
    }




    public void OnBtnPlayClicked()
    {
        EventSystem.DispatchEvent("LobbyScreenView_OnBtnPlayClicked");
    }

    public void OnBtnDailyChallengeClicked()
    {
        Debug.Log("DC Clicked.");
    }

    // Top Hud
    public void OnBtnBackClicked()
    {
        EventSystem.DispatchEvent("LobbyScreenView_OnBtnBackClicked");
    }

    public void OnBtnOptionClicked()
    {
        EventSystem.DispatchEvent("LobbyScreenView_OnBtnOptionClicked");
    }

    public void OnBtnBuyClicked()
    {
        Debug.Log("Buy Clicked.");
    }

    public void OnBtnXPClicked()
    {
        Debug.Log("XP Clicked.");
    }

    public void OnBtnChipClicked()
    {
        Debug.Log("Chip Clicked.");
    }
}
