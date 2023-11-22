using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 한개만 생성되는 객체들이 공통적으로 사용할 클래스 
/// 공통적으로 처리해야될 로직작성
/// 이름만 팩토리....Prefab 이라고붙이는게낫았으려나..
/// </summary>
public class Unique_Factory :ChildComponentSingeton<Unique_Factory>
{

    ///// <summary>
    ///// 옵션창 담을 변수
    ///// 기타옵션 및  저장 불러오기 종료 등
    ///// </summary>
    //[SerializeField]
    //private GameObject optionWindow;

    ///// <summary>
    ///// 플레이어 정보창 담을변수
    ///// 인벤, 스테이터스 , 스킬 등
    ///// </summary>
    //[SerializeField]
    //private GameObject playerWindow;

    ///// <summary>
    ///// 논플레이어 정보창 담을변수 
    ///// 상점 , 크래프팅 , 대화창? 등 
    ///// </summary>
    //[SerializeField]
    //private GameObject nonPlayerWindow;

    ///// <summary>
    ///// 로딩화면에 사용될 진행바
    ///// </summary>
    //[SerializeField]
    //private GameObject progressList;

    ///// <summary>
    ///// 기본 배경음악 
    ///// </summary>
    //[SerializeField]
    //private GameObject defaultBGM;

    ///// <summary>
    ///// 창에서 클릭이나 선택시 소리 
    ///// </summary>
    //[SerializeField]
    //private GameObject SystemEffectSound;

    ///// <summary>
    ///// 필요한오브젝트 가져오는 기능. 
    ///// prefab 오브젝트를 넘겨주기때문에 
    ///// 받은오브젝트 포지션은 수정이안된다.
    ///// </summary>
    ///// <param name="type">오브젝트 종류</param>
    ///// <returns>prefab 오브젝트 바로연결해주기</returns>
    //public GameObject GetObject(EnumList.UniqueFactoryObjectList type) {

    //    switch (type) {
    //        case EnumList.UniqueFactoryObjectList.OPTIONS_WINDOW:
    //            return optionWindow;
    //        case EnumList.UniqueFactoryObjectList.PLAYER_WINDOW:
    //            return playerWindow;
    //        case EnumList.UniqueFactoryObjectList.NON_PLAYER_WINDOW:
    //            return nonPlayerWindow;
    //        case EnumList.UniqueFactoryObjectList.PROGRESS_LIST:
    //            return progressList;
    //        case EnumList.UniqueFactoryObjectList.DEFAULT_BGM:
    //            return defaultBGM;
    //        default:
    //            return null;
    //    }
    //}

  
}
