using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvaluatorComponent
{
    IEnumerator Run();
    void Stop();
}
