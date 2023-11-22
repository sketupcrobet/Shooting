using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float health;
	public float speed;
	private GameManager GM;
	private GameObject mainCamera;

	public int[] inventoryCode = new int[24];
	public int[] inventoryCount = new int[24];
	public int[] equipCode = new int[6];
	public int[] hotbarCode = new int[10];

	[SerializeField] private WeaponData weaponData;
	public Interactable interactObj;
	public ItemSlot inventoryObj;

	private int nowAmmo;
	private int maxAmmo;
	private bool reloading;

	void Start()
	{
		StartCoroutine("AttackCycle");
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		mainCamera = transform.Find("Main Camera").gameObject;
		maxAmmo = weaponData.MaxAmmo;
		nowAmmo = maxAmmo;
		GM.UIObj.transform.Find("Ammo").GetComponent<TextMeshProUGUI>().text = nowAmmo.ToString() + "/" + maxAmmo.ToString();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (inventoryCode[int.Parse(inventoryObj.name.Split("(")[1].Split(")")[0])] != 0)
			{
				hotbarCode[0] = int.Parse(inventoryObj.name.Split("(")[1].Split(")")[0]) + 1;
			}
			InventoryRefresh();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{

		}
		if (!GM.isUIOpen)
		{
			Move();
			if (Input.GetKeyDown(KeyCode.G) && interactObj != null)
			{
				interactObj.Interact();
			}
			if (Input.GetKeyDown(KeyCode.I))
			{
				InventoryRefresh();
				GM.UIOpen(GM.UIObj.transform.Find("Inventory").gameObject);
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				if (nowAmmo < maxAmmo && reloading == false)
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
		for (int i = 0; inventoryCode.Length > i; i++)
		{
			if (inventoryCount[i] == 0)
			{
				inventoryCode[i] = 0;
			}
			if (inventoryCode[i] == 0)
			{
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/0");
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
			}
			else
			{
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/" + inventoryCode[i]).GetComponent<Item>().itemIcon;
				//GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/" + inventoryCode[i]);
				GM.UIObj.transform.Find("Inventory").Find("Player").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = inventoryCount[i].ToString();
			}
		}
		for (int i = 0; equipCode.Length > i; i++)
		{
			GM.UIObj.transform.Find("Inventory").Find("Equip").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/" + equipCode[i]).GetComponent<Item>().itemIcon;
			//GM.UIObj.transform.Find("Inventory").Find("Equip").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/" + EquipCode[i]);
		}
		if (interactObj != null)
		{
			for (int i = 0; interactObj.inventoryCode.Length > i; i++)
			{
				if (interactObj.inventoryCount[i] == 0)
				{
					interactObj.inventoryCode[i] = 0;
				}
				if (interactObj.inventoryCode[i] == 0)
				{
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/0");
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
				}
				else
				{
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/" + interactObj.inventoryCode[i]).GetComponent<Item>().itemIcon;
					//GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/" + OtherObj.inventoryCode[i]);
					GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = interactObj.inventoryCount[i].ToString();
				}
			}
			GM.UIObj.transform.Find("Inventory").Find("OName").GetChild(0).GetComponent<TextMeshProUGUI>().text = interactObj.name;
		}
		else
		{
			for (int i = 0; inventoryCode.Length > i; i++)
			{
				GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Item/0");
				GM.UIObj.transform.Find("Inventory").Find("Other").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
			}
			GM.UIObj.transform.Find("Inventory").Find("OName").GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
		}
		for (int i = 0; hotbarCode.Length > i; i++)
		{
			if (hotbarCode[i] != 0)
			{
				GM.UIObj.transform.Find("Inventory").Find("Hotbar").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/" + inventoryCode[hotbarCode[i] - 1]).GetComponent<Item>().itemIcon;
				GM.UIObj.transform.Find("Inventory").Find("Hotbar").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = inventoryCount[hotbarCode[i] - 1].ToString();
			}
			else
			{
				GM.UIObj.transform.Find("Inventory").Find("Hotbar").GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load("Item/0").GetComponent<Item>().itemIcon;
				GM.UIObj.transform.Find("Inventory").Find("Hotbar").GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
			}
		}
	}

	public bool AddItem(int itemCode, int itemCount)
	{
		bool iswork = false;
		for (int i = 0; inventoryCode.Length > i; i++)
		{
			if (inventoryCode[i] == itemCode)
			{
				inventoryCount[i] += itemCount;
				iswork = true;
				break;
			}
		}
		for (int i = 0; inventoryCode.Length > i && iswork == false; i++)
		{
			if (inventoryCode[i] == 0)
			{
				inventoryCode[i] = itemCode;
				inventoryCount[i] = itemCount;
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
		nowAmmo = maxAmmo;
		GM.UIObj.transform.Find("Ammo").GetComponent<TextMeshProUGUI>().text = nowAmmo.ToString() + "/" + maxAmmo.ToString();
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
				if (nowAmmo >= 1 && reloading == false)
				{
					GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.Find("Gun").position, transform.rotation);
					temp.GetComponent<Bullet>().DMG = weaponData.DMG;
					temp.GetComponent<Bullet>().BSPD = weaponData.BSPD;
					temp.GetComponent<Bullet>().range = weaponData.range;
					temp.GetComponent<Bullet>().PEN = weaponData.PEN;
					nowAmmo -= 1;
					GM.UIObj.transform.Find("Ammo").GetComponent<TextMeshProUGUI>().text = nowAmmo.ToString() + "/" + maxAmmo.ToString();
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Resources" || collision.tag == "Item" || collision.tag == "Box" || collision.tag == "NPC")
		{
			if (interactObj == null)
			{
				interactObj = collision.gameObject.GetComponent<Interactable>();
				GameObject interactNotice = Instantiate(Resources.Load<GameObject>("prefabs/InteractNotice"));
				interactNotice.transform.SetParent(interactObj.transform);
				interactNotice.transform.localPosition = new Vector3(0, 0, 0);
			}
			else
			{
				Destroy(interactObj.transform.Find("InteractNotice(Clone)").gameObject);
				interactObj = collision.gameObject.GetComponent<Interactable>();
				GameObject interactNotice = Instantiate(Resources.Load<GameObject>("prefabs/InteractNotice"));
				interactNotice.transform.SetParent(interactObj.transform);
				interactNotice.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (interactObj == collision.gameObject.GetComponent<Interactable>())
		{
			Destroy(interactObj.transform.Find("InteractNotice(Clone)").gameObject);
			interactObj = null;
		}
	}
}
