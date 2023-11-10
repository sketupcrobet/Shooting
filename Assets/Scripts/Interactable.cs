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
		//interactNotice = transform.GetChild(0).gameObject;
	}

	void Update()
	{

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
			Transform Inventory = transform.Find("Canvas").Find("Inventory").Find("Inventory");
			if (Inventory.childCount != maxCount)
			{
				for (int i = 0; i < maxCount; i++)
				{
					GameObject itemSlot = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"));
					itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/" + Random.Range(0, 25));
					itemSlot.transform.SetParent(Inventory);
					itemSlot.transform.localScale = Vector3.one;
				}
			}
			transform.Find("Canvas").Find("Inventory").gameObject.SetActive(true);
			PL.transform.Find("Canvas").Find("Inventory").gameObject.SetActive(true);
			GM.closeObj = transform.Find("Canvas").Find("Inventory").Find("Name").Find("Close").gameObject;
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
		//EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
		GM.closeObj.transform.parent.parent.gameObject.SetActive(false);
		PL.transform.Find("Canvas").Find("Inventory").gameObject.SetActive(false);
		GM.UIOpen = false;
	}
}
