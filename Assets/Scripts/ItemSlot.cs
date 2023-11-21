using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Sse4_2;

public class ItemSlot : MonoBehaviour
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
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				for (int i = 0; PL.OtherObj.InventoryCode.Length > i; i++)
				{
					if (PL.OtherObj.InventoryCode[i] == PL.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])])
					{
						PL.OtherObj.InventoryCount[i] += PL.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						isAdd = true;
						break;
					}
				}
				for (int i = 0; PL.OtherObj.InventoryCode.Length > i && isAdd == false; i++)
				{
					if (PL.OtherObj.InventoryCode[i] == 0)
					{
						PL.OtherObj.InventoryCode[i] = PL.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.OtherObj.InventoryCount[i] = PL.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						break;
					}
				}
			}
			else
			{
				string itemType = Resources.Load("Item/" + PL.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])]).GetComponent<Item>().itemType;
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
					if (PL.EquipCode[i] == 0)
					{
						PL.EquipCode[i] = PL.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] -= 1;
						break;
					}
				}
			}
		}
		else if (tag == "OtherInventory")
		{
			bool isAdd = false;
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				for (int i = 0; PL.InventoryCode.Length > i; i++)
				{
					if (PL.InventoryCode[i] == PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])])
					{
						PL.InventoryCount[i] += PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						isAdd = true;
						break;
					}
				}
				for (int i = 0; PL.InventoryCode.Length > i && isAdd == false; i++)
				{
					if (PL.InventoryCode[i] == 0)
					{
						PL.InventoryCode[i] = PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.InventoryCount[i] = PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
						break;
					}
				}
			}
			else
			{
				string itemType = Resources.Load("Item/" + PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])]).GetComponent<Item>().itemType;
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
					if (PL.EquipCode[i] == 0)
					{
						PL.EquipCode[i] = PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])];
						PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] -= 1;
						break;
					}
				}
			}
		}
		else if (tag == "EquipInventory")
		{
			bool isAdd = false;
			for (int i = 0; PL.InventoryCode.Length > i; i++)
			{
				if (PL.InventoryCode[i] == PL.EquipCode[int.Parse(name.Split("(")[1].Split(")")[0])])
				{
					PL.InventoryCount[i] += 1;
					PL.EquipCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
					isAdd = true;
					break;
				}
			}
			for (int i = 0; PL.InventoryCode.Length > i && isAdd == false; i++)
			{
				if (PL.InventoryCode[i] == 0)
				{
					PL.InventoryCode[i] = PL.EquipCode[int.Parse(name.Split("(")[1].Split(")")[0])];
					PL.InventoryCount[i] = 1;
					PL.EquipCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
					break;
				}
			}
		}
		PL.InventoryRefresh();
	}
}
