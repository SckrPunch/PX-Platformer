using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Unity;

//download the .dll from https://github.com/JamesNK/Newtonsoft.Json/releases and add it to the references.
//better to use JsonConvert.SerializeObject instead of JsonUtility when nested jsons are being made 
//you can also just use Jsonutility that comes with unity if you have a simple json
using Newtonsoft.Json;

public class JsonConverter : MonoBehaviour
{
    public List<DataJsonFormat> jsonObj_ = new List<DataJsonFormat>();
    public int count_obj = 0;


    public void collect()
    {
        jsonObj_.Add(new DataJsonFormat());
        Debug.Log(GameObject.FindObjectOfType<DeathMenu>().time_played);
        Debug.Log(GameObject.FindObjectOfType<DeathMenu>().total_retries);
        jsonObj_[count_obj].time_taken = GameObject.FindObjectOfType<DeathMenu>().time_played;
        jsonObj_[count_obj].No_of_retries = GameObject.FindObjectOfType<DeathMenu>().total_retries;
        jsonObj_[count_obj].retry_level = FieldManager.retry_level;
        jsonObj_[count_obj].retry_tutorial = FieldManager.retry_tutorial;

        count_obj++;
        //string json = JsonUtility.ToJson(jsonObj_);

        string json = JsonConvert.SerializeObject(jsonObj_);

        writeout(json);
    }

    void writeout(string json_)
    {
        string dir = Application.streamingAssetsPath + "/Data/JsonOutput";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string output_file = dir + "/" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".json";
        File.WriteAllText(output_file, json_);
    }
}
