using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// InputSystem 의 이벤트들을 특정상황별로 그룹화 하기위한 이넘
/// 총 8종류의 개별 상황을 설정할수있다.
/// 1111 1111  2진법 플래그로 관리
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,                       // 아무것도 설정안된 값 

    Use_Common = 1,                 // 공통으로 사용하기위한 값
    Use_TownMap = 2,                // 마을 에서 사용하기위한 값                   
    Use_BattleMap = 4,              // 배틀맵 에서 사용하기위한 값
    Use_InvenView = 8,              // 인벤창 사용하기위한 값
    QuickSlot = 16,
    Use_Custom02 = 32,
    Use_Custom03 = 64,
    Use_Custom04 = 128,
    
    Cancel_Custom04 = 127,
    Cancel_Custom03 = 191,
    Cancel_Custom02 = 223,
    Cancel_Custom01 = 239,
    Cancel_InvenView = 247,         // 인벤 입력을 제외하기위한 값셋팅
    Cancel_BattleMap = 251,         // 배틀맵에서 벗어나서(씬이동) 입력을 제외하기위한 값셋팅
    Cancel_TownMap = 253,           // 마을맵에서 벗어나서(씬이동) 입력을 제외하기위한 값셋팅
    Cancel_Common = 254,            // 공통 옵션의 입력을 제외하기위한 값셋팅
}

/// <summary>
/// 캔버스에 해당 클래스가 추가되고 캔버스는 화면전체영역으로 키운다. - 이벤트 핸들링 범위를 전체화면으로 하기위해 사이즈조절필요 
/// InputSystem 은 new 시점에서 관리 가능함으로 해당 클래스에서 관리하도록 설정한다.

/// MonoBehaviour 의 내장함수 On 관련 함수들은 각 클래스내부적으로 이밴트가 핸들링 되기때문에 가져올수없다 
/// EventSystem 의 이벤트함수 를사용하기위한 인터페이스류도 각 클래스에 이벤트가 핸들링되기때문에 못가져온다.

/// 
/// </summary>
public class InputSystemController : ChildComponentSingeton<InputSystemController>
{
    
    /// <summary>
    /// 전체적으로 인풋시스템을 관리할 변수 
    /// </summary>
    InputKeyMouse inputSystem;
    static public InputKeyMouse InputSystem => Instance.inputSystem;
    /// <summary>
    /// 현재 컨트롤할 내용을 판단할 이넘값
    /// </summary>
    [SerializeField]
    HotKey_Use hotKey = HotKey_Use.None;
    public HotKey_Use HotKey => hotKey;
    // Player 액션맵  
    public Action<Vector2> OnPlayer_Move;
    public Action OnPlayer_Jump;
    public Action OnPlayer_MoveMode_Change;
    public Action OnPlayer_Run;

    //UI_Inven 액션맵

    public Action OnUI_Inven_DoubleClick;
    public Action OnUI_Inven_ItemPickUp;
    public Action OnUI_Inven_Click;
    public Action OnUI_Inven_Click_Cancel;
    public Action<InputAction.CallbackContext> OnUI_Inven_Shift; //per 에서한번 cancel한번 
    public Action OnUI_Inven_EquipBox_Open;
    public Action OnUI_SkillBox_Open;
    public Action OnUI_Inven_Inven_Open;
    public Action OnUI_Inven_MouseClickRight;
    public Action On_StatusOpen;
    //QuickSlot

    public Action OnQuickSlot_Popup;
    public Action OnQuickSlot_Shift;
    public Action OnQuickSlot_Eight;
    public Action OnQuickSlot_Nine;
    public Action OnQuickSlot_Zero;
    public Action OnQuickSlot_Ctrl;
    public Action OnQuickSlot_Alt;
    public Action OnQuickSlot_Space;
    public Action OnQuickSlot_Insert;

    //Camera 액션맵

    public Action OnCamera_RightRotate;
    public Action OnCamera_LeftRotate;

    //BattleMap_Player 액션맵

    public Action OnBattleMap_Player_UnitMove;

    //Common 액션맵

    public Action On_Options_Options;
    public Action On_Common_Esc;

    //UI_ModalPopup 액션맵
    //public Action OnUI_ModalPopup_OnOff;


    protected override void Awake()
    {
        base.Awake();
        InputSystemActionSetting();
        
    }

    /// <summary>
    /// 인풋시스템에서 사용될 액션들 등록하는 함수 
    /// </summary>
    private void InputSystemActionSetting() 
    {
        inputSystem = new();

        inputSystem.Common.Enable();                //공통부분은 항상열어두자.

        inputSystem.Player.Move.performed += (context) => { OnPlayer_Move?.Invoke(context.ReadValue<Vector2>()); };
        inputSystem.Player.Jump.performed += (_) => { OnPlayer_Jump?.Invoke(); };
        inputSystem.Player.MoveMode_Change.performed += (_) => { OnPlayer_MoveMode_Change?.Invoke(); };
        inputSystem.Player.Run.performed += (_) => { OnPlayer_Run?.Invoke(); };

        inputSystem.Mouse.MouseClickRight.performed += (_) => { OnUI_Inven_MouseClickRight?.Invoke(); };


        inputSystem.UI_Inven.DoubleClick.performed += (_) => { OnUI_Inven_DoubleClick?.Invoke(); };
        inputSystem.UI_Inven.ItemPickUp.performed += (_) => { OnUI_Inven_ItemPickUp?.Invoke(); };
        inputSystem.UI_Inven.Click.performed += (_) => { OnUI_Inven_Click?.Invoke(); };
        inputSystem.UI_Inven.Click.canceled += (_) => { OnUI_Inven_Click_Cancel?.Invoke(); };
        inputSystem.UI_Inven.Shift.performed += (context) => { OnUI_Inven_Shift?.Invoke(context); };
        inputSystem.UI_Inven.Shift.canceled += (context) => { OnUI_Inven_Shift?.Invoke(context); };
        inputSystem.UI_Inven.EquipBox_Open.performed += (_) => { OnUI_Inven_EquipBox_Open?.Invoke(); };
        inputSystem.UI_Inven.InvenKey.performed += (_) => { OnUI_Inven_Inven_Open?.Invoke(); };
        inputSystem.UI_Inven.SkillBox_Open.performed += (_) => { OnUI_SkillBox_Open?.Invoke(); };
        inputSystem.UI_Inven.Status_Open.performed += (_) => On_StatusOpen?.Invoke(); 


        inputSystem.Camera.RightRotate.performed += (_) => { OnCamera_RightRotate?.Invoke(); };
        inputSystem.Camera.LeftRotate.performed += (_) => { OnCamera_LeftRotate?.Invoke(); };


        inputSystem.QuickSlot.PopUp.performed += (_) => { OnQuickSlot_Popup?.Invoke(); };
        inputSystem.QuickSlot.Shift.performed += (_) => { OnQuickSlot_Shift?.Invoke(); };
        inputSystem.QuickSlot.Eight.performed += (_) => { OnQuickSlot_Eight?.Invoke(); };
        inputSystem.QuickSlot.Nine.performed += (_) => { OnQuickSlot_Nine?.Invoke(); };
        inputSystem.QuickSlot.Zero.performed += (_) => { OnQuickSlot_Zero?.Invoke(); };
        inputSystem.QuickSlot.Ctrl.performed += (_) => { OnQuickSlot_Ctrl?.Invoke(); };
        inputSystem.QuickSlot.Alt.performed += (_) => { OnQuickSlot_Alt?.Invoke(); };
        inputSystem.QuickSlot.Space.performed += (_) => { OnQuickSlot_Space?.Invoke(); };
        inputSystem.QuickSlot.Insert.performed += (_) => { OnQuickSlot_Insert?.Invoke(); };


        inputSystem.BattleMap_Player.UnitMove.performed += (_) => { OnBattleMap_Player_UnitMove?.Invoke(); };


        inputSystem.Common.Esc.performed += (_) => { On_Common_Esc?.Invoke(); };


        inputSystem.Options.Options.performed += (_) => { On_Options_Options?.Invoke(); };


        //inputSystem.UI_ModalPopup.OnOff.performed += (_) => { OnUI_ModalPopup_OnOff(); }; //모달 창 따로 할까 생각중..
    }
    /// <summary>
    /// 인풋 시스템 연결하는 로직 
    /// </summary>
    /// <param name="enable_Key">현재 연결할 상황</param>
    public void EnableHotKey(HotKey_Use enable_Key) 
    {
        switch (enable_Key)
        {
            case HotKey_Use.Use_BattleMap:                                  //배틀맵 진입시
                inputSystem.BattleMap_Player.Enable();
                inputSystem.Options.Enable();
                inputSystem.Camera.Enable();
                inputSystem.Mouse.Enable();
                break;
            
            case HotKey_Use.Use_TownMap:                                    // 마을 진입시
                inputSystem.Options.Enable();
                inputSystem.Player.Enable();
                break;
            
            case HotKey_Use.Use_InvenView:                                  //인벤 사용가능 할시
                inputSystem.UI_Inven.Enable();
                break;

            case HotKey_Use.QuickSlot:
                inputSystem.QuickSlot.Enable();
                break;

            case HotKey_Use.Use_Custom02:
                break;

            case HotKey_Use.Use_Custom03:
                break;

            case HotKey_Use.Use_Custom04:
                break;

            default:
                Debug.Log($"입력값이 추가할수 없는값 입니다 값 :{enable_Key.ToString()}" );
                return;
        }
        hotKey |= enable_Key; //설정된 값 추가한다.
    }

    /// <summary>
    /// 인풋 시스템 연결끊는 로직
    /// </summary>
    /// <param name="disable_Key"></param>
    public void DisableHotKey(HotKey_Use disable_Key) 
    {
        //Debug.Log($"Disable 입력 키 : {Convert.ToString((int)disable_Key,2)} , 현재 키 :{Convert.ToString((int)hotKey,2)}");
        switch (disable_Key)
        {
            case HotKey_Use.None:                                                   //로딩,오프닝,타이틀  에서사용 열려있는 inputSystem 전부 끄고 필요한것만키기
                hotKey = HotKey_Use.None;                                           //키값 수정하고 & 하나 안하나 결과는같으니 기냥 대입하자

                ReadOnlyArray<InputActionMap> temp = inputSystem.asset.actionMaps;  //열려있는 액션맵 전부찾아서 
                foreach (InputActionMap action in temp)
                {
                    action.Disable();                                               //닫아버려
                }
                inputSystem.Common.Enable();                                        //항시 사용할 공통부분만 연다.
#if UNITY_EDITOR
                inputSystem.Test.Enable();                                          //포문돌면서 다닫기때문에 테스트도 닫혀버린다.. 그러니 열어두자
#endif
                break;
            case HotKey_Use.Cancel_BattleMap:                                       //배틀맵에서 벗어낫을때  
                hotKey &= HotKey_Use.Cancel_BattleMap;  //값수정하고

                inputSystem.Options.Disable();
                inputSystem.BattleMap_Player.Disable();                             //배틀맵에서 플레이어가 사용할 인풋관련키고
                inputSystem.Camera.Disable();                                       //카메라 회전관련 인풋 키고

                break;
            case HotKey_Use.Cancel_TownMap:                                         //마을에서 벗어낫을때  
                hotKey &= HotKey_Use.Cancel_TownMap;    //값수정하고

                inputSystem.Options.Disable();
                inputSystem.Player.Disable();                                       //wasd 와 점프 같은기능을 사용할수있게 킨다.

                break;
            case HotKey_Use.Cancel_InvenView:                                       // 인벤 사용못하게 할때
                hotKey &= HotKey_Use.Cancel_InvenView;  //값수정하고

                inputSystem.UI_Inven.Disable();                                     // 아이템창 사용할수있게 키고
                inputSystem.QuickSlot.Disable();                                    // 퀵슬롯 도 같이킨다.

                break;
            case HotKey_Use.Cancel_Custom01:
                break;      
            case HotKey_Use.Cancel_Custom02:
                break;      
            case HotKey_Use.Cancel_Custom03:
                break;      
            case HotKey_Use.Cancel_Custom04:
                break;
            default:
                Debug.Log($"입력값이 제거할수 없는값 입니다 값 :{disable_Key.ToString()}" );
                Debug.Log("byte 형식의 enum 으로 switch 할시에 0000 0101 이런값으로 체크하는데 딱맞는 값만 걸린다.");
                break;
        }
        //Debug.Log($"Disable 변경된 키 :{Convert.ToString((int)hotKey,2)}");
    }

}
