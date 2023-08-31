using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowController : MonoBehaviour
{
    Monster _targetMonster;
    int _damage;
    IObjectPool<ArrowController> _pool;

    private Vector3 startPos, endPos;
    //���� �ݱ���� �ɸ��� �ð�
    protected float timer;
    protected float timeToFloor;

    public void InitArrow(Monster monster, int Dmg, int ArrowLevel, IObjectPool<ArrowController> Pool)
    {
        _targetMonster = monster;
        _damage = Dmg;
        _pool = Pool;

        startPos = transform.position;
        endPos = _targetMonster.transform.position;
        StartCoroutine("BulletMove");
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
    }

    void OnReleaseArrow()
    {
        _pool.Release(this);
    }

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    protected IEnumerator BulletMove()
    {
        timer = 0;
        while (transform.position.y >= startPos.y)
        {
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, _targetMonster.transform.position, 5, timer);
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
    }
}
