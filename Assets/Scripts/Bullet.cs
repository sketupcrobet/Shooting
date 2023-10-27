using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float DMG;
	public float BSPD;
	public float Range;
	public float PEN;
	private Vector3 StartLoc;
	private float StartTime;
	private int count;

	void Start()
	{
		StartTime = Time.time;
		StartLoc = Camera.main.WorldToScreenPoint(transform.position);
	}

	void Update()
	{
		Vector3 CurrentPostion = transform.position;
		transform.position += (transform.right * BSPD * Time.deltaTime * 0.01f);
		Vector3 LaterPosition = transform.position;
		if (Range / BSPD < Time.time - StartTime)
		{
			Destroy(gameObject);
		}

		LayerMask MyLayermask = (1 << 6) | (1 << 7);

		RaycastHit2D[] ALLHitz = Physics2D.RaycastAll(CurrentPostion, transform.right, Vector3.Distance(CurrentPostion, LaterPosition), MyLayermask);
		for (int i = 0; i < ALLHitz.Length; i++)
		{
			/*PEN--;
			if (PEN >= 0)
			{
				ALLHitz[i].collider.GetComponent<Enemy>().Health -= DMG;
			}*/
			if (i == 0 && ALLHitz[0].collider.tag == "Enemy")
			{
				ALLHitz[0].collider.GetComponent<Enemy>().Health -= DMG;
			}
			Destroy(gameObject);
		}
	}

	/*private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			collision.GetComponent<Enemy>().Health -= DMG;
		}
		if (collision.tag != "Item" && collision.tag != "Player")
		{
			Destroy(gameObject);
		}
	}*/
}
