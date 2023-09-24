using UnityEngine;

public class UIMenu : MonoBehaviour
{
	public string menuName;
	public bool open;

	public void Open()
	{
		open = true;
		gameObject.SetActive(true);
	}

	public void Close()
	{
		open = false;
		gameObject.SetActive(false);
	}
}
