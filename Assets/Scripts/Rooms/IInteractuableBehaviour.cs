using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple interface to define actions to perform when this object is touched
/// </summary>
public interface IInteractuableBehaviour
{
    /// <summary>
    /// Called when a GameObject interacts with this GameObject, a behaviour can be defined here to what to do when interacted with this object.
    /// </summary>
    /// <param name="approachedBy">GameObject that tries to interact with this GameObject</param>
    void OnApproach(GameObject approachedBy);
}
