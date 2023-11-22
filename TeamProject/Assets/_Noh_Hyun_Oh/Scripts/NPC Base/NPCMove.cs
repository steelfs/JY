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
                        moveDir = wayPoints[moveIndex].position - moveObject.position; //방향구해서 
                        moveDir.y = 0.0f;
                        moveObject.rotation = Quaternion.LookRotation(moveDir,moveObject.up) ;
                        StartCoroutine(NpcMoving()); //이동시키고 
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
    /// 이동할 위치들
    /// </summary>
    Transform[] wayPoints;

    /// <summary>
    /// 이동 할 오브젝트
    /// </summary>
    [SerializeField]
    Transform moveObject;

    /// <summary>
    /// 바라볼 타겟
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
    /// 이동 모션실행시킬 컴포넌트
    /// </summary>
    Animator moveAnimator;

    /// <summary>
    /// 이동 속도
    /// </summary>
    [SerializeField]
    float moveSpeed = 1.5f;

    /// <summary>
    /// 회전 속도 
    /// </summary>
    //[SerializeField]
    //float rotateSpeed = 0.5f;

    /// <summary>
    /// 이동할 웨이포인트 인덱스
    /// </summary>
    int moveIndex = 0;

    WaitForSeconds couroutineWaitTime = new(2.0f);

    /// <summary>
    /// 이동할 방향백터
    /// </summary>
    Vector3 moveDir;

    /// <summary>
    /// 엔피씨 정보 
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
        
        //액션 연결하고 
        npcBase_Gyu.onTalkDisableButton += () => {
            NpcMovingState = NpcMoveState.moving;
        };

        //연결 하고 
        npcBase_Gyu.onTalkEnableButton += (npcId) => {
            NpcMovingState = NpcMoveState.talk;
        };

    }

    private void Start()
    {
        NpcMovingState = NpcMoveState.moving;
    }


    /// <summary>
    /// NPC 이동 처리용 코루틴 
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
    /// 다음 인덱스 계산후 일정시간 지난후 이동로직 실행시키는 코루틴
    /// </summary>
    IEnumerator NextPoint()
    {
        //다음 인덱스 계산
        moveIndex++;
        moveIndex %= wayPoints.Length;
        yield return couroutineWaitTime;
        NpcMovingState = NpcMoveState.moving;
        //Debug.Log(moveIndex);
    }

  
}
