using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    public static class MonoBehaviourExtension 
    {

        public static void InvokeNextFrame(this MonoBehaviour monoBehaviour, Action method)
        {
            if (method != null)
            {
                monoBehaviour.StartCoroutine(DelayCall(method));
            }
        }

        public static void Invoke(this MonoBehaviour monoBehaviour, Action method, float seconds)
        {
            if (method != null)
            {
                monoBehaviour.StartCoroutine(DelayCall(method, seconds));
            }
        }


        private static IEnumerator DelayCall(Action method, float? seconds = null)
        {
            yield return ((seconds.HasValue) ? new WaitForSeconds(seconds.Value) : null);
            method();
        }

	}
}
