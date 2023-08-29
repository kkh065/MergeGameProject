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
    }

    public void UpdateCharacterState()
    {
        //�ε����� �´� ȭ�찡���ͼ� ������ ����.
        //���� ���׷��̵�� (���ݷ�,���ݼӵ�,ġ��ŸȮ��,ġ��Ÿ����) ����
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
