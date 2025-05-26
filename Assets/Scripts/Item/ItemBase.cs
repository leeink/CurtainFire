using System;
using System.Collections;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public AudioClip useClip;
    
    public void Start()
    {
        StartCoroutine(nameof(CorDestroy));
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        var playerStatManager = other.gameObject.GetComponent<PlayerStatManager>();
        
        if (playerStatManager is not null)
        {
            Use(playerStatManager);
            ItemDestroy();
        }
    }

    protected virtual void Use(PlayerStatManager playerStatManager)
    {
        GameManager.Instance.PlayOneShotClip(useClip);
        ItemDestroy();
    }

    public virtual void ItemDestroy()
    {
        Destroy(gameObject);
    }

    protected virtual void ItemMove()
    {
        gameObject.transform.Translate(Vector3.left * (Time.deltaTime * 5));
    }
    
    public IEnumerator CorDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
