using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Rpg : Weapon
{
    [SerializeField] GameObject rocket;
    [SerializeField] Transform spawnPos;
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        rocket = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Rocket"), spawnPos.position, spawnPos.rotation);
        rocket.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Force);
        rocket.GetComponent<Rocket>().SetData(playerController, itemData);
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            print("oii");
            other.GetComponent<PlayerController>().PickWeapon(this);
        }
    }*/
}
