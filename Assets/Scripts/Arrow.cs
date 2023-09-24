using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    PlayerController playerOwner;

    bool collided;
    PhotonView PV;

    ItemData itemData;

    void Awake()
    {
        PV = GetComponent<PhotonView>();

    }


    private void Update()
    {
        if (collided)
            return;

        RotateWithRBVelocity();
    }

    private void RotateWithRBVelocity()
    {
        if (GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            //rotate with velocity
            Quaternion rot = transform.rotation;
            rot.SetLookRotation(GetComponent<Rigidbody>().velocity);
            transform.rotation = rot;
        }
    }

    public void SetData(PlayerController _playerOwner, ItemData _itemData)
    {        
        //Receive info about the owner of the projectile and the item that shoot the projectile
        playerOwner = _playerOwner;
        itemData = _itemData;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PV.IsMine)
        {
            if (collision.gameObject == playerOwner.gameObject)
                return;

            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(((WeaponData)itemData).damage);

            PV.RPC(nameof(RPC_ArrowStick), RpcTarget.All);

            Invoke("DestroyArrow", 3);
        }

    }


    [PunRPC]
    void RPC_ArrowStick()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        GetComponent<Rigidbody>().velocity = Vector3.zero;

        collided = true;
    }


    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
