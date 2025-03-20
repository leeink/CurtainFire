using UnityEngine;

public interface ILevel
{
    public void GainExperience(int amount)
    {
        Debug.Log($"Gain Experience: {amount}");
    }
    
    public void LevelUp()
    {
        Debug.Log("Level Up");
    }
}
