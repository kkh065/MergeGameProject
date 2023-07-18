using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    #region 지역변수
    UpgradeData _upgradeData = new UpgradeData();
    public UpgradeData UpgradeData { get; set; }
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
        
    }


    void Update()
    {
        
    }

    void SaveUpgradeData()
    {
        //업그레이드 데이터를 제이슨파일로 저장
        //_upgradeData
    }

    void ReadUpgradeData()
    {
        //제이슨파일에서 업그레이드 데이터 읽어오기
        //_upgradeData
    }
}

#region 세이브데이터 클래스


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

public class ArrowLevelData
{
    //캐릭터 화살 레벨 데이터. 장착화살 데이터랑 인벤토리에 들고있는거 데이터
}

#endregion
