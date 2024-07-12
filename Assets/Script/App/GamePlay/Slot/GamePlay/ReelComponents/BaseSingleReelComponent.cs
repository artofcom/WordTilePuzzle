using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseSingleReelComponent : ASingleReelComponent
{
    // Serialize Field ----------------------------------------
    //
    [SerializeField] Transform SocketRoot;
    [SerializeField] Transform Reel0, Reel1;
    
    // public UnityEngine.Events.UnityEvent MyUnityEvent;


    // Inner class & enum ----------------------------------------
    //
    public class TweenerData
    {
        public Core.Tween.Mover Tweener;
        public int RowIndex;
    }
    enum SpinState { IDLE, SPIN, STOPPING, };


    // private members ----------------------------------------
    //
    List<Transform> ListSymbolSockets = new List<Transform>();
    Queue<GameObject> QSymbols = new Queue<GameObject>();
    
    List<string> StopSymbols;

    int ReelIndex;
    SpinState eSpinState = SpinState.IDLE;
    float SocketHeight = 10.0f;
    int StoppinCount = 0;
    float MoveDurationToBottom = 1.0f;
    float StartMoveDurationToBottom = 1.5f;
    float StopMoveDurationToBottom = 1.5f;
    float SpinDuration;
    float SpinTime;
    Transform CurrentReel;
    float ZPosOffset;
    float CutYOffset;
    GameObject RecentlyAddedSymbol;
    AnimationCurve StartCurve;
    AnimationCurve StopCurve;
    Func<Transform, Vector3, string, GameObject> CreateSymbol = null;
    Action<GameObject> ReleseSymbol = null;



    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(SocketRoot);
        UnityEngine.Assertions.Assert.IsNotNull(Reel0);
        UnityEngine.Assertions.Assert.IsNotNull(Reel1);
        UnityEngine.Assertions.Assert.IsTrue(SocketRoot.childCount >= 3);
    }

    // Builders.
    //
    public override void Init(object data, int reelIdx)
    {
        MonoBehaviour dataHolder = (MonoBehaviour)data;
        BaseReelComponent baseReel = dataHolder.GetComponent<BaseReelComponent>();

        CreateSymbol = baseReel.GetSymbolFromPool;
        ReleseSymbol = baseReel.ReleaseSymbol;

        BuildSpinCurve(baseReel.StartCurve, baseReel.StopCurve);
        BuildSpinInfo(baseReel.MoveTimeToBottom, baseReel.SpinDuration, baseReel.CutSymbolYOffset,
                                  baseReel.StartMoveTimeToBottom, baseReel.StopMoveTimeToBottom);
        BuildReel(reelIdx, baseReel.SymbolZ);
    }

    void BuildSpinCurve(AnimationCurve start, AnimationCurve end)
    {
        StartCurve = start;
        StopCurve = end;
    }

    void BuildSpinInfo(float moveDurationToBottom, float spinTotalDuration, float cutYOffset,
                              float startMoveDurationToBottom, float stopMoveDurationToBottom)
    {
        CutYOffset = cutYOffset;
        StartMoveDurationToBottom = startMoveDurationToBottom;
        StopMoveDurationToBottom = stopMoveDurationToBottom;
        MoveDurationToBottom = moveDurationToBottom;
        SpinDuration = spinTotalDuration;
    }

    void BuildReel(int idxReel, float zPosOffset)
    {
        ReelIndex = idxReel;
        
        for(int q = 0; q < SocketRoot.childCount; ++q)
            ListSymbolSockets.Add(SocketRoot.GetChild(q));
        
        // Top -> Down sort.       
        ListSymbolSockets.Sort((a, b) =>
        {
            if (a.localPosition.y > b.localPosition.y) return -1;
            else if (a.localPosition.y < b.localPosition.y) return 1;
            return 0;
        });

        SocketHeight = ListSymbolSockets[0].position.y - ListSymbolSockets[1].position.y;
        eSpinState = SpinState.IDLE;
        StoppinCount = 0;
        
        ZPosOffset = zPosOffset;

        if(Reel0.gameObject.GetComponent<Core.Tween.Mover>() == null)
            Reel0.gameObject.AddComponent(typeof(Core.Tween.Mover));
        if (Reel1.gameObject.GetComponent<Core.Tween.Mover>() == null)
            Reel1.gameObject.AddComponent(typeof(Core.Tween.Mover));

        Reel0.position = ListSymbolSockets[0].position;
        Reel1.position = ListSymbolSockets[0].position;
        CurrentReel = Reel0;

        for (int z = ListSymbolSockets.Count-1; z >= 0; --z)
        {
            Transform trSlot = ListSymbolSockets[z];
            var vPos = new Vector3(trSlot.position.x, trSlot.position.y, trSlot.position.z + ZPosOffset);
            

            RecentlyAddedSymbol = CreateSymbol(CurrentReel, vPos, string.Empty);
            QSymbols.Enqueue(RecentlyAddedSymbol);

            //if(ReelIndex == 0)
            //    Debug.Log($"Symbol [{RecentlyAddedSymbol.name}]-[{z}]-[{trSlot.localPosition.y}]");
        }
    }
    //

    public override void Clear()
    {
        while(QSymbols.Count > 0)
            ReleseSymbol(QSymbols.Dequeue());
        
        ListSymbolSockets.Clear();
    }





    public override void StartSpin(List<string> stopSymbols)
    {
        if (eSpinState != SpinState.IDLE)
            return;

        StopSymbols = new List<string>(stopSymbols);
        SpinTime = SpinDuration;
        eSpinState = SpinState.SPIN;
        TweenerData tweenerData = new TweenerData();
        tweenerData.Tweener = CurrentReel.GetComponent<Core.Tween.Mover>();
        tweenerData.RowIndex = -1;
        tweenerData.Tweener.TriggerWithCurve( StartCurve,
            tweenerData.Tweener.transform.localPosition,
            ListSymbolSockets[ ListSymbolSockets.Count - 1 ].localPosition,
            StartMoveDurationToBottom, tweenerData, OnReelReachedBottom);

        StartCoroutine(coUpdateSpin());
    }

    IEnumerator coUpdateSpin()
    {
        Transform trTopSocket = ListSymbolSockets[0];
        Transform trBottomSocket = ListSymbolSockets[ListSymbolSockets.Count - 1];
        while (eSpinState != SpinState.IDLE)
        {
            yield return null;

            SpinTime -= Time.deltaTime;
                
            GameObject objBottom = QSymbols.Peek();
            if (objBottom.transform.position.y < trBottomSocket.position.y+CutYOffset)
            {
                QSymbols.Dequeue();
                ReleseSymbol(objBottom);

                //if (ReelIndex == 0)
                //    Debug.Log($"Deleting Symbol..[{objBottom.name}]");

                var vPos = new Vector3( RecentlyAddedSymbol.transform.position.x, RecentlyAddedSymbol.transform.position.y + SocketHeight,
                                        RecentlyAddedSymbol.transform.position.z);
                string newSymbol = eSpinState == SpinState.STOPPING && StoppinCount < StopSymbols.Count ? StopSymbols[StoppinCount] : string.Empty;
                RecentlyAddedSymbol = CreateSymbol(CurrentReel, vPos, newSymbol);
                QSymbols.Enqueue(RecentlyAddedSymbol);

                //if (ReelIndex == 0)
                //    Debug.Log($"Adding Symbol..[{RecentlyAddedSymbol.name}]");

                //if (eSpinState == SpinState.STOPPING && ReelIndex == 0)
                //    Debug.Log($"Creating Stopping Symbol..[{RecentlyAddedSymbol.name}]");

                if(eSpinState == SpinState.STOPPING)
                    ++StoppinCount;
            }
        }
    }

    void OnReelReachedBottom(object data)
    {
        //if (ReelIndex == 0)
        //    Debug.Log("Replacing Reel....!");

        // Switch Reel.
        CurrentReel = CurrentReel == Reel0 ? Reel1 : Reel0;
        // Swith Parent.
        GameObject[] Symbols = QSymbols.ToArray();
        for(int q = 0; q < Symbols.Length; ++q)
        {
            Symbols[q].transform.SetParent(CurrentReel, true);
        }

        Reel0.position = ListSymbolSockets[0].position;
        Reel1.position = ListSymbolSockets[0].position;

        // Socket 0 ---> Reel0 or Reel
        // Socket 1
        // Socket 2
        // Socket 3
        // Socket 4
        //
        AnimationCurve movementCurve = null;
        Action<object> CallbackDone = OnReelReachedBottom;
        float duration = MoveDurationToBottom;
        if (SpinTime < .0f)
        {
            //if (ReelIndex == 0)
            //    Debug.Log("Stopping....!");

            eSpinState = SpinState.STOPPING;
            movementCurve = StopCurve;
            CallbackDone = OnSpinFinished;
            duration = StopMoveDurationToBottom;
        }
        
        TweenerData tweenerData = new TweenerData();
        tweenerData.Tweener = CurrentReel.GetComponent<Core.Tween.Mover>();
        tweenerData.RowIndex = -1;
        //tweenerData.Tweener.Trigger(tweenerData.Tweener.transform.localPosition,
        tweenerData.Tweener.TriggerWithCurve(movementCurve, tweenerData.Tweener.transform.localPosition, 
            ListSymbolSockets[ListSymbolSockets.Count - 1].localPosition,
            duration, tweenerData, CallbackDone);
    }

    void OnSpinFinished(object data)
    {
        eSpinState = SpinState.IDLE;
        StoppinCount = 0;

        // Switch Reel.
        CurrentReel = CurrentReel == Reel0 ? Reel1 : Reel0;
        // Swith Parent.
        GameObject[] Symbols = QSymbols.ToArray();
        for (int q = 0; q < Symbols.Length; ++q)
        {
            Symbols[q].transform.SetParent(CurrentReel, true);
        }

        Reel0.position = ListSymbolSockets[0].position;
        Reel1.position = ListSymbolSockets[0].position;
    }
}
