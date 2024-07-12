using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using Game.Manager;
using Core.AssetBundle;
using Game.Manager.Data;

public class GameContext : IContext
{
    MonoBehaviour mCoroutineOwner;

    public GameControllerManager GameCtrlManager    { get; private set; }
    public AssetBundleManager ABManager             { get; private set; }

    public GameObject GamePrefab                    { get; set; } = null; 
    public bool IsInitialized                       { get; private set; } = false;
    public ISlotControlData GameControlData         { get; set; }


    public IEnumerator Init(MonoBehaviour coroutineRunner)
    {
        mCoroutineOwner = coroutineRunner;

        GameCtrlManager = new GameControllerManager();
        ABManager = new AssetBundleManager();

        yield return mCoroutineOwner.StartCoroutine(ABManager.Init(mCoroutineOwner, useRemoteBundle:false, string.Empty));

        GameCtrlManager.Init(mCoroutineOwner, ABManager, useRemoteBundle:false);

        IsInitialized = true;

        yield break;
    }



   // public void Spin()
   // {
   //     EventSystem.DispatchEvent("Context_OnClickBtnSpin");
   // }
//
}
