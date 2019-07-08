using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class FPSCounter : MonoBehaviour {

	public double FPS { get; private set; }
	
	void Update () {
		Scene scene = SceneManager.GetActiveScene();
		
		FPS = (double)(1f / Time.unscaledDeltaTime);
		FPS = Math.Round((double)FPS, 2);
		
		string path = Application.persistentDataPath + scene.name + ".txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(FPS);
        writer.Close();
		
		Console.WriteLine(Application.persistentDataPath);
	}
}