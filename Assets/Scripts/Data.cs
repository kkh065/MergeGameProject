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
    public UpgradeData UpgradeData { get { return _upgradeData; } set { _upgradeData = value; } }

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
        string json;
        using (StreamReader rd = new StreamReader(Application.persistentDataPath + "/UpgradeLevelData.json"))
        {
            json = await rd.ReadToEndAsync();//await is to make work async
        }                                   //also await is needed to null check
                                            //without calling false thousand times
        if (string.IsNullOrEmpty(json) == false)
        {
            await Task.Run(() => {
                _upgradeData = JsonUtility.FromJson<UpgradeData>(json);

                InventoryDataLoad();
            });            
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
        string json;
        using (StreamReader rd = new StreamReader(Application.persistentDataPath + "/ArrowLevelData.json"))
        {
            json = await rd.ReadToEndAsync();//await is to make work async
        }                                   //also await is needed to null check
                                            //without calling false thousand times
        if (string.IsNullOrEmpty(json) == false)
        {
            await Task.Run(() => {
                _arrowLevelData = JsonUtility.FromJson<ArrowLevelData>(json);

                StartCoroutine(SceneLoad()); // 씬로드 시작
            });
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
    public int[] EquipData;
    public List<int> InventoryData;
    //캐릭터 화살 레벨 데이터. 장착화살 데이터랑 인벤토리에 들고있는거 데이터
}

#endregion
