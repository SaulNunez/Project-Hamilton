using UnityEngine.Events;
using UnityEngine.UI;

public class SpecialButton : Button
{
    bool lastState = false;

    public UnityEvent onPressedStarted;
    public UnityEvent onPressedEnded;

    public bool IsCurrentlyPressed { get => lastState; }

    new void Update()
    {
        var currentPressedState = IsPressed();
        if(!lastState && currentPressedState)
        {
            onPressedStarted?.Invoke();
        } else if(lastState && currentPressedState)
        {
            onPressedEnded?.Invoke();
        }

        lastState = currentPressedState;
    }

}
