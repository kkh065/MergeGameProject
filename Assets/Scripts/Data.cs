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
    UpgradeData _upgradeData;
    ArrowLevelData _arrowLevelData;
    UpgradeTabList datas;
    public UpgradeData UpgradeData { get { return _upgradeData; } set { _upgradeData = value; } }
    public ArrowLevelData ArrowLevelData { get { return _arrowLevelData; } set { _arrowLevelData = value; } }
    public UpgradeTabList Datas { get { return datas; } set { datas = value; } }

    public bool IsLoadEnd { private set; get; }
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
        _upgradeData = new UpgradeData();
        _arrowLevelData = new ArrowLevelData();
        datas = new UpgradeTabList();
        datas.UpgradeList = new List<UpgradeTabData>();
        ReadUpgradeData();        
    }


    void Update()
    {

    }

    #region 세이브
    void SaveUpgradeData()
    {
        //업그레이드 데이터를 제이슨파일로 저장
        string Json = JsonUtility.ToJson(_upgradeData);

        string path = Application.persistentDataPath + "/UpgradeLevelData.json";

        using (StreamWriter outStream = File.CreateText(path))
        {
            outStream.Write(Json);
        }
    }

    public void SaveInventoryData(int[] ints, List<int> inventory)
    {
        _arrowLevelData.EquipData = ints;
        _arrowLevelData.InventoryData = inventory;

        string Json = JsonUtility.ToJson(_arrowLevelData);

        string path = Application.persistentDataPath + "/ArrowLevelData.json";

        using (StreamWriter outStream = File.CreateText(path))
        {
            outStream.Write(Json);
        }
    }

    void UpgradeTabDataCreate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpgradeTabData tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldAttackDamageLevel;
            tab.MaxLevel = 99999;
            tab.Name = "기본공격력";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldAttackSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "공격속도";
            tab.Increase = 0.01f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldCriticalLevel;
            tab.MaxLevel = 20;
            tab.Name = "치명타확률";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}%만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldCriticalDamageLevel;
            tab.MaxLevel = 500;
            tab.Name = "치명타 데미지";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}%만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldWallHpLevel;
            tab.MaxLevel = 30;
            tab.Name = "담장 강화";
            tab.Increase = 10;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"담장 체력을 {tab.Increase * tab.NowLevel}만큼 증가시킨다";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 4;
            datas.UpgradeList.Add(tab);


            //관리업그레이드
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.ManagementArcherLevel;
            tab.MaxLevel = 7;
            tab.Name = "아처 모집";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"배치 가능한 아처의 수 {tab.Increase * tab.NowLevel}칸 증가";
            tab.Type = UpgradeType.Management;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.ManagementWallHpLevel;
            tab.MaxLevel = 30;
            tab.Name = "담장 강화";
            tab.Increase = 10;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"담장 체력 {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Management;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            //공격 업그레이드
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaAttackDamegeLevel;
            tab.MaxLevel = 500;
            tab.Name = "기본 공격력";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaAttackSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "공격속도";
            tab.Increase = 0.05f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaCriticalLevel;
            tab.MaxLevel = 20;
            tab.Name = "치명타확률";
            tab.Increase = 0.5f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel}% 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaCriticalDamageLevel;
            tab.MaxLevel = 500;
            tab.Name = "치명타 데미지";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel}% 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            //제작업그레이드
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "빠른 제작";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"화살 제작 시간 {tab.Increase * tab.NowLevel}초 감소";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingArrowLevelLevel;
            tab.MaxLevel = 150;
            tab.Name = "제작 화살 레벨 증가";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"제작된 화살의 레벨 {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaMakingArrowLevelLevel;
            tab.MaxLevel = 80;
            tab.Name = "제작 화살 레벨 증가";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"제작된 화살의 레벨 {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingAutoSpeedLevel;
            tab.MaxLevel = 50;
            tab.Name = "자동 제작";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"(자동)10초에 {tab.Increase * tab.NowLevel}번 화살을 제작";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MergeAutoSpeedLevel;
            tab.MaxLevel = 50;
            tab.Name = "자동 합성";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"(자동)10초에 {tab.Increase * tab.NowLevel}번 화살을 결합";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 4;
            datas.UpgradeList.Add(tab);

            string Json = JsonUtility.ToJson(datas);

            string path = Application.persistentDataPath + "/UpgradeTabList.json";

            using (StreamWriter outStream = File.CreateText(path))
            {
                outStream.Write(Json);
            }

            Debug.Log(Json);
        }
    }

    #endregion


    #region 로드

    AsyncOperation Operation;
    IEnumerator SceneLoad()
    {
        Operation = SceneManager.LoadSceneAsync("GameScene");
        Operation.allowSceneActivation = false;

        while(true)
        {
            if (Operation.isDone == true)
            {
                IsLoadEnd = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    public void NextScene()
    {
        if(Operation != null)
        {
            Operation.allowSceneActivation = true;
        }
    }

    async void ReadUpgradeData()
    {
        if (File.Exists(Application.persistentDataPath + "/UpgradeLevelData.json"))
        {
            string json;
            using (StreamReader rd = new StreamReader(Application.persistentDataPath + "/UpgradeLevelData.json"))
            {
                json = await rd.ReadToEndAsync();//await is to make work async
            }                                   //also await is needed to null check
                                                //without calling false thousand times
            if (string.IsNullOrEmpty(json) == false)
            {
                await Task.Run(() =>
                {
                    _upgradeData = JsonUtility.FromJson<UpgradeData>(json);

                    InventoryDataLoad();
                });
            }
        }
        else
        {
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

    async void UpgradeTabData()
    {
        //업그레이드 탭 세팅

        if (File.Exists(Application.persistentDataPath + "/UpgradeTabList.json"))
        {
            string json = "";
            using (StreamReader inStream = new StreamReader(Application.persistentDataPath + "/UpgradeTabList.json"))
            {
                json = inStream.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json) == false)
            {
                datas = JsonUtility.FromJson<UpgradeTabList>(json);
            }
            else Debug.Log("내용이 없습니다.");
        }
        else
        {
            Debug.Log("파일이 없습니다.");
            UpgradeTabDataCreate();
        }
        //각 창에다가 업그레이드 탭 생성 -> 리스트로 만들어서 제이슨으로 저장후 불러와서 생성하자
        //이넘으로 업그레이드 타입 만들고 같이 저장했다가, 타입별로 부모 스위치케이스 ㄱㄱ
        //인덱스를 인자로 받아와서 스위치케이스로 나누기. 함수를 버튼에 델리게이트로 붙이면 인자값에 인덱스를 넣을 수 있을텐데
    }

    async void InventoryDataLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/ArrowLevelData.json"))
        {
            string json;
            using (StreamReader rd = new StreamReader(Application.persistentDataPath + "/ArrowLevelData.json"))
            {
                json = await rd.ReadToEndAsync();//await is to make work async
            }                                   //also await is needed to null check
                                                //without calling false thousand times
            if (string.IsNullOrEmpty(json) == false)
            {
                await Task.Run(() =>
                {
                    _arrowLevelData = JsonUtility.FromJson<ArrowLevelData>(json);

                    StartCoroutine(SceneLoad()); // 씬로드 시작
                });
            }
        }
        else
        {
            SaveInventoryData(_arrowLevelData.EquipData, _arrowLevelData.InventoryData);
            StartCoroutine(SceneLoad());
        }
    }

    #endregion
}


#region 세이브데이터 클래스

[Serializable]
public class UpgradeData
{
    //업그레이드 데이터

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
}


[Serializable]
public class ArrowLevelData
{
    public int[] EquipData = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
    public List<int> InventoryData = new List<int>();
    //캐릭터 화살 레벨 데이터. 장착화살 데이터랑 인벤토리에 들고있는거 데이터
}

#endregion
