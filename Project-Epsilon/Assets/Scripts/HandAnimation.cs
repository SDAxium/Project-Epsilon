using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference pinchAction;
    private Animator _animator;
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    private static readonly int Grip = Animator.StringToHash("Grip");

    private void OnEnable()
    {
        //grip
        gripAction.action.performed += Gripping;
        gripAction.action.canceled += GripRelease;

        //pinch
        pinchAction.action.performed += Pinching;
        pinchAction.action.canceled += PinchRelease;
    }

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start() => _animator = GetComponent<Animator>();

    private void Gripping(InputAction.CallbackContext obj)
    {
        if(_animator) _animator.SetFloat(Grip, obj.ReadValue<float>());
    }

    private void GripRelease(InputAction.CallbackContext obj)
    {
        if(_animator) _animator.SetFloat(Grip, 0f);
    }

    private void Pinching(InputAction.CallbackContext obj)
    {
        if(_animator) _animator.SetFloat(Trigger, obj.ReadValue<float>());    
    }

    private void PinchRelease(InputAction.CallbackContext obj)
    {
        if(_animator) _animator.SetFloat(Trigger, 0f);
    }
}
