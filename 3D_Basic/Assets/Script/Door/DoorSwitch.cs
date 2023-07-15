using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무언가를 사용하는 오브젝트
//이 물체를 사용하면 이 물체에 연결된 다른 오브젝트가 사용된다.
public class DoorSwitch : MonoBehaviour, Iinteractable
{
    public GameObject target;//이 스위치가 사용할 오브젝트
    Iinteractable useTarget;//이 스위치가 사용할 인터렉터블 참조

    bool isUsed = false;//이 스위치가 사용되었는지 true면 사용했음 false : 아직 사용안함

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
            useTarget = target.GetComponent<Iinteractable>();//대상에서 Iinteractable가져오기
            result = useTarget != null;     
        }
        if (!result)
        {
            //대상에서 Iinteractable 을 찾을 수 없으면 
            Debug.LogWarning($"{this.gameObject.name}사용할 수 있는 오브제트가 없습니다.");
        }
    }
    public void Use()//스위치 사용함수 
    {
        if (useTarget != null)// 이 스위치가 사용할 대상이 있어야하고
        {
            if (!isUsed) //만약 사용상태가 아닐때 
            {
                useTarget.Use(); //타깃의Use를 사용하고 
                StartCoroutine(ResetSwitch()); //일정시간 뒤 스위치 리셋
            }       
        }
    }

    IEnumerator ResetSwitch()//스위치 리셋 
    {
        isUsed = true;//사용중 표시
        anim.SetBool("IsOpen", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("IsOpen", false);
        isUsed = false;
    }
}
