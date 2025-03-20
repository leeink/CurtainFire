using UnityEngine;

public class FeverItem : ItemBase
{
    [SerializeField] private float speedUpValue;
    [SerializeField] private float attackRateUpValue;
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
        GameManager.Instance.GetComponent<FeverBuff>().enabled = true;
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
