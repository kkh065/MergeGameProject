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
    //땅에 닫기까지 걸리는 시간
    protected float timer;
    protected float timeToFloor;

    public void InitArrow(Monster monster, int Dmg, int ArrowLevel, IObjectPool<ArrowController> Pool)
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        _targetMonster = monster;
        _damage = Dmg;
        _pool = Pool;

        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arrow/Arrow" + ArrowLevel / 10);

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
        while (transform.position.y >= startPos.y - 5)
        {
            timer += Time.deltaTime;
            Vector3 NextPos = Parabola(startPos, _targetMonster.transform.position, 5, timer);

            float angle = Mathf.Atan2((NextPos - transform.position).y, (NextPos - transform.position).x) * Mathf.Rad2Deg - 45f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            transform.position = NextPos;

            yield return new WaitForEndOfFrame();
        }
    }
}
