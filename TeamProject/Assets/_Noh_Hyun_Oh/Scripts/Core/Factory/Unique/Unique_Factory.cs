using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �Ѱ��� �����Ǵ� ��ü���� ���������� ����� Ŭ���� 
/// ���������� ó���ؾߵ� �����ۼ�
/// �̸��� ���丮....Prefab �̶����̴°Գ���������..
/// </summary>
public class Unique_Factory :ChildComponentSingeton<Unique_Factory>
{

    ///// <summary>
    ///// �ɼ�â ���� ����
    ///// ��Ÿ�ɼ� ��  ���� �ҷ����� ���� ��
    ///// </summary>
    //[SerializeField]
    //private GameObject optionWindow;

    ///// <summary>
    ///// �÷��̾� ����â ��������
    ///// �κ�, �������ͽ� , ��ų ��
    ///// </summary>
    //[SerializeField]
    //private GameObject playerWindow;

    ///// <summary>
    ///// ���÷��̾� ����â �������� 
    ///// ���� , ũ������ , ��ȭâ? �� 
    ///// </summary>
    //[SerializeField]
    //private GameObject nonPlayerWindow;

    ///// <summary>
    ///// �ε�ȭ�鿡 ���� �����
    ///// </summary>
    //[SerializeField]
    //private GameObject progressList;

    ///// <summary>
    ///// �⺻ ������� 
    ///// </summary>
    //[SerializeField]
    //private GameObject defaultBGM;

    ///// <summary>
    ///// â���� Ŭ���̳� ���ý� �Ҹ� 
    ///// </summary>
    //[SerializeField]
    //private GameObject SystemEffectSound;

    ///// <summary>
    ///// �ʿ��ѿ�����Ʈ �������� ���. 
    ///// prefab ������Ʈ�� �Ѱ��ֱ⶧���� 
    ///// ����������Ʈ �������� �����̾ȵȴ�.
    ///// </summary>
    ///// <param name="type">������Ʈ ����</param>
    ///// <returns>prefab ������Ʈ �ٷο������ֱ�</returns>
    //public GameObject GetObject(EnumList.UniqueFactoryObjectList type) {

    //    switch (type) {
    //        case EnumList.UniqueFactoryObjectList.OPTIONS_WINDOW:
    //            return optionWindow;
    //        case EnumList.UniqueFactoryObjectList.PLAYER_WINDOW:
    //            return playerWindow;
    //        case EnumList.UniqueFactoryObjectList.NON_PLAYER_WINDOW:
    //            return nonPlayerWindow;
    //        case EnumList.UniqueFactoryObjectList.PROGRESS_LIST:
    //            return progressList;
    //        case EnumList.UniqueFactoryObjectList.DEFAULT_BGM:
    //            return defaultBGM;
    //        default:
    //            return null;
    //    }
    //}

  
}
