using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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

	public int MoveType;

	IEnumerator test;

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
		Vector3 myloc = transform.position;
		Vector3 targetloc = new Vector2(startPos.x + Random.Range(-moveRange, moveRange), startPos.y + Random.Range(-moveRange, moveRange));
		transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetloc.y - myloc.y, targetloc.x - myloc.x) * Mathf.Rad2Deg);
		isreset = true;
	}

	private void Move()
	{
		if (moveOn)
		{
			if (MoveType == 0)
			{
				if (transform.position == endPos)
				{
					if (isreset == true)
					{
						isreset = false;
						StartCoroutine(ResetCoroutine());
					}
				}
				if (transform.position.x < endPos.x)
				{
					GetComponent<SpriteRenderer>().flipX = true;
				}
				else if (transform.position.x > endPos.x)
				{
					GetComponent<SpriteRenderer>().flipX = false;
				}
				//transform.position = Vector2.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
				if (isreset == true)
				{
					transform.position += (transform.right * moveSpeed * Time.deltaTime);
				}
			}
			else if (MoveType == 1)
			{
				GameObject PL = GameObject.Find("Player");
				Vector3 myloc = transform.position;
				Vector3 targetloc = PL.transform.position;
				transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetloc.y - myloc.y, targetloc.x - myloc.x) * Mathf.Rad2Deg);
				//transform.position = Vector2.MoveTowards(myloc, targetloc, moveSpeed * Time.deltaTime);
				transform.position += (transform.right * moveSpeed * Time.deltaTime);
			}
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
			StopCoroutine(test);
			Vector3 myloc = transform.position;
			Vector3 targetloc = collision.transform.position;
			transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetloc.y - myloc.y, targetloc.x - myloc.x) * Mathf.Rad2Deg);
			MoveType = 1;
			transform.GetChild(0).GetComponent<CircleCollider2D>().radius = 10;
			if (Vector3.Distance(myloc, targetloc) <= 5)
			{
				isPlayer = true;
			}
			else
			{
				isPlayer = false;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			test = ResetChase();
			StartCoroutine(test);
		}
	}

	IEnumerator ResetChase()
	{
		yield return new WaitForSeconds(10);
		MoveType = 0;
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