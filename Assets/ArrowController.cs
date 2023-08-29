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
        
        //화살의 레벨에 따른 스킨 변경
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetMonster == null || _targetMonster.gameObject.activeSelf == false)
        {
            OnReleaseArrow();
            return;
        }

        // 타겟을 향해 날아가게끔 적용
        // 맞으면 데미지 처리 어떻게 할 지 생각 거리가 0.1보다 가까워지면 맞았다고 생각하고 해당 몬스터가 데미지를 입게끔 만들자
        if(Vector3.Distance(_targetMonster.transform.position, transform.position) < 0.1)
        {
            _targetMonster.TakeDamage(_damage);
            OnReleaseArrow();
        }
        else
        {
            //타겟한테 포물선을 그리면서 이동
        }
    }

    void OnReleaseArrow()
    {
        _pool.Release(this);
    }
}
