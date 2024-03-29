using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    #region 지역변수

    ArrowLevelData _arrowLevelData;
    AsyncOperation Operation;
    string Path;
    bool _isGo = false;

    List<AudioClip> _soundEffect = new List<AudioClip>();
    public List<AudioClip> SoundEffect { get { return _soundEffect; } }

    public UpgradeDataList UpgradeDatas;
    
    public ArrowLevelData ArrowLevelData { get { return _arrowLevelData; } set { _arrowLevelData = value; } }

    public bool IsLoadEnd { set; get; }

    Dictionary<string, Sprite> _currencyImage = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> CurrencyImage { get { return _currencyImage; } }


    Vector3[] _characterPosition = new Vector3[8]
    {
        new Vector3(-7f, -1f, 0),
        new Vector3(-7.5f, -2f, 0),
        new Vector3(-5.5f, -1f, 0),
        new Vector3(-6f, -2f, 0),
        new Vector3(-4f, -1f, 0),
        new Vector3(-4.5f, -2f, 0),
        new Vector3(-2.5f, -1f, 0),
        new Vector3(-3f, -2f, 0),
    };

    public Vector3[] CharacterPosition { get { return _characterPosition; } }

    #endregion

    private static Data instance = null;

    public static Data Instance
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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        IsLoadEnd = false;
        Path = Application.persistentDataPath;
        _arrowLevelData = new ArrowLevelData();
        UpgradeDatas = new UpgradeDataList();
        UpgradeDatas.upgradeDataList = new List<UpgradeData>();
        ReadUpgradeData();
        StartCoroutine(SceneLoad());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            PlayerPrefs.DeleteAll();
        }
    }


    #region 세이브
    public void SaveUpgradeData()
    {
        //업그레이드 데이터를 제이슨파일로 저장
        string Json = JsonUtility.ToJson(UpgradeDatas);

        string TempPath = Path + "/UpgradeLevelData.json";

        using (StreamWriter outStream = File.CreateText(TempPath))
        {
            outStream.Write(Json);
        }
    }

    public void SaveInventoryData()
    {
        string Json = JsonUtility.ToJson(_arrowLevelData);

        string TempPath = Path + "/ArrowLevelData.json";

        using (StreamWriter outStream = File.CreateText(TempPath))
        {
            outStream.Write(Json);
        }
    }

    void UpgradeDataCreate() //초기 생성용
    {
        UpgradeData data = new UpgradeData();
        data.CordName = "GoldAttackDamageLevel";
        data.Name = "기본공격력";
        data.Level = 0;
        data.MaxLevel = 99999;
        data.Increase = 5;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldAttackSpeedLevel";
        data.Name = "공격속도";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Increase = 0.01f;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldCriticalLevel";
        data.Name = "치명타확률";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Increase = 2;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 2;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldCriticalDamageLevel";
        data.Name = "치명타 데미지";
        data.Level = 0;
        data.MaxLevel = 500;
        data.Increase = 1;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 3;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldWallHpLevel";
        data.Name = "담장 강화";
        data.Level = 0;
        data.MaxLevel = 30;
        data.Increase = 10;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 4;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        //관리업그레이드
        data = new UpgradeData();
        data.CordName = "ManagementArcherLevel";
        data.Name = "아처 모집";
        data.Level = 0;
        data.MaxLevel = 7;
        data.Increase = 1;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Management;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "ManagementWallHpLevel";
        data.Name = "담장 강화";
        data.Level = 0;
        data.MaxLevel = 30;
        data.Increase = 10;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Management;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);


        //공격 업그레이드
        data = new UpgradeData();
        data.CordName = "DiaAttackDamegeLevel";
        data.Name = "기본 공격력";
        data.Level = 0;
        data.MaxLevel = 500;
        data.Increase = 5;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaAttackSpeedLevel";
        data.Name = "공격속도";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Increase = 0.05f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaCriticalLevel";
        data.Name = "치명타확률";
        data.Level = 0;
        data.MaxLevel = 30;
        data.Increase = 2;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 2;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaCriticalDamageLevel";
        data.Name = "치명타 데미지";
        data.Level = 0;
        data.MaxLevel = 500;
        data.Increase = 5;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 3;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        //제작업그레이드
        data = new UpgradeData();
        data.CordName = "MakingSpeedLevel";
        data.Name = "빠른 제작";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Increase = 0.2f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "MakingArrowLevelLevel";
        data.Name = "제작 화살 레벨 증가";
        data.Level = 0;
        data.MaxLevel = 150;
        data.Increase = 1;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Reincarnation;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaMakingArrowLevelLevel";
        data.Name = "제작 화살 레벨 증가";
        data.Level = 0;
        data.MaxLevel = 80;
        data.Increase = 1;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 2;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "MakingAutoSpeedLevel";
        data.Name = "자동 제작";
        data.Level = 0;
        data.MaxLevel = 50;
        data.Increase = 0.2f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 3;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "MergeAutoSpeedLevel";
        data.Name = "자동 합성";
        data.Level = 0;
        data.MaxLevel = 50;
        data.Increase = 0.2f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 4;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);
    }
    #endregion;


    #region 로드


    IEnumerator SceneLoad()
    {
        Operation = SceneManager.LoadSceneAsync("GameScene");
        Operation.allowSceneActivation = false;
        LoadSoundData();
        LoadCurrencyImage();

        while (true)
        {
            if (_isGo == false) // 제이선 데이터 로딩이 끝날떄까지 대기
            {
                yield return null;
                continue;
            }
            if (Operation.progress >= 0.9f && IsLoadEnd == true) // 씬로드 체크 후, 프로그래스바가 100%가 되면
            {
                Operation.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    async void ReadUpgradeData()
    {
        if (File.Exists(Path + "/UpgradeLevelData.json"))
        {
            string json;
            using (StreamReader rd = new StreamReader(Path + "/UpgradeLevelData.json"))
            {
                json = await rd.ReadToEndAsync();//await is to make work async
            }                                   //also await is needed to null check
                                                //without calling false thousand times
            if (string.IsNullOrEmpty(json) == false)
            {
                await Task.Run(() =>
                {
                    UpgradeDatas = JsonUtility.FromJson<UpgradeDataList>(json);
                    if(UpgradeDatas.upgradeDataList.Count <= 0)
                    {
                        UpgradeDataCreate();
                        SaveUpgradeData();
                    }
                    InventoryDataLoad();
                });
            }
        }
        else
        {
            UpgradeDataCreate();
            SaveUpgradeData();
            InventoryDataLoad();
        }
    }

    async void InventoryDataLoad()
    {
        if (File.Exists(Path + "/ArrowLevelData.json"))
        {
            string json;
            using (StreamReader rd = new StreamReader(Path + "/ArrowLevelData.json"))
            {
                json = await rd.ReadToEndAsync();
            }
            if (string.IsNullOrEmpty(json) == false)
            {
                await Task.Run(() =>
                {
                    _arrowLevelData = JsonUtility.FromJson<ArrowLevelData>(json);
                    _isGo = true;
                });
            }
        }
        else
        {
            SaveInventoryData();
            _isGo = true;
        }
    }

    #endregion;

    public UpgradeData GetUpgradeData(UpgradeType type, int ButtonIdx)
    {
        for(int i = 0; i < UpgradeDatas.upgradeDataList.Count; i++)
        {
            if (UpgradeDatas.upgradeDataList[i].UpgradeType == type && UpgradeDatas.upgradeDataList[i].ButtonIndex == ButtonIdx)
            {
                return UpgradeDatas.upgradeDataList[i];
            }
        }

        return null;
    }

    void LoadCurrencyImage()
    {
        _currencyImage.Add("Gold", Resources.Load<Sprite>("Currency/Gold"));
        _currencyImage.Add("Dia", Resources.Load<Sprite>("Currency/Dia"));
        _currencyImage.Add("Reincarnation", Resources.Load<Sprite>("Currency/Reincarnation"));
    }
    void LoadSoundData()
    {
        _soundEffect.Add(Resources.Load<AudioClip>("Sound/Button"));
        _soundEffect.Add(Resources.Load<AudioClip>("Sound/Toggle"));
        _soundEffect.Add(Resources.Load<AudioClip>("Sound/WallTakeDamage"));
        _soundEffect.Add(Resources.Load<AudioClip>("Sound/MakingArrow"));
        _soundEffect.Add(Resources.Load<AudioClip>("Sound/MergeArrow"));
    }
}
#region 세이브데이터 클래스

[Serializable]
public class UpgradeDataList
{
    public List<UpgradeData> upgradeDataList = new List<UpgradeData>();
}
[Serializable]
public class UpgradeData
{
    public string CordName;
    public string Name;
    public int Level;
    public int MaxLevel;
    public int Price;
    public UpgradeType UpgradeType;
    public PriceType priceType;
    public float Increase;
    public int ButtonIndex;
}


[Serializable]
public class ArrowLevelData
{
    public int[] EquipData = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
    public List<int> InventoryData = new List<int>();
    //캐릭터 화살 레벨 데이터. 장착화살 데이터랑 인벤토리에 들고있는거 데이터
}

public enum UpgradeType
{
    Gold,
    Management,
    Attack,
    Making,
}

public enum PriceType
{
    Gold,
    Dia,
    Reincarnation,
}

#endregion

public enum MonsterType
{
    Nomal,
    Boss,
}

public enum SoundIndex
{
    UIButton,
    UIToggle,
    WallTakeDamage,
    MakingArrow,
    MergeArrow,
}
