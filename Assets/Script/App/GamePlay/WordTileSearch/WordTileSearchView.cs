using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTileSearchView : AView
{
    [SerializeField] TileAreaProc TileAreaProcessor;

    const int W = 6;
    const int H = 6;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init()
    {
        TileAreaProcessor.Init(W, H);        
    }

}
