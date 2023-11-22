using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
	public float moveSpeed = 200f;

	private void Start()
	{
		Destroy(this.gameObject, 3f);
	}

	private void Update()
	{

		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}
}
