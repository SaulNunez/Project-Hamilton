using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class SuperPrintExtension
    {
        public static void SuperPrint(this MonoBehaviour monoBehaviour, object message)
        {
            Debug.Log($"{monoBehaviour.name}-{monoBehaviour.GetType().Name}: {message}: {DateTime.Now:O}");
        }
    }
}
