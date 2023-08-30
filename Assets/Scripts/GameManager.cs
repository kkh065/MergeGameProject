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

    //���� ���� ����
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

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� �������ش�.
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
        //��ȭ������
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


        //��������
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

        //ĳ���� ����
        UpdateCaracter();
        //���� �ε�
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

    public int GetMakingArrowLevel() //����ȭ�췹�� ���׷��̵� �߰��� ���� �����ؾ���
    {
        return 1;
    }

    public int GetMaxArrowLevel()
    {
        //�� ���� ���� ȭ�� ����?
        return 0;
    }

    #region �������� ����
    //���� �������� ������ ����, ���� ���� �� ���� �������� �ε�
   

    //��� ���Ͱ� ������ Ŭ����. �������������� �̵�
    
    // ���Ͱ� �ڽ��� ������ ���� ���ӸŴ��� �Լ� ȣ�� - 
    

    //���� �ı��Ǹ� ���, ���罺������ -1 �����.
    //ȯ������ - 1���������� ���ư�, ���罺������ ��� ȯ���� ����

    #endregion

    #region ���� ���� �� ai ����
    //���������� 5���� - ������ ����Ʈ�� ���� ����. ���������� ����Ʈ���� �����ϰ� ����.
    //5�������� ���� ������ ����

    //���ʹ� ���������� �ö󰥼��� ü���� ������.
    //Ǯ�� ����

    //���ʹ� �¾ ��ġ���� ������ �̵���.
    //���� ���� ��Ÿ��� ������ ���� ����
  
    public void OnReincarnationButton()
    {
        //ȯ���� ȹ��
        _fade.FaidIn();
        Stage = 1;
    }

    public void StageLoad()
    {
        //�װų� ȯ�� �� ���̵��� ��, ���������� 1���θ���� ����
        //����ü���ʱ�ȭ
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
            //��������
            Monster m = _monsterPool.Get();
            m.transform.position = _monsterSpawnPoint.transform.position;
            m.InitMonster(MonsterType.Boss, _monsterPool);
            _liveMonsterList.Add(m);
        }
        else
        {
            //�Ϲݸ�����
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
        //���� ����� 5��� ȹ��
        //�������� ����� 5���̾� ȹ��
        //���� ���͸���Ʈ���� �ڱ��ڽ� ����.
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

    #region ĳ���� �� ai ����
    //ĳ���� �����Ϳ��� ���� �޾ƿͼ� �׸�ŭ ���� �ϰ� �̴ֽ���

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
