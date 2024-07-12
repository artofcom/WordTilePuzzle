using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class SlotPlayView : AView
{   
    [SerializeField] public TopUIScreenView TopUIScreenView;
    [SerializeField] public BottomUIScreenView BottomUIScreenView;

    [HideInInspector] public AReelComponent ReelComponent;

    SlotMainComponent SlotMain = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(GameObject gamePrefab, GameContext context)
    {
        GameObject objSlot = Instantiate(gamePrefab, transform);

        SlotMain = objSlot.GetComponent<SlotMainComponent>();
        SlotMain.Init(context);

        ReelComponent = SlotMain.GetReelComponent();
        ReelComponent.Init(context, context.GameControlData);
    }

    private void OnEnable()
    {
        EventSystem.DispatchEvent("PlayScreenView_OnEnable");
    }

    private void OnDisable()
    {
        if(SlotMain != null)
            Destroy(SlotMain.gameObject);

        EventSystem.DispatchEvent("PlayScreenView_OnDisable");
    }

    // Top Hud
    public void OnBtnBackClicked()
    {
        EventSystem.DispatchEvent("PlayScreenView_OnBtnBackClicked");
    }

    public void OnBtnOptionClicked()
    {
        EventSystem.DispatchEvent("PlayScreenView_OnBtnOptionClicked");
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

    // Bottom Hud
    public void OnBtnDailyChallengeClicked()
    {
        Debug.Log("DC Clicked.");
    }

    public void OnBtnSpinClicked()
    {
        Debug.Log("Spin Clicked.");
        EventSystem.DispatchEvent("PlayScreenView_OnBtnSpinClicked");
    }

    public void OnBtnBetUpClicked()
    {
        Debug.Log("BetUp Clicked.");
    }

    public void OnBtnBetDownClicked()
    {
        Debug.Log("BetDown Clicked.");
    }

    public void OnBtnInfoClicked()
    {
        Debug.Log("Info Clicked.");
    }

    public void OnBtnTurboClicked()
    {
        Debug.Log("Turbo Clicked.");
    }
}
