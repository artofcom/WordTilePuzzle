using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTileSearchController : IController
{
    WordTileSearchView _view;
    WordTileSearchModel _model;
    GameContext _context;

    public WordTileSearchController(AView view, IContext context)
    {
        _view = view as WordTileSearchView;
        _context = context as GameContext;

        _model = new WordTileSearchModel();


        _view.Init();
    }
}
