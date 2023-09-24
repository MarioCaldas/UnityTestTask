using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Item> itens;

    private Item currentItem;

    int currentItemIndex = -1;

    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        SpawnItem();
    }

    void SpawnItem()
    {
       
        if (itens.Count <= 1)
        {
            currentItemIndex = Random.Range(0, itens.Count);
        }
        else if(currentItemIndex >= -1)
        {
            if (currentItemIndex < itens.Count - 1)
                currentItemIndex++;
            else
                currentItemIndex = 0;

        }

        if (PV.IsMine)
            PV.RPC(nameof(RPC_SpawnItem), RpcTarget.All, currentItemIndex);
    }

    [PunRPC]
    void RPC_SpawnItem(int teste)
    {
        currentItem = Instantiate(itens[teste], transform.position, Quaternion.identity);
        currentItem.transform.parent = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() && currentItem)
        {
            other.GetComponent<PlayerController>().PickItem(currentItem);
    
            PV.RPC(nameof(DestroyItem), RpcTarget.All);
        }
    }
    [PunRPC]
    void DestroyItem()
    {
        Destroy(currentItem.gameObject);

        Invoke("SpawnItem", 4);
    }
}
