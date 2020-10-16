using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadTutorial()
    {
        FieldManager.start_time = Time.time;
        SceneManager.LoadScene(1);
    }
}
