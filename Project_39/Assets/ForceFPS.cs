using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceFPS : MonoBehaviour {

	int frameRate = 60;
	double oldTime = 0.0;
	double deltaTime = 0.0;
	double currentTime = 0.0;

	// Start is called before the first frame update
	void Start() {
		oldTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update() {

		// Gets the scene name.
		Scene scene = SceneManager.GetActiveScene();

		// Determines the target FPS by checking the scene name.
		if (scene.name == "UE - 1") {
			frameRate = 35;
		}
		else if (scene.name == "UE - 2") {
			frameRate = 45;
		}
		else if (scene.name == "UE - 3") {
			frameRate = 20;
		}
		else if (scene.name == "UE - 4") {
			frameRate = 31;
		}
		else if (scene.name == "UE - 5") {
			frameRate = 5;
		}
		else if (scene.name == "UE - 6") {
			frameRate = 25;
		}
		else if (scene.name == "UE - 7") {
			frameRate = 55;
		}
		else if (scene.name == "UE - 8") {
			frameRate = 32;
		}
		else if (scene.name == "UE - 9") {
			frameRate = 40;
		}
		else if (scene.name == "UE - 10") {
			frameRate = 30;
		}
		else if (scene.name == "UE - 11") {
			frameRate = 10;
		}
		else if (scene.name == "UE - 12") {
			frameRate = 33;
		}
		else if (scene.name == "UE - 13") {
			frameRate = 50;
		}
		else if (scene.name == "UE - 14") {
			frameRate = 60;
		}
		else if (scene.name == "UE - 15") {
			frameRate = 15;
		}
		else if (scene.name == "UE - 16") {
			frameRate = 34;
		}
		else {
			frameRate = 60;
		}

		// Busy waits to cap the FPS.
		deltaTime = 1.0 / frameRate;
		currentTime = Time.realtimeSinceStartup;

		while ((currentTime - oldTime) < deltaTime) {
			currentTime = Time.realtimeSinceStartup;
		}

		oldTime = Time.realtimeSinceStartup;
	}
}
