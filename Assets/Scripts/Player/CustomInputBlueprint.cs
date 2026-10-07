using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[Serializable]
public class CustomInputBlueprint<T> where T : struct
{
    [SerializeField] private InputActionReference actionRef;
    [HideInInspector] public T value;

    private Action actionPerformedInstructions;
    private Action actionCancelledInstructions;

    private void ActionPerformed(InputAction.CallbackContext context)
    {
        actionPerformedInstructions?.Invoke();
        if(typeof(T) == typeof(bool))
        {
            value = (T)(object)context.ReadValueAsButton();
        }
        else
        {
            value = context.ReadValue<T>();
        }
    }
    private void ActionCancelled(InputAction.CallbackContext context)
    {
        actionCancelledInstructions?.Invoke();
        if (typeof(T) == typeof(bool))
        {
            value = (T)(object)context.ReadValueAsButton();
        }
        else
        {
            value = context.ReadValue<T>();
        }
    }

    public void SetActionPerformedInstructions(Action instructions)
    {
        actionPerformedInstructions += instructions;
    }
    public void SetActionCancelledInstructions(Action instructions)
    {
        actionCancelledInstructions += instructions;
    }
    public void RemoveActionPerformedInstructions(Action instructions)
    {
        actionPerformedInstructions -= instructions;
    }
    public void RemoveActionCancelledInstructions(Action instructions)
    {
        actionCancelledInstructions -= instructions;
    }

    public void Initilization()
    {
        actionRef.action.performed += ActionPerformed;
        actionRef.action.canceled += ActionCancelled;
    }
    public void Destruction()
    {
        actionRef.action.performed -= ActionPerformed;
        actionRef.action.canceled -= ActionCancelled;
    }
}