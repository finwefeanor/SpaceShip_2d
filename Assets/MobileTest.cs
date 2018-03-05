using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MobileTest : MonoBehaviour 
{
    Rigidbody rigidBody;

    public float engineThrust;
    public float rotationSpeed;
    public Vector3 speed;

    float directionx;
    float moveHorizontal;
    bool moveHorizontalRightButton;
    float moveVertical;

    // Use this for initialization
    void Start () 
    {
        rigidBody = GetComponent<Rigidbody>();

    }

    void Update() {
        //AddForceUpward();
        //RotateRight();
        moveHorizontal = CrossPlatformInputManager.GetAxis("TouchHorizontal");
        moveVertical = CrossPlatformInputManager.GetAxis("Vertical");
    }

    void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            rigidBody.AddRelativeForce(0, 1 * engineThrust * Time.deltaTime, 0);
        }
        AddForceUpward();
        BoostButton();
        Rotate();
    }

    private void AddForceUpward() {

        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            //directionVer = CrossPlatformInputManager.GetAxis("Vertical");
            rigidBody.AddRelativeForce(0, 2*speed.y * engineThrust * Time.deltaTime, 0);
        }
        else if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            rigidBody.AddRelativeForce(Vector3.up * (engineThrust), 0);
        }

    }

    private void BoostButton() 
    {
        bool isBoosting = CrossPlatformInputManager.GetButton("Boost");
        rigidBody.AddRelativeForce(Vector3.up * (moveVertical*speed.y*engineThrust), 0);    
    }

    private void Rotate() {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationMultiplier = rotationSpeed * Time.deltaTime;
        transform.Rotate(-Vector3.forward * moveHorizontal * speed.z * Time.deltaTime);
        //transform.Rotate(Vector3.forward * moveHorizontal * speed.z*Time.deltaTime);
        //transform.Rotate(Vector3.forward * rotationThisFrame);
    }

}
