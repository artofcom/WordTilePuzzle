using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScreen : IMVCS
{
    PopUpScreenView _view;
    GameContext _context;

    public PopUpScreen(AView view, IContext context)
    {
        _view = view as PopUpScreenView;
        _context = context as GameContext;
    }


    public void Initialize()
    {

    }
}
