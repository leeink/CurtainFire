using UnityEngine;
using UnityEngine.Pool;

public class ThreeWayAttack : IAttackable
{
    public void Attack(IObjectPool<Bullet> pool, Transform playerTransform)
    {
        for (int i = -15; i <= 15; i += 15)
        {
            Bullet bullet = pool.Get();
            bullet.transform.position = playerTransform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, i);
            bullet.gameObject.SetActive(true);
        }
    }
}
