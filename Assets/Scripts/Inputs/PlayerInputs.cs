// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""41aae3d3-da29-40d8-88d0-955545dfee06"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""33c533aa-3ed7-4dfb-8164-c90e9bbfb141"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b8a9a7dc-a531-4801-bfdd-a689a316f9ad"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""60f1a2e0-8c15-43a5-9ab8-051d2e218e1e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f6b082b7-109d-48be-8bb1-5e74f0f2e5e3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse + Keyboard"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5c47f4c-ff34-4c39-b91f-1070cb21155b"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f5dadbd-bf41-4358-b0f6-9f1bd599d894"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse + Keyboard"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse + Keyboard"",
            ""bindingGroup"": ""Mouse + Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_LeftClick = m_Main.FindAction("LeftClick", throwIfNotFound: true);
        m_Main_MousePosition = m_Main.FindAction("MousePosition", throwIfNotFound: true);
        m_Main_RightClick = m_Main.FindAction("RightClick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_LeftClick;
    private readonly InputAction m_Main_MousePosition;
    private readonly InputAction m_Main_RightClick;
    public struct MainActions
    {
        private @PlayerInputs m_Wrapper;
        public MainActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_Main_LeftClick;
        public InputAction @MousePosition => m_Wrapper.m_Main_MousePosition;
        public InputAction @RightClick => m_Wrapper.m_Main_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @LeftClick.started -= m_Wrapper.m_MainActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnLeftClick;
                @MousePosition.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMousePosition;
                @RightClick.started -= m_Wrapper.m_MainActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnRightClick;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    private int m_MouseKeyboardSchemeIndex = -1;
    public InputControlScheme MouseKeyboardScheme
    {
        get
        {
            if (m_MouseKeyboardSchemeIndex == -1) m_MouseKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse + Keyboard");
            return asset.controlSchemes[m_MouseKeyboardSchemeIndex];
        }
    }
    public interface IMainActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
}
