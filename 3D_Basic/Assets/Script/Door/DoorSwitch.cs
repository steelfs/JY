using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���𰡸� ����ϴ� ������Ʈ
//�� ��ü�� ����ϸ� �� ��ü�� ����� �ٸ� ������Ʈ�� ���ȴ�.
public class DoorSwitch : MonoBehaviour, Iinteractable
{
    public GameObject target;//�� ����ġ�� ����� ������Ʈ
    Iinteractable useTarget;//�� ����ġ�� ����� ���ͷ��ͺ� ����

    bool isUsed = false;//�� ����ġ�� ���Ǿ����� true�� ������� false : ���� ������

    Animator anim;
    public bool IsDirectUse => true;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        bool result = false;
        if (target != null)
        {
            useTarget = target.GetComponent<Iinteractable>();//��󿡼� Iinteractable��������
            result = useTarget != null;     
        }
        if (!result)
        {
            //��󿡼� Iinteractable �� ã�� �� ������ 
            Debug.LogWarning($"{this.gameObject.name}����� �� �ִ� ������Ʈ�� �����ϴ�.");
        }
    }
    public void Use()//����ġ ����Լ� 
    {
        if (useTarget != null)// �� ����ġ�� ����� ����� �־���ϰ�
        {
            if (!isUsed) //���� �����°� �ƴҶ� 
            {
                useTarget.Use(); //Ÿ����Use�� ����ϰ� 
                StartCoroutine(ResetSwitch()); //�����ð� �� ����ġ ����
            }       
        }
    }

    IEnumerator ResetSwitch()//����ġ ���� 
    {
        isUsed = true;//����� ǥ��
        anim.SetBool("IsOpen", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("IsOpen", false);
        isUsed = false;
    }
}
