using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TurnGaugeOnOff : MonoBehaviour
{
    /// <summary>
    /// 위치조절될 버튼 부모오브젝트
    /// </summary>
    RectTransform battleButtonRt;

    /// <summary>
    /// 턴게이지 UI 높이 구하기
    /// </summary>
    float viewHeight = 0.0f;

    /// <summary>
    /// 보이는속도
    /// </summary>
    [SerializeField]
    float moveSpeed = 1000.0f;

    /// <summary>
    /// 임시로 담아둘 변수 
    /// </summary>
    Vector2 tempMin = Vector2.zero;
    Vector2 tempMax = Vector2.zero;
    private void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();
        viewHeight = rt.rect.height;
        
        //엄청 귀찮게 찾아오기 .. 위치바껴도 찾을수있게.
        Transform battleUIController = transform.parent.GetComponentInChildren<BattleActionUIController>(true).transform;

        battleButtonRt = battleUIController.parent.parent.GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        StartCoroutine(UIOpen()); //열때마다 새롭게 연다.
    }
    private void OnDisable()
    {
        SetTopBottomValue(0);

    }
    IEnumerator UIOpen()
    {
        float timeElapsed = 0.0f; //시간누적하고 
        while (timeElapsed < viewHeight)
        {
            timeElapsed += Time.deltaTime * moveSpeed; //부드럽게 증가시키기위해 델타타임 누적시키고
            SetTopBottomValue(timeElapsed);
            yield return null;

        }
        SetTopBottomValue(viewHeight);//값이 더커질수있어서 넘치는것을 해결하기위해 추가

    }

    /// <summary>
    ///  rectTransform 의 top과 bottom 값을 
    ///  들어온인자값으로 적용한다  top 은 -인자값  bottom 은 인자값 그대로 
    ///<param name="value">적용할 값</param>
    /// </summary>
    private void SetTopBottomValue(float value)
    {
        tempMin = battleButtonRt.offsetMin; //bottom
        tempMax = battleButtonRt.offsetMax; //top
        tempMin.y = value; //bottom 값은 양수값 그대로 들어간다
        tempMax.y = value; //top 값은 양수값 넣으면 음수값으로 변환되어 들어간다.
        battleButtonRt.offsetMin = tempMin; //bottom
        battleButtonRt.offsetMax = tempMax; //top
    }
}
