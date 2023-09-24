using UnityEngine;

public abstract class Weapon : Item
{
    private int ammoAmount;
    [SerializeField] private int ammoCapacity;
    private PlayerController playerController;
    [SerializeField] protected float projectileSpeed;

    public abstract override void Use();


    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();

        ammoAmount = ammoCapacity / 2;
    }

    public virtual void Reload(int _ammoAmount)
    {
        ammoAmount += _ammoAmount;
        if (ammoAmount >= ammoCapacity)
            ammoAmount = ammoCapacity;
    }

    protected bool CanUse()
    {
        return ammoAmount > 0;
    }

    public void TryUse()
    {
        if (CanUse())
        {
            Use();
            ammoAmount--; 
        }
    }
   
    public int GetAmmoAmount()
    {
        return ammoAmount;
    }
    protected PlayerController GetPlayerController()
    {
        return playerController;
    }
}
