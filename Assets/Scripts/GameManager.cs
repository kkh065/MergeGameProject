using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    MainUIController _mainUI;
    CurrencyContoroller _currencyUI;
    int _gold = 0;
    public int Gold
    {
        get { return _gold; }
        set 
        {
            _gold = value;
            _currencyUI.UpdateGold();
        }
    }

    int _dia = 0;
    public int Dia
    {
        get { return _dia; }
        set 
        {
            _dia = value;
            _currencyUI.UpdateDia();
        }
    }

    int _reincarnation = 0;
    public int Reincarnation
    {
        get { return _reincarnation; }
        set 
        {
            _reincarnation = value;
            _currencyUI.UpdateReincarnation();
        }
    }

    public int[] EquipArrowData;
    public List<int> InventoryData;



    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제해준다.
            Destroy(this.gameObject);
        }

        EquipArrowData = Data.Instance.ArrowLevelData.EquipData;
        InventoryData = Data.Instance.ArrowLevelData.InventoryData;
    }

    void Start()
    {
        _mainUI = GameObject.Find("MainUI").GetComponent<MainUIController>();
        _currencyUI = GameObject.Find("Currency").GetComponent<CurrencyContoroller>();
        InitCurrency();
    }

    void Update()
    {

    }

    void InitCurrency()
    {
        if(PlayerPrefs.HasKey("Gold"))
        {
            _gold = PlayerPrefs.GetInt("Gold");
        }

        if (PlayerPrefs.HasKey("Dia"))
        {
            _dia = PlayerPrefs.GetInt("Dia");
        }

        if (PlayerPrefs.HasKey("Reincarnation"))
        {
            _reincarnation = PlayerPrefs.GetInt("Reincarnation");
        }
    }

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt("Gold", _gold);
        PlayerPrefs.SetInt("Dia", _dia);
        PlayerPrefs.SetInt("Reincarnation", _reincarnation);
    }

    public int GetMakingArrowLevel() //제작화살레벨 업그레이드 추가시 여기 수정해야함
    {
        return 1;
    }

    public int GetMaxArrowLevel()
    {
        //내 가장 높은 화살 레벨?
        return 0;
    }

    #region 스테이지 관리
    //현재 스테이지 데이터 저장, 게임 시작 시 현재 스테이지 로드
   

    //모든 몬스터가 죽으면 클리어. 다음스테이지로 이동
    List<GameObject> _liveMonsterList = new List<GameObject>();
    // 몬스터가 자신이 죽을때 마다 게임매니저 함수 호출 - 
    public void MonsterDie(GameObject monster)
    {
        //몬스터 사망시 5골드 획득
        //보스몬스터 사망시 5다이아 획득
        //현재 몬스터리스트에서 자기자신 삭제.
        _liveMonsterList.Remove(monster);
        if (_liveMonsterList.Count <= 0)
        {
            //스테이지 클리어!
            //다음스테이지 로드            
        }
    }

    //벽이 파괴되면 사망, 현재스테이지 -1 재시작.
    //환생구현 - 1스테이지로 돌아감, 현재스테이지 비례 환생석 지급

    #endregion

    #region 몬스터 스폰 및 ai 구현
    //스테이지당 5마리 - 스폰시 리스트를 만들어서 담음. 몬스터프리팹 리스트에서 랜덤하게 생성.
    //5스테이지 마다 보스몹 스폰

    //몬스터는 스테이지가 올라갈수록 체력이 증가함.
    //풀링 구현

    //몬스터는 태어난 위치에서 벽까지 이동함.
    //벽이 공격 사거리에 들어오면 벽을 공격
    #endregion

    #region 캐릭터 및 ai 구현
    //시작할때 캐릭터 데이터에서 숫자 받아와서 그만큼 생성
    //각 캐릭터들은 현재 소환된 몬스터 리스트를 돌아서 몬스터가 있다면 자동으로 공격
    //공격속도 구현 및 애니메이션 맞춰서 구현
    #endregion
}
