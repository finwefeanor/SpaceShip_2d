using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveScript : MonoBehaviour {

	float directionX;
	public float moveSpeed = 5f;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		directionX = CrossPlatformInputManager.GetAxis ("Horizontal");

        rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y);
    }

	

}
