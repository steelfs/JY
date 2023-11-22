using System;
using UnityEngine;
using UnityEngine.UI;
public class MoveActionButton : BattleActionButtonBase
{
    /// <summary>
    /// �ϴ� ���ư��Ը���� ���߿� �ð�������� 
    /// </summary>
    static bool isMoveButtonClick= false;
    public static bool IsMoveButtonClick 
    {
        get => isMoveButtonClick;
        set 
        {
            if (isMoveButtonClick != value) 
            {
                isMoveButtonClick = value;
                //��ư Ŭ������ �����̿���
                isButtonClick?.Invoke(value);
            }
        }
    }
    public static Action<bool> isButtonClick;

    Image backgroundImg;
    Color backColorOrigin;
    protected override void Awake()
    {
        base.Awake();
        backgroundImg = transform.GetChild(0).GetComponent<Image>();
        isButtonClick = OnClickCheckImageView;
        backColorOrigin = backgroundImg.color;
    }

    protected override void OnClick()
    {
        if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //���� ������ üũ�ϰ� ����ȯ�����ϸ�true �ϱ� �Ʊ���
        {
            BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //�Ʊ����̸� �Ʊ������� �������������� �׳�����ȯ��Ų��.

            //���ݹ��� ����
            if (!SpaceSurvival_GameManager.Instance.AttackRange.isAttacRange) //���ݾ��ϰ�������   
            {
                ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // �� ������Ʈã�Ƽ� 
                if (player == null)
                {
                    Debug.LogWarning("������ �����̾����ϴ�");
                    return;
                }
                if (!player.IsMoveCheck) //�̵����� �ƴѰ�츸  
                {
                    float moveSize = player.CharcterData.Player_Status.Stamina > player.MoveSize ? player.MoveSize : player.CharcterData.Player_Status.Stamina;
                    SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(player.CurrentTile);
                    SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(player.CurrentTile, moveSize);//�̵�����ǥ�����ֱ� 
                    isMoveButtonClick = true;
                }
            }
            else //���� ���¸� 
            {
                SpaceSurvival_GameManager.Instance.To_AttackRange_From_MoveRange();
                isMoveButtonClick = true;
            }

        }
        GameManager.Inst.ChangeCursor(false);
    }

    protected override void OnMouseEnter()
    {
        uiController.ViewButtons();
    }
    protected override void OnMouseExit()
    {
        uiController.ResetButtons();
    }

    private void OnClickCheckImageView(bool isClick)
    {
        if (isClick)
        {
            backgroundImg.color = Color.black;
        }
        else 
        {
            backgroundImg.color = backColorOrigin;
        }

    }
}
