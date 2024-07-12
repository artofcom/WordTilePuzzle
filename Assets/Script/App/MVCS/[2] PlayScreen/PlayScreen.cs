using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScreen : IMVCS
{
    PlayScreenView _view;
    PlayScreenController _controller;
    GameContext _context;


    //TopUIScreen _TopUIScreen;
    //BottomUIScreen _BottomUIScreen;

    public PlayScreen(AView view, IContext context)
    {
        _view = view as PlayScreenView;
        _context = context as GameContext;
    }


    public void Initialize()
    {
        // _context.Gametype == something...
        //_controller = new WordTileSearchController(_view, _context);
        //else
        //_controller = new XXXController...();

        _controller = new PlayScreenController(_view, _context);

        //_TopUIScreen = new TopUIScreen(_view.TopUIScreenView, _context);
        //_TopUIScreen.Initialize();
        //_BottomUIScreen = new BottomUIScreen(_view.BottomUIScreenView, _context);
        //_BottomUIScreen.Initialize();
    }
}
