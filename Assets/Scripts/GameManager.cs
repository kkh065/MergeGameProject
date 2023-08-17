using UnityEditor;
using UnityEngine;

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

    public int GetMakingArrowLevel() //제작화살레벨 업그레이드 추가시 여기 수정해야함
    {
        return 1;
    }

    public int GetMaxArrowLevel()
    {
        //내 가장 높은 화살 레벨?
        return 0;
    }
}
