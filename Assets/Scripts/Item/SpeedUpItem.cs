using System;
using System.Collections;
using UnityEngine;

public class SpeedUpItem : ItemBase
{
    public float speedUpValue;
    
    public new void Start()
    {
        base.Start();
    }

    public void Update()
    {
        ItemMove();
    }

    protected override void Use(PlayerStatManager playerStatManager)
    {
        base.Use(playerStatManager);
        playerStatManager.AttackRate -= 0.001f;
        playerStatManager.Speed += speedUpValue;
    }

    public override void ItemDestroy()
    {
        base.ItemDestroy();
    }
    
    protected override void ItemMove()
    {
        base.ItemMove();
    }
}
