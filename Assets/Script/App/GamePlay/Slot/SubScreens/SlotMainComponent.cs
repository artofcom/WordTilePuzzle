using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMainComponent : MonoBehaviour
{
    [SerializeField] AReelComponent ReelComp;

    GameContext _context;


    public AReelComponent GetReelComponent()
    {
        return ReelComp;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(ReelComp);
    }

    public void Init(GameContext context)
    {
        //_context = context;

        //ReelScreen reelScreen = new ReelScreen(ReelView, _context);
        //reelScreen.Initialize();
    }
}
