using UnityEngine;

public interface ILivingEntity
{
    public void Heal(int amount);
    public void OnDamage(int amount);
    public void Die();
}
