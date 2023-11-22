using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    enum NpcMoveState 
    {
        stay= 0,
        moving,
        talk,
    }
    [SerializeField]
    NpcMoveState npcState = NpcMoveState.stay;
    NpcMoveState NpcMovingState 
    {
        get => npcState;
        set 
        {
            if (npcState != value) 
            {
                npcState = value;
                StopAllCoroutines();
                switch (npcState)
                {
                    case NpcMoveState.stay:
                        lookTarget = null;
                        moveDir = Vector3.zero;
                        StartCoroutine(NextPoint());
                        break;
                    case NpcMoveState.moving:
                        lookTarget = null;
                        moveDir = wayPoints[moveIndex].position - moveObject.position; //���ⱸ�ؼ� 
                        moveDir.y = 0.0f;
                        moveObject.rotation = Quaternion.LookRotation(moveDir,moveObject.up) ;
                        StartCoroutine(NpcMoving()); //�̵���Ű�� 
                        break;
                    case NpcMoveState.talk:
                        moveDir = LookTarget.position - moveObject.position;
                        moveDir.y = 0.0f;
                        moveObject.rotation = Quaternion.LookRotation(moveDir,moveObject.up) ;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// �̵��� ��ġ��
    /// </summary>
    Transform[] wayPoints;

    /// <summary>
    /// �̵� �� ������Ʈ
    /// </summary>
    [SerializeField]
    Transform moveObject;

    /// <summary>
    /// �ٶ� Ÿ��
    /// </summary>
    [SerializeField]
    Transform lookTarget;
    public Transform LookTarget 
    {
        get 
        {
            if (lookTarget == null) 
            {
                lookTarget = getTarget?.Invoke();
            }
            return lookTarget;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public Func<Transform> getTarget;
    
    /// <summary>
    /// �̵� ��ǽ����ų ������Ʈ
    /// </summary>
    Animator moveAnimator;

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    [SerializeField]
    float moveSpeed = 1.5f;

    /// <summary>
    /// ȸ�� �ӵ� 
    /// </summary>
    //[SerializeField]
    //float rotateSpeed = 0.5f;

    /// <summary>
    /// �̵��� ��������Ʈ �ε���
    /// </summary>
    int moveIndex = 0;

    WaitForSeconds couroutineWaitTime = new(2.0f);

    /// <summary>
    /// �̵��� �������
    /// </summary>
    Vector3 moveDir;

    /// <summary>
    /// ���Ǿ� ���� 
    /// </summary>
    NpcBase_Gyu npcBase_Gyu;

    

    private void Awake()
    {
        int wayCount = transform.childCount;

        wayPoints = new Transform[wayCount];

        for (int i = 0; i < wayCount; i++)
        {
            wayPoints[i] = transform.GetChild(i);
        }

        moveObject = transform.parent.GetChild(0);
        
        npcBase_Gyu = transform.parent.GetChild(0).GetComponent<NpcBase_Gyu>();
        
        moveAnimator = transform.parent.GetChild(0).GetComponent<Animator>();
        
        //�׼� �����ϰ� 
        npcBase_Gyu.onTalkDisableButton += () => {
            NpcMovingState = NpcMoveState.moving;
        };

        //���� �ϰ� 
        npcBase_Gyu.onTalkEnableButton += (npcId) => {
            NpcMovingState = NpcMoveState.talk;
        };

    }

    private void Start()
    {
        NpcMovingState = NpcMoveState.moving;
    }


    /// <summary>
    /// NPC �̵� ó���� �ڷ�ƾ 
    /// </summary>
    IEnumerator NpcMoving()
    {
        Transform endPoint = wayPoints[moveIndex];
        Vector3 dir = moveDir.normalized;
        moveAnimator.SetBool("Move", true);
        while ((endPoint.position - moveObject.position).magnitude > 0.4f)
        {
            moveObject.position += Time.deltaTime * moveSpeed * dir ;
            yield return null;
        }
        moveObject.position = endPoint.position;
        moveAnimator.SetBool("Move", false);
        NpcMovingState = NpcMoveState.stay;
    }

    /// <summary>
    /// ���� �ε��� ����� �����ð� ������ �̵����� �����Ű�� �ڷ�ƾ
    /// </summary>
    IEnumerator NextPoint()
    {
        //���� �ε��� ���
        moveIndex++;
        moveIndex %= wayPoints.Length;
        yield return couroutineWaitTime;
        NpcMovingState = NpcMoveState.moving;
        //Debug.Log(moveIndex);
    }

  
}
