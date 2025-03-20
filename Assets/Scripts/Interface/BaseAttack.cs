using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Pool;

public class BaseAttack : IAttackable
{
    public void Attack(IObjectPool<Bullet> pool, Transform playerTransform)
    {
        int cnt = 4;
        float angle = 60;
        float gap = cnt > 1 ? angle / (float)(cnt - 1) : 0;
        float startAngle = -angle / 2f;

        for (int i = 0; i < cnt; ++i)
        {
            float theta = startAngle + gap * (float)i;
            theta *= Mathf.Deg2Rad;
            Bullet bullet = pool.Get();
            Vector3 dir = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            bullet.transform.rotation = Quaternion.Euler(dir * Mathf.Rad2Deg);
            bullet.Direction = dir;
        }
    }
}
