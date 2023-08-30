using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    [SerializeField] Fade _fade;
    [SerializeField] WallHpSlider _hpSlider;
    [SerializeField] GameObject _arrow;
    [SerializeField] GameObject _arrowPoolParent;
    [SerializeField] GameObject _characterPool;
    [SerializeField] GameObject _character;

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

    int _stage = 0;
    public int Stage { get { return _stage; } set { _stage = value; } }
    int _wallHp = 0;
    public int WallHP 
    {
        get { return _wallHp; }
        set 
        {
            _wallHp = value;
            _hpSlider.SetHpUI();
            if (_wallHp <= 0)
            {
                OnReincarnationButton();
            }
        } 
    }
    int _wallMaxHp = 0;
    public int WallMaxHP { get { return _wallHp; } set { _wallHp = value; } }

    public int[] EquipArrowData;
    public List<int> InventoryData;

    //몬스터 관련 변수
    List<Monster> _liveMonsterList = new List<Monster>();
    public List<Monster> LiveMonsterList { get { return _liveMonsterList; } set { _liveMonsterList = value; } }
    public IObjectPool<Monster> _monsterPool;

    [SerializeField] GameObject[] _monsterPrefabs;
    [SerializeField] int _maxPoolSize;
    GameObject _monsterSpawnPoint;
    GameObject _wall;


    IObjectPool<ArrowController> _arrowpool;
    int _maxArrowCount = 50;

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
        _monsterSpawnPoint = GameObject.Find("MonsterSpawnPoint");
        _monsterPool = new ObjectPool<Monster>(CreateMonster, GetMonster, ReleaseMonster, DestroyMonster, maxSize: _maxPoolSize);
        _arrowpool = new ObjectPool<ArrowController>(CreateArrow, GetArrow, ReleaseArrow, DestroyArrow, maxSize: _maxArrowCount);
        _wall = GameObject.Find("Wall");
    }

    void Start()
    {
        _mainUI = GameObject.Find("MainUI").GetComponent<MainUIController>();
        _currencyUI = GameObject.Find("Currency").GetComponent<CurrencyContoroller>();
        InitGamaData();
    }

    void InitGamaData()
    {
        //재화데이터
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


        //벽데이터
        if (PlayerPrefs.HasKey("WallMaxHp"))
        {
            _wallMaxHp = PlayerPrefs.GetInt("WallMaxHp");
        }
        else
        {
            _wallMaxHp = 100;
        }
        _wallHp = _wallMaxHp;

        //

        if (PlayerPrefs.HasKey("Stage"))
        {
            _stage = PlayerPrefs.GetInt("Stage");
        }
        else
        {
            _stage = 1;
        }

        //캐릭터 세팅
        UpdateCaracter();
        //게임 로드
        StageLoad();
    }
    public void SaveGameData()
    {
        PlayerPrefs.SetInt("Gold", _gold);
        PlayerPrefs.SetInt("Dia", _dia);
        PlayerPrefs.SetInt("Reincarnation", _reincarnation);

        PlayerPrefs.SetInt("WallMaxHp", _wallMaxHp);
        PlayerPrefs.SetInt("Stage", _stage);
    }

    public Vector2 GetWallPos() => _wall.transform.position;

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
    
    // 몬스터가 자신이 죽을때 마다 게임매니저 함수 호출 - 
    

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
  
    public void OnReincarnationButton()
    {
        //환생석 획득
        _fade.FaidIn();
        Stage = 1;
    }

    public void StageLoad()
    {
        //죽거나 환생 시 페이드인 후, 스테이지를 1으로만들고 시작
        //담장체력초기화
        _wallHp = _wallMaxHp;
        _fade.FaidOut();
    }


    public void WaveLoad()
    {
        MonsterSpawn();
    }

    void MonsterSpawn()
    {
        if(_stage % 5 == 0)
        {
            //보스생성
            Monster m = _monsterPool.Get();
            m.transform.position = _monsterSpawnPoint.transform.position;
            m.InitMonster(MonsterType.Boss, _monsterPool);
            _liveMonsterList.Add(m);
        }
        else
        {
            //일반몹생성
            for (int i = 0; i < 5; i++)
            {
                Monster m = _monsterPool.Get();
                m.transform.position = _monsterSpawnPoint.transform.position;
                m.InitMonster(MonsterType.Nomal, _monsterPool);
                _liveMonsterList.Add(m);
            }
        }
    }
    public void MonsterDie(GameObject monster)
    {
        //몬스터 사망시 5골드 획득
        //보스몬스터 사망시 5다이아 획득
        //현재 몬스터리스트에서 자기자신 삭제.
        _liveMonsterList.Remove(monster.GetComponent<Monster>());
        if (_liveMonsterList.Count <= 0)
        {
            _stage++;
            SaveGameData();
            WaveLoad();
        }
    }

    Monster CreateMonster()
    {
        GameObject monster = Instantiate(_monsterPrefabs[Random.Range(0, _monsterPrefabs.Length)], _monsterSpawnPoint.transform);
        return monster.GetComponent<Monster>();
    }

    void GetMonster(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }

    void ReleaseMonster(Monster monster)
    {
        _liveMonsterList.Remove(monster);
        monster.gameObject.SetActive(false);       
    }

    void DestroyMonster(Monster monster)
    {
        _liveMonsterList.Remove(monster);
        Destroy(monster.gameObject);
    }
    #endregion

    #region 캐릭터 및 ai 구현
    //캐릭터 데이터에서 숫자 받아와서 그만큼 생성 하고 이닛실행

    void UpdateCaracter()
    {
        for (int i = 0; i < 1 + Data.Instance.GetUpgradeData(UpgradeType.Management, 0).Level; i++)
        {
            bool IsCreate = true;
            for(int j = 0; j < _characterPool.transform.childCount; j++)
            {
                if(i == j)
                {
                    _characterPool.transform.GetChild(j).gameObject.GetComponent<CharacterAI>().UpdateCharacterState();
                    IsCreate = false;
                    break;
                }
            }

            if(IsCreate)
            {
                GameObject Character = Instantiate(_character, _characterPool.transform);
                Character.GetComponent<CharacterAI>().InitCharacter(i, _arrowpool);
            }
        }
    }

    ArrowController CreateArrow()
    {
        GameObject Arrow = Instantiate(_arrow, _arrowPoolParent.transform);
        return Arrow.GetComponent<ArrowController>();
    }

    void GetArrow(ArrowController Arrow)
    {
        Arrow.gameObject.SetActive(true);
    }

    void ReleaseArrow(ArrowController Arrow)
    {
        Arrow.gameObject.SetActive(false);
    }

    void DestroyArrow(ArrowController Arrow)
    {
        Destroy(Arrow.gameObject);
    }

    #endregion

}
