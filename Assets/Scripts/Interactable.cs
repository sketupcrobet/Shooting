using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
			Debug.Log(name + "��(��) ȹ���ߴ�");
			Destroy(gameObject);
		}
		else if (InteractType == "Axe")
		{
			GM.HaveAxe = true;
			Debug.Log(name + "��(��) ȹ���ߴ�");
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
				Debug.Log("��+1");
				Destroy(gameObject);
			}
		}
		else if (InteractType == "Iron")
		{
			if (Mine() == true)
			{
				Debug.Log("ö+1");
				Destroy(gameObject);
			}
		}
		else if (InteractType == "Wood")
		{
			if (Chop() == true)
			{
				Debug.Log("����+1");
				Destroy(gameObject);
			}
		}
		else if (InteractType == "Box")
		{
			if (transform.GetChild(1).GetChild(0).GetChild(0).childCount == 0)
			{
				for (int i = 0; i < 50; i++)
				{
					GameObject temp = Instantiate(Resources.Load("Prefabs/ItemSlot") as GameObject);
					temp.GetComponent<Image>().color = new Color(0, 0, Random.Range(0, 256) / 255f);
					temp.transform.SetParent(transform.GetChild(1).GetChild(0).GetChild(0));
					temp.transform.localScale = Vector3.one;
				}
			}
			transform.GetChild(1).gameObject.SetActive(true);
			GM.UIOpen = true;
		}
		else
		{
			Debug.Log("��ȣ�ۿ� Ÿ�� ����");
		}
	}

	private bool Mine()
	{
		if (GM.HavePickAxe == true)
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
		if (GM.HaveAxe == true)
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
		transform.GetChild(1).gameObject.SetActive(false);
		GM.UIOpen = false;
	}
}
