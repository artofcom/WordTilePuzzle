using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : IController
{
    AView _view;
    GameContext _context;

    public TitleScreenController(AView view, GameContext ctx)
    {
        _view = view;
        _context = ctx;



        _view.StartCoroutine(coInit());
    }




    IEnumerator coInit()
    {
        yield return new WaitUntil(() => _context.IsInitialized == true);

        _view.StartCoroutine(_context.GameCtrlManager.CoLoadControllerBundle(
            (successed) =>
            {
                UnityEngine.Assertions.Assert.IsTrue(successed == true);
                // IsConfigLoaded = true;

            }));
    }
}
