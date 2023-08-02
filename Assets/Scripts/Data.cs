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

                StartCoroutine(SceneLoad()); // ���ε� ����
            });
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
    public int[] EquipData;
    public List<int> InventoryData;
    //ĳ���� ȭ�� ���� ������. ����ȭ�� �����Ͷ� �κ��丮�� ����ִ°� ������
}

#endregion
