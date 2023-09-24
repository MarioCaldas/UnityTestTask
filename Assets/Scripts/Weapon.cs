using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public int ammoAmount;
    public int ammoCapacity;
    public PlayerController playerController;
    public int weaponId;
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
        else
        {
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<PlayerController>())
    //    {
    //        print("oii");
    //        other.GetComponent<PlayerController>().PickWeapon(this);
    //        Destroy(gameObject);
    //    }
    //}
}
