using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System;

namespace Hordewood.Input
{
    public enum InputDeviceKind { Keyboard, Xbox, PlayStation, Switch, GenericGamepad }

    public class InputDeviceTracker : MonoBehaviour
    {
        public static InputDeviceTracker Instance { get; private set; }

        public InputDeviceKind CurrentDevice { get; private set; } = InputDeviceKind.Keyboard;
        public event Action<InputDeviceKind> OnDeviceChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable() => InputSystem.onEvent += OnInputEvent;
        private void OnDisable() => InputSystem.onEvent -= OnInputEvent;

        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

            InputDeviceKind kind = device switch
            {
                Keyboard   => InputDeviceKind.Keyboard,
                Mouse      => InputDeviceKind.Keyboard,
                Gamepad gp => ResolveGamepadKind(gp),
                _          => CurrentDevice
            };

            if (kind == CurrentDevice) return;

            CurrentDevice = kind;
            OnDeviceChanged?.Invoke(kind);
        }

        private InputDeviceKind ResolveGamepadKind(Gamepad gamepad)
        {
            string name = gamepad.name.ToLowerInvariant();

            if (name.Contains("dualshock") || name.Contains("dualsense") || name.Contains("ps4") || name.Contains("ps5"))
                return InputDeviceKind.PlayStation;

            if (name.Contains("switch") || name.Contains("pro controller"))
                return InputDeviceKind.Switch;

            if (name.Contains("xinput") || name.Contains("xbox"))
                return InputDeviceKind.Xbox;

            return InputDeviceKind.GenericGamepad;
        }
    }
}
