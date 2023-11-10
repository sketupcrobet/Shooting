using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float health;

	public bool moveOn;
	private Vector3 startPos;
	private Vector3 endPos;
	public float moveRange;
	public float moveSpeed;
	private bool isreset;
	public float minDelay;
	public float maxDelay;

	[SerializeField] private WeaponData weaponData;
	private bool isPlayer;

	public int MoveType;

	IEnumerator resetChase;

	private GameObject PL;

	void Start()
	{
		moveOn = true;
		startPos = transform.position;
		ResetPos();
		StartCoroutine("AttackCycle");
		resetChase = ResetChase();
		PL = GameObject.Find("Player");
	}

	void Update()
	{
		Move();
		if (health <= 0)
		{
			Destroy(gameObject);
		}
		Debug.DrawRay(transform.position, GameObject.Find("Player").transform.position - transform.position, Color.yellow);
	}

	IEnumerator AttackCycle()
	{
		while (true)
		{
			if (isPlayer)
			{
				GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.Find("Gun").position, transform.rotation);
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

	IEnumerator ResetMove()
	{
		yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
		ResetPos();
	}

	IEnumerator ResetChase()
	{
		yield return new WaitForSeconds(10);
		MoveType = 2;
		isreset = false;
		StartCoroutine(ResetMove());
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
			//통상
			if (MoveType == 0)
			{
				transform.GetChild(0).GetComponent<CircleCollider2D>().radius = 5;
				if (transform.position == endPos)
				{
					if (isreset == true)
					{
						isreset = false;
						StartCoroutine(ResetMove());
					}
				}
				if (isreset == true)
				{
					transform.position += transform.right * moveSpeed * Time.deltaTime;
				}
			}
			//교전
			else if (MoveType == 1)
			{
				transform.GetChild(0).GetComponent<CircleCollider2D>().radius = 10;
				Vector3 myloc = transform.position;
				Vector3 targetloc = PL.transform.position;
				transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetloc.y - myloc.y, targetloc.x - myloc.x) * Mathf.Rad2Deg);
				transform.position += transform.right * moveSpeed * Time.deltaTime;
			}
			//경계
			else if (MoveType == 2)
			{
				transform.GetChild(0).GetComponent<CircleCollider2D>().radius = 8;
				if (transform.position == endPos)
				{
					if (isreset == true)
					{
						isreset = false;
						StartCoroutine(ResetMove());
					}
				}
				if (isreset == true)
				{
					transform.position += transform.right * moveSpeed * Time.deltaTime;
				}
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
			LayerMask MyLayermask = (1 << 6) | (1 << 8);

			RaycastHit2D ALLHitz = Physics2D.Raycast(transform.position, GameObject.Find("Player").transform.position - transform.position, transform.GetChild(0).GetComponent<CircleCollider2D>().radius, MyLayermask);
			if (ALLHitz.transform.gameObject.tag == "Player")
			{

				StopCoroutine(resetChase);
				Vector3 mypos = transform.position;
				Vector3 targetpos = collision.transform.position;
				transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetpos.y - mypos.y, targetpos.x - mypos.x) * Mathf.Rad2Deg);
				MoveType = 1;
				if (Vector3.Distance(mypos, targetpos) <= 5)
				{
					isPlayer = true;
				}
				else
				{
					isPlayer = false;
				}
			}
			else
			{
				resetChase = ResetChase();
				StartCoroutine(resetChase);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			resetChase = ResetChase();
			StartCoroutine(resetChase);
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