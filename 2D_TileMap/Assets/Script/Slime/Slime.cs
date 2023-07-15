using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : PooledObject
{
    //����� 
    //�ƿ������� �α��� 0
    //disolve�� Fade�� 1
    //Phase�� Split�� 0
    //���� ���� Phase�� Split�� 0���� 1�� ���Ѵ�.
    //�÷��̾�� ���ݴ��� �� �ִ� ���°� �Ǹ� �β��� 0.005�� �ȴ�.
    //������ disolve�� Fade�� 1 -> 0���� ��ȭ�Ѵ�

    SpriteRenderer spriteRenderer;
    Material mainMaterial;

    Node current = null; // �� �������� ��ġ�ϰ� �ִ� ���
    Node Current
    {
        get => current;
        set
        {
            if (current != value)
            {
                if (current != null)//ó�� ������  null�̶� ��¿ �� ���� �߰�
                {
                    current.nodeType = Node.NodeType.Plain; //���� ��带 Plain���� �ǵ�����
                }
                current = value;
                if (current != null)//������ null�� ���õ��� �� ����ؼ� �߰�
                {
                    current.nodeType = Node.NodeType.Monster;
                }
              

                spriteRenderer.sortingOrder = -Mathf.FloorToInt(transform.position.y * 100);// �Ʒ��ʿ� �ִ� �������� ���� �׷������� ���� ����
            }
        }
    }

    float pathWaitTime = 0.0f;//�ٸ� �����ӿ� ���ؼ� ��ΰ� ������ �� ��ٸ��� �ð� 
    const float MaxPathWaitTime = 1.0f;//�ִ�� ��ٸ��� �ð�

    GridMap map;//�� �������� �ִ� �׸����

    List<Vector2Int> path = new List<Vector2Int>();//�������� �̵��� ��� 

    PathLine pathLine;//�̵��� ��θ� �׸��� Ŭ����
    public PathLine PathLine => pathLine; //�б����� ������Ƽ 
    Vector2Int gridPosition => map.WorldToGrid(transform.position);//�������� ��ǥȮ�ο� ������Ƽ

    public bool isShowPathLine = false;//PathLine�� ������ �Ⱥ����� �����ϴ� ���� 

    bool isActivate = false;//�������� Ȱ�������� ǥ�ÿ� 

    public float phaseDuration = 0.5f;//������ ����ð�
    public float disolveDuration = 1.0f;// ����ð�

    public float moveSpeed = 2.0f;

    public float lifeTimeBonus = 3.0f;

    const float visibleOutLineThickNess = 0.005f;//�ƿ������� ���϶� ������ �β�
    const float visiblePhaseThickNess = 0.1f;//������ ����� �β�

    //���̴� ������Ƽ�� ���̵�� �̸� ���س���
    readonly int outLineThickness = Shader.PropertyToID("_OutLineThickness");
    readonly int phaseSplit = Shader.PropertyToID("_PhaseSplit");
    readonly int phaseThickness = Shader.PropertyToID("_PhaseThickness");
    readonly int disolveFade = Shader.PropertyToID("_DisolveFade");

    Action onDisolveEnd;
    Action onPhaseEnd;
    public Action onGoalArrive;//������ ������ �˸��� ��������Ʈ
    public Action onDie;

    Transform pool = null; //�����ӵ��� ����ִ� Ǯ�� Ʈ������
    public Transform Pool//pool �� �� �ѹ��� ���� �����ϴ� ������Ƽ
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

    public void Initialize(GridMap gridMap, Vector3 pos)//������ �ʱ�ȭ�� �Լ� param = �������� �ִ� ��
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
        {  //path != null ��ΰ� ������ ������ �ƴҰ��(path��  null�� �� ���)��
            if (path != null && path.Count > 0 && pathWaitTime < MaxPathWaitTime)//���List �ȿ� ��ǥ���� �ִٸ� , ��ٸ� �ð� Ȯ��
            {
                Vector2Int destGrid = path[0]; //���� ù��° ù ��° ��ǥ�� ��ǥ�������� ���ϰ� 
             
                if (!map.IsMonster(destGrid) || map.GetNode(destGrid) == Current)//destgrid�� ���Ͱ� ���ų� destgrid�� Current(�� ��ġ)�϶� �̵� ����
                {
                    //�����̵��ϱ�
                    Vector3 dest = map.GridToWorld(destGrid);// ��ǥ�� ������ǥ�� ��ȯ
                    Vector3 dir = dest - transform.position;// ���⼳��
                    if (dir.sqrMagnitude < 0.001f)
                    {
                        transform.position = dest;
                        path.RemoveAt(0); // �����ߴٸ� �ش���ǥ�� ����Ʈ���� �����.
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
                    //�ٸ� ���Ϳ� ���� ����ߴ�.
                    pathWaitTime += Time.deltaTime;
                }
            }
            else// List �� ī��Ʈ�� ����ٴ� ���� �������� �����ߴٴ� ���̹Ƿ� �����ߴٴ� ��������Ʈ�� ����
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
    void ResetShaderProperty()//������Ƽ(����)�� �ʱ�ȭ (��Ȱ�� ���)
    {
        mainMaterial.SetFloat(outLineThickness, 0.0f); //�ƿ����� �Ⱥ��̰� �ʱ�ȭ
        mainMaterial.SetFloat(phaseSplit, 1.0f);// ������ �ʱ�ȭ(�Ⱥ��̴»���)
        mainMaterial.SetFloat(disolveFade, 1.0f);// ������ �ʱ����(�������� ������ ���̴� ����)
    }

    IEnumerator StartPhase()//������ ���� �ڷ�ƾ
    {
        float timeElapsed = 0.0f; //�ð� ������ 
        float phaseNormalize = 1.0f / phaseDuration;//�����⸦ ���ֻ���ϴ� ���� ���ϱ� ���� �̸� ���� ���س��� ���� ������ timeElapsed / phaseNormalize 

        mainMaterial.SetFloat(phaseThickness, visiblePhaseThickNess); //������ ���� ���̰� �ϱ�

        while (timeElapsed < phaseDuration) // ����� �������� �ð��̸�
        {
            timeElapsed += Time.deltaTime;// �ð�����
            mainMaterial.SetFloat(phaseSplit, 1 - (timeElapsed * phaseNormalize)); // Split�����ؼ� ���̴� ���� ����
            yield return null;//���� ������ ���� ���
        }

        //����� ������Ȳ
        mainMaterial.SetFloat(phaseThickness, 0.0f);//������ �� �Ⱥ��̰� �ϰ� 
        mainMaterial.SetFloat(phaseSplit, 0.0f);//phaseSplit �� -���� �Ǵ� ���� ����� 0���� ����
        onPhaseEnd?.Invoke();
    }

    public void ShowOutLine(bool isShow = true)//�ƿ������� �������� �����ϴ� �Լ� param =isShow�� true�� ȣ���ϸ� �����ְ� false�� ȣ���ϸ� �Ⱥ����ش�
    {
        if (isShow)
        {
            mainMaterial.SetFloat(outLineThickness, visibleOutLineThickNess);//������ �α��� ����
        }
        else
        {
            mainMaterial.SetFloat(outLineThickness, 0.0f); //�Ⱥ����ִ� ��Ȳ 0���� ����
        }
    }
    IEnumerator StartDisolve()//������� ���� ����
    {
        float timeElapsed = 0.0f;
        float Normalize = 1.0f / disolveDuration;//�����⸦ ���ֻ���ϴ� ���� ���ϱ� ���� �̸� ���� ���س���

        while (timeElapsed < disolveDuration)
        {
            timeElapsed += Time.deltaTime;
            mainMaterial.SetFloat(disolveFade, 1 - (timeElapsed * Normalize));
            yield return null;
        }
        mainMaterial.SetFloat (disolveFade, 0.0f);
        onDisolveEnd?.Invoke();
    }

    public void Die()//������ ����
    {
        isActivate = false; //Ȱ���� �����ٰ� ǥ��
        onDie?.Invoke();
        StartCoroutine(StartDisolve()); //disolve����
    }

    public void ReturnToPool() //disolve ������ ����Ǵ� ��������Ʈ ��ȣ�� ����� �Լ� 
    {
        Current = null;
        onDie = null;// onDie delegate�� ����� �Լ����� null�� ����� ������ �ߺ��Ǵ� ���� �����ϱ� ����
        transform.SetParent(pool);// Ǯ�� �ٽ� �θ�� ����
        gameObject.SetActive(false);
    }

    public void TestShader(int index)//�׽�Ʈ �ڵ�
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

    public void SetDestination(Vector2Int destination)//������ �����ϴ� �Լ� 
    {
        path = AStar.PathFind(map, gridPosition, destination);
        pathLine.DrawPath(map, path);
    }
}

