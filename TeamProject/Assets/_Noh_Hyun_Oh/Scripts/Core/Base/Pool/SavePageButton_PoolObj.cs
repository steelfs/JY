using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 풀에서 생성될때 자동으로 데이터까지 초기화하도록 고쳤다.
/// </summary>
public class SavePageButton_PoolObj : Base_PoolObj 
{
    /// <summary>
    /// 화면에 보여줄 페이징번호 
    /// </summary>
    int pageIndex = -1;
    public int PageIndex { 
        get => pageIndex;
        set { 
            pageIndex = value;
            realIndex = value - 1;
            text.text = $"{pageIndex}";
        }
    }
    /// <summary>
    /// 페이지버튼 누를때마다 -1연산이 필요해서 기냥 변수로뺏다.
    /// </summary>
    int realIndex = -1; 
    
    /// <summary>
    /// 처리할 클래스 가져오기
    /// </summary>
    SaveWindowManager proccessClass;

    /// <summary>
    /// 화면에 보여줄 텍스트위치 가져오기
    /// </summary>
    TextMeshProUGUI text;

    /// <summary>
    /// 오브젝트찾기
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }
    protected override void OnEnable()
    {
        //위치값 초기화 안시키기위해 오버라이딩
    }
    private void Start()
    {
        proccessClass = WindowList.Instance.MainWindow;
    }
    /// <summary>
    /// 페이지 버튼 클릭 이벤트
    /// </summary>
    public void OnPageDownButton()
    {
        if (realIndex > -1)
        {
            proccessClass.SetPageList(realIndex); 
        }
    }
}
