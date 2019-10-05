using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighFPSTest : MonoBehaviour {

	double set_time = 180.0;
	double timer = 180.0;
	double logTime = 0.0;
	double capOldTime = 0.0;
	double capDeltaTime = 0.0;
	double capCurrentTime = 0.0;
	int index = 0;
	// double[] order = {55.0, 56.0, 57.0, 58.0, 59.0, 60.0};
	double[] order = {61.0, 62.0, 63.0, 64.0, 65.0, 60.0};

	// Start is called before the first frame update
	void Start() {
		capOldTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update() {

		double frameRate = order[index];

		// Busy waits to cap the FPS.
		if (index == 0 || index == 1 || index == 2 || index == 3 || index == 4) {
			capDeltaTime = 1.0 / frameRate;
			capCurrentTime = Time.realtimeSinceStartup;
			while ((capCurrentTime - capOldTime) < capDeltaTime) {
				capCurrentTime = Time.realtimeSinceStartup;
			}
			capOldTime = Time.realtimeSinceStartup;
		}

		// Logs the FPS every 100 ms.
		logTime += Time.deltaTime;
		if (logTime >= 0.1) {
			logTime = 0;
			double currentFPS = Math.Round(1.0 / Time.unscaledDeltaTime, 2);
			StreamWriter writer = new StreamWriter(Application.persistentDataPath + "Results.txt", true);
			writer.Write(currentFPS + "\t");
			writer.Close();
		}

		// Waits for 20 seconds before advancing to the next FPS.
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			StreamWriter writerE = new StreamWriter(Application.persistentDataPath + "Results.txt", true);
			writerE.WriteLine("");
			writerE.Close();
			
			++index;
			
			if (index == 6) {
				SceneManager.LoadScene("STOP");
			}

			// Resets the timer.
			timer = set_time;
		}
	}
}
