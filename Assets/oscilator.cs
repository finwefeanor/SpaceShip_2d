using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscilator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor; // 0 for not moved, 1 for fully moved.
    Vector3 startingPos;

	void Start() 
    {
        startingPos = transform.position;
    }

	void Update() 
    {
        //todo protect against period is zero. other solution:
        // if (period <= Mathf.Epsilon){ return;}
        if (period != 0)
        {
            float cycles = Time.time / period; // grows constinually from 0 as the period increases
            const float tau = Mathf.PI * 2f; // about 6.28
            float rawSinWave = Mathf.Sin(cycles * tau);


            movementFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movementFactor * movementVector;
            transform.position = startingPos + offset;
        }

    }
}
