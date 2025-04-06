using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    private Animator _fxAnimator;
   
    private void Awake()
    {
        _fxAnimator = GetComponent<Animator>();
    }
    
    void Start()
    {
        StartCoroutine(nameof(RemoveFX));
    }
    
    IEnumerator RemoveFX()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
