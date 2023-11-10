using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public string interactType;
	private GameObject interactNotice;
	private GameManager GM;
	private GameObject PL;

	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		PL = GameObject.Find("Player");
		interactNotice = transform.GetChild(0).gameObject;
	}

	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			interactNotice.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			interactNotice.gameObject.SetActive(false);
		}
	}

	public void Interact()
	{
		if (interactType == "PickAxe")
		{
			GM.havePickAxe = true;
			Debug.Log(name + "À»(¸¦) È¹µæÇß´Ù");
			Destroy(gameObject);
		}
		else if (interactType == "Axe")
		{
			GM.haveAxe = true;
			Debug.Log(name + "À»(¸¦) È¹µæÇß´Ù");
			Destroy(gameObject);
		}
		else if (interactType == "Craft")
		{
			GameObject.Find("ScreenContainer").transform.Find("Craft").gameObject.SetActive(true);
		}
		else if (interactType == "Stone")
		{
			if (Mine() == true)
			{
				Debug.Log("µ¹+1");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Iron")
		{
			if (Mine() == true)
			{
				Debug.Log("Ã¶+1");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Wood")
		{
			if (Chop() == true)
			{
				Debug.Log("³ª¹«+1");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Box")
		{
			int maxCount = 24;
			if (transform.GetChild(1).GetChild(1).childCount != maxCount)
			{
				for (int i = 0; i < maxCount; i++)
				{
					GameObject itemSlot = Instantiate(Resources.Load("Prefabs/ItemSlot") as GameObject);
					itemSlot.GetComponent<Image>().color = new Color(0, 0, Random.Range(0, 256) / 255f);
					itemSlot.transform.SetParent(transform.GetChild(1).GetChild(1));
					itemSlot.transform.localScale = Vector3.one;
				}
			}
			transform.GetChild(1).gameObject.SetActive(true);
			PL.transform.GetChild(2).gameObject.SetActive(true);
			GM.UIOpen = true;
		}
		else
		{
			Debug.Log("»óÈ£ÀÛ¿ë Å¸ÀÔ ¿¡·¯");
		}
	}

	private bool Mine()
	{
		if (GM.havePickAxe == true)
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
		if (GM.haveAxe == true)
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

	public void UIClose()
	{
		EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
		PL.transform.GetChild(2).gameObject.SetActive(false);
		GM.UIOpen = false;
	}
}
