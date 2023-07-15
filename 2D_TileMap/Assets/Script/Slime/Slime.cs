using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : PooledObject
{
    //등장시 
    //아웃라인의 두깨는 0
    //disolve의 Fade는 1
    //Phase의 Split은 0
    //등장 직후 Phase의 Split이 0에서 1로 변한다.
    //플레이어에게 공격당할 수 있는 상태가 되면 두께는 0.005가 된다.
    //죽으면 disolve의 Fade가 1 -> 0으로 변화한다

    SpriteRenderer spriteRenderer;
    Material mainMaterial;

    Node current = null; // 이 슬라임이 위치하고 있는 노드
    Node Current
    {
        get => current;
        set
        {
            if (current != value)
            {
                if (current != null)//처음 생성시  null이라 어쩔 수 없이 추가
                {
                    current.nodeType = Node.NodeType.Plain; //이전 노드를 Plain으로 되돌리기
                }
                current = value;
                if (current != null)//마지막 null로 셋팅됐을 때 대비해서 추가
                {
                    current.nodeType = Node.NodeType.Monster;
                }
              

                spriteRenderer.sortingOrder = -Mathf.FloorToInt(transform.position.y * 100);// 아랫쪽에 있는 슬라임이 위에 그려지도록 순서 조정
            }
        }
    }

    float pathWaitTime = 0.0f;//다른 슬라임에 의해서 경로가 막혔을 때 기다리는 시간 
    const float MaxPathWaitTime = 1.0f;//최대로 기다리는 시간

    GridMap map;//이 슬라임이 있는 그리드맵

    List<Vector2Int> path = new List<Vector2Int>();//슬라임이 이동할 경로 

    PathLine pathLine;//이동할 경로를 그리는 클래스
    public PathLine PathLine => pathLine; //읽기전용 프로퍼티 
    Vector2Int gridPosition => map.WorldToGrid(transform.position);//슬라임의 좌표확인용 프로퍼티

    public bool isShowPathLine = false;//PathLine이 보일지 안보일지 결정하는 변수 

    bool isActivate = false;//슬라임이 활동중인지 표시용 

    public float phaseDuration = 0.5f;//페이즈 진행시간
    public float disolveDuration = 1.0f;// 진행시간

    public float moveSpeed = 2.0f;

    public float lifeTimeBonus = 3.0f;

    const float visibleOutLineThickNess = 0.005f;//아웃라인이 보일때 설정하 두께
    const float visiblePhaseThickNess = 0.1f;//페이즈 진행시 두께

    //셰이더 프로퍼티를 아이디로 미리 구해놓기
    readonly int outLineThickness = Shader.PropertyToID("_OutLineThickness");
    readonly int phaseSplit = Shader.PropertyToID("_PhaseSplit");
    readonly int phaseThickness = Shader.PropertyToID("_PhaseThickness");
    readonly int disolveFade = Shader.PropertyToID("_DisolveFade");

    Action onDisolveEnd;
    Action onPhaseEnd;
    public Action onGoalArrive;//목적지 도착을 알리는 델리게이트
    public Action onDie;

    Transform pool = null; //슬라임들이 들어있는 풀의 트렌스폼
    public Transform Pool//pool 에 단 한번만 값을 설정하는 프로퍼티
    {
        set
        {
            if (pool == null)
            {
                pool = value;
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;

        pathLine = GetComponentInChildren<PathLine>();
        //  onDisolveEnd += () => gameObject.SetActive(false);
        onDisolveEnd += ReturnToPool;
        onPhaseEnd += () =>
        {
            isActivate = true;
            pathLine.gameObject.SetActive(isShowPathLine);
        };

        onGoalArrive = () =>
        {
            Vector2Int pos;
            do
            {
                pos = map.GetRandomMoveablePosition();
            } while (pos == gridPosition);
            SetDestination(pos);
        };
    }
    private void OnEnable()
    {
        isActivate = false;
        path = new List<Vector2Int>();
        ResetShaderProperty();
        StartCoroutine(StartPhase());
    }

    public void Initialize(GridMap gridMap, Vector3 pos)//슬라임 초기화용 함수 param = 슬라임이 있는 맵
    {
        map = gridMap;
        transform.position = map.GridToWorld((map.WorldToGrid(pos)));
        Current = map.GetNode(pos);
    }
    protected override void OnDisable()
    {
        if (path != null)
        {
            path.Clear();
            path = null;
            PathLine.ClearPath();

        }
        base.OnDisable();
    }
    private void Update()
    {
        if (isActivate)
        {  //path != null 경로가 못가는 지역이 아닐경우(path가  null이 될 경우)를
            if (path != null && path.Count > 0 && pathWaitTime < MaxPathWaitTime)//경로List 안에 좌표들이 있다면 , 기다린 시간 확인
            {
                Vector2Int destGrid = path[0]; //그중 첫번째 첫 번째 좌표를 목표지점으로 정하고 
             
                if (!map.IsMonster(destGrid) || map.GetNode(destGrid) == Current)//destgrid에 몬스터가 없거나 destgrid가 Current(내 위치)일때 이동 가능
                {
                    //실제이동하기
                    Vector3 dest = map.GridToWorld(destGrid);// 좌표를 월드좌표로 변환
                    Vector3 dir = dest - transform.position;// 방향설정
                    if (dir.sqrMagnitude < 0.001f)
                    {
                        transform.position = dest;
                        path.RemoveAt(0); // 도착했다면 해당좌표를 리스트에서 지운다.
                    }
                    else
                    {
                        transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);
                        Current = map.GetNode(transform.position);
                    }
                    pathWaitTime = 0.0f;
                }
                else
                {
                    //다른 몬스터에 의해 대기했다.
                    pathWaitTime += Time.deltaTime;
                }
            }
            else// List 의 카운트가 비었다는 것은 목적지에 도착했다는 뜻이므로 도착했다는 델리게이트를 실행
            {
                pathWaitTime = 0.0f;
                onGoalArrive();
            }
        }
  
    }
    private void OnValidate()
    {
        if (pathLine != null)
        {
            pathLine.gameObject.SetActive(isShowPathLine);
        }
    }
    void ResetShaderProperty()//프로퍼티(변수)들 초기화 (재활용 대비)
    {
        mainMaterial.SetFloat(outLineThickness, 0.0f); //아웃라인 안보이게 초기화
        mainMaterial.SetFloat(phaseSplit, 1.0f);// 페이즈 초기화(안보이는상태)
        mainMaterial.SetFloat(disolveFade, 1.0f);// 디졸브 초기상태(슬라임이 완전히 보이는 상태)
    }

    IEnumerator StartPhase()//페이즈 진행 코루틴
    {
        float timeElapsed = 0.0f; //시간 누적용 
        float phaseNormalize = 1.0f / phaseDuration;//나누기를 자주사용하는 것을 피하기 위해 미리 값을 구해놓음 원래 구조는 timeElapsed / phaseNormalize 

        mainMaterial.SetFloat(phaseThickness, visiblePhaseThickNess); //페이즈 선이 보이게 하기

        while (timeElapsed < phaseDuration) // 페이즈가 진행중인 시간이면
        {
            timeElapsed += Time.deltaTime;// 시간누적
            mainMaterial.SetFloat(phaseSplit, 1 - (timeElapsed * phaseNormalize)); // Split조절해서 보이는 영역 변경
            yield return null;//다음 프레임 까지 대기
        }

        //페이즈가 끝난상황
        mainMaterial.SetFloat(phaseThickness, 0.0f);//페이즈 선 안보이게 하고 
        mainMaterial.SetFloat(phaseSplit, 0.0f);//phaseSplit 이 -값이 되는 것을 대비해 0으로 설정
        onPhaseEnd?.Invoke();
    }

    public void ShowOutLine(bool isShow = true)//아웃라인을 보여줄지 결정하는 함수 param =isShow가 true로 호출하면 보여주고 false로 호출하면 안보여준다
    {
        if (isShow)
        {
            mainMaterial.SetFloat(outLineThickness, visibleOutLineThickNess);//지정된 두깨로 설정
        }
        else
        {
            mainMaterial.SetFloat(outLineThickness, 0.0f); //안보여주는 상황 0으로 설정
        }
    }
    IEnumerator StartDisolve()//페이즈와 거의 같음
    {
        float timeElapsed = 0.0f;
        float Normalize = 1.0f / disolveDuration;//나누기를 자주사용하는 것을 피하기 위해 미리 값을 구해놓음

        while (timeElapsed < disolveDuration)
        {
            timeElapsed += Time.deltaTime;
            mainMaterial.SetFloat(disolveFade, 1 - (timeElapsed * Normalize));
            yield return null;
        }
        mainMaterial.SetFloat (disolveFade, 0.0f);
        onDisolveEnd?.Invoke();
    }

    public void Die()//죽을떄 실행
    {
        isActivate = false; //활동이 끝났다고 표시
        onDie?.Invoke();
        StartCoroutine(StartDisolve()); //disolve실행
    }

    public void ReturnToPool() //disolve 끝날때 실행되는 델리게이트 신호에 연결된 함수 
    {
        Current = null;
        onDie = null;// onDie delegate에 연결된 함수들을 null로 만든다 연결이 중복되는 것을 방지하기 위함
        transform.SetParent(pool);// 풀을 다시 부모로 설정
        gameObject.SetActive(false);
    }

    public void TestShader(int index)//테스트 코드
    {
        switch (index)
        {
            case 1:
                ResetShaderProperty();
                break;
            case 2:
                ShowOutLine();
                break;
            case 3:
                ShowOutLine(false);
                break;
            case 4:
                StartCoroutine(StartPhase());
                break;
            case 5:
                StartCoroutine(StartDisolve());
                break;
        }
    }

    public void SetDestination(Vector2Int destination)//목적지 설정하는 함수 
    {
        path = AStar.PathFind(map, gridPosition, destination);
        pathLine.DrawPath(map, path);
    }
}

