using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class BottomUIScreenController : IController
{
    //  Events ----------------------------------------
    EventsGroup Events = new EventsGroup();

    BottomUIScreenView _view;
    GameContext _context;

    public BottomUIScreenController(BottomUIScreenView view, GameContext context)
    {
        _view = view;
        _context = context;

        //Events.RegisterEvent("PlayBottomUIScreenView_OnClickBtnSpin", PlayBottomUIScreenView_OnClickBtnSpin);
    }




    void PlayBottomUIScreenView_OnClickBtnSpin(object data)
    {
        //_context.Spin();
    }
}
