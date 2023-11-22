
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����̻� ������ UI�� ���� �������̽� 
/// </summary>
public interface IStateData
{
    /// <summary>
    /// ������Ʈ ���� ���ǵ� ������Ƽ ã�� ����
    /// </summary>
    GameObject gameObject { get; }
    /// <summary>
    /// �����̻� Ÿ��
    /// </summary>
    //EnumList.StateType Type { get; set; }
    
    /// <summary>
    /// ��ų ������
    /// </summary>
    SkillData SkillData { get; set; }
    /// <summary>
    /// ���ϴ� ���ҵǴ� ��ġ 
    /// </summary>
    float ReducedDuration { get; set; }

    /// <summary>
    /// �ִ� ���ӽð�
    /// </summary>
    float MaxDuration { get; set; }
    /// <summary>
    /// ���� ���ӽð�
    /// </summary>
    float CurrentDuration { get; set; }

    /// <summary>
    /// ���� �ʱ�ȭ �ϱ����� �Լ�
    /// �ʿ� ��� Ǯ�� ��ġ�ű�� , ��� ���� null ���� float �� -1���� type �� none �� ����
    /// </summary>
    void ResetData();
}