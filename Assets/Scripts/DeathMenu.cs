using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Unity;
using Newtonsoft.Json;

public class DeathMenu : MonoBehaviour
{
    public int time_played;
    public int total_retries;
    public List<DataJsonFormat> jsonObj_ = new List<DataJsonFormat>();
    public int count_obj = 0;

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

    public void GetDataFields()
    {
        float end_time = Time.time;

        //data
        time_played = Mathf.RoundToInt(end_time - FieldManager.start_time);
        total_retries = FieldManager.retry_tutorial + FieldManager.retry_level;

        //start json
        jsonObj_.Add(new DataJsonFormat());
        jsonObj_[count_obj].time_taken = time_played;
        jsonObj_[count_obj].No_of_retries = total_retries;
        jsonObj_[count_obj].retry_level = FieldManager.retry_level;
        jsonObj_[count_obj].retry_tutorial = FieldManager.retry_tutorial;

        count_obj++;

        string json = JsonConvert.SerializeObject(jsonObj_);
        //end json

        WriteJSON(json);
    }

    public void WriteJSON(string json_)
    {
        string dir = Application.streamingAssetsPath + "/Data/JsonOutput";
       
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string output_file = dir + "/" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".json";
        File.WriteAllText(output_file, json_);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
