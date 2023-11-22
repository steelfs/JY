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
/// InputSystem �� �̺�Ʈ���� Ư����Ȳ���� �׷�ȭ �ϱ����� �̳�
/// �� 8������ ���� ��Ȳ�� �����Ҽ��ִ�.
/// 1111 1111  2���� �÷��׷� ����
/// </summary>
[Flags]
public enum HotKey_Use : byte
{
    None = 0,                       // �ƹ��͵� �����ȵ� �� 

    Use_Common = 1,                 // �������� ����ϱ����� ��
    Use_TownMap = 2,                // ���� ���� ����ϱ����� ��                   
    Use_BattleMap = 4,              // ��Ʋ�� ���� ����ϱ����� ��
    Use_InvenView = 8,              // �κ�â ����ϱ����� ��
    QuickSlot = 16,
    Use_Custom02 = 32,
    Use_Custom03 = 64,
    Use_Custom04 = 128,
    
    Cancel_Custom04 = 127,
    Cancel_Custom03 = 191,
    Cancel_Custom02 = 223,
    Cancel_Custom01 = 239,
    Cancel_InvenView = 247,         // �κ� �Է��� �����ϱ����� ������
    Cancel_BattleMap = 251,         // ��Ʋ�ʿ��� �����(���̵�) �Է��� �����ϱ����� ������
    Cancel_TownMap = 253,           // �����ʿ��� �����(���̵�) �Է��� �����ϱ����� ������
    Cancel_Common = 254,            // ���� �ɼ��� �Է��� �����ϱ����� ������
}

/// <summary>
/// ĵ������ �ش� Ŭ������ �߰��ǰ� ĵ������ ȭ����ü�������� Ű���. - �̺�Ʈ �ڵ鸵 ������ ��üȭ������ �ϱ����� �����������ʿ� 
/// InputSystem �� new �������� ���� ���������� �ش� Ŭ�������� �����ϵ��� �����Ѵ�.

/// MonoBehaviour �� �����Լ� On ���� �Լ����� �� Ŭ�������������� �̹�Ʈ�� �ڵ鸵 �Ǳ⶧���� �����ü����� 
/// EventSystem �� �̺�Ʈ�Լ� ������ϱ����� �������̽����� �� Ŭ������ �̺�Ʈ�� �ڵ鸵�Ǳ⶧���� �������´�.

/// 
/// </summary>
public class InputSystemController : ChildComponentSingeton<InputSystemController>
{
    
    /// <summary>
    /// ��ü������ ��ǲ�ý����� ������ ���� 
    /// </summary>
    InputKeyMouse inputSystem;
    static public InputKeyMouse InputSystem => Instance.inputSystem;
    /// <summary>
    /// ���� ��Ʈ���� ������ �Ǵ��� �̳Ѱ�
    /// </summary>
    [SerializeField]
    HotKey_Use hotKey = HotKey_Use.None;
    public HotKey_Use HotKey => hotKey;
    // Player �׼Ǹ�  
    public Action<Vector2> OnPlayer_Move;
    public Action OnPlayer_Jump;
    public Action OnPlayer_MoveMode_Change;
    public Action OnPlayer_Run;

    //UI_Inven �׼Ǹ�

    public Action OnUI_Inven_DoubleClick;
    public Action OnUI_Inven_ItemPickUp;
    public Action OnUI_Inven_Click;
    public Action OnUI_Inven_Click_Cancel;
    public Action<InputAction.CallbackContext> OnUI_Inven_Shift; //per �����ѹ� cancel�ѹ� 
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

    //Camera �׼Ǹ�

    public Action OnCamera_RightRotate;
    public Action OnCamera_LeftRotate;

    //BattleMap_Player �׼Ǹ�

    public Action OnBattleMap_Player_UnitMove;

    //Common �׼Ǹ�

    public Action On_Options_Options;
    public Action On_Common_Esc;

    //UI_ModalPopup �׼Ǹ�
    //public Action OnUI_ModalPopup_OnOff;


    protected override void Awake()
    {
        base.Awake();
        InputSystemActionSetting();
        
    }

    /// <summary>
    /// ��ǲ�ý��ۿ��� ���� �׼ǵ� ����ϴ� �Լ� 
    /// </summary>
    private void InputSystemActionSetting() 
    {
        inputSystem = new();

        inputSystem.Common.Enable();                //����κ��� �׻󿭾����.

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


        //inputSystem.UI_ModalPopup.OnOff.performed += (_) => { OnUI_ModalPopup_OnOff(); }; //��� â ���� �ұ� ������..
    }
    /// <summary>
    /// ��ǲ �ý��� �����ϴ� ���� 
    /// </summary>
    /// <param name="enable_Key">���� ������ ��Ȳ</param>
    public void EnableHotKey(HotKey_Use enable_Key) 
    {
        switch (enable_Key)
        {
            case HotKey_Use.Use_BattleMap:                                  //��Ʋ�� ���Խ�
                inputSystem.BattleMap_Player.Enable();
                inputSystem.Options.Enable();
                inputSystem.Camera.Enable();
                inputSystem.Mouse.Enable();
                break;
            
            case HotKey_Use.Use_TownMap:                                    // ���� ���Խ�
                inputSystem.Options.Enable();
                inputSystem.Player.Enable();
                break;
            
            case HotKey_Use.Use_InvenView:                                  //�κ� ��밡�� �ҽ�
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
                Debug.Log($"�Է°��� �߰��Ҽ� ���°� �Դϴ� �� :{enable_Key.ToString()}" );
                return;
        }
        hotKey |= enable_Key; //������ �� �߰��Ѵ�.
    }

    /// <summary>
    /// ��ǲ �ý��� ������� ����
    /// </summary>
    /// <param name="disable_Key"></param>
    public void DisableHotKey(HotKey_Use disable_Key) 
    {
        //Debug.Log($"Disable �Է� Ű : {Convert.ToString((int)disable_Key,2)} , ���� Ű :{Convert.ToString((int)hotKey,2)}");
        switch (disable_Key)
        {
            case HotKey_Use.None:                                                   //�ε�,������,Ÿ��Ʋ  ������� �����ִ� inputSystem ���� ���� �ʿ��Ѱ͸�Ű��
                hotKey = HotKey_Use.None;                                           //Ű�� �����ϰ� & �ϳ� ���ϳ� ����°����� ��� ��������

                ReadOnlyArray<InputActionMap> temp = inputSystem.asset.actionMaps;  //�����ִ� �׼Ǹ� ����ã�Ƽ� 
                foreach (InputActionMap action in temp)
                {
                    action.Disable();                                               //�ݾƹ���
                }
                inputSystem.Common.Enable();                                        //�׽� ����� ����κи� ����.
#if UNITY_EDITOR
                inputSystem.Test.Enable();                                          //�������鼭 �ٴݱ⶧���� �׽�Ʈ�� ����������.. �׷��� �������
#endif
                break;
            case HotKey_Use.Cancel_BattleMap:                                       //��Ʋ�ʿ��� �������  
                hotKey &= HotKey_Use.Cancel_BattleMap;  //�������ϰ�

                inputSystem.Options.Disable();
                inputSystem.BattleMap_Player.Disable();                             //��Ʋ�ʿ��� �÷��̾ ����� ��ǲ����Ű��
                inputSystem.Camera.Disable();                                       //ī�޶� ȸ������ ��ǲ Ű��

                break;
            case HotKey_Use.Cancel_TownMap:                                         //�������� �������  
                hotKey &= HotKey_Use.Cancel_TownMap;    //�������ϰ�

                inputSystem.Options.Disable();
                inputSystem.Player.Disable();                                       //wasd �� ���� ��������� ����Ҽ��ְ� Ų��.

                break;
            case HotKey_Use.Cancel_InvenView:                                       // �κ� �����ϰ� �Ҷ�
                hotKey &= HotKey_Use.Cancel_InvenView;  //�������ϰ�

                inputSystem.UI_Inven.Disable();                                     // ������â ����Ҽ��ְ� Ű��
                inputSystem.QuickSlot.Disable();                                    // ������ �� ����Ų��.

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
                Debug.Log($"�Է°��� �����Ҽ� ���°� �Դϴ� �� :{disable_Key.ToString()}" );
                Debug.Log("byte ������ enum ���� switch �ҽÿ� 0000 0101 �̷������� üũ�ϴµ� ���´� ���� �ɸ���.");
                break;
        }
        //Debug.Log($"Disable ����� Ű :{Convert.ToString((int)hotKey,2)}");
    }

}
