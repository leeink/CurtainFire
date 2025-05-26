using System;
using UnityEngine;

public class GameClearTrigger : MonoBehaviour
{
    private void OnDestroy()
    {
        GameManager.Instance.GameClear();
    }
}
