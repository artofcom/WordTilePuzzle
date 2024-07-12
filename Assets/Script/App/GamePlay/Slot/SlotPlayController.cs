using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using Slot;

public class SlotPlayController : IController
{
    SlotPlayView _view;
    GameContext _context;

    EventsGroup Events = new EventsGroup();

    BaseGame mGame;

    public AReelComponent ReelComponent         { get; set; } 
    public IFeatureComponent FeatureComponent   { get; set; }
    public IEvaluatorComponent EvaluatorComponent { get; set; }

    public SlotPlayController(SlotPlayView view, GameContext context)
    {
        _view = view;
        _context = context;

        Events.RegisterEvent("PlayScreenView_OnEnable", PlayScreenView_OnEnable);
        Events.RegisterEvent("PlayScreenView_OnDisable", PlayScreenView_OnDisable);
        Events.RegisterEvent("PlayScreenView_OnBtnSpinClicked", PlayScreenView_OnBtnSpinClicked);
    }

    void PlayScreenView_OnEnable(object data)
    {
        _view.Init(_context.GamePrefab, _context);

        Init(_view.ReelComponent);
    }

    void PlayScreenView_OnDisable(object data)
    {
        ReelComponent   = null;
        FeatureComponent= null;
        EvaluatorComponent = null;
    }

    void PlayScreenView_OnBtnSpinClicked(object data)
    {
        List<int> reelStopIndices;

        mGame.Spin(out reelStopIndices);

        mGame.Judge(reelStopIndices);



        //
        List<List<string>> vReelStopSymbols = new List<List<string>>();
        for (int k = 0; k < reelStopIndices.Count; ++k)
            vReelStopSymbols.Add( mGame.ReelStopIndexToString(k, reelStopIndices[k], 3) );
        
        StartSpin(vReelStopSymbols);
    }

    void Init(AReelComponent reelComponent)
    {
        // read data.
        mGame = new BaseGame();
        mGame.Init(_context.GameControlData);

        // based on the data.
        ReelComponent = reelComponent;
        FeatureComponent = new EmptyFeatureComponent();
        EvaluatorComponent = new DefaultEvaluatorComponent();
        //
    }


    public void StartSpin(List<List<string>> vReelStopSymbols)
    {
        UnityEngine.Assertions.Assert.IsNotNull(ReelComponent);
        UnityEngine.Assertions.Assert.IsNotNull(FeatureComponent);
        UnityEngine.Assertions.Assert.IsNotNull(EvaluatorComponent);

        EvaluatorComponent.Stop();
        ReelComponent.StartSpin(vReelStopSymbols);
    }

    void PlayScreenView_OnReelStopped(object data)
    {
        _view.StartCoroutine(CoEvaluation());
    }

    IEnumerator CoEvaluation()
    {
        yield return _view.StartCoroutine(FeatureComponent.Run());

        yield return _view.StartCoroutine(EvaluatorComponent.Run());
    }
}
