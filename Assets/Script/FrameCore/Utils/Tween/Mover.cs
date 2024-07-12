using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Core.Tween
{
    public class Mover : EaseFuncLib
    {
        object Param;
        AnimationCurve Curve;

        // Trigger  -----------------------------------
        //
        public void Trigger(Vector3 vStart, Vector3 vEnd, float duration, object param, Action<object> finAction = null)
        {
            Param = param;

            UpdateEaseFunction();

            Curve = null;
            StartCoroutine(coTween(vStart, vEnd, duration, finAction));
        }

        public void TriggerWithEase(DurationEase easeType, Vector3 vStart, Vector3 vEnd, float duration, object param, Action<object> finAction)
        {
            EaseType = easeType;

            Trigger(vStart, vEnd, duration, param, finAction);
        }

        public void TriggerWithCurve(AnimationCurve curve, Vector3 vStart, Vector3 vEnd, float duration, object param, Action<object> finAction)
        {
            Curve = curve;
            Param = param;

            if(Curve == null)
                UpdateEaseFunction();

            StartCoroutine(coTween(vStart, vEnd, duration, finAction));
        }


        /*
        public static IEnumerator CoMove(Transform trTarget, Vector3 vTo, float duration)
        {
            float fStartT = Time.time;
            while (Time.time < fStartT + duration)
            {
                //transform.localScale = Vector3.Lerp(vStart, vTo, Mathf.Clamp01((Time.time - fStartT) / duration));

                float timeDelta = durationEaseFunc(Time.time - fStartT, duration);
                transform.localPosition = Vector3.LerpUnclamped(vStart, vEnd, timeDelta);

                yield return null;
            }
            transform.localPosition = vEnd;

            if (finAction != null)
                finAction.Invoke();
        }*/


        // Member func  -----------------------------------
        //
        IEnumerator coTween(Vector3 vStart, Vector3 vEnd, float duration, Action<object> finAction)
        {
            transform.localPosition = vStart;

            float fStartT = Time.time;
            while (Time.time < fStartT + duration)
            {
                //transform.localScale = Vector3.Lerp(vStart, vTo, Mathf.Clamp01((Time.time - fStartT) / duration));

                float timeDelta = Curve == null ? durationEaseFunc(Time.time - fStartT, duration) :
                                                  Curve.Evaluate((Time.time - fStartT) / duration);
                
                transform.localPosition = Vector3.LerpUnclamped(vStart, vEnd, timeDelta);

                yield return null;
            }
            transform.localPosition = vEnd;

            if (finAction != null)
                finAction.Invoke(Param);
        }

    }

}