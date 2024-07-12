using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUIScreen : IMVCS
{
    TopUIScreenView _view;
    TopUIScreenController _controller;
    GameContext _context;

    public TopUIScreen(TopUIScreenView view, GameContext context)
    {
        _view = view;
        _context = context;
    }

    public void Initialize()
    {
        _controller = new TopUIScreenController(_view, _context);
    }
}
