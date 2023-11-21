using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public string interactType;
	private GameManager GM;
	private Player PL;

	public int[] InventoryCode;
	public int[] InventoryCount;

	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		PL = GameObject.Find("Player").GetComponent<Player>();
	}

	void Update()
	{

	}

	public void Interact()
	{
		if (interactType == "PickAxe")
		{
			GM.havePickAxe = true;
			if (PL.AddItem(1, 1))
			{
				Debug.Log(name + "À»(¸¦) È¹µæÇß´Ù");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Axe")
		{
			GM.haveAxe = true;
			if (PL.AddItem(2, 1))
			{
				Debug.Log(name + "À»(¸¦) È¹µæÇß´Ù");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Craft")
		{
			GameObject.Find("ScreenContainer").transform.Find("Craft").gameObject.SetActive(true);
		}
		else if (interactType == "Stone")
		{
			if (Mine() == true)
			{
				if (PL.AddItem(3, 1) == true)
				{
					Debug.Log(name + "À»(¸¦) Ã¤±¤Çß´Ù");
					Debug.Log("µ¹+1");
					Destroy(gameObject);
				}
			}
		}
		else if (interactType == "Iron")
		{
			if (Mine() == true)
			{
				if (PL.AddItem(4, 1) == true)
				{
					Debug.Log(name + "À»(¸¦) Ã¤±¤Çß´Ù");
					Debug.Log("Ã¶+1");
					Destroy(gameObject);
				}
			}
		}
		else if (interactType == "Wood")
		{
			if (Chop() == true)
			{
				if (PL.AddItem(5, 1) == true)
				{
					Debug.Log(name + "À»(¸¦) ¹ú¸ñÇß´Ù");
					Debug.Log("³ª¹«+1");
					Destroy(gameObject);
				}
			}
		}
		else if (interactType == "Box")
		{
			if (InventoryCode.Length == 0)
			{
				InventoryCode = new int[24];
				InventoryCount = new int[24];
				for (int i = 0; InventoryCode.Length > i; i++)
				{
					InventoryCode[i] = Random.Range(0, 7);
					InventoryCount[i] = Random.Range(1, 2);
				}
			}
			PL.OtherObj = GetComponent<Interactable>();
			PL.InventoryRefresh();
			GM.UIOpen(GM.UIObj.transform.Find("Inventory").gameObject);
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
		GM.UIClose();
	}
}
