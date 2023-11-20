using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float health;
	public float speed;
	[SerializeField] private GameObject interactObj;
	[SerializeField] private WeaponData weaponData;
	private GameManager GM;
	private GameObject mainCamera;

	public int[] InventoryCode = new int[24];
	public int[] InventoryCount = new int[24];
	public Interactable OtherObj;

	private int NowAmmo;
	private int MaxAmmo;
	private bool reloading;

	void Start()
	{
		StartCoroutine("AttackCycle");
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		mainCamera = transform.Find("Main Camera").gameObject;
		MaxAmmo = weaponData.MaxAmmo;
		NowAmmo = MaxAmmo;
		GM.UIObj.transform.Find("Ammo").GetComponent<TextMeshProUGUI>().text = NowAmmo.ToString() + "/" + MaxAmmo.ToString();
	}

	void Update()
	{
		if (!GM.isUIOpen)
		{
			Move();
			Interact();
			if (Input.GetKeyDown(KeyCode.I))
			{
				InventoryRefresh();
				GM.UIOpen(GM.UIObj.transform.Find("Inventory").gameObject);
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				if (NowAmmo < MaxAmmo && reloading == false)
				{
					reloading = true;
					StartCoroutine(Reload());
				}
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GM.UIOpen(GM.UIObj.transform.Find("EscapeMenu").gameObject);
			}
		}
		else
		{
			transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
			if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
			{
				GM.closeObj.GetComponent<Button>().onClick.Invoke();
			}
		}
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void InventoryRefresh()
	{
		for (int i = 0; InventoryCode.Length > i; i++)
		{
			if (InventoryCount[i] == 0)
			{
				InventoryCode[i] = 0;
			}
			if (InventoryCode[i] == 0)
			{
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/0");
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
			}
			else
			{
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/" + InventoryCode[i]);
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = InventoryCount[i].ToString();
			}
		}
		if (OtherObj != null)
		{
			for (int i = 0; OtherObj.InventoryCode.Length > i; i++)
			{
				if (OtherObj.InventoryCount[i] == 0)
				{
					OtherObj.InventoryCode[i] = 0;
				}
				if (OtherObj.InventoryCode[i] == 0)
				{
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/0");
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
				}
				else
				{
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/" + OtherObj.InventoryCode[i]);
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = OtherObj.InventoryCount[i].ToString();
				}
			}
		}
		else
		{
			for (int i = 0; InventoryCode.Length > i; i++)
			{

				GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/0");
				GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
			}
		}
	}

	public bool AddItem(int itemCode, int itemCount)
	{
		bool iswork = false;
		for (int i = 0; InventoryCode.Length > i; i++)
		{
			if (InventoryCode[i] == itemCode)
			{
				InventoryCount[i] += itemCount;
				iswork = true;
				break;
			}
		}
		for (int i = 0; InventoryCode.Length > i && iswork == false; i++)
		{
			if (InventoryCode[i] == 0)
			{
				InventoryCode[i] = itemCode;
				InventoryCount[i] = itemCount;
				iswork = true;
				break;
			}
		}
		if (iswork == false)
		{
			Debug.Log("¿Œ∫•≈‰∏Æ∞° ∞°µÊ√°¥Ÿ");
		}
		return iswork;
	}

	IEnumerator Reload()
	{
		Slider reloadingBar = GM.UIObj.transform.Find("ReloadingBar").GetComponent<Slider>();
		reloadingBar.value = 0;
		reloadingBar.maxValue = weaponData.ReloadTime / 0.1f;
		int count = 0;
		reloadingBar.gameObject.SetActive(true);
		for (int i = 0; i < weaponData.ReloadTime / 0.1f; i++)
		{
			count++;
			reloadingBar.value = count;
			yield return new WaitForSeconds(0.1f);
		}
		NowAmmo = MaxAmmo;
		GM.UIObj.transform.Find("Ammo").GetComponent<TextMeshProUGUI>().text = NowAmmo.ToString() + "/" + MaxAmmo.ToString();
		reloading = false;
		yield return new WaitForSeconds(0.3f);
		reloadingBar.gameObject.SetActive(false);
	}

	IEnumerator AttackCycle()
	{
		while (true)
		{
			if (Input.GetMouseButton(0) && !GM.isUIOpen)
			{
				if (NowAmmo >= 1 && reloading == false)
				{
					GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.Find("Gun").position, transform.rotation);
					temp.GetComponent<Bullet>().DMG = weaponData.DMG;
					temp.GetComponent<Bullet>().BSPD = weaponData.BSPD;
					temp.GetComponent<Bullet>().range = weaponData.range;
					temp.GetComponent<Bullet>().PEN = weaponData.PEN;
					NowAmmo -= 1;
					GM.UIObj.transform.Find("Ammo").GetComponent<TextMeshProUGUI>().text = NowAmmo.ToString() + "/" + MaxAmmo.ToString();
					yield return new WaitForSeconds(60 / weaponData.RPM);
				}
				else
				{
					Debug.Log("Empty");
					yield return new WaitForSeconds(0.1f);
				}
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
			if (interactObj == null)
			{
				interactObj = collision.gameObject;
				GameObject interactNotice = Instantiate(Resources.Load<GameObject>("prefabs/InteractNotice"));
				interactNotice.transform.SetParent(interactObj.transform);
				interactNotice.transform.localPosition = new Vector3(0, 0, 0);
			}
			else
			{
				Destroy(interactObj.transform.Find("InteractNotice(Clone)").gameObject);
				interactObj = collision.gameObject;
				GameObject interactNotice = Instantiate(Resources.Load<GameObject>("prefabs/InteractNotice"));
				interactNotice.transform.SetParent(interactObj.transform);
				interactNotice.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (interactObj == collision.gameObject)
		{
			Destroy(interactObj.transform.Find("InteractNotice(Clone)").gameObject);
			interactObj = null;
		}
	}
}
