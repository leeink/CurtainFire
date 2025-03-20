using System.Collections;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        StartCoroutine(nameof(CorDestroy));
    }

    // Update is called once per frame
    void Update()
    {
        
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
