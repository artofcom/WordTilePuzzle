using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomUIScreen : IMVCS
{
    BottomUIScreenView _view;
    BottomUIScreenController _controller;
    GameContext _context;

    public BottomUIScreen(BottomUIScreenView view, GameContext context)
    {
        _view = view;
        _context = context;
    }

    public void Initialize()
    {
        _controller = new BottomUIScreenController(_view, _context);
    }
}
