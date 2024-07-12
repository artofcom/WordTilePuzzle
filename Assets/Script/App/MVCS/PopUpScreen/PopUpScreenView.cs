using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class PopUpScreenView : AView
{
    [SerializeField] MessageDlgScreenView MessageDlgView;
    [SerializeField] OptionDlgScreenView OptionDlgView;


    EventsGroup Events = new EventsGroup();

    // Start is called before the first frame update
    void Start()
    {
        MessageDlgView.gameObject.SetActive(false);
        OptionDlgView.gameObject.SetActive(false);

        Events.RegisterEvent("LobbyScreenView_OnBtnOptionClicked", OnBtnOptionClicked);
        Events.RegisterEvent("PlayScreenView_OnBtnOptionClicked", OnBtnOptionClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnBtnOptionClicked(object data)
    {
        OptionDlgView.gameObject.SetActive(true);
    }
}
