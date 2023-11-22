using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Player PL;

	void Start()
	{
		PL = GameObject.Find("Player").GetComponent<Player>();
	}

	void Update()
	{

	}

	public void ClickThis()
	{
		if (tag == "PlayerInventory")
		{
			bool isAdd = false;
			if (Input.GetKey(KeyCode.LeftControl))
			{
				for (int i = 0; PL.interactObj.inventoryCode.Length > i; i++)
				{
					if (PL.interactObj.inventoryCode[i] == PL.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])])
					{
						PL.interactObj.inventoryCount[i] += PL.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						isAdd = true;
						break;
					}
				}
				for (int i = 0; PL.interactObj.inventoryCode.Length > i && isAdd == false; i++)
				{
					if (PL.interactObj.inventoryCode[i] == 0)
					{
						PL.interactObj.inventoryCode[i] = PL.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.interactObj.inventoryCount[i] = PL.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						break;
					}
				}
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				string itemType = Resources.Load("Item/" + PL.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])]).GetComponent<Item>().itemType;
				int startnum = 0;
				int maxnum = 0;
				if (itemType == "Tool" || itemType == "Weapon")
				{
					startnum = 0;
					maxnum = 2;
				}
				else if (itemType == "Resource")
				{
					startnum = 2;
					maxnum = 6;
				}
				for (int i = startnum; maxnum > i; i++)
				{
					if (PL.equipCode[i] == 0)
					{
						PL.equipCode[i] = PL.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] -= 1;
						break;
					}
				}
			}
		}
		else if (tag == "OtherInventory")
		{
			bool isAdd = false;
			if (Input.GetKey(KeyCode.LeftControl))
			{
				for (int i = 0; PL.inventoryCode.Length > i; i++)
				{
					if (PL.inventoryCode[i] == PL.interactObj.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])])
					{
						PL.inventoryCount[i] += PL.interactObj.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.interactObj.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.interactObj.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						isAdd = true;
						break;
					}
				}
				for (int i = 0; PL.inventoryCode.Length > i && isAdd == false; i++)
				{
					if (PL.inventoryCode[i] == 0)
					{
						PL.inventoryCode[i] = PL.interactObj.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.inventoryCount[i] = PL.interactObj.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.interactObj.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.interactObj.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						break;
					}
				}
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				string itemType = Resources.Load("Item/" + PL.interactObj.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])]).GetComponent<Item>().itemType;
				int startnum = 0;
				int maxnum = 0;
				if (itemType == "Tool" || itemType == "Weapon")
				{
					startnum = 0;
					maxnum = 2;
				}
				else if (itemType == "Resource")
				{
					startnum = 2;
					maxnum = 6;
				}
				for (int i = startnum; maxnum > i; i++)
				{
					if (PL.equipCode[i] == 0)
					{
						PL.equipCode[i] = PL.interactObj.inventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.interactObj.inventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] -= 1;
						break;
					}
				}
			}
		}
		else if (tag == "EquipInventory")
		{
			bool isAdd = false;
			if (Input.GetKey(KeyCode.LeftControl))
			{
				for (int i = 0; PL.inventoryCode.Length > i; i++)
				{
					if (PL.inventoryCode[i] == PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])])
					{
						PL.inventoryCount[i] += 1;
						PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						isAdd = true;
						break;
					}
				}
				for (int i = 0; PL.inventoryCode.Length > i && isAdd == false; i++)
				{
					if (PL.inventoryCode[i] == 0)
					{
						PL.inventoryCode[i] = PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.inventoryCount[i] = 1;
						PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						break;
					}
				}
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				for (int i = 0; PL.interactObj.inventoryCode.Length > i; i++)
				{
					if (PL.interactObj.inventoryCode[i] == PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])])
					{
						PL.interactObj.inventoryCount[i] += 1;
						PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						isAdd = true;
						break;
					}
				}
				for (int i = 0; PL.inventoryCode.Length > i && isAdd == false; i++)
				{
					if (PL.interactObj.inventoryCode[i] == 0)
					{
						PL.interactObj.inventoryCode[i] = PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.interactObj.inventoryCount[i] = 1;
						PL.equipCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						break;
					}
				}
			}
		}
		PL.InventoryRefresh();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		PL.inventoryObj = GetComponent<ItemSlot>();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		PL.inventoryObj = null;
	}
}
