using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }

    void Start()
    {

    }


    void Update()
    {
        
    }

    public int GetMakingArrowLevel() //����ȭ�췹�� ���׷��̵� �߰��� ���� �����ؾ���
    {
        return 1 + Data.Instance.UpgradeData.MakingArrowLevelLevel + Data.Instance.UpgradeData.DiaMakingArrowLevelLevel;
    }

    public int GetMaxArrowLevel()
    {
        //�� ���� ���� ȭ�� ����?
        return 0;
    }
}
