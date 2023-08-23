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
    List<GameObject> _liveMonsterList = new List<GameObject>();
    // ���Ͱ� �ڽ��� ������ ���� ���ӸŴ��� �Լ� ȣ�� - 
    public void MonsterDie(GameObject monster)
    {
        //���� ����� 5��� ȹ��
        //�������� ����� 5���̾� ȹ��
        //���� ���͸���Ʈ���� �ڱ��ڽ� ����.
        _liveMonsterList.Remove(monster);
        if (_liveMonsterList.Count <= 0)
        {
            //�������� Ŭ����!
            //������������ �ε�            
        }
    }

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
    #endregion

    #region ĳ���� �� ai ����
    //�����Ҷ� ĳ���� �����Ϳ��� ���� �޾ƿͼ� �׸�ŭ ����
    //�� ĳ���͵��� ���� ��ȯ�� ���� ����Ʈ�� ���Ƽ� ���Ͱ� �ִٸ� �ڵ����� ����
    //���ݼӵ� ���� �� �ִϸ��̼� ���缭 ����
    #endregion
}
