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
            ""id"": ""ecb3839d-5855-445c-82ad-dd7c78845f62"",
            ""actions"": [
                {
                    ""name"": ""Test1"",
                    ""type"": ""Button"",
                    ""id"": ""21cf93e6-d9fe-4dc4-b4de-59d26b71bd7f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test2"",
                    ""type"": ""Button"",
                    ""id"": ""301a4591-1332-45df-9b84-65e484e42e8e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test3"",
                    ""type"": ""Button"",
                    ""id"": ""2013375f-cfbe-4371-9ad6-826d99a8cfa1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test4"",
                    ""type"": ""Button"",
                    ""id"": ""ee865e38-d690-4867-80ea-97595f388a7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test5"",
                    ""type"": ""Button"",
                    ""id"": ""6532ada7-df08-4d68-8bfe-676059f311d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test6"",
                    ""type"": ""Button"",
                    ""id"": ""d2314baf-272f-4cc8-a035-73ce84abc10c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test7"",
                    ""type"": ""Button"",
                    ""id"": ""efe476a8-6101-4de1-af4b-7567f67f4be6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test8"",
                    ""type"": ""Button"",
                    ""id"": ""7885bd0c-0ee9-40bf-93fe-63096713beb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test9"",
                    ""type"": ""Button"",
                    ""id"": ""10a806e2-51af-46c0-9ad4-da68189c245c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LClick"",
                    ""type"": ""Button"",
                    ""id"": ""d1fe2a17-0cfa-4462-996a-eaf6eec9bf24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""582c18a9-c157-4bfe-90c8-137ca1e27e01"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7ffe549-596f-4172-abd5-7591596237b2"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b31ac41f-5faf-4c20-bcc6-742cbd576466"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2dc03d13-6a16-44f7-b372-e50d21413e04"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""896da977-d305-4f63-9d88-ac5d68c82a4b"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3666d67b-bb87-4dce-94b3-ce7956614174"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4089d596-ad10-477e-8ef2-7a39add5b368"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a26d0793-2ed2-4c2e-9ed8-33fae39f787a"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test8"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f7ead2d-af16-4ddc-bef4-46543106a8c6"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Test9"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""064e0e3f-331a-4c6d-ba33-ecb550c39c90"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""LClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
    }
}
