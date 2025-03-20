using UnityEngine;
using UnityEngine.Pool;

public class FinalAttack: IAttackable
{
    public void Attack(IObjectPool<Bullet> pool, Transform playerTransform)
    {
        for (var i = -120; i <= 120; i += 30)
        {
            Bullet bullet = pool.Get();
            bullet.transform.position = playerTransform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, i);
            bullet.gameObject.SetActive(true);
        }
    }
}
