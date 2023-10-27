using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
	public float Speed;
	private int InteractCount;
	public GameObject InteractObj;
	[SerializeField] private GameObject Bullet;
	[SerializeField] private WeaponData WeaponData;

	public float test1;
	public bool test2;

	void Start()
	{
		StartCoroutine("AttackCycle");
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
			if (Input.GetMouseButton(0))
			{
				GameObject temp = Instantiate(Bullet, transform.Find("Gun").position, transform.rotation);
				//temp.transform.SetParent(GameObject.Find("Canvas").transform);
				//temp.transform.localScale = Vector3.one;
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
		Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 oPosition = transform.position;
		transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mPosition.y - oPosition.y, mPosition.x - oPosition.x) * Mathf.Rad2Deg);
		transform.GetChild(1).localRotation = Quaternion.Euler(0, 0, -Mathf.Atan2(mPosition.y - oPosition.y, mPosition.x - oPosition.x) * Mathf.Rad2Deg);

		float MoveX = Input.GetAxisRaw("Horizontal");
		float MoveY = Input.GetAxisRaw("Vertical");

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
