//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Action/PlayerInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""Test"",
            ""id"": ""000e50cc-e527-4e2b-96e4-866f5973b6b4"",
            ""actions"": [
                {
                    ""name"": ""Test1"",
                    ""type"": ""Button"",
                    ""id"": ""2d40385c-cbb3-4fe9-a142-2de83700c192"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test2"",
                    ""type"": ""Button"",
                    ""id"": ""9da43f3f-aa5d-4c00-a105-709aaeebebf1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test3"",
                    ""type"": ""Button"",
                    ""id"": ""a2cef98a-6d12-4b5f-904d-923828f9520e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test4"",
                    ""type"": ""Button"",
                    ""id"": ""416f7228-ada3-4932-a502-88a28b0ad550"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test5"",
                    ""type"": ""Button"",
                    ""id"": ""d6774d49-769d-4dd1-b2c3-5879455e1b7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test6"",
                    ""type"": ""Button"",
                    ""id"": ""3e9f8a6c-738a-47fa-8500-10e5d7424d74"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test7"",
                    ""type"": ""Button"",
                    ""id"": ""52ef7b06-b60c-454a-a625-2170423d67e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test8"",
                    ""type"": ""Button"",
                    ""id"": ""dfdf2777-d72a-4e7b-bb68-e845804bdf2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test9"",
                    ""type"": ""Button"",
                    ""id"": ""bec005d1-2df0-4f03-891a-981dd4f91a3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LClick"",
                    ""type"": ""Button"",
                    ""id"": ""a65e656c-0967-4263-b572-14b09328364b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RClick"",
                    ""type"": ""Button"",
                    ""id"": ""d4d56240-1e66-48e6-866b-38a66848ff8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Console"",
                    ""type"": ""Button"",
                    ""id"": ""f4b38e20-d9a3-4dcb-b1da-c0cd3b1d78e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Console_Clear"",
                    ""type"": ""Button"",
                    ""id"": ""997ae985-3a60-4ebf-83ce-2c0b8c469d30"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""521a798f-eb84-48a0-83a2-11a39cca30ae"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd8ca400-32a8-459a-8db6-3812eaf6fba5"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e9c773f-6127-473e-99c7-f23f8e95579a"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""449ff97d-c340-4677-9cf9-76d41d57a6b6"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d78460d-f7cb-40e5-bdac-f5437f2262f0"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8799b8cc-90b0-4ded-a13b-250a5f634212"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4aa0c8d4-0ca4-4417-96d9-54f760937759"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43575b91-89fc-4b8e-ac16-b9d5dfb1c0b3"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test8"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9529aee-1ac0-4b61-a6cf-dfa077ec5c9a"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test9"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3b03708-21c3-47a7-b39d-bf3d8a7a1173"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91c96a4c-82b2-4ec9-bdfe-2abf0dbe0fc0"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2352951c-c0bf-4146-874c-7c1f38082480"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Console"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""ca55cec1-0bea-47e7-953d-b348d0ee74b8"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Console_Clear"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""0010c165-339d-4b55-9e35-3631f4b20dbd"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Console_Clear"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""d2a9ec51-d24d-4d9a-97f6-2a95f3eff245"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Console_Clear"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""91390d13-a49b-410a-8cbd-0d2ff8a45eb5"",
            ""actions"": [
                {
                    ""name"": ""MoveForward"",
                    ""type"": ""Value"",
                    ""id"": ""e87f43e7-975c-4878-8035-b4fc8ac73798"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""8fafc869-a2a3-4c30-91fb-ccf51d8dcb4b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""699403a3-476c-4551-bafc-54b26606c3d8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveForward"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3c1a0335-ebf8-4bf4-ae5c-2e011fdc8828"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""eae88044-e1e3-40bd-b2ad-894a94ab56de"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""8e30d328-7d4a-4c05-a164-f452475e0848"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""74fa2a57-017c-41b9-a7e3-779a96b3a126"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0ee871b6-e51b-47da-a237-a11f914d54c7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KM"",
            ""bindingGroup"": ""KM"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test_Test1 = m_Test.FindAction("Test1", throwIfNotFound: true);
        m_Test_Test2 = m_Test.FindAction("Test2", throwIfNotFound: true);
        m_Test_Test3 = m_Test.FindAction("Test3", throwIfNotFound: true);
        m_Test_Test4 = m_Test.FindAction("Test4", throwIfNotFound: true);
        m_Test_Test5 = m_Test.FindAction("Test5", throwIfNotFound: true);
        m_Test_Test6 = m_Test.FindAction("Test6", throwIfNotFound: true);
        m_Test_Test7 = m_Test.FindAction("Test7", throwIfNotFound: true);
        m_Test_Test8 = m_Test.FindAction("Test8", throwIfNotFound: true);
        m_Test_Test9 = m_Test.FindAction("Test9", throwIfNotFound: true);
        m_Test_LClick = m_Test.FindAction("LClick", throwIfNotFound: true);
        m_Test_RClick = m_Test.FindAction("RClick", throwIfNotFound: true);
        m_Test_Console = m_Test.FindAction("Console", throwIfNotFound: true);
        m_Test_Console_Clear = m_Test.FindAction("Console_Clear", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveForward = m_Player.FindAction("MoveForward", throwIfNotFound: true);
        m_Player_Rotate = m_Player.FindAction("Rotate", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Test
    private readonly InputActionMap m_Test;
    private List<ITestActions> m_TestActionsCallbackInterfaces = new List<ITestActions>();
    private readonly InputAction m_Test_Test1;
    private readonly InputAction m_Test_Test2;
    private readonly InputAction m_Test_Test3;
    private readonly InputAction m_Test_Test4;
    private readonly InputAction m_Test_Test5;
    private readonly InputAction m_Test_Test6;
    private readonly InputAction m_Test_Test7;
    private readonly InputAction m_Test_Test8;
    private readonly InputAction m_Test_Test9;
    private readonly InputAction m_Test_LClick;
    private readonly InputAction m_Test_RClick;
    private readonly InputAction m_Test_Console;
    private readonly InputAction m_Test_Console_Clear;
    public struct TestActions
    {
        private @PlayerInputAction m_Wrapper;
        public TestActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Test1 => m_Wrapper.m_Test_Test1;
        public InputAction @Test2 => m_Wrapper.m_Test_Test2;
        public InputAction @Test3 => m_Wrapper.m_Test_Test3;
        public InputAction @Test4 => m_Wrapper.m_Test_Test4;
        public InputAction @Test5 => m_Wrapper.m_Test_Test5;
        public InputAction @Test6 => m_Wrapper.m_Test_Test6;
        public InputAction @Test7 => m_Wrapper.m_Test_Test7;
        public InputAction @Test8 => m_Wrapper.m_Test_Test8;
        public InputAction @Test9 => m_Wrapper.m_Test_Test9;
        public InputAction @LClick => m_Wrapper.m_Test_LClick;
        public InputAction @RClick => m_Wrapper.m_Test_RClick;
        public InputAction @Console => m_Wrapper.m_Test_Console;
        public InputAction @Console_Clear => m_Wrapper.m_Test_Console_Clear;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void AddCallbacks(ITestActions instance)
        {
            if (instance == null || m_Wrapper.m_TestActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TestActionsCallbackInterfaces.Add(instance);
            @Test1.started += instance.OnTest1;
            @Test1.performed += instance.OnTest1;
            @Test1.canceled += instance.OnTest1;
            @Test2.started += instance.OnTest2;
            @Test2.performed += instance.OnTest2;
            @Test2.canceled += instance.OnTest2;
            @Test3.started += instance.OnTest3;
            @Test3.performed += instance.OnTest3;
            @Test3.canceled += instance.OnTest3;
            @Test4.started += instance.OnTest4;
            @Test4.performed += instance.OnTest4;
            @Test4.canceled += instance.OnTest4;
            @Test5.started += instance.OnTest5;
            @Test5.performed += instance.OnTest5;
            @Test5.canceled += instance.OnTest5;
            @Test6.started += instance.OnTest6;
            @Test6.performed += instance.OnTest6;
            @Test6.canceled += instance.OnTest6;
            @Test7.started += instance.OnTest7;
            @Test7.performed += instance.OnTest7;
            @Test7.canceled += instance.OnTest7;
            @Test8.started += instance.OnTest8;
            @Test8.performed += instance.OnTest8;
            @Test8.canceled += instance.OnTest8;
            @Test9.started += instance.OnTest9;
            @Test9.performed += instance.OnTest9;
            @Test9.canceled += instance.OnTest9;
            @LClick.started += instance.OnLClick;
            @LClick.performed += instance.OnLClick;
            @LClick.canceled += instance.OnLClick;
            @RClick.started += instance.OnRClick;
            @RClick.performed += instance.OnRClick;
            @RClick.canceled += instance.OnRClick;
            @Console.started += instance.OnConsole;
            @Console.performed += instance.OnConsole;
            @Console.canceled += instance.OnConsole;
            @Console_Clear.started += instance.OnConsole_Clear;
            @Console_Clear.performed += instance.OnConsole_Clear;
            @Console_Clear.canceled += instance.OnConsole_Clear;
        }

        private void UnregisterCallbacks(ITestActions instance)
        {
            @Test1.started -= instance.OnTest1;
            @Test1.performed -= instance.OnTest1;
            @Test1.canceled -= instance.OnTest1;
            @Test2.started -= instance.OnTest2;
            @Test2.performed -= instance.OnTest2;
            @Test2.canceled -= instance.OnTest2;
            @Test3.started -= instance.OnTest3;
            @Test3.performed -= instance.OnTest3;
            @Test3.canceled -= instance.OnTest3;
            @Test4.started -= instance.OnTest4;
            @Test4.performed -= instance.OnTest4;
            @Test4.canceled -= instance.OnTest4;
            @Test5.started -= instance.OnTest5;
            @Test5.performed -= instance.OnTest5;
            @Test5.canceled -= instance.OnTest5;
            @Test6.started -= instance.OnTest6;
            @Test6.performed -= instance.OnTest6;
            @Test6.canceled -= instance.OnTest6;
            @Test7.started -= instance.OnTest7;
            @Test7.performed -= instance.OnTest7;
            @Test7.canceled -= instance.OnTest7;
            @Test8.started -= instance.OnTest8;
            @Test8.performed -= instance.OnTest8;
            @Test8.canceled -= instance.OnTest8;
            @Test9.started -= instance.OnTest9;
            @Test9.performed -= instance.OnTest9;
            @Test9.canceled -= instance.OnTest9;
            @LClick.started -= instance.OnLClick;
            @LClick.performed -= instance.OnLClick;
            @LClick.canceled -= instance.OnLClick;
            @RClick.started -= instance.OnRClick;
            @RClick.performed -= instance.OnRClick;
            @RClick.canceled -= instance.OnRClick;
            @Console.started -= instance.OnConsole;
            @Console.performed -= instance.OnConsole;
            @Console.canceled -= instance.OnConsole;
            @Console_Clear.started -= instance.OnConsole_Clear;
            @Console_Clear.performed -= instance.OnConsole_Clear;
            @Console_Clear.canceled -= instance.OnConsole_Clear;
        }

        public void RemoveCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITestActions instance)
        {
            foreach (var item in m_Wrapper.m_TestActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TestActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TestActions @Test => new TestActions(this);

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_MoveForward;
    private readonly InputAction m_Player_Rotate;
    public struct PlayerActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveForward => m_Wrapper.m_Player_MoveForward;
        public InputAction @Rotate => m_Wrapper.m_Player_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @MoveForward.started += instance.OnMoveForward;
            @MoveForward.performed += instance.OnMoveForward;
            @MoveForward.canceled += instance.OnMoveForward;
            @Rotate.started += instance.OnRotate;
            @Rotate.performed += instance.OnRotate;
            @Rotate.canceled += instance.OnRotate;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MoveForward.started -= instance.OnMoveForward;
            @MoveForward.performed -= instance.OnMoveForward;
            @MoveForward.canceled -= instance.OnMoveForward;
            @Rotate.started -= instance.OnRotate;
            @Rotate.performed -= instance.OnRotate;
            @Rotate.canceled -= instance.OnRotate;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KMSchemeIndex = -1;
    public InputControlScheme KMScheme
    {
        get
        {
            if (m_KMSchemeIndex == -1) m_KMSchemeIndex = asset.FindControlSchemeIndex("KM");
            return asset.controlSchemes[m_KMSchemeIndex];
        }
    }
    public interface ITestActions
    {
        void OnTest1(InputAction.CallbackContext context);
        void OnTest2(InputAction.CallbackContext context);
        void OnTest3(InputAction.CallbackContext context);
        void OnTest4(InputAction.CallbackContext context);
        void OnTest5(InputAction.CallbackContext context);
        void OnTest6(InputAction.CallbackContext context);
        void OnTest7(InputAction.CallbackContext context);
        void OnTest8(InputAction.CallbackContext context);
        void OnTest9(InputAction.CallbackContext context);
        void OnLClick(InputAction.CallbackContext context);
        void OnRClick(InputAction.CallbackContext context);
        void OnConsole(InputAction.CallbackContext context);
        void OnConsole_Clear(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnMoveForward(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
}
