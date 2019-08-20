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
	public GameObject s;
	public GameObject debug;
	public GameObject look1;
	public GameObject look2;
	public Image img;
	private RaycastHit _hit;

	bool gazeStatus = false;
	int sceneNumber = 0;
	int frameRate = 60;
	int direction = 0;
	int decided = 0;
	int zoom = 0;
	double logTime = 0.0;
	double gazeTime = 0.0;
	double decideTime = 0.0;
	double resetTime = 0.0;
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
	string path;
	string pathFPS;
	Scene scene;
	int[] order = {35, 45, 20, 31, 5, 25, 55, 32, 40, 30, 10, 33, 50, 60, 15, 34,
				   35, 50, 34, 25, 31, 40, 5, 45, 55, 32, 60, 30, 15, 33, 10, 20};

	// Start is called before the first frame update
	void Start() {

		// Initialises the user's ID.
		if (!(PlayerPrefs.GetInt("ID") >= 0)) {
			PlayerPrefs.SetInt("ID", 0);
		}

		// Gets the scene name.
		scene = SceneManager.GetActiveScene();

		// Defines where to save the results.
		path = Application.persistentDataPath + "Results.txt";
		pathFPS = Application.persistentDataPath + "FPSLog.txt";

		capOldTime = Time.realtimeSinceStartup;

		// Enables the FPS counter.
		debug.SetActive(false);
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
			n.GetComponent<TextMesh>().text = (32 - sceneNumber).ToString();

			// Gives the appearance of progression.
			if (zoom == 0) {
				n.GetComponent<TextMesh>().fontSize += 20;
				if (n.GetComponent<TextMesh>().fontSize == 150) {
					zoom = 1;
				}
			}
			else if (zoom == 1) {
				n.GetComponent<TextMesh>().fontSize -= 20;
				if (n.GetComponent<TextMesh>().fontSize == 50) {
					zoom = 2;
				}
			}
			else if (zoom == 2) {
				n.GetComponent<TextMesh>().characterSize = 0;
			}
		}

		// Busy waits to cap the FPS.
		capDeltaTime = 1.0 / frameRate;
		capCurrentTime = Time.realtimeSinceStartup;
		while ((capCurrentTime - capOldTime) < capDeltaTime) {
			capCurrentTime = Time.realtimeSinceStartup;
		}
		capOldTime = Time.realtimeSinceStartup;

		/************************* All of the below happens before the user makes a decision. *************************/
		if (decided == 0) {

			/************************* Waits for a decision. *************************/
			decideTime += Time.deltaTime;

			/************************* Gets the camera's in-game (not Euler!) rotation values. *************************/
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

			/************************* Measures the user's range of motion before they finish deciding. *************************/
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
			prevY = currentY;
			prevX = currentX;

			/************************* Moves the lion when appropriate. *************************/
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

			/************************* Logs the FPS every 100 ms. *************************/
			logTime += Time.deltaTime;
			if (logTime >= 0.1 && scene.name == "UE - SCENES") {
				logTime = 0;
				double currentFPS = Math.Round(1.0 / Time.unscaledDeltaTime, 2);
				StreamWriter writer = new StreamWriter(pathFPS, true);
				writer.Write(currentFPS);
				writer.Write("\t");
				writer.Close();
			}
		}

		// Allows the user to skip to the moving object test.
		if (scene.name == "UE - SCENES" && sceneNumber < 16) {
			s.SetActive(true);
		}
		else {
			s.SetActive(false);
		}

		// Once the user evaluation is completed, reset it after a few seconds.
		if (scene.name == "UE - STOP") {
			resetTime += Time.deltaTime;
			if (resetTime >= 10) {
				SceneManager.LoadScene("START");
			}
		}

		// Fills the reticle if it's on a target.
		if (gazeStatus == true) {
			gazeTime += Time.deltaTime;
			img.fillAmount = (float)(gazeTime / 1);
		}

		// Creates a ray.
		Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

		// Unfills the reticle when appropriate.
		if (Physics.Raycast(r, out _hit) == false || (_hit.transform.CompareTag("Untagged") == true && decided == 1)) {
			GazeOff();
		}

		/************************* Actions for different button presses. *************************/
		if (Physics.Raycast(r, out _hit) == true && img.fillAmount >= 1 && _hit.transform.CompareTag("Lion") == false) {

			if (_hit.transform.CompareTag("Start")) {
				PlayerPrefs.SetInt("ID", PlayerPrefs.GetInt("ID") + 1);
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("Started new user evaluation.");
				writer.Close();
				StreamWriter writerLog = new StreamWriter(pathFPS, true);
				writerLog.WriteLine("Started new user evaluation.");
				writerLog.Close();
				SceneManager.LoadScene("T - 1");
			}
			else if (_hit.transform.CompareTag("Reset")) {
				PlayerPrefs.SetInt("ID", PlayerPrefs.GetInt("ID") - 1);
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("\t");
				writer.WriteLine("User evaluation reset.");
				writer.Close();
				StreamWriter writerLog = new StreamWriter(pathFPS, true);
				writerLog.WriteLine("\t");
				writerLog.WriteLine("User evaluation reset.");
				writerLog.Close();
				SceneManager.LoadScene("START");
			}
			else if (_hit.transform.CompareTag("Skip")) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("\t");
				writer.WriteLine("Skipped to moving object test.");
				writer.Close();
				StreamWriter writerLog = new StreamWriter(pathFPS, true);
				writerLog.WriteLine("\t");
				writerLog.WriteLine("Skipped to moving object test.");
				writerLog.Close();
				sceneNumber = 16;
				decideTime = 0.0;
				maxLeft = 0.0;
				maxRight = 0.0;
				maxUp = 0.0;
				maxDown = 0.0;
				decided = 0;
				zoom = 0;
				a.SetActive(true);
				b.SetActive(true);
				look1.SetActive(false);
				look2.SetActive(false);
				n.GetComponent<TextMesh>().fontSize = 50;
				n.GetComponent<TextMesh>().characterSize = (float)0.1;
			}
			else {

				// The user has decided.
				decided = 1;
				a.SetActive(false);
				b.SetActive(false);
				look1.SetActive(true);
				look2.SetActive(true);
				l.transform.position = new Vector3(0, -1, 5);

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

				// Logs some metadata.
				if (scene.name == "UE - SCENES") {
					StreamWriter writerExtra = new StreamWriter(path, true);
					writerExtra.Write(PlayerPrefs.GetInt("ID").ToString() + "\t");
					writerExtra.Close();
					if (sceneNumber < 16) {
						StreamWriter writer = new StreamWriter(path, true);
						writer.Write("Static\t");
						writer.Close();
					}
					else {
						StreamWriter writer = new StreamWriter(path, true);
						writer.Write("Dynamic\t");
						writer.Close();
					}
				}

				// Logs the decision.
				if (_hit.transform.CompareTag("Smooth")) {
					string smooth = fps + "\tSmooth\t";
					StreamWriter writer = new StreamWriter(path, true);
					writer.Write(smooth);
					writer.Close();
				}
				else if (_hit.transform.CompareTag("Laggy")) {
					string laggy = fps + "\tLaggy\t";
					StreamWriter writer = new StreamWriter(path, true);
					writer.Write(laggy);
					writer.Close();
				}

				// Finds and logs the current FPS + other stuff.
				if (scene.name == "UE - SCENES") {
					double currentFPS = Math.Round(1.0 / Time.unscaledDeltaTime, 2);
					StreamWriter writer = new StreamWriter(path, true);

					writer.Write(Math.Round(decideTime - 1, 2));
					writer.Write("\t");
					decideTime = 0.0;

					writer.Write(Math.Round(maxLeft, 2));
					writer.Write("\t");
					maxLeft = 0.0;

					writer.Write(Math.Round(maxRight, 2));
					writer.Write("\t");
					maxRight = 0.0;

					writer.Write(Math.Round(maxUp, 2));
					writer.Write("\t");
					maxUp = 0.0;

					writer.WriteLine(Math.Round(maxDown, 2));
					maxDown = 0.0;
					writer.Close();

					StreamWriter writerLog = new StreamWriter(pathFPS, true);
					writerLog.WriteLine(currentFPS);
					writerLog.Close();
				}
			}
		}

		/************************* Actions once the user has decided and looked at the lion. *************************/
		else if (Physics.Raycast(r, out _hit) == true && _hit.transform.CompareTag("Lion") && decided == 1) {

			// Readies the next FPS test.
			decided = 0;
			zoom = 0;
			a.SetActive(true);
			b.SetActive(true);
			look1.SetActive(false);
			look2.SetActive(false);
			++sceneNumber;

			// Resets rotation values.
			if (c.transform.eulerAngles.y > 180) {
				prevY = c.transform.eulerAngles.y - 360;
			}
			else {
				prevY = c.transform.eulerAngles.y;
			}
			if (c.transform.eulerAngles.x > 180) {
				prevX = c.transform.eulerAngles.x - 360;
			}
			else {
				prevX = c.transform.eulerAngles.x;
			}

			// Resets the text's font size.
			if (scene.name == "UE - SCENES") {
				n.GetComponent<TextMesh>().fontSize = 50;
				n.GetComponent<TextMesh>().characterSize = (float)0.1;
			}

			// Loads the next scene when appropriate.
			if (scene.name == "T - 1") {
				SceneManager.LoadScene("T - 2");
			}
			else if (scene.name == "T - 2") {
				SceneManager.LoadScene("T - 3");
			}
			else if (scene.name == "T - 3") {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("User ID\tTest Type\tSet FPS\tUser Decision\tTime to Decide (s)\tMax Left Rotation (deg)\tMax Right Rotation (deg)\tMax Up Rotation (deg)\tMax Down Rotation (deg)");
				writer.Close();
				StreamWriter writerLog = new StreamWriter(pathFPS, true);
				writerLog.WriteLine("Actual FPS until Decision (every 100 ms)");
				writerLog.Close();
				SceneManager.LoadScene("UE - SCENES");
			}
			else if (scene.name == "UE - SCENES" && sceneNumber == 32) {
				StreamWriter writer = new StreamWriter(path, true);
				writer.WriteLine("User evaluation completed.");
				writer.Close();
				StreamWriter writerLog = new StreamWriter(pathFPS, true);
				writerLog.WriteLine("User evaluation completed.");
				writerLog.Close();
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
