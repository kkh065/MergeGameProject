using UnityEngine;
using UnityEngine.Pool;

public class CharacterAI : MonoBehaviour
{
    float _attackSpeed;
    float _attackCool;
    int _criticalPercent = 0;
    int _damage = 0;
    int _criticalDamage = 0;
    int _index = 0;

    IObjectPool<ArrowController> _arrowPool;
    Animator _ani;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
    }


    public void InitCharacter(int idx, IObjectPool<ArrowController> ArrowPool)
    {
        //�ش� ĳ������ �ε��� ����
        _index = idx;
        _arrowPool = ArrowPool;
        UpdateCharacterState();
    }

    public void UpdateCharacterState()
    {
        //���� ���׷��̵�� (���ݷ�,���ݼӵ�,ġ��ŸȮ��,ġ��Ÿ����) ����
        //�⺻������
        _damage = GameManager.Instance.EquipArrowData[_index] +
            (Data.Instance.GetUpgradeData(UpgradeType.Gold, 0).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Gold, 0).Increase) +
            (Data.Instance.GetUpgradeData(UpgradeType.Attack, 0).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Attack, 0).Increase);

        //ġ��Ÿ ����
        int a = Data.Instance.GetUpgradeData(UpgradeType.Gold, 3).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Gold, 3).Increase;
        int b = Data.Instance.GetUpgradeData(UpgradeType.Attack, 3).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Attack, 3).Increase;
        float c = (a + b + 200) / 100f;
        _criticalDamage = (int)(_damage * c);

        //���ݼӵ�
        float speed1 = Data.Instance.GetUpgradeData(UpgradeType.Gold, 1).Level * Data.Instance.GetUpgradeData(UpgradeType.Gold, 1).Increase;
        float speed2 = Data.Instance.GetUpgradeData(UpgradeType.Attack, 1).Level * Data.Instance.GetUpgradeData(UpgradeType.Attack, 1).Increase;
        _attackSpeed = 2f - speed1 - speed2;

        //ġ��ŸȮ��
        int Critical1 = Data.Instance.GetUpgradeData(UpgradeType.Gold, 2).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Gold, 2).Increase;
        int Critical2 = Data.Instance.GetUpgradeData(UpgradeType.Attack, 2).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Attack, 2).Increase;
        _criticalPercent = Critical1 + Critical2;
    }

    // Update is called once per frame
    void Update()
    {
        _attackCool += Time.deltaTime;
        if (_attackCool > _attackSpeed && GameManager.Instance.LiveMonsterList.Count > 0)
        {
            _ani.SetTrigger("Attack");
            _attackCool = 0;
        }
    }

    public void Attack()
    {
        //ũ��Ƽ�� Ȯ�� ��� �� ũ��Ƽ���̸� ũ����, �ƴϸ� �׳ɵ� �־ ����
        int RandomPercent = Random.Range(0, 99);
        int LastDamage = 0;

        if(RandomPercent < _criticalPercent)//ũ��Ƽ�� ���
        {
            LastDamage = _criticalDamage;
        }
        else
        {
            LastDamage = _damage;
        }

        //ȭ�����, ��ġ�ʱ�ȭ, ȭ�� �̴� ����
        ArrowController Arrow = _arrowPool.Get();
        Arrow.transform.position = transform.position + new Vector3(0, 1, 0);
        if(GameManager.Instance.LiveMonsterList.Count > 0)
        Arrow.InitArrow(GameManager.Instance.LiveMonsterList[0], LastDamage, GameManager.Instance.EquipArrowData[_index], _arrowPool);
    }
}
