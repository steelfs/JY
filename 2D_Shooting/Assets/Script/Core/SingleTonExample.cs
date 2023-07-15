using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤 객체를 하나만 가지는 디자인 패턴
/// </summary>
public class SingleTonExample : MonoBehaviour
{
   

    /// <summary>
    /// 이미 종료처리에 들어갔는지 확인하기 위한 변수
    /// </summary>
    private static bool isShutDown = false;


    /// <summary>
    /// 싱글톤의 객체
    /// </summary>
    private static SingleTonExample instance;

    /// <summary>
    /// 싱글톤 객체를 가져오기 위한 프로퍼티 객체가 만들어지지 않았으면 새로 만든다.
    /// </summary>
    public static SingleTonExample Instance
    {
        get
        {
            if (isShutDown) //종료처리에 들어간 상황이면 
            {
                Debug.LogWarning("싱글톤은 이미 삭제중이다.");// 경고메세지 출력
                return null;
            }
            if (instance == null)
            {
                //instance가 없으면 새로 만든다.
                SingleTonExample singleTon = FindObjectOfType<SingleTonExample>();
                if (singleTon == null) // 이미 씬에 싱글톤이 있는지 확인
                {

                    GameObject gameObj = new GameObject();//빈 오브젝트 생성
                    gameObj.name = "SingleTon";             //이름수정
                    gameObj.AddComponent<SingleTonExample>();// 싱글톤 컴포넌트 추가
                  
                }
                instance= singleTon; //instance에 찾았거나 만들어진 객체 대입
                DontDestroyOnLoad(instance.gameObject); //씬이 바뀌거나 사라져도 객체를 파괴하지 않는다.
            
            }
            return instance; //instance 리턴 (이미 있거나 새로 만들어졌거나)
        }
    }
    public int testI = 0;

    private void Awake()
    {
        if (instance == null)
        {
            //씬에 배치되어있는 첫번째 싱글톤 게임 오브젝트
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            //첫번째 싱글톤 게임보으젝트가 만들어진 이후에 생성된 싱글톤이면 
            if (instance != this)
            {
                Destroy(this.gameObject); // 첫번째 싱글톤과 다른 것이면 나중에 만들어진것(이것을) 삭제해라
            }
        }
    }

    /// <summary>
    /// 프로그램이 종료될때 실행되는 함수
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;
    }
}
