using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class TitleScreenView : AView
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickBtnStart()
    {
        EventSystem.DispatchEvent("TitleScreenView_OnClickBtnStart");
    }
}
