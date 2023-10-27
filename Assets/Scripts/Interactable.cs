using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public string InteractType;
	[SerializeField] private GameObject InteractNotice;
	private GameManager GM;

	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			InteractNotice.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			InteractNotice.gameObject.SetActive(false);
		}
	}

	public void Interact()
	{
		if (InteractType == "PickAxe")
		{
			GM.HavePickAxe = true;
			Debug.Log(name + "À»(¸¦) È¹µæÇß´Ù");
			Destroy(gameObject);
		}
		else if (InteractType == "Axe")
		{
			GM.HaveAxe = true;
			Debug.Log(name + "À»(¸¦) È¹µæÇß´Ù");
			Destroy(gameObject);
		}
		else if (InteractType == "Craft")
		{
			GameObject.Find("ScreenContainer").transform.Find("Craft").gameObject.SetActive(true);
		}
		else if (InteractType == "Stone")
		{
			if (Mine() == true)
			{
				Debug.Log("µ¹+1");
				Destroy(gameObject);
			}
		}
		else if (InteractType == "Iron")
		{
			if (Mine() == true)
			{
				Debug.Log("Ã¶+1");
				Destroy(gameObject);
			}
		}
		else if (InteractType == "Wood")
		{
			if (Chop() == true)
			{
				Debug.Log("³ª¹«+1");
				Destroy(gameObject);
			}
		}
		else if (InteractType == "Box")
		{

		}
		else
		{
			Debug.Log("»óÈ£ÀÛ¿ë Å¸ÀÔ ¿¡·¯");
		}
	}

	private bool Mine()
	{
		if (GM.HavePickAxe == true)
		{
			Debug.Log(name + "À»(¸¦) Ã¤±¤Çß´Ù");
			return true;
		}
		else
		{
			Debug.Log("°î±ªÀÌ°¡ ¾ø´Ù");
			return false;
		}
	}

	private bool Chop()
	{
		if (GM.HaveAxe == true)
		{
			Debug.Log(name + "À»(¸¦) ¹ú¸ñÇß´Ù");
			return true;
		}
		else
		{
			Debug.Log("µµ³¢°¡ ¾ø´Ù");
			return false;
		}
	}
}
