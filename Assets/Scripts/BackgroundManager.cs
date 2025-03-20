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
        Invoke(nameof(DeActiveBackground), 8f);
        _currentBackgroundIndex++;
        if (_currentBackgroundIndex >= backgrounds.Length)
        {
            _currentBackgroundIndex = 0;
        }
        backgrounds[_currentBackgroundIndex].SetActive(true);
    }

    void DeActiveBackground()
    {
        backgrounds[_currentBackgroundIndex - 1].SetActive(false);
    }
}
