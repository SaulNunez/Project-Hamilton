using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpinUIOnDrag : MonoBehaviour
{
    [SerializeField]
    Transform transform;

    public UnityEvent onCycle;

    float lastAngle = 0;

    public void OnDrag()
    {
        var mousePosition = Input.mousePosition;
        var direction = mousePosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = angle <= 0 ? 360 + angle : angle;
        if(angle - lastAngle > 0 || angle <= 60 && lastAngle >= 300)
        {
            print($"Angle: {angle}, last angle: {lastAngle}");
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (angle <= 60 && lastAngle >= 300)
            {
                print("Hello");
                onCycle?.Invoke();
            }
            transform.rotation = rotation;

            lastAngle = angle;
        }
    }
}
