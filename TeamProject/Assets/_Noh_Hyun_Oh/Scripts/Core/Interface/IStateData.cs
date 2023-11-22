
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 상태이상 아이콘 UI에 사용될 인터페이스 
/// </summary>
public interface IStateData
{
    /// <summary>
    /// 컴포넌트 에서 정의된 프로퍼티 찾아 쓰기
    /// </summary>
    GameObject gameObject { get; }
    /// <summary>
    /// 상태이상 타입
    /// </summary>
    //EnumList.StateType Type { get; set; }
    
    /// <summary>
    /// 스킬 데이터
    /// </summary>
    SkillData SkillData { get; set; }
    /// <summary>
    /// 한턴당 감소되는 수치 
    /// </summary>
    float ReducedDuration { get; set; }

    /// <summary>
    /// 최대 지속시간
    /// </summary>
    float MaxDuration { get; set; }
    /// <summary>
    /// 남은 지속시간
    /// </summary>
    float CurrentDuration { get; set; }

    /// <summary>
    /// 값을 초기화 하기위한 함수
    /// 필요 기능 풀로 위치옮기기 , 모든 값들 null 셋팅 float 은 -1셋팅 type 은 none 값 셋팅
    /// </summary>
    void ResetData();
}