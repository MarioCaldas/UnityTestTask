using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
	[SerializeField] Image healthbarImage;
	[SerializeField] GameObject ui;

	[SerializeField] private TextMeshProUGUI ammoAmountText;

	[SerializeField] GameObject cameraHolder;

	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	[SerializeField] List<Weapon> weapons;
	[SerializeField] List<Weapon> availableWeapons;

	int itemIndex;
	int previousItemIndex = -1;

	float verticalLookRotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	Rigidbody rb;

	PhotonView PV;

	const float maxHealth = 100f;
	float currentHealth = maxHealth;

	PlayerManager playerManager;

	bool cursorLocked;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();

		playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
	}

	void Start()
	{

		if (PV.IsMine)
		{
			//Equip random weapon
			int randomWeapon = Random.Range(0, weapons.Count);
			availableWeapons.Add(weapons[randomWeapon]);
			EquipItem(0);

		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
			Destroy(ui);
		}



	}

	void Update()
	{
		if (!PV.IsMine)
			return;

		Look();
		Move();
		Jump();

		//Switch weapons
		for (int i = 0; i < availableWeapons.Count; i++)
		{
			if (Input.GetKeyDown((i + 1).ToString()))
			{
				EquipItem(i);
				break;
			}
		}

		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
		{
			if (itemIndex >= availableWeapons.Count - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		{
			if (itemIndex <= 0)
			{
				EquipItem(availableWeapons.Count - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			availableWeapons[itemIndex].TryUse();
			UpdateAmmoAmountHUD();
		}

		if (Input.GetKeyDown(KeyCode.C))
        {
			if(!cursorLocked)
            {
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				cursorLocked = true;
			}
			else
            {
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				cursorLocked = false;
            }
        }

		if (transform.position.y < -10f) // Die if you fall out of the world
		{
			Die();
		}

	}

	void Look()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}

	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rb.AddForce(transform.up * jumpForce);
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        availableWeapons[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            availableWeapons[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        UpdateAmmoAmountHUD();


		//Send equiped item for the others players
		if(PV.IsMine)
			PV.RPC(nameof(RpcEquipItem), RpcTarget.All, availableWeapons[itemIndex].GetComponent<PhotonView>().ViewID);


    }

    [PunRPC]
	void RpcEquipItem(int _index)
	{

		foreach (Weapon item in weapons)
		{
			if (item.GetComponent<PhotonView>().ViewID == _index)
			{
				if (!availableWeapons.Contains(item))
				{
					availableWeapons.Add(item);
				}
				int weaponIndex = availableWeapons.FindIndex(w => w.GetComponent<PhotonView>().ViewID == _index);

				EquipItem(weaponIndex);
			}
		}
	}


	public void PickItem(Item _item)
    {
		if(_item is Weapon)
        {
			foreach (Weapon item in weapons)
			{
				if (item.itemData.itemName == _item.itemData.itemName)
				{
					if (!availableWeapons.Contains(item))
						availableWeapons.Add(item);
				}
			}
		}
		else if(_item is AmmoPack)
        {
			ReloadWeapon((AmmoPackData)_item.itemData);
		}
		else if(_item is HealthPotion)
        {
			ResetHealth(((HealthItemData)_item.itemData).healthAmount);
        }

	}

	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}

	void FixedUpdate()
	{
		if (!PV.IsMine)
			return;

		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

	public void ResetHealth(int healthAmount)
    {
		currentHealth += healthAmount;
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;

		UpdateHealthBarHUD();
    }

	public void ReloadWeapon(AmmoPackData ammoData)
    {
        foreach (Weapon weapon in availableWeapons)
        {
			print(weapon.itemData.itemName);
			if (weapon.itemData.itemName == ammoData.weaponId)
            {
				weapon.Reload(ammoData.ammoAmount);
            }
        }
		UpdateAmmoAmountHUD();
	}

	public void TakeDamage(float damage)
	{
		PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
	}

	[PunRPC]
	void RPC_TakeDamage(float damage, PhotonMessageInfo info)
	{
		currentHealth -= damage;

		UpdateHealthBarHUD();

		if (currentHealth <= 0)
		{
			Die();
			//Find player manager associated with player that send this RPC
			PlayerManager.Find(info.Sender).GetKill();
		}
	}

	void UpdateAmmoAmountHUD()
    {
		ammoAmountText.text = availableWeapons[itemIndex].GetAmmoAmount().ToString();
	}

	void UpdateHealthBarHUD()
    {
		healthbarImage.fillAmount = currentHealth / maxHealth;
	}

	void Die()
	{
		playerManager.Die();
	}


}