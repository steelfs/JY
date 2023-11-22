using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
/// <summary>
///  턴 게이지에 보일 유닛오브젝트
///  유닛 오브젝트는 추가되서 활성화되면 자동으로 위치를 잡는다 
/// </summary>
public class TurnGaugeUnit : Base_PoolObj
{
    /// <summary>
    /// 게이지가 움직이는 시간 늘릴수록 빠르다
    /// </summary>
    [SerializeField]
    private float gaugeSpeed = 1.5f;

    /// <summary>
    /// 유닛의 현재위치 게이지바 두께
    /// </summary>
    [SerializeField]
    [Range(0.0f, 0.01f)]
    private float boldBar = 0.01f;

    /// <summary>
    /// 진행도 처리할 값 0.0f ~ 1.0f;
    /// 이값을 기준으로 게이지 위치를 변경한다.
    /// </summary>
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float progressValue = 0.0f;
    public float ProgressValue
    {
        get => progressValue;
        set
        {
            progressValue = value;
            if (value > 1.0f) //최대값은 1.0f이다.
            {
                progressValue = 1.0f;
            }
        }
    }

    /// <summary>
    /// 진행도위에 보일 아이콘이미지 추후 추가
    /// </summary>
    Image icon;

    /// <summary>
    /// 아이콘이 움직이는 모션  추후 추가
    /// </summary>
    Animator iconAnim;

    /// <summary>
    /// Position 이동이아니라 앵커값을 조절할것이라 필요
    /// </summary>
    RectTransform moveRect;

    /// <summary>
    /// 진행상황 수정용 변수
    /// anchor값 담을 변수
    /// </summary>
    Vector2 minValue;
    Vector2 maxValue;

    /// <summary>
    /// 에디터화면의 right left 값을 담을 변수 
    /// </summary>
    Vector2 rectLeft;
    Vector2 rectRight;

    /// <summary>
    /// 값이변환이되면 서서히 움직이는 동작을 만들어줄 코르틴
    /// 씬이동후에는 멈추기위해 변수로뺏다.
    /// </summary>
    IEnumerator ProgressBar;

    protected override void Awake()
    {
        base.Awake();
        icon = GetComponent<Image>();
        moveRect = GetComponent<RectTransform>();
        iconAnim = GetComponent<Animator>();
        ProgressBar = ProgressMove();
    }

    protected override void OnEnable()
    {
        StartCoroutine(ProgressBar);
    }
    protected override void OnDisable()
    {
        StopCoroutine(ProgressBar);
        base.OnDisable();
    }

  

    /// <summary>
    /// progressValue 값이 변경되면 서서히 움직이는 느낌을 주기위해 추가 
    /// 전체 창사이즈가 변경되도 같은느낌으로 움직이는느낌을 주기위해 앵커포지션을 사용했다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProgressMove() 
    {
        if (moveRect != null)
        {

            minValue = moveRect.anchorMin;
            maxValue = moveRect.anchorMax;
           
            rectLeft = new Vector2(0, moveRect.offsetMin.y); //에디터상의 left 값 수정  이건 양수로 입력됨
            rectRight = new Vector2(0, moveRect.offsetMax.y); //에디터상의 right 값 수정 -로 입력되기때문에 양수입력하려면 -추가
            while (true) //위치값 잡기 
            {
                
                minValue.x = Mathf.Lerp(minValue.x, progressValue ,Time.deltaTime * gaugeSpeed);
                //min max 값이 같으면 진행바에서 막대기가 안보임으로 조금은 차이를두었다.
                maxValue.x = Mathf.Lerp(maxValue.x , progressValue + boldBar,Time.deltaTime* gaugeSpeed);

                moveRect.anchoredPosition3D = Vector3.zero;
                //앵커 포지션값 조절
                moveRect.anchorMin = minValue;
                moveRect.anchorMax = maxValue;

                //앵커 포지션값을 조절하면 right  left 값이 자동으로바뀌는데 제대로 보이게하려면 
                //초기값으로 돌려야한다 그래서 0값으로 셋팅
                moveRect.offsetMin = rectLeft;
                moveRect.offsetMax = rectRight;
                yield return null;
            }
            
        }
        
    }

    /// <summary>
    /// 초기화 
    /// </summary>
    public void ResetData() 
    {
        ProgressValue = 0.0f;//값 초기화하고
        if (TurnManager.Instance.IsViewGauge) //턴유아이 활성화되있으면 
        {
            gameObject.SetActive(false); //큐로 돌리고
        }
        else //안되있으면
        {
            //Debug.Log($"실행 : {onDisable}");
            onDisable?.Invoke(); //큐로 돌리기위해 강제로 함수실행 
        }
    }

}
