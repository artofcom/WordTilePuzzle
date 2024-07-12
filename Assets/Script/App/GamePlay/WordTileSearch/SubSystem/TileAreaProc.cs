using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileAreaProc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] GameObject WordTilePrefab;
    [SerializeField] Transform TileAreaLT;
    [SerializeField] Transform TileAreaRB;
    [SerializeField] RectTransform TileAreaRoot;

    int mNumWidth, mNumHeight;

    List<WordTile> mWordTiles = new List<WordTile>();

    bool mIsPressed = false;
    int mIdxStartTileX, mIdxStartTileY;
    int mIdxEndTileX, mIdxEndTileY;
    List<WordTile> mSelectedTiles = new List<WordTile>();   // Orders matter.

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Init(int widthCount, int heightCount)
    {
        mNumWidth = widthCount;
        mNumHeight = heightCount;

        mWordTiles.Clear();

        float StartX = TileAreaLT.position.x;
        float StartY = TileAreaRB.position.y;
        float Width = (TileAreaRB.position.x - TileAreaLT.position.x) / ((float)mNumWidth);
        float Height = (TileAreaLT.position.y - TileAreaRB.position.y) / ((float)mNumHeight);

        for (int y = 0; y < mNumHeight; ++y)
        {
            for (int x = 0; x < mNumWidth; ++x)
            {
                GameObject objWTile = Instantiate(WordTilePrefab, TileAreaRoot, true);
                objWTile.transform.position = new Vector3(StartX + Width * 0.5f + ((float)x) * Width, StartY + Height * 0.5f + ((float)y) * Height, objWTile.transform.position.z);
                objWTile.SetActive(true);

                string temp = ((char)('A' + Random.Range(0, 26))).ToString();
                objWTile.GetComponent<WordTile>().Refresh(temp, selected:false);
                mWordTiles.Add(objWTile.GetComponent<WordTile>());
            }
        }
    }

    #region Point Event Receivers.
    public void OnDrag(PointerEventData eventData)
    {
        if (!mIsPressed) return;

        Vector2 pos = PointerDataToRelativePos(eventData, TileAreaRoot);
        Debug.Log($">> Pointer is Dragging....{pos.x}, {pos.y}");// + _word);

        //Core.Events.EventSystem.DispatchEvent("OnPointerDown3DAniView", (object)true);
        (int ix, int iy) = GetTileIndex(pos);
        var wordTile = GetWordTile(ix, iy);
        if (wordTile != null)
        {
            if (mIdxEndTileX != ix || mIdxEndTileY != iy)
            {
                ClearSelection();

                mIdxEndTileX = ix;
                mIdxEndTileY = iy;

                // collect all the tiles between start and end tiles.
                CollectSellectedWordTiles();

                foreach (var tile in mSelectedTiles)
                {
                    tile.Refresh("", selected: true);
                }
            }
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("> Pointer Down.");
        mIsPressed = true;

        Vector2 pos = PointerDataToRelativePos(eventData, TileAreaRoot);
        (int ix, int iy) = GetTileIndex(pos);
        var wordTile = GetWordTile(ix, iy);
        if (wordTile != null)
        {
            mIdxStartTileX = ix;
            mIdxStartTileY = iy;
           
            wordTile.Refresh("", true);
        }
        mSelectedTiles.Clear();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("> Pointer Up. ");// + _word);
        mIsPressed = false;

        // De-Select All.
        ClearSelection();
    }
    #endregion


    #region Tile Picking Processor

    void SelectAndAddTile(int iX, int iY)
    {
        var tile = GetWordTile(iX, iY);
        tile.Refresh("", selected: true);
        mSelectedTiles.Add(tile);
    }

    void CollectSellectedWordTiles()
    {
        if (mIdxStartTileX < 0 || mIdxStartTileX >= mNumWidth || mIdxStartTileY < 0 || mIdxStartTileY >= mNumHeight)
        {
            Debug.Assert(false, "Start Tile Index Error!");
            return;
        }
        if (mIdxEndTileX < 0 || mIdxEndTileX >= mNumWidth || mIdxEndTileY < 0 || mIdxEndTileY >= mNumHeight)
        {
            Debug.Assert(false, "End Tile Index Error!");
            return;
        }


        if (mIdxStartTileX != mIdxEndTileX && mIdxStartTileY != mIdxEndTileY)
        {
            if (mIdxStartTileX < mIdxEndTileX)
            {
                if (mIdxStartTileY < mIdxEndTileY) AddTilesToRightUp();
                else                               AddTilesToRightDown();
            }
            else
            {
                if (mIdxStartTileY < mIdxEndTileY) AddTilesToLeftUp();
                else                               AddTilesToLeftDown();
            }
        }
        else if (mIdxStartTileX == mIdxEndTileX)   AddTilesHorizontal();
        else if (mIdxStartTileY == mIdxEndTileY)   AddTilesVertical();
        else
            Debug.Assert(false, "Invalid Tile Selection Case !!!");

    }

    (int, int) GetTileIndex(Vector2 ptPos)
    {
        int ix = -1, iy = -1;

        const float fSensibility = 0.65f;   // 0 ~ 1 : 0 means never select, 1 means select as long as in the tile area

        float OneWidth = (TileAreaRB.localPosition.x - TileAreaLT.localPosition.x) / ((float)mNumWidth);
        float OneHeight = (TileAreaLT.localPosition.y - TileAreaRB.localPosition.y) / ((float)mNumHeight);

        // Sensibility filter. 
        int y = ((int)ptPos.y) % ((int)OneHeight);
        int x = ((int)ptPos.x) % ((int)OneWidth);
        Vector2 vCenter = new Vector2(OneWidth * 0.5f, OneHeight * 0.5f);
        if (x < (vCenter.x * (1.0f - fSensibility)) || x > (vCenter.x * (1.0f + fSensibility)))
            return (ix, iy);
        if (y < (vCenter.y * (1.0f - fSensibility)) || x > (vCenter.y * (1.0f + fSensibility)))
            return (ix, iy);

        iy = (int)(ptPos.y / OneHeight);
        ix = (int)(ptPos.x / OneWidth);

        return (ix, iy);
    }

    WordTile GetWordTile(int ix, int iy)
    {
        if (ix < 0 || ix >= mNumWidth)
            return null;

        if (iy < 0 || iy >= mNumHeight)
            return null;

        int idx = iy * mNumWidth + ix;
        return mWordTiles[idx];
    }

    Vector2 PointerDataToRelativePos(PointerEventData eventData, RectTransform rectTransform)
    {
        Vector2 result;
        Vector2 clickPosition = eventData.position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, clickPosition, Camera.main, out result);

        float Width = TileAreaRB.localPosition.x - TileAreaLT.localPosition.x;
        float Height = TileAreaLT.localPosition.y - TileAreaRB.localPosition.y;

        result += new Vector2(Width * 0.5f, Height * 0.5f);
        return result;
    }

    void ClearSelection()
    {
        foreach (var tile in mSelectedTiles)
        {
            tile.Refresh("", false);
        }
        mSelectedTiles.Clear();
    }

#endregion





#region Collecting Tiles per Direction
void AddTilesToRightUp()
    {
        int ix = mIdxStartTileX; int iy = mIdxStartTileY;
        while (true)
        {
            if (ix > mIdxEndTileX || iy > mIdxEndTileY)
                break;
            SelectAndAddTile(ix, iy);
            ++ix; ++iy;
        }
    }

    void AddTilesToRightDown()
    {
        int ix = mIdxStartTileX; int iy = mIdxStartTileY;
        while (true)
        {
            if (ix > mIdxEndTileX || iy < mIdxEndTileY)
                break;
            SelectAndAddTile(ix, iy);
            ++ix; --iy;
        }
    }

    void AddTilesToLeftUp()
    {
        int ix = mIdxStartTileX; int iy = mIdxStartTileY;
        while (true)
        {
            if (ix < mIdxEndTileX || iy > mIdxEndTileY)
                break;
            SelectAndAddTile(ix, iy);
            --ix; ++iy;
        }
    }

    void AddTilesToLeftDown()
    {
        int ix = mIdxStartTileX; int iy = mIdxStartTileY;
        while (true)
        {
            if (ix < mIdxEndTileX || iy < mIdxEndTileY)
                break;
            SelectAndAddTile(ix, iy);
            --ix; --iy;
        }
    }

    void AddTilesHorizontal()
    {
        bool inc = mIdxStartTileY < mIdxEndTileY;
        int q = mIdxStartTileY;

        while (true)
        {
            if (inc) { if (q > mIdxEndTileY) break; }
            else { if (q < mIdxEndTileY) break; }
            SelectAndAddTile(mIdxStartTileX, q);
            q = inc ? q + 1 : q - 1;
        }
    }

    void AddTilesVertical()
    {
        bool inc = mIdxStartTileX < mIdxEndTileX;
        int q = mIdxStartTileX;

        while (true)
        {
            if (inc) { if (q > mIdxEndTileX) break; }
            else { if (q < mIdxEndTileX) break; }
            SelectAndAddTile(q, mIdxStartTileY);
            q = inc ? q + 1 : q - 1;
        }
    }

    #endregion


}
