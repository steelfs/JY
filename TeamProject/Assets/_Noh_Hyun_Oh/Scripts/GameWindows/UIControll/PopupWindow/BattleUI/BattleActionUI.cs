using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleActionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler ,IEndDragHandler
{
    /// <summary>
    /// Ŭ���ؼ� �巡�׷� �̵��� �޴��� �Ⱥ����ߵǼ� �߰����غ���
    /// �巡�׻�Ȳ�� �Է��Ѵ�.
    /// </summary>
    bool isMove = false;
    /// <summary>
    /// ��������Ʈ�� ��ư ������ �̳��� ������� ��������� ������.
    /// �̳� �߰��Ϸ��� ��ư�� ��������. �׸��� Action��������Ʈ�� ����߰��������
    /// </summary>
    public enum BattleButton
    {
        Move = 0,
        Attack,
        Skill,
        Item,
        Options,
        TurnEnd,
        
    }
    Animator hideAnim;
    int str_OnViewButton;

    /// <summary>
    /// BattleButton �� �������°� ����Ʈ�� ����߰��� �ʿ��ϴ�.
    /// </summary>
    public Action<Transform>[] list;
    private void Awake()
    {
        str_OnViewButton = Animator.StringToHash("OnViewButton");
        hideAnim = transform.GetComponent<Animator>();
        Transform buttonPanel = transform.GetChild(0);
        //ButtonCreateBase<BattleButton>.SettingNoneScriptButton(buttonPanel, BattleButton.Skill,null);
        //SettingNoneScriptButton();
    }
  
    /// <summary>
    /// ��ư�� ��ũ��Ʈ�� ������������ �߾�
    /// 
    /// �̽�ũ��Ʈ �ڽ����� ��ư�� �����ϰ� �̳��߰� �׸��� �ؿ� ����ġ���� ������ ���� ����.
    /// </summary>
    //private void SettingNoneScriptButton() {
    //    //��ư �ٲ���ͼ�
    //    //Button[] ActionButtons = transform.GetComponentsInChildren<Button>(true);//�ڽ� ��ư����ͼ�
    //    //�̳��� �迭�� �޾ƿ���
    //    BattleButton[] battleButtonEnumList = (BattleButton[])Enum.GetValues(typeof(BattleButton)); // ��ư���� ��ũ��Ʈ����� �����Ƽ� �̸�ó������.
    //    //�̰��ʿ���� �׼� �������ַ��� ��� 
    //    list = new Action<Transform>[battleButtonEnumList.Length];//        
        
    //    Transform buttonPanel = transform.GetChild(0);
    //    RectTransform rt = buttonPanel.GetComponent<RectTransform>();
    //    rt.sizeDelta = new Vector2(width,height*battleButtonEnumList.Length);
    //    for (int i = 0; i < battleButtonEnumList.Length; i++) //�̳� ũ�⸸ŭ�� ������
    //    {

    //        //���ٽ����� �ѱ� ��������
    //        //int index = i; //�����ȿ��� ���ٽ��� ������ i�� ���ٽľȿ� �Ѱ��ָ� �������ε������� �޴´�
    //        //���ٽ��� �������� �������޹޴°Ծƴ϶� ������ �ּҸ� �������ϰ� �ִٰ� ������ ������ ������ �ּҸ� ã�ư� ���� �����Ѵ� .
    //        //�׷��� �ӽú����ϳ��� i���� ��� �Ѱ��ִ� �߳Ѿ�� 
    //        //������ �Ƹ� �����ȿ��� �����߱⶧���� �����ѹ��������� index ��� ������ �Ź� ���ø޸𸮿� ���λ�����̴� 
    //        //index ������ �����ϰ� ���ٽĿ��� �����ٛ��⶧���� ���ٽľȿ����� �������Ƽ� ������� ���� index �ּҰ��� �����ϰ��������̴�.
    //        //�׷��� �ٽ����������� ���ο� index ������ �����ϰ� ���ο� �ּҿ��ٰ� ����⶧���� ���ٽĿ��� index�������������� �ּҰ� �ٸ��͵��� 
    //        //������ ����� ������ �ε������� �����Ͽ� ����ε� ���� �����Ҽ��� �ִ�.
    //        //������ �÷����� �ϴ� ���ٽ��� ����index(�������鼭 �������̾�����index �ּҰ�) �� �����ϰ��ֱ⶧���� û�Ҵ�󿡼� ���ܵǴ� �������ȵȴ�.
    //        //���� index ������ �����ۿ��� �����ߴٸ� �̶��� ����εȰ��� ���޵����������̴� 
    //        //�迭 �ε����� ���ڷ� �ѱ涧 ��Ÿ������ ��ȯ�̵Ǳ⶧���� ��������Ʈ���� Ư���� ��ü�� �ѱ�������.
    //        //ref �� ����ص������� ���������� ����Ҷ� �����̰ɸ���.
    //        //ex ) �̺�Ʈ�����ʿ� �߰��ϱ����� ���ٽľ��� �������� ����������� ref out in ���� ������ ����̺Ұ����ϴ�.
    //        GameObject button = null;
    //        //���� �޾��ֱ����ؼ� �̳����� üũ.
    //        switch (battleButtonEnumList[i])
    //        {
    //            case BattleButton.Move:
    //            case BattleButton.Attack:
    //            case BattleButton.Skill:
    //            case BattleButton.Item:
    //            case BattleButton.Options:  
    //            case BattleButton.TurnEnd:
    //            case BattleButton.nextGame:
    //            case BattleButton.newGame:
    //                button = CreateButtonObject(battleButtonEnumList[i].ToString(),buttonPanel,i);
    //                break;
    //            default:
    //                break;
    //        }
    //        int index = i;
    //        if (button != null) 
    //        {
    //            //�������鼭 ���� ������ �޾��ֱ� 
    //            button.GetComponent<Button>().
    //                onClick.
    //                AddListener(() => {
    //                    list[index]?.Invoke(transform); 
    //                });  
    //        }
    //    }
    //}
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMove)
        {
            hideAnim.SetBool(str_OnViewButton, true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideAnim.SetBool(str_OnViewButton, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMove = true;
        transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMove = false;
    }
}
