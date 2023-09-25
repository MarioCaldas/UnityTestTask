using Photon.Pun;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    PlayerController playerOwner;

    bool collided;
    PhotonView PV;

    [SerializeField] GameObject explosionVfx;

    ItemData itemData;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (collided)
            return;
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

            PV.RPC(nameof(RPC_RocketImpact), RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_RocketImpact()
    {
        Instantiate(explosionVfx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
