using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public int time_played;
    public int total_retries;
    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        int scene_num = scene.buildIndex;

        if(scene_num == 1)
        {
            FieldManager.retry_tutorial++;
            Debug.Log(FieldManager.retry_tutorial);
        }
        else
        {
            FieldManager.retry_level++;
            Debug.Log(FieldManager.retry_level);
        }

        SceneManager.LoadScene(scene_num);
    }

    public void QuitGame()
    {
        float end_time = Time.time;

        //data
        time_played = Mathf.RoundToInt(end_time - FieldManager.start_time);
        total_retries = FieldManager.retry_tutorial + FieldManager.retry_level;
        //GameObject.FindObjectOfType<JsonConverter>().collect();
        Debug.Log(total_retries);
        Application.Quit();
    }
}
