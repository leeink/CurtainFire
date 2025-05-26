using System;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    [SerializeField] private int beamTickDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Boss") || other.CompareTag("Enemy")) return;
        if (other.TryGetComponent<ILivingEntity>(out var entity))
        {
            entity.OnDamage(beamTickDamage);
        }
    }
}
