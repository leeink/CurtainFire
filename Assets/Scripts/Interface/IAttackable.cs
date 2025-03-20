using UnityEngine;
using UnityEngine.Pool;

public interface IAttackable
{
    public void Attack(IObjectPool<Bullet> pool, Transform playerTransform);
}
