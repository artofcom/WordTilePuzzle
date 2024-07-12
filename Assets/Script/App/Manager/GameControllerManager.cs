using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.AssetBundle;
using System;
using Game.Manager.Data;

namespace Game.Manager
{
    public class GameControllerManager
    {
        AssetBundle mControlBundle;
        AssetBundleManager mAssetBundleManager;
        MonoBehaviour mCoroutineOwner;

        Dictionary<string, SlotControlData> DictControlInfo = new Dictionary<string, SlotControlData>();

        string mLastLoadedGameKey = string.Empty;

        bool mIsUseRemoteBundle = false;
        const string BUNDLE_NAME = "C000_Controllers";

        // Public Funcs
        //
        public void Init(MonoBehaviour coroutineOwner, AssetBundleManager ABManager, bool useRemoteBundle)
        {
            mAssetBundleManager = ABManager;
            mIsUseRemoteBundle = useRemoteBundle;
            mCoroutineOwner = coroutineOwner;
        }

        public IEnumerator CoLoadControllerBundle(Action<bool> callbackDone)
        {
            // Should call this AFTER Init.
            UnityEngine.Assertions.Assert.IsTrue(mAssetBundleManager != null);
            UnityEngine.Assertions.Assert.IsTrue(mCoroutineOwner != null);

            if (!mIsUseRemoteBundle)
            {
                callbackDone.Invoke(true);
                yield break;
            }

            // Cache/Fetch latest version of Control Asset Bundle.
            yield return mCoroutineOwner.StartCoroutine(
                mAssetBundleManager.FetchBundle(BUNDLE_NAME,
                    (loadedBundle) =>
                    {
                        mControlBundle = loadedBundle;
                        callbackDone.Invoke(loadedBundle != null);
                    },
                    (progress) =>
                    {
                        // Downloading the Control Bundle..
                    })
                );
        }

        public SlotControlData GetControllerInfo(string controllerName)
        {
            string key = controllerName.ToLower();
            if (DictControlInfo.ContainsKey(key))
                return DictControlInfo[key];

            return null;
        }

        public SlotControlData GetCurrentControllerInfo()
        {
            if (string.IsNullOrEmpty(mLastLoadedGameKey))
                return null;

            return GetControllerInfo(mLastLoadedGameKey);
        }

        public IEnumerator CoLoadController(string strControllerName, Action<SlotControlData> callbackDone)
        {
            SlotControlData infoRet;
            infoRet = GetControllerInfo(strControllerName);
            if (infoRet != null)
            {
                if (callbackDone != null)
                    callbackDone.Invoke(infoRet);
                yield break;
            }

            string key = strControllerName.ToLower();

            // Should call this AFTER Init.
            UnityEngine.Assertions.Assert.IsTrue(mAssetBundleManager != null);
            UnityEngine.Assertions.Assert.IsTrue(mCoroutineOwner != null);

            if (mIsUseRemoteBundle)
            {
                UnityEngine.Assertions.Assert.IsTrue(mControlBundle != null);

                yield return mCoroutineOwner.StartCoroutine(
                    mAssetBundleManager.LoadAssetFromBundle<TextAsset>(mControlBundle, strControllerName,
                    (loadedText) =>
                    {
                        if (loadedText != null && !string.IsNullOrEmpty(loadedText.text))
                        {
                            infoRet = JsonUtility.FromJson<SlotControlData>(loadedText.text);
                            DictControlInfo[key] = infoRet;
                        }

                        mLastLoadedGameKey = infoRet.Key;

                        if (callbackDone != null)
                            callbackDone.Invoke(infoRet);
                    })
                );

            }
            else
            {
                yield return mCoroutineOwner.StartCoroutine(
                    mAssetBundleManager.LoadAssetBundle<TextAsset>(BUNDLE_NAME, strControllerName,
                    (loadedText) =>
                    {
                        if (loadedText != null && !string.IsNullOrEmpty(loadedText.text))
                        {
                            infoRet = JsonUtility.FromJson<SlotControlData>(loadedText.text);
                            DictControlInfo[key] = infoRet;
                        }
                        mLastLoadedGameKey = infoRet.Key;

                        if (callbackDone != null)
                            callbackDone.Invoke(infoRet);
                    },
                    (progress) => { })
                );
            }
        }
    }
}
