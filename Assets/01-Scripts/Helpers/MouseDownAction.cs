using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseDownAction : MonoBehaviour
{
    [SerializeField] private UnityEvent clickActions;

    public void OnMouseDown()
    {
        Debug.Log($"MouseDown: {gameObject.name}");
        clickActions?.Invoke();
    }

    public void MouseDown()
    {
        Debug.Log($"MouseDown: {gameObject.name}");
        clickActions?.Invoke();
    }
}
