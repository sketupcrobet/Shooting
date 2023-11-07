using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float Health;

	public bool moveOn;
	private Vector3 startPos;
	private Vector3 endPos;
	public float moveRange;
	public float moveSpeed;
	private bool isreset;
	public float minDelay;
	public float maxDelay;

	[SerializeField] private WeaponData WeaponData;
	private bool isPlayer;

	void Start()
	{
		moveOn = true;
		startPos = transform.position;
		ResetPos();
		StartCoroutine("AttackCycle");
	}

	void Update()
	{
		Move();
		if (Health <= 0)
		{
			Destroy(gameObject);
		}
	}

	IEnumerator AttackCycle()
	{
		while (true)
		{
			if (isPlayer)
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

	IEnumerator ResetCoroutine()
	{
		yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
		ResetPos();
	}

	private void ResetPos()
	{
		endPos = new Vector2(startPos.x + Random.Range(-moveRange, moveRange), startPos.y + Random.Range(-moveRange, moveRange));
		isreset = true;
	}

	private void Move()
	{
		if (moveOn)
		{
			if (gameObject.transform.position == endPos)
			{
				if (isreset == true)
				{
					isreset = false;
					StartCoroutine(ResetCoroutine());
				}
			}
			if (gameObject.transform.position.x < endPos.x)
			{
				gameObject.GetComponent<SpriteRenderer>().flipX = true;
			}
			else if (gameObject.transform.position.x > endPos.x)
			{
				gameObject.GetComponent<SpriteRenderer>().flipX = false;
			}
			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, endPos, moveSpeed * Time.deltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			endPos = transform.position;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Vector3 mPosition = collision.transform.position;
			Vector3 oPosition = transform.position;
			transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mPosition.y - oPosition.y, mPosition.x - oPosition.x) * Mathf.Rad2Deg);
			isPlayer = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			isPlayer = false;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		UnityEditor.Handles.color = Color.green;
		if (startPos == Vector3.zero)
		{
			UnityEditor.Handles.DrawWireCube(transform.position, new Vector3(moveRange * 2, moveRange * 2, 0));
		}
		else
		{
			UnityEditor.Handles.DrawWireCube(startPos, new Vector3(moveRange * 2, moveRange * 2, 0));
		}
	}
#endif
}