using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class Player : MonoBehaviour
{
    public float moveableRangeX = 8.2f; 
    public float moveableRangeY = 4.3f;

    int score = 0;
    public int Score //델리게이트를 사용하면 그냥 set함수에서 호출할 때보다 결합도가 낮아져서 유지보수가 더 수월하다.
    {
        get => score;
        set
        {
            if (score != value)
            {
                score = value;
                OnScoreChange?.Invoke(score);
            }                    
        }
    }
    public void AddScore(int newscore)
    {
        Score += newscore;
    }
    public Action<int> OnScoreChange; // 1. 델리게이트를 만들어준다  2. 어디서 호출할지를 지정한다. 호출위치에서 함수이름?.Invoke(매개변수)
                                      // 3. 다른곳에서 델리게이트를 만든 클래스,객체를 찾은 다음 델리게이트 함수에 다른 함수를 연결해준다. 
    public int powerBonus = 300;
    private int power = 0;
    private int Power
    {
        get => power;
        set
        {
            if (power != value)
            {
                power = value;

                if (power > 3)
                {
                    AddScore(powerBonus);
                }
                power = Mathf.Clamp(power, 1, 3);
                RefreshFirePositions(power);
            }
        }
    }

    private int life = 3;
    private int Life
    {
        get => life;
        set
        {
            life = value;
            if (life > 0)
            {
                OnHit();//적에게 맞았을때 실행
            }
            else
            {
                OnDie();
            }
            onLifeChange?.Invoke(life);
        }
    }
    bool isAlive => life > 0;// 생존을 판단하는 프로퍼티

    public int initialLife = 3;

    public float invincibleTime = 2.0f;

    public Action<int> onLifeChange;
    public Action<int> onDie; //파라미터는 최종점수

    public float fireAngle = 30.0f;

    public float speed = 2.0f;
    public float fireInterval = 0.2f;

    float boost = 1.0f;
    Vector3 direction;
    PlayerInputAction playerInputAction;
    Animator anim;
    IEnumerator fireCoroutine;//총알 연사용 코루틴
    readonly int inputY_String = Animator.StringToHash("InputY");
    public GameObject bullet;
    public GameObject fireFlash; //총알 발사 이펙트
    GameObject Explosion;

    Transform[] fireTransforms; //총알 발사위치

    WaitForSeconds fireWait; //캐싱
    WaitForSeconds flashWait;

    Rigidbody2D rigid;

    SpriteRenderer spriteRenderer;
    bool isInvincible = false;
    float timeElapsed = 0.0f;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fireCoroutine = FireCoroutine(); //함수자체를 저장
        
        Transform fireRoot = transform.GetChild(0); //발사위치 루트 찾기
        fireTransforms= new Transform[fireRoot.childCount]; //루트의 자식 수 만크 배열 확보
        for (int i= 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i] = fireRoot.GetChild(i); // 총알발사 트랜스폼 찾기
        }
        fireWait = new WaitForSeconds(fireInterval); //코루틴에서 사용할 인터벌 미리 만들어놓기
        flashWait = new WaitForSeconds(0.1f); //캐싱

        fireFlash = transform.GetChild(1).gameObject;
    }
    private void Start()
    {
        Power = 1;
        Life = initialLife;
    }
    private void OnEnable()
    {
        playerInputAction.Player.Enable();
        playerInputAction.Player.Move.performed += OnMove;
        playerInputAction.Player.Move.canceled += OnMove;
        playerInputAction.Player.Boost.performed += OnBoost;
        playerInputAction.Player.Boost.canceled += OnBoost;
        playerInputAction.Player.Fire.performed += OnFire_Start;
        playerInputAction.Player.Fire.canceled += OnFire_Stop;
    }
    private void OnDisable()
    {
        playerInputAction.Player.Move.performed -= OnMove;
        playerInputAction.Player.Move.canceled -= OnMove;
        playerInputAction.Player.Boost.performed -= OnBoost;
        playerInputAction.Player.Boost.canceled -= OnBoost;
        playerInputAction.Player.Fire.performed -= OnFire_Start;
        playerInputAction.Player.Fire.canceled -= OnFire_Stop;
        playerInputAction.Player.Disable();
    }
    private void OnFire_Start(InputAction.CallbackContext _)
    {
        //GameObject temp = GameObject.Find("FireTransform"); 이름으로 찾기   모든 오브젝트를 검색해야하고 문자열로 찾아야하기 때문에 성느면에서 비효율적이다.
        //GameObject temp2 = GameObject.FindGameObjectWithTag()  태그로 찾기 씬 전부를 찾는다 . 숫자로 변경될수있어서  문자열보다는 빠르긴 하다
        //GameObject temp3 = GameObject.FindObjectOfType<Transform>  //특정 컴포넌트를 가진 오브젝트를 찾는다.
        //Transform child = transform.GetChild(0);
        //bullet.transform.position = child.position;
        //bullet.transform.rotation = child.rotation;

        //Bullet.transform.position.x = this.transform.position.x + 2;
        StartCoroutine(fireCoroutine);
    }
  
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            for (int i= 0; i < Power; i++)
            {
                Transform firePos = fireTransforms[i];
                Factory.Inst.GetObject(Pool_Object_Type.Player_Bullet, firePos.position, firePos.rotation.eulerAngles.z);
            }
           // Bullet bulletComp = newbullet.GetComponent<Bullet>();
            //bulletComp.onEnemyKill += AddScore; 아래와 같은 코드 OnEnemyKill 에 AddScore함수 등록
            //bulletComp.onEnemyKill += (newScore) => Score += newScore; // 람다식 (newScore 파라미터) 이후는 함수 바디부분
            StartCoroutine(FlashEffect());

            yield return fireWait;
        }
    }

    //System.Func<int, float> onTest;
    //void Test()
    //{
    //    onTest += (testScore) => testScore + 3.45f;
    //}

    IEnumerator FlashEffect()
    {
        fireFlash.SetActive(true); //활성화
        yield return flashWait;
        fireFlash.SetActive(false); //
    }
    private void OnFire_Stop(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //트리거 영역 안에 들어갔다. 겹쳤을 때
    //    //파라미터 Collider2D collision : 상대방의 콜라이더
    //    Debug.Log($"{collision.gameObject.name} 영역에 들어갔다");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    //트리거 영역안에서 움직일 때.
    //    Debug.Log($"{collision.gameObject.name} 영역에 들어갔다");
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    //트리거 영역에서 나올때 한번 실행 됨
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //다른 컬라이더랑 충돌한 순간 실행 (겹칠 수 없다)
    //}
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // 충돌한 상태에서 유지되고있을 때 실행(붙어있을 때)
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // 떨어졌을 때 실행
    //}


    private void Update()
    {
        if (isInvincible)
        {
            timeElapsed += Time.deltaTime * 30; //시간변화를 증폭시켜서 누적시키기
            float alpha = (Mathf.Cos(timeElapsed) + 1.0f) * 0.5f; //코사인 결과를 0~1 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
    }
    private void FixedUpdate()
    {
        //transform.position += Time.deltaTime * speed * boost * direction;
        if (isAlive)
        {
            rigid.MovePosition(rigid.position + (Vector2)(Time.fixedDeltaTime* speed * boost * direction));
        }
        //else
        //{
        //    rigid.AddTorque(30.0f); // 회전력 더하기  z축방향으로 30도씩 회전
        //   //  //특정방향으로 힘을 더하기 왼쪽방향으로 0.3만큼
        //}
     

    }

 
    private void OnBoost(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            boost = 1.0f;
        }
        else
        {
            boost = 5.0f;
        }
    }



    private void OnMove(InputAction.CallbackContext context)
    {       
        Vector2 value = context.ReadValue<Vector2>();
        direction = value;  
        anim.SetFloat(inputY_String, direction.y);  //   //anim.SetFloat("InputY", direction.y); 같은 코드

    
        //if (transform.position.x)
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")||collision.gameObject.CompareTag("EnemyBullet"))
        {
            Life--;
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            Power ++;
            collision.gameObject.SetActive(false);
        }
    }
    void RefreshFirePositions(int power) //
    {
        for (int i = 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i].gameObject.SetActive(false);//모든 하위 발사위치를 비활성화
        }

        for (int i = 0; i < power; i++)
        {
            //총알간 사이각 30도 
            // power 1일 때  0도 회전
            // 2일때 1개는 -15도 1개는 15도 회전
            // 3일 때 -30, 0, 30도 회전 

            fireTransforms[i].rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + (i * -fireAngle));//power로 시작각 정하고 추가로 i * 발사각만큼 추가
            fireTransforms[i].localPosition = Vector3.zero;
            fireTransforms[i].Translate(0.5f, 0, 0);

            fireTransforms[i].gameObject.SetActive(true);
        }
    }
    private void OnHit()
    {
        Power--;
        StartCoroutine(EnterInvincible()); //무적모드 진입
    }

    IEnumerator EnterInvincible()// 무적모드 들어가고 시간 지나면 원상복귀되는 코루틴
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        isInvincible = true;//무적모드 돌입
        timeElapsed = 0.0f; // 알파값 변환을 위한 시간누적 변수
        
        yield return new WaitForSeconds(invincibleTime); //무적시간 

        spriteRenderer.color= Color.white;  //알파값 원상복귀
        isInvincible = false;               // 무적모드 끝
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어 원상복귀
    }
    private void OnDie()
    {
        Collider2D bodyCollider = GetComponent<Collider2D>();
        bodyCollider.enabled = false; //콜라이더 비활성화

        Factory.Inst.GetObject(Pool_Object_Type.Enemy_Explosion, transform.position); //터지는 이펙트 추가

        playerInputAction.Player.Disable(); //플에이어 입력 막기

        direction = Vector3.zero; //이동 초기화
        StopAllCoroutines();        //연사코루틴 정지

        rigid.gravityScale = 1.0f;
        rigid.freezeRotation = false;

        rigid.AddTorque(10000);
        rigid.AddForce(Vector2.left * 10.0f, ForceMode2D.Impulse);
       
        onDie?.Invoke(Score);
    }
    void TestDie()
    {
        Life = 0;
    }
}
