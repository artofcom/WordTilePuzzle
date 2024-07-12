using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    public abstract class AGameCore
    {
        public virtual void Init(Game.Manager.Data.ISlotControlData controllerData)
        {

        }

        public abstract bool Spin(out List<int> outReelStopIndex);
    }
}
