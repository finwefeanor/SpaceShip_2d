using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscilator_rotation : MonoBehaviour {

    [SerializeField] Vector3 rotationVector = new Vector3(0, 0, 90f);
    [SerializeField] float period = 2f;

    // todo remove from inspector later
    [Range(0, 90)] [SerializeField] float rotationFactor; // 0 for not moved, 1 for fully moved.

    float startingRot;

	void Start() 
    {
        //startingRot = transform.rotation;
    }

	void Update() 
    {
        //set movement factor
        float cycles = Time.time / period; // grows constinually from 0 as the period increases
        float rawSinWave = period * Time.time;

        //const float tau = Mathf.PI * 90f; // about 6.28
        //float rawSinWave = Mathf.Sin(cycles * tau);

        rotationFactor = rawSinWave; // / 2f + 0.5f;
        Vector3 offset = rotationFactor * rotationVector;
        transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.forward * rawSinWave);
    }
}
