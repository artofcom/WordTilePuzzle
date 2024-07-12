using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASingleReelComponent : MonoBehaviour
{
    public virtual void Init(object data, int reelIndex)    { }
    public virtual void StartSpin(List<string> StopSymbols) { }
    public virtual void Clear()                             { }
}
