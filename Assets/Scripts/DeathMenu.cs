using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Unity;
using Newtonsoft.Json;
using TMPro;

public class DeathMenu : MonoBehaviour
{
    public int time_played;
    public int total_retries;
    public List<DataJsonFormat> jsonObj_ = new List<DataJsonFormat>();
    public int count_obj = 0;
    public List<string> positiveLines = new List<string>();
    public List<string> negativeLines = new List<string>();
    public GameObject deathText;
    public GameObject neutralText;

    //set game type 1 = positive 2 = neutral 3 = negative
    public int gameType = 3;

    private int lineSelection;

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeH4h69c9RlCvsGBObJf8GHh9Bczn6H6ggwIioy9NfCIP1W-w/formResponse";

    private void Awake()
    {
        positiveLines.Add("'I have not failed.I have found 10, 000 ways that don’t work.' — Thomas Edison");
        positiveLines.Add("'Giving up is the only sure way to fail.' ― Gena Showalter");
        positiveLines.Add("'The one who falls and gets up is stronger than the one who never tried.' ― Roy T. Bennett");
        positiveLines.Add("'Failure happens all the time.What makes you better is how you react to it.' ― Mia Hamm");
        positiveLines.Add("'Failure is only the opportunity to more intelligently begin again.' ― Henry Ford");

        negativeLines.Add("What's wrong with you?");
        negativeLines.Add("This game is supposed to be easy...");
        negativeLines.Add("Ha ha! You died!");
        negativeLines.Add("Don't do that next time.");
        negativeLines.Add("I don't think that you play many video games...");
    }

    private void OnEnable()
    {
        lineSelection = UnityEngine.Random.Range(0, 4);

        switch (gameType)
        {
            case 1:
                deathText.GetComponent<TMP_Text>().SetText(positiveLines[lineSelection]);
                break;
            case 2:
                deathText.SetActive(false);
                neutralText.SetActive(true);
                break;
            case 3:
                deathText.GetComponent<TMP_Text>().SetText(negativeLines[lineSelection]);
                break;
            default:
                break;
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        int scene_num = scene.buildIndex;

        if(scene_num == 1)
        {
            FieldManager.retry_tutorial++;
        }
        else
        {
            FieldManager.retry_level++;
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

    public void GoToSurvey()
    {
        Scene scene = SceneManager.GetActiveScene();
        int scene_num = scene.buildIndex;
        SceneManager.LoadScene(3);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
