using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AReelComponent : MonoBehaviour
{
    public virtual void Init(IContext context, Game.Manager.Data.ISlotControlData ctrlData) { }
    public virtual void StartSpin(List<List<string>> StopSymbols) { }
}
