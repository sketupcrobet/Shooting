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
			Debug.Log(name + "��(��) ȹ���ߴ�");
			Destroy(gameObject);
		}
		else if (interactType == "Axe")
		{
			GM.haveAxe = true;
			Debug.Log(name + "��(��) ȹ���ߴ�");
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
				Debug.Log("��+1");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Iron")
		{
			if (Mine() == true)
			{
				Debug.Log("ö+1");
				Destroy(gameObject);
			}
		}
		else if (interactType == "Wood")
		{
			if (Chop() == true)
			{
				Debug.Log("����+1");
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
			Debug.Log("��ȣ�ۿ� Ÿ�� ����");
		}
	}

	private bool Mine()
	{
		if (GM.havePickAxe == true)
		{
			Debug.Log(name + "��(��) ä���ߴ�");
			return true;
		}
		else
		{
			Debug.Log("��̰� ����");
			return false;
		}
	}

	private bool Chop()
	{
		if (GM.haveAxe == true)
		{
			Debug.Log(name + "��(��) �����ߴ�");
			return true;
		}
		else
		{
			Debug.Log("������ ����");
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
