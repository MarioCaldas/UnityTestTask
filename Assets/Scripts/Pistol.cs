using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] Camera cam;

    [SerializeField] GameObject muzzle;

    [SerializeField] Animation shootAnim;

    public override void Use()
    {
        Shoot();
    }


    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,  1 << LayerMask.NameToLayer("Player")))
        {
            print(hit.transform.name);

            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((WeaponData)itemData).damage);
        }

        shootAnim.Play();  
    }

}
