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

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeH4h69c9RlCvsGBObJf8GHh9Bczn6H6ggwIioy9NfCIP1W-w/formResponse";

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

        StartCoroutine(Post(time_played, total_retries, FieldManager.retry_level, FieldManager.retry_tutorial));

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

    IEnumerator Post(int time_played, int total_retries, int retry_level, int retry_tutorial)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1515374140", time_played);
        form.AddField("entry.1367920399", total_retries);
        form.AddField("entry.605934596", retry_tutorial);
        form.AddField("entry.1277183444", retry_level);

        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;

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
