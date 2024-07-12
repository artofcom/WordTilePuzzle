using System;
using System.Collections.Generic;

namespace Game.Manager.Data
{
    public interface ISlotControlData
    {

    }

    [Serializable]
    public class ReelInfo
    {
        public List<string> PaidSpin;
        public List<string> FreeSpin;
    }

    [Serializable]
    public class GameRule
    {
        public int RowCount;
        public int ColCount;
        public string SymbolNames;
        public List<string> Paylines;
        public ReelInfo Reels;
    }

    [Serializable]
    public class ClientInfo
    {
        public bool IsVertical;
        public string Bundle;
        public string Prefab;
    }


    [Serializable]
    public class SlotControlData : ISlotControlData
    {
        public string Key;
        public string DisplayName;
        public GameRule Rule;
        public ClientInfo ClientData;
    }
}