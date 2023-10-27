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
}
