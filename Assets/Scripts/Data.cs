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
    public UpgradeDataList UpgradeDatas;
    
    public ArrowLevelData ArrowLevelData { get { return _arrowLevelData; } set { _arrowLevelData = value; } }

    public bool IsLoadEnd { set; get; }
    string Path;

    bool _isGo = false;
    AsyncOperation Operation;

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
        data.Explan = $"{data.Name}이 {data.Increase * data.Level}만큼 증가";
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
        data.Explan = $"{data.Name}가 {data.Increase * data.Level}만큼 증가";
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
        data.Explan = $"{data.Name}이 {data.Increase * data.Level}%만큼 증가";
        data.Increase = 1;
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
        data.Explan = $"{data.Name}이 {data.Increase * data.Level}만큼 증가";
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
        data.Explan = $"{data.Name}이 {data.Increase * data.Level}만큼 증가";
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
        data.Explan = $"배치 가능한 아처의 수 {data.Increase * data.Level}칸 증가";
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
        data.Explan = $"담장 체력 {data.Increase * data.Level} 증가";
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
        data.Explan = $"{data.Name} {data.Increase * data.Level} 증가";
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
        data.Explan = $"{data.Name} {data.Increase * data.Level} 증가";
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
        data.MaxLevel = 20;
        data.Explan = $"{data.Name} {data.Increase * data.Level}% 증가";
        data.Increase = 0.5f;
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
        data.Explan = $"{data.Name} {data.Increase * data.Level}% 증가";
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
        data.Explan = $"화살 제작 시간 {data.Increase * data.Level}초 감소";
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
        data.Explan = $"제작된 화살의 레벨 {data.Increase * data.Level} 증가";
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
        data.Explan = $"제작된 화살의 레벨 {data.Increase * data.Level} 증가";
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
        data.Explan = $"(자동)10초에 {data.Increase * data.Level}번 화살을 제작";
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
        data.Explan = $"(자동)10초에 {data.Increase * data.Level}번 화살을 결합";
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
                    Debug.Log(UpgradeDatas.upgradeDataList.Count);
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

        /*
        //제이슨파일에서 업그레이드 데이터 읽어오기
        if (File.Exists(Application.persistentDataPath + "/UpgradeLevelData.json"))
        {
            string json = "";
            using (StreamReader inStream = new StreamReader(Application.persistentDataPath + "/UpgradeLevelData.json"))
            {
                json = inStream.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json) == false)
            {
                _upgradeData = JsonUtility.FromJson<UpgradeData>(json);
            }
            else Debug.Log("내용이 없습니다.");
        }
        else
        {
            Debug.Log("파일이 없습니다.");
            SaveUpgradeData();
        }

        Debug.Log("로딩완료");
        */
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
}
#region 세이브데이터 클래스

[Serializable]
public class UpgradeDataList
{
    //업그레이드 데이터
    public List<UpgradeData> upgradeDataList = new List<UpgradeData>();
    /*
    //골드 업그레이드
    public int GoldAttackDamageLevel = 0;
    public int GoldAttackSpeedLevel = 0;
    public int GoldCriticalLevel = 0;
    public int GoldCriticalDamageLevel = 0;
    public int GoldWallHpLevel = 0;

    //관리 업그레이드
    public int ManagementArcherLevel = 0;
    public int ManagementWallHpLevel = 0;
    //public int ManagementUpgrade3Level = 0;
    //public int ManagementUpgrade4Level = 0;
    //public int ManagementUpgrade5Level = 0;

    //공격 업그레이드
    public int DiaAttackDamegeLevel = 0;
    public int DiaAttackSpeedLevel = 0;
    public int DiaCriticalLevel = 0;
    public int DiaCriticalDamageLevel = 0;
    //public int AttackUpgrade5Level = 0;

    //제작 업그레이드
    public int MakingSpeedLevel = 0;
    public int MakingArrowLevelLevel = 0;
    public int DiaMakingArrowLevelLevel = 0;
    public int MakingAutoSpeedLevel = 0;
    public int MergeAutoSpeedLevel = 0;

    //특수 업그레이드 (카드개념? 보스전 연동 보상)
    */
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
    public PriceType priceType; // 0 = 골드, 1 = 다이아
    public String Explan;
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
