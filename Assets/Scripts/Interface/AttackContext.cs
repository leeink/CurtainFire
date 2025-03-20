using UnityEngine;
using UnityEngine.Pool;

public class AttackContext : MonoBehaviour
{
    private IAttackable attackStrategy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setAttackStrategy(new BaseAttack());
    }
    
    public void setAttackStrategy(IAttackable attackStrategy)
    {
        this.attackStrategy = attackStrategy;
    }
    
    public void AttackStrategy(IObjectPool<Bullet> pool, Transform playerTransform)
    {
        attackStrategy.Attack(pool, playerTransform);
    }
}
