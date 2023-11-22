/*
 enum을 선언할때 보통 클래스 밖에서 선언하기때문에 그냥 네임스페이스로 묶어서 사용
 enum 이 필요한경우 여기에 추가.
 */
namespace EnumList
{
    /// <summary>
    /// 씬리스트 
    /// 맴버변수명을 씬이름으로맞춰야한다. 그리고 빌드쪽에 순서도 맞춰야한다.
    /// </summary>
    public enum SceneName
    {
        NONE = -1,//셋팅안됬을때의값 이값셋팅되면안된다 기본적으로.
        OPENING,
        TITLE,
        LOADING,
        ENDING,
        //Item_Test,//인벤창쪽으로넘어가나확인
        //CreateCharcter, //아직안만듬
        TestBattleMap,  //맵은 종류가많음 가장밑에 추가
        SpaceShip,  //맵은 종류가많음 가장밑에 추가
        BattleShip  // 마을맵 맵핑
    }
    /// <summary>
    /// 로딩화면에 보여줄 이미지 종류리스트
    /// </summary>
    public enum ProgressType
    {
        BAR = 0,

    }
    /// <summary>
    /// 타이틀에서 사용할 메뉴종류 리스트
    /// </summary>
    public enum TitleMenu
    {
        NEWGAME = 0,
        CONTINUE,
        OPTIONS,
        EXIT
    }
    /// <summary>
    /// BGM 리스트
    /// </summary>
    public enum BGM_List
    {

    }
    /// <summary>
    /// 효과음 리스트
    /// </summary>
    public enum EffectSound
    {
        EXPLOSION1,
        EXPLOSION2,
        EXPLOSION3
    }
    /// <summary>
    /// 저장화면 버튼정보
    /// </summary>
    public enum SaveLoadButtonList
    {
        NONE = 0,
        SAVE,
        LOAD,
        COPY,
        DELETE,
    }
    public enum PopupList
    {
        NONE = -1,
        SAVE_LOAD_POPUP,
    }
    /// <summary>
    /// 게임 객체
    /// </summary>
    public enum MultipleFactoryObjectList
    {
        SAVE_DATA_POOL = 0, //저장화면에 보여줄 오브젝트생산용 풀
        SAVE_PAGE_BUTTON_POOL,
        TURN_GAUGE_UNIT_POOL,
        TRACKING_BATTLE_UI_POOL,
        STATE_POOL,
        BATTLEMAP_PLAYER_POOL,
        BATTLEMAP_ENEMY_POOL,
        CHARCTER_PLAYER_POOL,
        TILE_POOL,
        MERCHANT_iTEM_POLL,
        SIZE_S_HUMAN_ENEMY_POOL,
        SIZE_S_ROBOT_ENEMY_POOL,
        SIZE_M_HUMAN_HUNTER_ENEMY_POOL,
        SIZE_M_HUMAN_PSIONIC_ENEMY_POOL,
        SIZE_L_ROBOT_ENEMY_POOL,
    }


    public enum UniqueFactoryObjectList
    {
        OPTIONS_WINDOW = 0, //옵션창 ESC나 O 키눌렀을때 나오게 하려고 생각중
        PLAYER_WINDOW,     //플레이어 정보창 특정단축키에 연결해서 사용하려고 생각중
        NON_PLAYER_WINDOW,  //NPC 에게 말을걸고 상점이나 휴식 창같은 거에 사용될예정
        PROGRESS_LIST,     //프로그래스바 종류가 늘어날시 담을 려고 넣어둠
        DEFAULT_BGM,          //배경음악 처리할 싱글톤담으려고 생각중
        SYSTEM_EFFECT_SOUND //이팩트사운드 담을 싱글톤 아직 제작안함.
    }

    /// <summary>
    /// 화상 감전 동상 중독 공포 
    /// </summary>
    //public enum StateType
    //{
    //    None = 0, //빈값
    //    ElectricShock, //감전
    //    Freeze, //동상
    //    Poison, //중독
    //    Fear, //공포
    //    Burns //화상
    //}

    /// <summary>
    /// 캐릭터 카메라 
    /// </summary>
    public enum CameraFollowType
    {
        Custom = -1,
        MiniMap,
        UITexture,
        QuarterView,
    }

}
