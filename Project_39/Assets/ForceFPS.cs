using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceFPS : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update() {

		// Gets the scene name.
		Scene scene = SceneManager.GetActiveScene();

		// Determines the target FPS by checking the scene name.
		if (scene.name == "UE - 1") {
			// Application.targetFrameRate = 35;
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 2") {
			// Application.targetFrameRate = 45;
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 3") {
			Application.targetFrameRate = 20;
		}
		else if (scene.name == "UE - 4") {
			Application.targetFrameRate = 5;
		}
		else if (scene.name == "UE - 5") {
			// Application.targetFrameRate = 25;
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 6") {
			// Application.targetFrameRate = 55;
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 7") {
			// Application.targetFrameRate = 40;
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 8") {
			Application.targetFrameRate = 30;
		}
		else if (scene.name == "UE - 9") {
			Application.targetFrameRate = 10;
		}
		else if (scene.name == "UE - 10") {
			// Application.targetFrameRate = 50;
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 11") {
			Application.targetFrameRate = 60;
		}
		else if (scene.name == "UE - 12") {
			Application.targetFrameRate = 15;
		}
		else {
			Application.targetFrameRate = 60;
		}
    }
}
