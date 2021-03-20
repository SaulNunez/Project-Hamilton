using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by boiler sabotage component.
/// 
/// There's a variety of gauges. This is used to provided several gauges configurations.
/// </summary>
[Serializable]
public class BoilerSabotageOptions
{
    public Sprite gauge;
    public int maximum;
    public int minimum;
}
