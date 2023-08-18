using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    #region ��������
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


    #region ���̺�
    public void SaveUpgradeData()
    {
        //���׷��̵� �����͸� ���̽����Ϸ� ����
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

    void UpgradeDataCreate() //�ʱ� ������
    {
        UpgradeData data = new UpgradeData();
        data.CordName = "GoldAttackDamageLevel";
        data.Name = "�⺻���ݷ�";
        data.Level = 0;
        data.MaxLevel = 99999;
        data.Explan = $"{data.Name}�� {data.Increase * data.Level}��ŭ ����";
        data.Increase = 5;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldAttackSpeedLevel";
        data.Name = "���ݼӵ�";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Explan = $"{data.Name}�� {data.Increase * data.Level}��ŭ ����";
        data.Increase = 0.01f;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldCriticalLevel";
        data.Name = "ġ��ŸȮ��";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Explan = $"{data.Name}�� {data.Increase * data.Level}%��ŭ ����";
        data.Increase = 1;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 2;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldCriticalDamageLevel";
        data.Name = "ġ��Ÿ ������";
        data.Level = 0;
        data.MaxLevel = 500;
        data.Explan = $"{data.Name}�� {data.Increase * data.Level}��ŭ ����";
        data.Increase = 1;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 3;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "GoldWallHpLevel";
        data.Name = "���� ��ȭ";
        data.Level = 0;
        data.MaxLevel = 30;
        data.Explan = $"{data.Name}�� {data.Increase * data.Level}��ŭ ����";
        data.Increase = 10;
        data.Price = 100 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Gold;
        data.ButtonIndex = 4;
        data.priceType = PriceType.Gold;
        UpgradeDatas.upgradeDataList.Add(data);

        //�������׷��̵�
        data = new UpgradeData();
        data.CordName = "ManagementArcherLevel";
        data.Name = "��ó ����";
        data.Level = 0;
        data.MaxLevel = 7;
        data.Explan = $"��ġ ������ ��ó�� �� {data.Increase * data.Level}ĭ ����";
        data.Increase = 1;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Management;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "ManagementWallHpLevel";
        data.Name = "���� ��ȭ";
        data.Level = 0;
        data.MaxLevel = 30;
        data.Explan = $"���� ü�� {data.Increase * data.Level} ����";
        data.Increase = 10;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Management;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);


        //���� ���׷��̵�
        data = new UpgradeData();
        data.CordName = "DiaAttackDamegeLevel";
        data.Name = "�⺻ ���ݷ�";
        data.Level = 0;
        data.MaxLevel = 500;
        data.Explan = $"{data.Name} {data.Increase * data.Level} ����";
        data.Increase = 5;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaAttackSpeedLevel";
        data.Name = "���ݼӵ�";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Explan = $"{data.Name} {data.Increase * data.Level} ����";
        data.Increase = 0.05f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaCriticalLevel";
        data.Name = "ġ��ŸȮ��";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Explan = $"{data.Name} {data.Increase * data.Level}% ����";
        data.Increase = 0.5f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 2;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaCriticalDamageLevel";
        data.Name = "ġ��Ÿ ������";
        data.Level = 0;
        data.MaxLevel = 500;
        data.Explan = $"{data.Name} {data.Increase * data.Level}% ����";
        data.Increase = 5;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Attack;
        data.ButtonIndex = 3;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        //���۾��׷��̵�
        data = new UpgradeData();
        data.CordName = "MakingSpeedLevel";
        data.Name = "���� ����";
        data.Level = 0;
        data.MaxLevel = 20;
        data.Explan = $"ȭ�� ���� �ð� {data.Increase * data.Level}�� ����";
        data.Increase = 0.2f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 0;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "MakingArrowLevelLevel";
        data.Name = "���� ȭ�� ���� ����";
        data.Level = 0;
        data.MaxLevel = 150;
        data.Explan = $"���۵� ȭ���� ���� {data.Increase * data.Level} ����";
        data.Increase = 1;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 1;
        data.priceType = PriceType.Reincarnation;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "DiaMakingArrowLevelLevel";
        data.Name = "���� ȭ�� ���� ����";
        data.Level = 0;
        data.MaxLevel = 80;
        data.Explan = $"���۵� ȭ���� ���� {data.Increase * data.Level} ����";
        data.Increase = 1;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 2;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "MakingAutoSpeedLevel";
        data.Name = "�ڵ� ����";
        data.Level = 0;
        data.MaxLevel = 50;
        data.Explan = $"(�ڵ�)10�ʿ� {data.Increase * data.Level}�� ȭ���� ����";
        data.Increase = 0.2f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 3;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);

        data = new UpgradeData();
        data.CordName = "MergeAutoSpeedLevel";
        data.Name = "�ڵ� �ռ�";
        data.Level = 0;
        data.MaxLevel = 50;
        data.Explan = $"(�ڵ�)10�ʿ� {data.Increase * data.Level}�� ȭ���� ����";
        data.Increase = 0.2f;
        data.Price = 5 * (data.Level + 1);
        data.UpgradeType = UpgradeType.Making;
        data.ButtonIndex = 4;
        data.priceType = PriceType.Dia;
        UpgradeDatas.upgradeDataList.Add(data);
    }
    #endregion;


    #region �ε�


    IEnumerator SceneLoad()
    {
        Operation = SceneManager.LoadSceneAsync("GameScene");
        Operation.allowSceneActivation = false;

        while (true)
        {
            if (_isGo == false) // ���̼� ������ �ε��� ���������� ���
            {
                yield return null;
                continue;
            }
            if (Operation.progress >= 0.9f && IsLoadEnd == true) // ���ε� üũ ��, ���α׷����ٰ� 100%�� �Ǹ�
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
        //���̽����Ͽ��� ���׷��̵� ������ �о����
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
            else Debug.Log("������ �����ϴ�.");
        }
        else
        {
            Debug.Log("������ �����ϴ�.");
            SaveUpgradeData();
        }

        Debug.Log("�ε��Ϸ�");
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
#region ���̺굥���� Ŭ����

[Serializable]
public class UpgradeDataList
{
    //���׷��̵� ������
    public List<UpgradeData> upgradeDataList = new List<UpgradeData>();
    /*
    //��� ���׷��̵�
    public int GoldAttackDamageLevel = 0;
    public int GoldAttackSpeedLevel = 0;
    public int GoldCriticalLevel = 0;
    public int GoldCriticalDamageLevel = 0;
    public int GoldWallHpLevel = 0;

    //���� ���׷��̵�
    public int ManagementArcherLevel = 0;
    public int ManagementWallHpLevel = 0;
    //public int ManagementUpgrade3Level = 0;
    //public int ManagementUpgrade4Level = 0;
    //public int ManagementUpgrade5Level = 0;

    //���� ���׷��̵�
    public int DiaAttackDamegeLevel = 0;
    public int DiaAttackSpeedLevel = 0;
    public int DiaCriticalLevel = 0;
    public int DiaCriticalDamageLevel = 0;
    //public int AttackUpgrade5Level = 0;

    //���� ���׷��̵�
    public int MakingSpeedLevel = 0;
    public int MakingArrowLevelLevel = 0;
    public int DiaMakingArrowLevelLevel = 0;
    public int MakingAutoSpeedLevel = 0;
    public int MergeAutoSpeedLevel = 0;

    //Ư�� ���׷��̵� (ī�尳��? ������ ���� ����)
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
    public PriceType priceType; // 0 = ���, 1 = ���̾�
    public String Explan;
    public float Increase;
    public int ButtonIndex;
}


[Serializable]
public class ArrowLevelData
{
    public int[] EquipData = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
    public List<int> InventoryData = new List<int>();
    //ĳ���� ȭ�� ���� ������. ����ȭ�� �����Ͷ� �κ��丮�� ����ִ°� ������
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
