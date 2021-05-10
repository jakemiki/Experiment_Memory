using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    void Interact();
}

public class Key : MonoBehaviour, IInteractable
{
    public UnityEvent onPress = new UnityEvent();

    public void Interact()
    {
        onPress.Invoke();
    }
}
