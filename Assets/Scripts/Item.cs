using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public ItemData itemData;
	public GameObject itemGameObject;

	public abstract void Use();

}