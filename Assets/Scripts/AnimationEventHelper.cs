using UnityEngine;
using UnityEngine.Events;

public class Animatior : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnAttackPeformed;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        OnAttackPeformed?.Invoke();
    }
}
