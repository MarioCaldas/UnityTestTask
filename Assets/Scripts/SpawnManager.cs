using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	public float sphereRadius = 35f;

	private List<Spawner> spawners = new List<Spawner>();

	[SerializeField] private List<GameObject> itemsToSpawn = new List<GameObject>();

	private int totalObjectsToSpawn = 10;

	void Awake()
	{
		Instance = this;
	}

    private void Start()
    {
        foreach (Transform item in transform)
        {
			spawners.Add(item.GetComponent<Spawner>());
        }

        //SpawnObjectsWithProportion();
    }


    public Vector3 GetRandomPointInSphere()
	{
		Vector3 randomPoint = Random.insideUnitSphere * sphereRadius;

		randomPoint += transform.position;

		return new Vector3(randomPoint.x, 10, randomPoint.z);
    }
    /*
    private void SpawnObjectsWithProportion()
    {
        // Calculate how many objects of each type to spawn
        int objectsPerType = totalObjectsToSpawn / itemsToSpawn.Count;

        // Loop to spawn objects
        for (int i = 0; i < totalObjectsToSpawn; i++)
        {
            // Randomly select an object type based on proportions
            int randomIndex = Random.Range(0, itemsToSpawn.Count);
            GameObject objectToSpawn = itemsToSpawn[randomIndex];

            // Instantiate the selected object at the spawn point
            PhotonNetwork.Instantiate(objectToSpawn, GetRandomPointInSphere(), Quaternion.identity);
        }
    }*/
    /*void SpawnItem()
    {
        PV.RPC(nameof(RPC_SpawnItem()), RpcTarget.All);
    }

    [PunRPC]
    void RPC_SpawnItem(GameObject item)
    {
        Instantiate(objectToSpawn, GetRandomPointInSphere(), Quaternion.identity);
    }*/
}
