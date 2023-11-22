using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// ��Ʋ �ʿ��� ���ֵ��� ������ �־���� �������̽� �ʿ��ϸ� �߰�����
/// </summary>
public interface ICharcterBase 
{
    public Transform transform { get; }
    /// <summary>
    /// ���� ��Ʈ�� ������ �����´�
    /// </summary>
    public bool IsControll { get; set; }
    /// <summary>
    /// �̵� ���װ� �־ �߰� �̵� �������� üũ�ϴ� ������Ƽ 
    /// </summary>
    public bool IsMoveCheck { get;}

    /// <summary>
    /// ������ UI ĳ�̿� ������Ƽ
    /// </summary>
    TrackingBattleUI BattleUI { get; set; }

    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform BattleUICanvas { get;  }

    /// <summary>
    /// ���� ĳ���Ͱ� �ִ� Ÿ�� 
    /// </summary>
    Tile CurrentTile { get; }

    /// <summary>
    /// �ܺο��� Ÿ�� ���ÿ�
    /// </summary>
    Func<Tile> GetCurrentTile { get; set; }

    
    /// <summary>
    /// ĳ���Ͱ� �̵��Ҽ��ִ� �Ÿ� (�ൿ�°��� �Ѱ��൵��)
    /// </summary>
    float MoveSize { get; }

    /// <summary>
    /// �������� ������� �ʱ�ȭ�� �Լ�
    /// </summary>
    public void ResetData();

    /// <summary>
    /// ĳ������ �̵��Լ�
    /// </summary>
    public IEnumerator CharcterMove(Tile selectedTile);

    /// <summary>
    /// ĳ������ �����Լ�
    /// </summary>
    public IEnumerator CharcterAttack(Tile selectedTile);

    /// <summary>
    /// ���� ���� �ȿ� �ִ��� üũ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool IsAttackRange();

}
