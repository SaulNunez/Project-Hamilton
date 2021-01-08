using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "activity", menuName = "Activity/Create activity", order = 1)]
public class ActivityInfo: ScriptableObject
{
    public string id;
    [TextArea]
    public string activityName;
}
