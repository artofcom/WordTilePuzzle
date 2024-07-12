using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyFeatureComponent : IFeatureComponent
{
    public IEnumerator Run()
    {
        yield break;
    }
}
