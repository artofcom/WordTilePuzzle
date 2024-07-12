using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlay : IMVCS
{
    SlotPlayView _view;
    SlotPlayController _controller;
    GameContext _context;


    
    TopUIScreen _TopUIScreen;
    BottomUIScreen _BottomUIScreen;

    public SlotPlay(AView view, IContext context)
    {
        _view = view as SlotPlayView;
        _context = context as GameContext;
    }


    public void Initialize()
    {
        _controller = new SlotPlayController(_view, _context);


        _TopUIScreen = new TopUIScreen(_view.TopUIScreenView, _context);
        _TopUIScreen.Initialize();
        _BottomUIScreen = new BottomUIScreen(_view.BottomUIScreenView, _context);
        _BottomUIScreen.Initialize();
    }
}
