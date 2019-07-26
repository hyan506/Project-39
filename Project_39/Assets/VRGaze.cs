using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRGaze : MonoBehaviour {

	public GameObject a;
	public GameObject b;
	public Image img;
	private RaycastHit _hit;
	bool gazeStatus = false;
	float gazeTime = 0;
	float activeTime = 0;

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

		// Waits for a few seconds before the buttons are revealed.
		activeTime += Time.deltaTime;
		if (activeTime / 3 >= 1) {
			a.SetActive(true);
			b.SetActive(true);
		}

		// Fills the reticle if it's on a target.
		if (gazeStatus == true) {
			gazeTime += Time.deltaTime;
			img.fillAmount = gazeTime / 2;
		}

		// Creates a ray.
		Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

		// Logs the appropriate response and loads the next scene.
		if (Physics.Raycast(r, out _hit) == true && img.fillAmount >= 1) {

			// Determines the current FPS.
			Scene scene = SceneManager.GetActiveScene();
			string fps = "0";
			if (scene.name == "UE - 1") {
				fps = "35";
			}
			else if (scene.name == "UE - 2") {
				fps = "45";
			}
			else if (scene.name == "UE - 3") {
				fps = "20";
			}
			else if (scene.name == "UE - 4") {
				fps = "5";
			}
			else if (scene.name == "UE - 5") {
				fps = "25";
			}
			else if (scene.name == "UE - 6") {
				fps = "55";
			}
			else if (scene.name == "UE - 7") {
				fps = "40";
			}
			else if (scene.name == "UE - 8") {
				fps = "30";
			}
			else if (scene.name == "UE - 9") {
				fps = "10";
			}
			else if (scene.name == "UE - 10") {
				fps = "50";
			}
			else if (scene.name == "UE - 11") {
				fps = "60";
			}
			else if (scene.name == "UE - 12") {
				fps = "15";
			}

			// Logs the appropriate response.
			string path = Application.persistentDataPath + "Results.txt";
			string smooth = fps + " - Smooth";
			string laggy = fps + " - Laggy";
			if (_hit.transform.CompareTag("Smooth")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine(smooth);
				writer.Close();
			}
			else if (_hit.transform.CompareTag("Laggy")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine(laggy);
				writer.Close();
			}

			// Loads the next scene.
			if (scene.name == "UE - 1") {
				SceneManager.LoadScene("UE - 2");
			}
			else if (scene.name == "UE - 2") {
				SceneManager.LoadScene("UE - 3");
			}
			else if (scene.name == "UE - 3") {
				SceneManager.LoadScene("UE - 4");
			}
			else if (scene.name == "UE - 4") {
				SceneManager.LoadScene("UE - 5");
			}
			else if (scene.name == "UE - 5") {
				SceneManager.LoadScene("UE - 6");
			}
			else if (scene.name == "UE - 6") {
				SceneManager.LoadScene("UE - 7");
			}
			else if (scene.name == "UE - 7") {
				SceneManager.LoadScene("UE - 8");
			}
			else if (scene.name == "UE - 8") {
				SceneManager.LoadScene("UE - 9");
			}
			else if (scene.name == "UE - 9") {
				SceneManager.LoadScene("UE - 10");
			}
			else if (scene.name == "UE - 10") {
				SceneManager.LoadScene("UE - 11");
			}
			else if (scene.name == "UE - 11") {
				SceneManager.LoadScene("UE - 12");
			}
			else if (scene.name == "UE - 12") {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("END");
				writer.Close();
				SceneManager.LoadScene("UE - STOP");
			}
		}
	}

	// Executes when the reticle is on a target.
	public void GazeOn() {
		gazeStatus = true;
	}

	// Executes when the reticle is not on a target.
	public void GazeOff() {
		gazeStatus = false;
		gazeTime = 0;
		img.fillAmount = 0;
	}
}
