using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowController : MonoBehaviour
{
    Monster _targetMonster;
    int _damage;
    IObjectPool<ArrowController> _pool;

    public void InitArrow(Monster monster, int Dmg, int ArrowLevel, IObjectPool<ArrowController> Pool)
    {
        _targetMonster = monster;
        _damage = Dmg;
        _pool = Pool;
        
        //ȭ���� ������ ���� ��Ų ����
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetMonster == null || _targetMonster.gameObject.activeSelf == false)
        {
            OnReleaseArrow();
            return;
        }

        // Ÿ���� ���� ���ư��Բ� ����
        // ������ ������ ó�� ��� �� �� ���� �Ÿ��� 0.1���� ��������� �¾Ҵٰ� �����ϰ� �ش� ���Ͱ� �������� �԰Բ� ������
        if(Vector3.Distance(_targetMonster.transform.position, transform.position) < 0.1)
        {
            _targetMonster.TakeDamage(_damage);
            OnReleaseArrow();
        }
        else
        {
            //Ÿ������ �������� �׸��鼭 �̵�
        }
    }

    void OnReleaseArrow()
    {
        _pool.Release(this);
    }
}
