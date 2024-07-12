using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class BottomUIScreenView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {}

    private void OnEnable()
    {
        EventSystem.DispatchEvent("PlayBottomUIScreenView_OnEnable");
    }
    private void OnDisable()
    {
        EventSystem.DispatchEvent("PlayBottomUIScreenView_OnDisable");
    }

    public void OnClickBtnSpin()
    {
        // EventSystem.DispatchEvent("PlayBottomUIScreenView_OnClickBtnSpin");
    }
}
