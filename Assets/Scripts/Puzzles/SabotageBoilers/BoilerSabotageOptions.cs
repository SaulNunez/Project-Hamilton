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
    /// <summary>
    /// Image to use for gauge representation
    /// </summary>
    public Sprite gauge;
    /// <summary>
    /// Maximum pressure of this valve
    /// </summary>
    public int maximum;
    /// <summary>
    /// Minimum pressure of this valve
    /// </summary>
    public int minimum;
}
