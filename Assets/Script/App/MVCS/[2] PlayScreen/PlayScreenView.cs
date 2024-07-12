using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;

public class PlayScreenView : AView
{
    [SerializeField] Transform GamePlayRoot;

    public AView GamePlayView { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(GameObject gamePrefab, GameContext context)
    {
        GameObject objGamePlay = Instantiate(gamePrefab, GamePlayRoot);

        GamePlayView = objGamePlay.GetComponent<AView>();

        //SlotMain = objSlot.GetComponent<SlotMainComponent>();
        //SlotMain.Init(context);
    }

    private void OnEnable()
    {
        EventSystem.DispatchEvent("PlayScreenView_OnEnable");
    }

    private void OnDisable()
    {
        EventSystem.DispatchEvent("PlayScreenView_OnDisable");
    }
}
