using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using Slot;

public class PlayScreenController : IController
{
    PlayScreenView _view;
    GameContext _context;

    AView _gamePlayView;
    IController _gamePlayController;

    //  Events ----------------------------------------
    //
    EventsGroup Events = new EventsGroup();

    public PlayScreenController(PlayScreenView view, GameContext context)
    {
        _view = view;
        _context = context;

        Events.RegisterEvent("PlayScreenView_OnEnable", PlayScreenView_OnEnable);
        Events.RegisterEvent("PlayScreenView_OnDisable", PlayScreenView_OnDisable);
    }

    void PlayScreenView_OnEnable(object data)
    {
        _view.Init(_context.GamePrefab, _context);

        // _context.GameType ???
        _gamePlayController = new WordTileSearchController(_view.GamePlayView, _context);


        //Init(_view.ReelComponent);
    }

    void PlayScreenView_OnDisable(object data)
    {
    }

    void Init(AReelComponent reelComponent)
    {
        // read data.
        //mGame = new BaseGame();
        //mGame.Init(_context.GameControlData);

        // based on the data.
        //ReelComponent = reelComponent;
        //FeatureComponent = new EmptyFeatureComponent();
        //EvaluatorComponent = new DefaultEvaluatorComponent();
        //
    }
}
