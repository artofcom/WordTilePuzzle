using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager.Data;

namespace Slot
{
    public class ReelData
    {
        public List<int> SymbolIndices = new List<int>();
    }
    public class HitLineData
    {
        public List<int> HitLines = new List<int>();
    }
    public class WinData
    {
        public string SymbolName;
        public int minCount;
    }

    public class BaseGame : AGameCore
    {
        public int ReelCount => Reels.Count;



        SlotControlData mCtrlData;
        List<string> SymbolNames = new List<string>();
        List<ReelData> Reels = new List<ReelData>();
        List<HitLineData> HitLines = new List<HitLineData>();
        List<WinData> WinInfo = new List<WinData>();


        // Local field area.
        int mRandomCounter = 1;
        int mTotalCase;
        List<int> ReelHitIndices = new List<int>();
        System.Random mRandom;

        public override void Init(ISlotControlData controllerData)
        {
            base.Init(controllerData);

            mCtrlData = controllerData as SlotControlData;

            // Adding Basic Reel Symbol Info.
            SymbolNames.Clear();
            string[] resultStr = mCtrlData.Rule.SymbolNames.Split(',');
            UnityEngine.Assertions.Assert.IsTrue(resultStr != null && resultStr.Length > 0);
            for (int q = 0; q < resultStr.Length; ++q)
                SymbolNames.Add(resultStr[q]);

            // Init Each Reel.
            InitReelData();

            InitHitLineData();

            InitWinData();

            mRandomCounter = 0;
            mRandom = new System.Random();

            // TOTAL CASE : 5 x 5 x 5 x 5 x 5 = 3125
            mTotalCase = 1;
            for (int q = 0; q < Reels.Count; ++q)
                mTotalCase *= Reels[q].SymbolIndices.Count;
            for (int k = 0; k < mTotalCase; ++k)
                ReelHitIndices.Add(k);
            //
        }

        public override bool Spin(out List<int> outReelStopIndices)
        {
            //for (int q = 0; q < mTotalCase; ++q)
            //    DebugSpin();

            int randIdx = GenerateRandomNumber();
            outReelStopIndices = new List<int>(new int[Reels.Count]);
            int mul = 1;
            for (int q = Reels.Count - 1; q >= 0; --q)
            {
                outReelStopIndices[q] = (randIdx / mul) % Reels[q].SymbolIndices.Count;
                mul *= Reels[q].SymbolIndices.Count;
            }
            return true;
        }

        public void Judge(List<int> reelStopIndex)
        {
            for(int k = 0; k < reelStopIndex.Count; ++k)
            {
                UnityEngine.Assertions.Assert.IsTrue(reelStopIndex[k] >= 0 && reelStopIndex[k] < Reels[k].SymbolIndices.Count);

                string symbolName = SymbolNames[ Reels[k].SymbolIndices[ reelStopIndex[k] ] ];
                UnityEngine.Debug.Log($"[{k}]-[{symbolName}]");
            }


            for(int win = 0; win < WinInfo.Count; ++win)
            {
                string targetSymbol = WinInfo[win].SymbolName;
                int minCount = WinInfo[win].minCount;

                for(int h = 0; h < HitLines.Count; ++h)
                {
                    string curHitSymbol = string.Empty;
                    HitLineData lineData = HitLines[h];
                    int hitCount = 1;
                    for(int k = 0; k < lineData.HitLines.Count; ++k)
                    {
                        int idxSymbol = lineData.HitLines[k] + reelStopIndex[k];
                        idxSymbol %= Reels[k].SymbolIndices.Count;
                        string symbol = SymbolNames[idxSymbol];

                        // Case only for ANY symbool.
                        if (0 == k)
                            curHitSymbol = symbol;
                        else
                        {
                            if (curHitSymbol == symbol)
                                ++hitCount;
                            else break;
                        }
                    }

                    if(hitCount >= minCount)
                    {
                        UnityEngine.Debug.Log($"Line:{lineData.HitLines[0]}, Symbol:{curHitSymbol}, Count:{hitCount}");
                    }
                }
            }

        }

        string GetSymbolNameByStopIndex(int idxReel, int idxStop)
        {
            return "";
        }

        public List<string> ReelStopIndexToString(int idxReel, int idxReelStop, int rowCount)
        {
            List<string> listOut = new List<string>();

            UnityEngine.Assertions.Assert.IsTrue(idxReel >= 0 && idxReel < Reels.Count);

            for (int q = 0; q < rowCount; ++q)
            {
                int idx = (idxReelStop + q) % Reels[idxReel].SymbolIndices.Count;
                UnityEngine.Assertions.Assert.IsTrue(idx >= 0 && idx < Reels[idxReel].SymbolIndices.Count);

                listOut.Add(SymbolNames[Reels[idxReel].SymbolIndices[idx]]);
            }
            return listOut;
        }

        void DebugSpin()
        { 
            // TOTAL CASE : 5 x 5 x 5 x 5 x 5 = 3125

            int randIdx = GenerateRandomNumber();

            List<int> reelStopIndices = new List<int>( new int[Reels.Count] );
            int mul = 1;
            for(int q = Reels.Count-1; q>=0; --q)
            {
                reelStopIndices[q] = (randIdx / mul) % Reels[q].SymbolIndices.Count;
                mul *= Reels[q].SymbolIndices.Count;
            }

            UnityEngine.Debug.Log($"{reelStopIndices[0]}:{reelStopIndices[1]}:{reelStopIndices[2]}");
        }

        int SymbolNameToIndex(string symbolName)
        {
            for(int q = 0; q < SymbolNames.Count; ++q)
            {
                if (SymbolNames[q] == symbolName)
                    return q;
            }

            UnityEngine.Assertions.Assert.IsTrue(false, "Looking for Invalid SymbolName.."+symbolName);
            return -1;
        }

        void InitReelData()
        {
            Reels.Clear();

            // Paid Spin Only for now.
            UnityEngine.Assertions.Assert.IsTrue(mCtrlData.Rule.Reels.PaidSpin.Count == mCtrlData.Rule.RowCount);

            for(int idxReel = 0; idxReel < mCtrlData.Rule.Reels.PaidSpin.Count; ++idxReel)
            {
                string[] resolvedString = mCtrlData.Rule.Reels.PaidSpin[idxReel].Split(',');
                UnityEngine.Assertions.Assert.IsTrue(resolvedString != null && resolvedString.Length > 0);

                ReelData reelData = new ReelData();
                for (int q = 0; q < resolvedString.Length; ++q)
                    reelData.SymbolIndices.Add(SymbolNameToIndex(resolvedString[q]));
                Reels.Add(reelData);
            }
        }

        void InitHitLineData()
        {
            HitLines.Clear();

            UnityEngine.Assertions.Assert.IsTrue(mCtrlData.Rule.Paylines.Count > 0);
            for (int k = 0; k < mCtrlData.Rule.Paylines.Count; ++k)
            {
                string[] resolvedString = mCtrlData.Rule.Paylines[k].Split(',');
                UnityEngine.Assertions.Assert.IsTrue(resolvedString != null && resolvedString.Length > 0);

                HitLineData hitData = new HitLineData();
                for (int q = 0; q < resolvedString.Length; ++q)
                {
                    int idxLine;
                    bool ret = int.TryParse(resolvedString[q], out idxLine);
                    if (ret)    hitData.HitLines.Add(idxLine);
                    else        UnityEngine.Assertions.Assert.IsTrue(false, "Parsing Number Error! - " + resolvedString[q]);
                }
                HitLines.Add(hitData);
            }
        }

        void InitWinData()
        {
            WinInfo.Clear();

            WinData winData = new WinData();
            winData.SymbolName = "ANY";
            winData.minCount = 2;
            WinInfo.Add(winData);
        }

        int GenerateRandomNumber()
        {
            //mRandomCounter %= mTotalCase;
            return mRandomCounter++;

            /*
            int rndIdx = mRandom.Next(mRandomCounter, mTotalCase);

            int tempIdx = ReelHitIndices[rndIdx];

            ReelHitIndices[rndIdx] = ReelHitIndices[mRandomCounter];
            ReelHitIndices[mRandomCounter++] = tempIdx;

            return tempIdx;
            */
        }
    }
}