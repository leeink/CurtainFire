using System;
using System.Collections;
using UnityEngine;

public class HealItem : ItemBase
{
    public int HealValue;
    
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
        playerStatManager.Health += HealValue;
        playerStatManager.UpdateHealthUI();
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
