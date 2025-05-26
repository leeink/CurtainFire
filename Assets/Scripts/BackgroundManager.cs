using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;
    private int _currentBackgroundIndex = 0;
    void Start()
    {
        GameManager.Instance.OnStageChange += ChangeBackground;
    }

    void ChangeBackground()
    {
        _currentBackgroundIndex++;
        
        if (_currentBackgroundIndex >= backgrounds.Length)
        {
            _currentBackgroundIndex = 0;
        }
        backgrounds[_currentBackgroundIndex].SetActive(true);
        
        Invoke(nameof(DeActiveBackground), 8f);
    }

    void DeActiveBackground()
    {
        if (_currentBackgroundIndex == 0)
        {
            backgrounds[backgrounds.Length - 1].SetActive(false);   
        }
        else
        {
            backgrounds[_currentBackgroundIndex - 1].SetActive(false); 
        }
        
    }
}
