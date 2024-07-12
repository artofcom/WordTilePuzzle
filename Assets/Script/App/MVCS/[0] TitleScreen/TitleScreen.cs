using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : IMVCS
{
    AView _view;
    GameContext _context;
    TitleScreenController _controller;

    public TitleScreen(AView view, IContext context)
    {
        _view = view;
        _context = context as GameContext;
    }


    public void Initialize()
    {
        _controller = new TitleScreenController(_view, _context);

    }
}
