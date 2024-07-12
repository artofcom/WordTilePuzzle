using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using System.Linq;
using Core.Utils;
using Game.Manager.Data;

public class BaseReelComponent : AReelComponent
{
    [SerializeField] public GameObject[] Symbols;
    [SerializeField] public ASingleReelComponent[] Reels;
    [SerializeField] public Transform SymbolPoolRoot;

    [SerializeField] public AnimationCurve StartCurve;
    [SerializeField] public AnimationCurve StopCurve;
    [SerializeField] public float MoveTimeToBottom = 1.0f;
    [SerializeField] public float StartMoveTimeToBottom = 1.5f;
    [SerializeField] public float StopMoveTimeToBottom = 1.5f;
    [SerializeField] public float SymbolZ = -5.0f;
    [SerializeField] public float SpinDuration = 10;
    [SerializeField] public float SpinDelayPerReel = 0.1f;
    [SerializeField] public float CutSymbolYOffset = -0.5f;

    GameContext _context;
    ISlotControlData mCtrlData;

    // Caches ----------------------------------------
    Dictionary<string, GameObjectPooler> DictSymbolPool = new Dictionary<string, GameObjectPooler>();
    List<List<string>> ReelSymbols = new List<List<string>>();


    //  Events ----------------------------------------
    EventsGroup Events = new EventsGroup();

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(Symbols);
        UnityEngine.Assertions.Assert.IsNotNull(Reels);
    }

    private void OnEnable()
    {
        EventSystem.DispatchEvent("ReelScreenView_OnEnable");
    }

    private void OnDisable()
    {
        EventSystem.DispatchEvent("ReelScreenView_OnDisable");
    }



    // Event Handler ----------------------------------------
    //
    //void Context_OnClickBtnSpin(object data)
    //{
    //    StartSpin();
    //}

    //void ReelScreenView_OnEnable(object data)
    public override void Init(IContext context, ISlotControlData ctrlData)
    {
        _context = context as GameContext;
        mCtrlData = ctrlData;

        InitEvents();

        InitSymbolPool();

        InitReels();
    }
    public override void StartSpin(List<List<string>> data)
    {
        StartCoroutine(CoSpin(data));
    }
    //
    //
    //


    void ReelScreenView_OnDisable(object data)
    {
        ClearCache();
    }




    // Init Related ----------------------------------------
    //
    void InitEvents()
    {
       // Events.RegisterEvent("Context_OnClickBtnSpin", Context_OnClickBtnSpin);
        Events.RegisterEvent("ReelScreenView_OnDisable", ReelScreenView_OnDisable);
    }
    void InitReels()
    {
        // Caching Reel Socket Positions.
        for (int q = 0; q < Reels.Length; ++q)
        {
            string reelSymbols = (mCtrlData as SlotControlData).Rule.Reels.PaidSpin[q];
            ReelSymbols.Add(reelSymbols.Split(',').ToList());

            // Init Reel Component.
            Reels[q].Init(this, q);
            
        }
    }


    // Spin Related ----------------------------------------
    //
   
    IEnumerator CoSpin(List<List<string>> reelStopSymbols)
    {
        //_view.Reels[0].StartSpin();
        //yield break;
        for (int reel = 0; reel < Reels.Length; ++reel)
        {
            int idxCut = UnityEngine.Random.Range(0, ReelSymbols[reel].Count);
            List<string> symbolsToStop = new List<string>();
            for (int q = 0; q < (mCtrlData as SlotControlData).Rule.RowCount; ++q)
            {
                int idxTarget = (q + idxCut) % ReelSymbols[reel].Count;
                symbolsToStop.Add(ReelSymbols[reel][idxTarget]);
            }
            //Reels[reel].StartSpin(symbolsToStop);

            Reels[reel].StartSpin(reelStopSymbols[reel]);
            yield return new WaitForSeconds(SpinDelayPerReel);
        }
    }


    // Private Helpers ----------------------------------------
    //
    void ClearCache()
    {
        for (int reel = 0; reel < Reels.Length; ++reel)
            Reels[reel].Clear();

        DictSymbolPool.Clear();
        ReelSymbols.Clear();

        Events.UnRegisterAll();
    }

    IEnumerator CoTriggerWithDelay(float delay, System.Action function)
    {
        yield return new WaitForSeconds(delay);

        function.Invoke();
    }


    #region Symbol Pool

    // Pool Related ----------------------------------------
    //
    public GameObject GetSymbolFromPool(Transform trParent, Vector3 vPos, string symbol)
    {
        int idx = -1;
        if (!string.IsNullOrEmpty(symbol))
        {
            for (int q = 0; q < Symbols.Length; ++q)
            {
                if (Symbols[q].name == symbol)
                {
                    idx = q;
                    break;
                }
            }
        }
        if (idx < 0)
            idx = UnityEngine.Random.Range(0, Symbols.Length);

        GameObject src = Symbols[idx];
        GameObject newSymbol = DictSymbolPool[src.name].Get();
        newSymbol.transform.position = vPos; // + Vector3.forward * _view.SymbolZ;
        newSymbol.transform.SetParent(trParent, true);
        newSymbol.SetActive(true);
        return newSymbol;
    }

    public void ReleaseSymbol(GameObject target)
    {
        if (DictSymbolPool.ContainsKey(target.name))
            DictSymbolPool[target.name].Release(target);
    }

    void InitSymbolPool()
    {
        DictSymbolPool.Clear();
        for (int q = 0; q < Symbols.Length; ++q)
        {
            GameObjectPooler pooler = new GameObjectPooler();
            pooler.Create(Symbols[q], SymbolPoolRoot);
            DictSymbolPool[Symbols[q].name] = pooler;
        }
    }
    #endregion
}
