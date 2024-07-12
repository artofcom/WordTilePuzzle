using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreen : IMVCS
{
    AView _view;
    GameContext _context;
    LobbyScreenController _controller;

    public LobbyScreen(AView view, IContext context)
    {
        _view = view;
        _context = context as GameContext;
    }


    public void Initialize()
    {
        _controller = new LobbyScreenController(_view, _context);


    }
}
