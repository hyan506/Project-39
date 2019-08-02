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
	public GameObject c;
	public GameObject l;
	public GameObject n;
	public GameObject debug;
	public Image img;
	private RaycastHit _hit;
	bool gazeStatus = false;
	int sceneNumber = 0;
	int frameRate = 60;
	int direction = 0;
	double gazeTime = 0.0;
	double decideTime = 0.0;
	double waitOldTime = 0.0;
	double capOldTime = 0.0;
	double capDeltaTime = 0.0;
	double capCurrentTime = 0.0;
	double currentY = 0.0;
	double currentX = 0.0;
	double prevY = 0.0;
	double prevX = 0.0;
	double currentLeft = 0.0;
	double currentRight = 0.0;
	double currentUp = 0.0;
	double currentDown = 0.0;
	double maxLeft = 0.0;
	double maxRight = 0.0;
	double maxUp = 0.0;
	double maxDown = 0.0;
	string fps = "60";
	Scene scene;
	int[] order = {35, 45, 20, 31, 5, 25, 55, 32, 40, 30, 10, 33, 50, 60, 15, 34,
				   35, 50, 34, 25, 31, 40, 5, 45, 55, 32, 60, 30, 15, 33, 10, 20};

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
		else if (scene.name == "UE - STOP") {
			frameRate = 60;
		}
		else {
			frameRate = order[sceneNumber];
			fps = frameRate.ToString();
			n.GetComponent<TextMesh>().text = "Scenes Left: " + (32 - sceneNumber);
		}

		// Busy waits to cap the FPS.
		capDeltaTime = 1.0 / frameRate;
		capCurrentTime = Time.realtimeSinceStartup;
		while ((capCurrentTime - capOldTime) < capDeltaTime) {
			capCurrentTime = Time.realtimeSinceStartup;
		}
		capOldTime = Time.realtimeSinceStartup;

		// Waits for a decision.
		decideTime += Time.deltaTime;

		// Gets the camera's in-game (not Euler!) rotation values.
		if (c.transform.eulerAngles.y > 180) {
			currentY = c.transform.eulerAngles.y - 360;
		}
		else {
			currentY = c.transform.eulerAngles.y;
		}
		if (c.transform.eulerAngles.x > 180) {
			currentX = c.transform.eulerAngles.x - 360;
		}
		else {
			currentX = c.transform.eulerAngles.x;
		}

		// Measures the user's range of motion before the buttons appear.
		if (a.activeSelf == false && b.activeSelf == false) {

			// The user is currently looking left.
			if (currentY < prevY) {

				// This means that the user has stopped looking right. Update the max right value and reset.
				if (currentRight > maxRight) {
					maxRight = currentRight;
				}
				currentRight = 0;

				// Updates the current left value.
				currentLeft += Math.Abs(currentY - prevY);
			}

			// The user is currently looking right.
			else if (currentY > prevY) {

				// This means that the user has stopped looking left. Update the max left value and reset.
				if (currentLeft > maxLeft) {
					maxLeft = currentLeft;
				}
				currentLeft = 0;

				// Updates the current right value.
				currentRight += Math.Abs(currentY - prevY);
			}

			// The user is currently looking up.
			if (currentX < prevX) {

				// This means that the user has stopped looking down. Update the max down value and reset.
				if (currentDown > maxDown) {
					maxDown = currentDown;
				}
				currentDown = 0;

				// Updates the current up value.
				currentUp += Math.Abs(currentX - prevX);
			}

			// The user is currently looking down.
			else if (currentX > prevX) {

				// This means that the user has stopped looking up. Update the max up value and reset.
				if (currentUp > maxUp) {
					maxUp = currentUp;
				}
				currentUp = 0;

				// Updates the current down value.
				currentDown += Math.Abs(currentX - prevX);
			}
		}
		prevY = currentY;
		prevX = currentX;

		// Moves the lion when appropriate.
		if (sceneNumber >= 16) {
			l.transform.rotation = Quaternion.Euler(0, 180, 0);

			// Moves the lion.
			if (direction == 0) {
				l.transform.Translate((float)0.05, 0, 0);
			}
			else if (direction == 1) {
				l.transform.Translate((float)-0.05, 0, 0);
			}

			// Changes the lion's direction.
			if (l.transform.position.x <= -1.5) {
				direction = 1;
			}
			else if (l.transform.position.x >= 1.5) {
				direction = 0;
			}
		}

		// Waits for a few seconds before the buttons are revealed.
		if (Time.realtimeSinceStartup - waitOldTime >= 5) {
			a.SetActive(true);
			b.SetActive(true);

			// Measures the user's range of motion one last time.
			if (currentRight > maxRight) {
				maxRight = currentRight;
			}
			currentRight = 0;
			if (currentLeft > maxLeft) {
				maxLeft = currentLeft;
			}
			currentLeft = 0;
			if (currentDown > maxDown) {
				maxDown = currentDown;
			}
			currentDown = 0;
			if (currentUp > maxUp) {
				maxUp = currentUp;
			}
			currentUp = 0;
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
			//string path = Application.persistentDataPath + "Results.txt";
			string path = "C:/Users/panca/Desktop/Results.txt";
			string smooth = fps + " - Smooth\t\t";
			string laggy = fps + " - Laggy\t\t";

			if (_hit.transform.CompareTag("Start")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("New user evaluation started.");
				writer.Close();
			}
			else if (_hit.transform.CompareTag("Smooth")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.Write(smooth);
				writer.Close();
			}
			else if (_hit.transform.CompareTag("Laggy")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.Write(laggy);
				writer.Close();
			}

			// Finds and logs the current FPS.
			double currentFPS = Math.Round(1.0 / Time.unscaledDeltaTime, 2);
			StreamWriter writerExtra = new StreamWriter(path, true);

			writerExtra.Write("(Actual FPS: ");
			writerExtra.Write(currentFPS);

			writerExtra.Write("\t\tTime to Decide: ");
			writerExtra.Write(Math.Round(decideTime - 6, 2));
			decideTime = 0.0;

			writerExtra.Write("\t\tMax Left: ");
			writerExtra.Write(Math.Round(maxLeft, 2));
			maxLeft = 0.0;

			writerExtra.Write("\t\tMax Right: ");
			writerExtra.Write(Math.Round(maxRight, 2));
			maxRight = 0.0;

			writerExtra.Write("\t\tMax Up: ");
			writerExtra.Write(Math.Round(maxUp, 2));
			maxUp = 0.0;

			writerExtra.Write("\t\tMax Down: ");
			writerExtra.Write(Math.Round(maxDown, 2));
			maxDown = 0.0;
			writerExtra.WriteLine(")");
			writerExtra.Close();

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
