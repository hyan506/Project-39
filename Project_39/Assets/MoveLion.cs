using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLion : MonoBehaviour
{

	int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		// Moves the lion.
		if (direction == 1) {
			transform.Translate((float)0.05, 0, 0);
		}
		else if (direction == 2) {
			transform.Translate((float)-0.05, 0, 0);
		}

		// Changes the lion's direction.
        if (transform.position.x <= -1.5) {
			direction = 2;
		}
		else if (transform.position.x >= 1.5) {
			direction = 1;
		}
    }
}
