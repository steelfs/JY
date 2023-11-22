/* 
 * 빈오브젝트 생성
 * Start 함수에서 Slot UI오브젝트 생성 -> SetActive false
 * 마우스오버 -> ItemDescription Setactive true, false
 * 
 *  아이탬 드롭 pool 만들어서 확률적으로 드롭  //아이탬의 위치 좌표(parameter)
 *  아이탬 장착
 *  
 *  
 *  
 *  필요한 데이터 클래스/구조 정의하기:

아이템 클래스 정의 : 아이템의 기본적인 속성(이름, 이미지, 분류 등)과 고유한 속성(공격력, 방어력, 회복량 등)을 포함하게 만듭니다.
아이템 분류를 위한 Enum 정의 : 장비, 소비, 기타 등의 아이템 분류를 Enum으로 만듭니다.
인벤토리 시스템 구현:

    enum : 장비, 소비, 재료, 기타
            
        소비 :   HP, MP, DarkForce, 피로도 회복
        주문서 : 강화,
        
        장비 : 
        재료 : 





       계층구조
       GameManager(SingleTon) - Inventory, UI_Spawner, Item_Spawner, 삭제ButtonController 등 기능별 중심 클래스의 참조를 참조해서 다른곳에서 필요할때 찾아 쓸수있도록 한다.
           
           Inventory - 
           ButtonController - UI로 표시되는 버튼들은 어떤 버튼이 어떤클래스의 어떤 함수와 연결되는지 참조를 쉽게 찾을 수 있도록  이 클래스를 거쳐서 구현된다.
           UI_Spawner -  Inventory 클레스에서 필요할때 각 슬롯의 정보를 접근하여 수정할 수 있도록 Slot 들을 생성함과 동시에 Dictionary 를 생성해
                         key값으로 Current_Inventory_State Enum값을 가지고 value는 해당 인벤토리의 Slot들을 바인딩한다.
           Item_Spawner - UI_Spawner가 있는데 굳이 따로만들어야하나 싶었지만 막연하게 나중에 왠지 기능들을 분리해야할 일이 생길수도있을 것같아서 일단 분리해서 만들기로했다.
                   몬스터가 죽을 때, 혹은 특정 위치에  위치를 파라미터로 받아 아이탬을 드롭하는 기능


    
 *  
 *  UI버튼을 함수에 직접 연결해주는데 연결된 함구의 위치가 제각각 모두 다른클레스안에 위치한다면 나중에 버튼이 많아질때 찾는 시간이 오래걸리고 불편하다. 
 *  따라서 버튼기능을 모아놓은 클레스가 필요할것같다
 *
 *  ObjectSpawner UI와 아이템 오브젝트를 생성할 클래스를 나눌것인가? 합칠것인가, 결정 후 //혹시 나중에 모듈화 할수도 있으므로 분리하는 것으로 
 *      
 *      
 *      
 * 

 *      설계적으로 Inventory와 UI오브젝트 스포너가 모두 싱글톤일 필요가 없을것 같다. 하나의 게임메니저를 싱글톤으로 만들고 거기서 참조한것을 다른곳에서 가져다 쓰게하는것이 깔끔할 것 같다.//완료
        ButtonController 생성 및 현재 구현된 함수 재연결 //완료
        Add슬롯함수 - 만약 Inventory의 IsInitialized = false = 모든탭에 10개씩 생성, true면 현제 state의 슬롯에 5개 추가// 완료 (매개변수를 nullable 로 받음)
 *      AddSlot함수와 CreateSlot함수의 중복된 기능을 하나의 함수에서 처리 //완료
 *      Animation/완료
 *      에니메이션 로컬포지션 = 부모오브젝트 생성 후 애니메이터를 부여 후 자식오브젤트의 포지션을 조정한다//완료   새 씬에서 자식 오브젝트의 피봇을 재설정해서 해결
 *      buttoncontroller 삭제 재연결/ 완료
 *      
 *      6월 27일 FeedBack
 *      , ItemData 인터페이스의 내용을 그냥 ItemBase 안에 모두 포함시키기 굳이 ItemBase를 상속받을건데 ItemData라는 인터페이스를 따로 만들 이유가X/완료
 *      몬스터가 죽을 때 직접 호출하지 말고 어떤 위치에서 누가 죽었다는 신호만 보내도록. 왜냐하면 결국 해당 타일로 이동해야 아이템을 습득하도록 할 것이기 때문에 
 *      타일위에 어떤 아이템이 있는지 정보도 결국 함께 전달해야하기 때문이다.
 *       PointerHandler 인터페이스 추가//완료
 *      ItemSpawner에 위치값과 Enemy 종류를 파라미터로 받아서 그곳에 확률적으로 여러 종류의 아이템오브젝트 생성하는 함수 작성 // 완료
 *      
 *      ------------------------------------------------------------------------------------------------------------------
 *      7월 4일 FeedBack
 *      생성 파괴되는 빈도가 많지 않은 오브젝트를 풀링하는 것은 오리혀 메모리 낭비.
 *      불필요한 오브젝트풀 삭제
 *      
 *      아이템 이미지 테두리 지우고 이미지를 한개의 이미지에 모두 합친 뒤 SpriteEditor로 잘라서 사용하도록 
 *      ------------------------------------------------------------------------------------------------------------------
 *      
 *      
 *      의문점 : const, readonly를 사용하면 상속이 불가능한가
 *               강사님이 만들어준 풀에 배열이 필요한 이유가 무엇인가. 그냥 큐만 있으면 순서대로 오브젝트를 활성화, 비활성화 할수있는것 아닌가?
 *               Pool pool = pools.Find(p => p.name == prefabName);?
 *               ItemBase 인터페이스?  이미지경로등을 까먹고 지정을 안해주니 오류메세지는 안뜨지만 이미지 로드가 안된다. 왜 문제가 생기는지 찾는데 계속 시간이 걸린다 이럴바에야 인터페이스를 붙혀줄까?
 *      Inventory의 GetItem - 인벤토리에 이미지를 로드하기 위해 필요한것 ItemBase 타입의 currentItem 변수: 아이템 분류Enum(장비, 소비, 기바),이미지로드를 하기위한이름 
 *      slot.currentItem.name == item.name 과 item.IsStackable 이 true면 수량텍스트만 변경// 완료
 *      
 *       이미지의 저장은 스프라이트 아틀라스를 사용하고 나중에 에셋번들을 사용하기로 한다.
 *       아틀라스를 이용해서 Resouces.Load를 이용하니 이미지가 겹쳐서 나오는 문제가 있다, 따라서 그냥 스프라이트를 한 이미지에 몰아넣고 에디터로 자른 뒤 REsouces.LoadAll로 배열을 불러온 뒤
 *       foreach (Sprite s in spite(배열))
 *       {
 *          if (s.name == spritename)
 *          {
 *              slotImage.sprite = s;
 *          }
 *       }
 *      해결 완료
 *      SetItemData 함수 내 획득한 아이템의 아이템 타입에 따라 어떤 탭에, 몇번째 슬롯에 저장할 것인지, 이미 보유중인 아이템인지, 중복소지 가능한 아이템인지,
 *      저장 후 ItemName 을 파라미터로 해당Slot에 Update Slot함수 호출// 완료
 *      
 *      ItemData_Enhance - ItemData, IEnhancable 상속받고 
 *      Enhancer_Description, SuccessRateText, Warning PopUp, ItemLevelText, 슬롯 애니메이션, 성공실패 파티클, 
 *      
 *      
 *      
 *      
 *      
 *      
 *      
 *      
 *      
 *      
 * 
 * 
 * 
 * 
 

 */
