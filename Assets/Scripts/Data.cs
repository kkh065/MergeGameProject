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
        _upgradeData = new UpgradeData();
        _arrowLevelData = new ArrowLevelData();
        datas = new UpgradeTabList();
        datas.UpgradeList = new List<UpgradeTabData>();
        ReadUpgradeData();        
    }


    void Update()
    {

    }

    #region ���̺�
    void SaveUpgradeData()
    {
        //���׷��̵� �����͸� ���̽����Ϸ� ����
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
            tab.Name = "�⺻���ݷ�";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}�� {tab.Increase * tab.NowLevel}��ŭ ����";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldAttackSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "���ݼӵ�";
            tab.Increase = 0.01f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}�� {tab.Increase * tab.NowLevel}��ŭ ����";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldCriticalLevel;
            tab.MaxLevel = 20;
            tab.Name = "ġ��ŸȮ��";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}�� {tab.Increase * tab.NowLevel}%��ŭ ����";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldCriticalDamageLevel;
            tab.MaxLevel = 500;
            tab.Name = "ġ��Ÿ ������";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}�� {tab.Increase * tab.NowLevel}%��ŭ ����";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldWallHpLevel;
            tab.MaxLevel = 30;
            tab.Name = "���� ��ȭ";
            tab.Increase = 10;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"���� ü���� {tab.Increase * tab.NowLevel}��ŭ ������Ų��";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 4;
            datas.UpgradeList.Add(tab);


            //�������׷��̵�
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.ManagementArcherLevel;
            tab.MaxLevel = 7;
            tab.Name = "��ó ����";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"��ġ ������ ��ó�� �� {tab.Increase * tab.NowLevel}ĭ ����";
            tab.Type = UpgradeType.Management;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.ManagementWallHpLevel;
            tab.MaxLevel = 30;
            tab.Name = "���� ��ȭ";
            tab.Increase = 10;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"���� ü�� {tab.Increase * tab.NowLevel} ����";
            tab.Type = UpgradeType.Management;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            //���� ���׷��̵�
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaAttackDamegeLevel;
            tab.MaxLevel = 500;
            tab.Name = "�⺻ ���ݷ�";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel} ����";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaAttackSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "���ݼӵ�";
            tab.Increase = 0.05f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel} ����";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaCriticalLevel;
            tab.MaxLevel = 20;
            tab.Name = "ġ��ŸȮ��";
            tab.Increase = 0.5f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel}% ����";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaCriticalDamageLevel;
            tab.MaxLevel = 500;
            tab.Name = "ġ��Ÿ ������";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel}% ����";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            //���۾��׷��̵�
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "���� ����";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"ȭ�� ���� �ð� {tab.Increase * tab.NowLevel}�� ����";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingArrowLevelLevel;
            tab.MaxLevel = 150;
            tab.Name = "���� ȭ�� ���� ����";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"���۵� ȭ���� ���� {tab.Increase * tab.NowLevel} ����";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaMakingArrowLevelLevel;
            tab.MaxLevel = 80;
            tab.Name = "���� ȭ�� ���� ����";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"���۵� ȭ���� ���� {tab.Increase * tab.NowLevel} ����";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingAutoSpeedLevel;
            tab.MaxLevel = 50;
            tab.Name = "�ڵ� ����";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"(�ڵ�)10�ʿ� {tab.Increase * tab.NowLevel}�� ȭ���� ����";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MergeAutoSpeedLevel;
            tab.MaxLevel = 50;
            tab.Name = "�ڵ� �ռ�";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"(�ڵ�)10�ʿ� {tab.Increase * tab.NowLevel}�� ȭ���� ����";
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


    #region �ε�

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

    async void UpgradeTabData()
    {
        //���׷��̵� �� ����

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
            else Debug.Log("������ �����ϴ�.");
        }
        else
        {
            Debug.Log("������ �����ϴ�.");
            UpgradeTabDataCreate();
        }
        //�� â���ٰ� ���׷��̵� �� ���� -> ����Ʈ�� ���� ���̽����� ������ �ҷ��ͼ� ��������
        //�̳����� ���׷��̵� Ÿ�� ����� ���� �����ߴٰ�, Ÿ�Ժ��� �θ� ����ġ���̽� ����
        //�ε����� ���ڷ� �޾ƿͼ� ����ġ���̽��� ������. �Լ��� ��ư�� ��������Ʈ�� ���̸� ���ڰ��� �ε����� ���� �� �����ٵ�
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

                    StartCoroutine(SceneLoad()); // ���ε� ����
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


#region ���̺굥���� Ŭ����

[Serializable]
public class UpgradeData
{
    //���׷��̵� ������

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
}


[Serializable]
public class ArrowLevelData
{
    public int[] EquipData = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
    public List<int> InventoryData = new List<int>();
    //ĳ���� ȭ�� ���� ������. ����ȭ�� �����Ͷ� �κ��丮�� ����ִ°� ������
}

#endregion
