using UnityEngine;

public interface IStatable
{
    public void Damage(int damage)
    {
        Debug.Log($"Damage: {damage}");
    }

    public void Die()
    {
        Debug.Log("Die");
    }
    
    public void Heal(int amount)
    {
        Debug.Log($"Heal");
    }
    
    public void SpeedUp(int amount)
    {
        Debug.Log($"Speed Up: {amount}");
    }
    
    public void SpeedDown(int amount)
    {
        Debug.Log($"Speed Down: {amount}");
    }
}
