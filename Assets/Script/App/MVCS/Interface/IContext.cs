using UnityEngine;
using System.Collections;

public interface IContext 
{
    IEnumerator Init(MonoBehaviour monoObject);
}
