using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	public float sphereRadius = 35f;

	private List<Spawner> spawners = new List<Spawner>();


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
    }

    public Vector3 GetRandomPointInSphere()
	{
		Vector3 randomPoint = Random.insideUnitSphere * sphereRadius;

		randomPoint += transform.position;

		return new Vector3(randomPoint.x, 10, randomPoint.z);
    }
  
}
