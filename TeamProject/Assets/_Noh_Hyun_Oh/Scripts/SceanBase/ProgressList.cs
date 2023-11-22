using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 옵션에서 프로그래스이미지 변경 기능 넣기위해 따로 제작  
/// 씬이동시에도 유지되기때문에 여기서 처리한다.
/// </summary>
public class ProgressList : ChildComponentSingeton<ProgressList> 

    
    /// 프로그래스바 변경시 이스크립트에 추가
{
    /// <summary>
    /// prefab 원본이라 transform 값이 변경이안된다.
    /// </summary>
    [SerializeField]
    private GameObject originBar;
    /// <summary>
    /// transform 변경을위해 복사해둔 이미지파일
    /// </summary>
    private GameObject progressBar;

    /// <summary>
    /// ProgressBar  
    /// </summary>
    /// <param name="type">타입</param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public GameObject GetProgress(EnumList.ProgressType type, Transform transform) {
    
        switch (type) {
            case EnumList.ProgressType.BAR:
                if (progressBar == null) //최초에 값이 없으면 생성한다.
                {
                    //로딩창은 항상 초기화 되서 미리만들어두어야 매번안만든다. 
                    //하나만만들어서 사용되기때문에 팩토리안만들고 해당클래스에서 처리                
                    progressBar = Instantiate(originBar, transform); //새로운객체만들어서 필요할때 넘겨준다.
                }
                return progressBar;
            default:
                Debug.LogWarning($"{type.ToString()} 의 값이 존재하지 않습니다");
                return null;
        }
    }
}
