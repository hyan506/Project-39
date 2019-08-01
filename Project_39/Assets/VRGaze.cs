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
	public GameObject n;
	public GameObject debug;
	public Image img;
	private RaycastHit _hit;
	bool gazeStatus = false;
	int sceneNumber = 1;
	int frameRate = 60;
	double gazeTime = 0.0;
	double waitOldTime = 0.0;
	double capOldTime = 0.0;
	double capDeltaTime = 0.0;
	double capCurrentTime = 0.0;
	string fps = "60";
	Scene scene;

	// Start is called before the first frame update
	void Start() {

		// Gets the scene name.
		scene = SceneManager.GetActiveScene();

		waitOldTime = Time.realtimeSinceStartup;
		capOldTime = Time.realtimeSinceStartup;

		// Enables the FPS counter.
		debug.SetActive(true);
	}

	// Update is called once per frame
	void Update() {

		// Determines metadata by checking the scene number.
		if (scene.name == "START") {
			frameRate = 60;
		}
		else if (scene.name == "T - 1") {
			frameRate = 60;
		}
		else if (scene.name == "T - 2") {
			frameRate = 15;
		}
		else if (scene.name == "T - 3") {
			frameRate = 30;
		}
		else if (sceneNumber == 1) {
			fps = "35";
			frameRate = 35;
			n.GetComponent<TextMesh>().text = "Scenes Left: 16";
		}
		else if (sceneNumber == 2) {
			fps = "45";
			frameRate = 45;
			n.GetComponent<TextMesh>().text = "Scenes Left: 15";
		}
		else if (sceneNumber == 3) {
			fps = "20";
			frameRate = 20;
			n.GetComponent<TextMesh>().text = "Scenes Left: 14";
		}
		else if (sceneNumber == 4) {
			fps = "31";
			frameRate = 31;
			n.GetComponent<TextMesh>().text = "Scenes Left: 13";
		}
		else if (sceneNumber == 5) {
			fps = "5";
			frameRate = 5;
			n.GetComponent<TextMesh>().text = "Scenes Left: 12";
		}
		else if (sceneNumber == 6) {
			fps = "25";
			frameRate = 25;
			n.GetComponent<TextMesh>().text = "Scenes Left: 11";
		}
		else if (sceneNumber == 7) {
			fps = "55";
			frameRate = 55;
			n.GetComponent<TextMesh>().text = "Scenes Left: 10";
		}
		else if (sceneNumber == 8) {
			fps = "32";
			frameRate = 32;
			n.GetComponent<TextMesh>().text = "Scenes Left: 9";
		}
		else if (sceneNumber == 9) {
			fps = "40";
			frameRate = 40;
			n.GetComponent<TextMesh>().text = "Scenes Left: 8";
		}
		else if (sceneNumber == 10) {
			fps = "30";
			frameRate = 30;
			n.GetComponent<TextMesh>().text = "Scenes Left: 7";
		}
		else if (sceneNumber == 11) {
			fps = "10";
			frameRate = 10;
			n.GetComponent<TextMesh>().text = "Scenes Left: 6";
		}
		else if (sceneNumber == 12) {
			fps = "33";
			frameRate = 33;
			n.GetComponent<TextMesh>().text = "Scenes Left: 5";
		}
		else if (sceneNumber == 13) {
			fps = "50";
			frameRate = 50;
			n.GetComponent<TextMesh>().text = "Scenes Left: 4";
		}
		else if (sceneNumber == 14) {
			fps = "60";
			frameRate = 60;
			n.GetComponent<TextMesh>().text = "Scenes Left: 3";
		}
		else if (sceneNumber == 15) {
			fps = "15";
			frameRate = 15;
			n.GetComponent<TextMesh>().text = "Scenes Left: 2";
		}
		else if (sceneNumber == 16) {
			fps = "34";
			frameRate = 34;
			n.GetComponent<TextMesh>().text = "Scenes Left: 1";
		}
		else {
			frameRate = 60;
		}

		// Busy waits to cap the FPS.
		capDeltaTime = 1.0 / frameRate;
		capCurrentTime = Time.realtimeSinceStartup;

		while ((capCurrentTime - capOldTime) < capDeltaTime) {
			capCurrentTime = Time.realtimeSinceStartup;
		}

		capOldTime = Time.realtimeSinceStartup;

		// Waits for a few seconds before the buttons are revealed.
		if (Time.realtimeSinceStartup - waitOldTime >= 5) {
			a.SetActive(true);
			b.SetActive(true);
		}

		// Fills the reticle if it's on a target.
		if (gazeStatus == true) {
			gazeTime += Time.deltaTime;
			img.fillAmount = (float)(gazeTime / 1);
		}

		// Creates a ray.
		Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

		// Logs the appropriate response and loads the next scene.
		if (Physics.Raycast(r, out _hit) == true && img.fillAmount >= 1) {

			a.SetActive(false);
			b.SetActive(false);
			waitOldTime = Time.realtimeSinceStartup;
			++sceneNumber;

			// Logs the appropriate response.
			string path = Application.persistentDataPath + "Results.txt";
			string smooth = fps + " - Smooth";
			string laggy = fps + " - Laggy";
			if (_hit.transform.CompareTag("Start")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("New user evaluation started.");
				writer.Close();
			}
			else if (_hit.transform.CompareTag("Smooth")) {
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
			if (scene.name == "START") {
				SceneManager.LoadScene("T - 1");
			}
			else if (scene.name == "T - 1") {
				SceneManager.LoadScene("T - 2");
			}
			else if (scene.name == "T - 2") {
				SceneManager.LoadScene("T - 3");
			}
			else if (scene.name == "T - 3") {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("Tutorial completed.");
				writer.Close();
				SceneManager.LoadScene("UE - SCENES");
			}
			/*else if (scene.name == "UE - 5") {
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
				SceneManager.LoadScene("UE - 13");
			}
			else if (scene.name == "UE - 13") {
				SceneManager.LoadScene("UE - 14");
			}
			else if (scene.name == "UE - 14") {
				SceneManager.LoadScene("UE - 15");
			}
			else if (scene.name == "UE - 15") {
				SceneManager.LoadScene("UE - 16");
			}*/
			else if (sceneNumber == 17) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("END");
				writer.Close();
				SceneManager.LoadScene("UE - STOP");
			}
		}
		else if (Physics.Raycast(r, out _hit) == false) {
			GazeOff();
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
