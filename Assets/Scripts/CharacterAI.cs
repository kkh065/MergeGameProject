using UnityEngine;
using UnityEngine.Pool;

public class CharacterAI : MonoBehaviour
{
    float _attackSpeed;
    float _attackCool;
    int _damage = 0;
    int _criticalDamage = 0;
    int _index = 0;

    IObjectPool<ArrowController> _arrowPool;


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
        _damage = GameManager.Instance.EquipArrowData[_index] +
            (Data.Instance.GetUpgradeData(UpgradeType.Gold, 0).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Gold, 0).Increase) +
            (Data.Instance.GetUpgradeData(UpgradeType.Attack, 0).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Attack, 0).Increase);

        int a = Data.Instance.GetUpgradeData(UpgradeType.Gold, 3).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Gold, 3).Increase;
        int b = Data.Instance.GetUpgradeData(UpgradeType.Attack, 3).Level * (int)Data.Instance.GetUpgradeData(UpgradeType.Attack, 3).Increase;
        float c = (a + b + 200) / 100;
        _criticalDamage = (int)((float)_damage * c);
        
        //���ݼӵ�

        //ġ��ŸȮ��        
    }

    // Update is called once per frame
    void Update()
    {
        _attackCool += Time.deltaTime;
        if (_attackCool > _attackSpeed && GameManager.Instance.LiveMonsterList.Count > 0)
        {
            Attack();
            _attackCool = 0;
        }
    }

    void Attack()
    {
        //ũ��Ƽ�� Ȯ�� ��� �� ũ��Ƽ���̸� ũ����, �ƴϸ� �׳ɵ� �־ ����
        int LastDamage = 0;
        if(true)//ũ��Ƽ�� ���
        {
            //ũ��Ƽ���̸�
            LastDamage = _criticalDamage;
        }
        else
        {
            LastDamage = _damage;
        }

        ArrowController Arrow = _arrowPool.Get();
        Arrow.transform.position = transform.position;
        Arrow.InitArrow(GameManager.Instance.LiveMonsterList[0], LastDamage, GameManager.Instance.EquipArrowData[_index], _arrowPool);
        //ȭ�����, ��ġ�ʱ�ȭ, ȭ�� �̴� ����
    }
}
