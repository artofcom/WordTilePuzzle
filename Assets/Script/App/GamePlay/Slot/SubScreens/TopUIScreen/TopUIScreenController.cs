using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUIScreenController : IController
{
    TopUIScreenView _view;
    GameContext _context;

    public TopUIScreenController(TopUIScreenView view, GameContext context)
    {
        _view = view;
        _context = context;
    }
}
