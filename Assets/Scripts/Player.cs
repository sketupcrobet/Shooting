using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float health;
	public float speed;
	private int interactCount;
	[SerializeField] private GameObject interactObj;
	[SerializeField] private WeaponData weaponData;
	private GameManager GM;
	private GameObject mainCamera;
	public int[] InventoryNum = new int[24];
	public int[] InventoryCount = new int[24];

	void Start()
	{
		StartCoroutine("AttackCycle");
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		mainCamera = transform.Find("Main Camera").gameObject;
	}

	void Update()
	{
		if (!GM.UIOpen)
		{
			Move();
			Interact();
			if (Input.GetKeyDown(KeyCode.I))
			{
				for (int i = 0; i < InventoryNum.Length; i++)
				{
					if (InventoryNum[i] == 0 || InventoryCount[i] == 0)
					{
						transform.GetChild(2).GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/0") as Sprite;
						transform.GetChild(2).GetChild(1).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
					}
					else
					{
						Debug.Log(transform.GetChild(2).GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>());
						//transform.GetChild(2).GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/" + InventoryNum[i].ToString()) as Sprite;
						transform.GetChild(2).GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/0") as Sprite;
						transform.GetChild(2).GetChild(1).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = InventoryCount[i].ToString();
					}
				}
				transform.GetChild(2).gameObject.SetActive(true);
				transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
				GM.UIOpen = true;
			}

		}
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

	IEnumerator AttackCycle()
	{
		while (true)
		{
			if (Input.GetMouseButton(0) && !GM.UIOpen)
			{
				GameObject temp = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, transform.Find("Gun").position, transform.rotation);
				temp.GetComponent<Bullet>().DMG = weaponData.DMG;
				temp.GetComponent<Bullet>().BSPD = weaponData.BSPD;
				temp.GetComponent<Bullet>().range = weaponData.range;
				temp.GetComponent<Bullet>().PEN = weaponData.PEN;
				yield return new WaitForSeconds(60 / weaponData.RPM);
			}
			else
			{
				yield return null;
			}
		}
	}

	private void Move()
	{
		float MoveX;
		float MoveY;
		Vector3 mypos = transform.position;
		Vector3 targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetpos.y - mypos.y, targetpos.x - mypos.x) * Mathf.Rad2Deg);
		mainCamera.transform.localRotation = Quaternion.Euler(0, 0, -Mathf.Atan2(targetpos.y - mypos.y, targetpos.x - mypos.x) * Mathf.Rad2Deg);

		MoveX = Input.GetAxisRaw("Horizontal");
		MoveY = Input.GetAxisRaw("Vertical");
		transform.GetComponent<Rigidbody2D>().velocity = new Vector3(MoveX * speed, MoveY * speed, 0);
	}

	private void Interact()
	{
		if (Input.GetKeyDown(KeyCode.G) && interactObj != null)
		{
			interactObj.GetComponent<Interactable>().Interact();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Resources" || collision.tag == "Item" || collision.tag == "Box" || collision.tag == "NPC")
		{
			interactCount++;
			interactObj = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		interactCount--;
		if (interactObj == collision.gameObject || interactCount == 0)
		{
			interactObj = null;
		}
	}
}
