using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float Speed;
	private int InteractCount;
	private GameObject InteractObj;
	[SerializeField] private WeaponData WeaponData;
	private GameManager GM;

	public float test1;
	public bool test2;

	void Start()
	{
		StartCoroutine("AttackCycle");
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Update()
	{
		Move();
		Interact();
	}

	IEnumerator AttackCycle()
	{
		while (true)
		{
			if (Input.GetMouseButton(0) && !GM.UIOpen)
			{
				GameObject temp = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, transform.Find("Gun").position, transform.rotation);
				temp.GetComponent<Bullet>().DMG = WeaponData.DMG;
				temp.GetComponent<Bullet>().BSPD = WeaponData.BSPD;
				temp.GetComponent<Bullet>().Range = WeaponData.Range;
				temp.GetComponent<Bullet>().PEN = WeaponData.PEN;
				yield return new WaitForSeconds(60 / WeaponData.RPM);
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
		if (!GM.UIOpen)
		{
			Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 oPosition = transform.position;
			transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mPosition.y - oPosition.y, mPosition.x - oPosition.x) * Mathf.Rad2Deg);
			transform.Find("Main Camera").localRotation = Quaternion.Euler(0, 0, -Mathf.Atan2(mPosition.y - oPosition.y, mPosition.x - oPosition.x) * Mathf.Rad2Deg);

			MoveX = Input.GetAxisRaw("Horizontal");
			MoveY = Input.GetAxisRaw("Vertical");
		}
		else
		{
			MoveX = 0;
			MoveY = 0;
		}
		transform.GetComponent<Rigidbody2D>().velocity = new Vector3(MoveX * Speed, MoveY * Speed, 0);
	}

	private void Interact()
	{
		if (Input.GetKeyDown(KeyCode.G) && InteractObj != null)
		{
			InteractObj.GetComponent<Interactable>().Interact();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		InteractCount++;
		InteractObj = collision.gameObject;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		InteractCount--;
		if (InteractObj == collision.gameObject || InteractCount == 0)
		{
			InteractObj = null;
		}
	}
}
