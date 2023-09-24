using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform spawnPos;


    public override void Use()
    {
        Shoot();
    }
  
    void Shoot()
    {
        arrow = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Arrow"), spawnPos.position, spawnPos.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        arrow.GetComponent<Arrow>().SetData(playerController, itemData);
    }

 
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            print("oii");
            other.GetComponent<PlayerController>().PickWeapon(this);
        }
    }
    */
}
