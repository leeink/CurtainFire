using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int Health;
    public int Damage;
    public float Speed;
    public int Experience;
    public int Point;
}
