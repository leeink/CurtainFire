using System;
using System.Collections;
using UnityEngine;

public class PowerUpItem : ItemBase
{
    public int powerUpValue;
    
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
        playerStatManager.Attack += powerUpValue;
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
