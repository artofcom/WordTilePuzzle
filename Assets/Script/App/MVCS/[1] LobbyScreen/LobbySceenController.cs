using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using Game.Manager.Data;

public class LobbyScreenController : IController
{
    LobbyScreenView _view;
    GameContext _context;

    //  Events ----------------------------------------
    //
    EventsGroup Events = new EventsGroup();


    string mGameMainPrefab;

    public LobbyScreenController(AView view, GameContext ctx)
    {
        _view = view as LobbyScreenView;
        _context = ctx;

        Events.RegisterEvent("LobbyScreenView_OnBtnPlayClicked", LobbyScreenView_OnBtnPlayClicked);
    }




    void LobbyScreenView_OnBtnPlayClicked(object data)
    {
        _view.StartCoroutine(coLoadGame());
    }

    IEnumerator coLoadGame()
    {
        // yield return _view.StartCoroutine(coLoadGameController());

        yield return _view.StartCoroutine(coLoadGameBundle());
    }


    IEnumerator coLoadGameBundle()
    {
        /*
        string bundleName = "G001_CobraHearts";
        string assetName = // "SlotMainPortrait.prefab";//
                           "SlotMain.prefab";
        string LOCAL_BUNDLE_PATH = "Assets/Bundles";
        string assetPathExt = $"{bundleName}/{assetName}";// + GetFileExtension(typeof(T));
        string externalizedAssetPath = $"{LOCAL_BUNDLE_PATH}/{assetPathExt}";
        Debug.Log("[Fetching] Loading locally..." + externalizedAssetPath);
        GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(externalizedAssetPath);
        */

        mGameMainPrefab = "G010_WordTileSearch/GameMain";

        GameObject prefab = Resources.Load($"Bundles/{mGameMainPrefab}") as GameObject;
        UnityEngine.Assertions.Assert.IsNotNull(prefab);

        _context.GamePrefab = prefab;
        yield return null;

        EventSystem.DispatchEvent("LobbyScreenView_OnGamePrefabLoaded");
    }

    /*
     * IEnumerator coLoadGameController()
    {
        // Load Controller config.
        //string controllerName = "G001_CobraHearts";// SurgeInfo.ControllerAsset;
        string controllerName = "G030_TripleGolds";// SurgeInfo.ControllerAsset;
        //if (true)  // for now -  !_context.BootStrap.setting.UseRemoteBundle)
       //     controllerName += ".json";

        bool isFinishLoadingConfig = false;
        yield return _view.StartCoroutine(_context.GameCtrlManager.CoLoadController(controllerName,
            (loadedInfo) =>
            {
                _context.GameControlData = loadedInfo as ISlotControlData;

                UnityEngine.Assertions.Assert.IsNotNull(loadedInfo);

                if (loadedInfo != null) Debug.Log("Controller Loaded Successfully!");
                else Debug.Log("Controller Load Failed!!!");


                Debug.Log($"Slot - {loadedInfo.DisplayName}");
                Debug.Log($"Reel - {loadedInfo.Rule.RowCount}X{loadedInfo.Rule.ColCount}");
                Debug.Log($"Paylines - {loadedInfo.Rule.Paylines[0]}");
                Debug.Log($"Reel - {loadedInfo.Rule.Reels.PaidSpin[0]}");
                Debug.Log($"ClientData - {loadedInfo.ClientData.Bundle}/{loadedInfo.ClientData.Prefab}");

                mGameMainPrefab = $"{loadedInfo.ClientData.Bundle}/{loadedInfo.ClientData.Prefab}";

                isFinishLoadingConfig = true;
            }));

        yield return new WaitUntil(() => isFinishLoadingConfig == true);
    }

    IEnumerator coLoadGameBundle()
    {
        /*
        string bundleName = "G001_CobraHearts";
        string assetName = // "SlotMainPortrait.prefab";//
                           "SlotMain.prefab";
        string LOCAL_BUNDLE_PATH = "Assets/Bundles";
        string assetPathExt = $"{bundleName}/{assetName}";// + GetFileExtension(typeof(T));
        string externalizedAssetPath = $"{LOCAL_BUNDLE_PATH}/{assetPathExt}";
        Debug.Log("[Fetching] Loading locally..." + externalizedAssetPath);
        GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(externalizedAssetPath);
        

    GameObject prefab = Resources.Load($"Bundles/{mGameMainPrefab}") as GameObject;
    UnityEngine.Assertions.Assert.IsNotNull(prefab);

        _context.GamePrefab = prefab;
        yield return null;

        EventSystem.DispatchEvent("LobbyScreenView_OnGamePrefabLoaded");
    }*/
}
