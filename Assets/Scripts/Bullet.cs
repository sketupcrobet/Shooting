using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float DMG;
	public float BSPD;
	public float range;
	public float PEN;
	private Vector3 startLoc;
	private float startTime;
	private int count;

	void Start()
	{
		startTime = Time.time;
		startLoc = Camera.main.WorldToScreenPoint(transform.position);
	}

	void Update()
	{
		Vector3 CurrentPostion = transform.position;
		transform.position += (transform.right * BSPD * Time.deltaTime * 0.01f);
		Vector3 LaterPosition = transform.position;
		if (range / BSPD < Time.time - startTime)
		{
			Destroy(gameObject);
		}

		LayerMask MyLayermask = (1 << 6) | (1 << 7) | (1 << 8);

		RaycastHit2D[] raycast = Physics2D.RaycastAll(CurrentPostion, transform.right, Vector3.Distance(CurrentPostion, LaterPosition), MyLayermask);
		for (int i = 0; i < raycast.Length; i++)
		{
			if (raycast[0].collider.tag == "Player")
			{
				raycast[0].collider.GetComponent<Player>().health -= DMG;
			}
			if (raycast[0].collider.tag == "Enemy")
			{
				raycast[0].collider.GetComponent<Enemy>().health -= DMG;
			}
			Destroy(gameObject);
		}
	}
}
