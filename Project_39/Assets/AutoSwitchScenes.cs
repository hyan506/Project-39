using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutoSwitchScenes : MonoBehaviour
{
	public string LevelToLoad;
	private float timer = 20f;
	private Text timerSeconds;
	
    // Start is called before the first frame update
    void Start()
    {
        timerSeconds = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
		timerSeconds.text = "Time Left: " + timer.ToString("f0");
		if (timer <= 0)
		{
			SceneManager.LoadScene(LevelToLoad);
			
			// Resets the timer.
			timer = 20f;
		}
    }
}
