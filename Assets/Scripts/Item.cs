using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
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
		else if (tag == "OtherInventory")
		{
			bool isAdd = false;
			for (int i = 0; PL.InventoryCode.Length > i; i++)
			{
				if (PL.InventoryCode[i] == PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])])
				{
					PL.InventoryCount[i] += PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])];
					PL.OtherObj.InventoryCode[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
					PL.OtherObj.InventoryCount[int.Parse(name.Split("(")[1].Split(")")[0])] = 0;
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
		PL.InventoryRefresh();
	}
}
