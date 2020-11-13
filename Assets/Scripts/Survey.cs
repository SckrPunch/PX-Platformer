using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Survey : MonoBehaviour
{
    public GameObject[] sections;
    public GameObject activeSection;
    public GameObject previousSection;
    public GameObject nextSection;
    public GameObject activeSubsection;
    public GameObject nextSubsection;
    public GameObject previousSubsection;
    public GameObject errorMessage;
    public int gameType;

    public List<GameObject> subSections = new List<GameObject>();
    private List<GameObject> dropdowns = new List<GameObject>();
    private List<GameObject> textInput = new List<GameObject>();

    private List<int> answersSection1_1 = new List<int>();
    private List<int> answersSection1_2 = new List<int>();
    private List<int> answersSection1_3 = new List<int>();
    private List<int> answersSection1_4 = new List<int>();
    private List<int> answersSection1_5 = new List<int>();
    private List<int> answersSection1_6 = new List<int>();
    private List<int> answersSection2_1_dropdown = new List<int>();
    private List<string> answersSection2_1_text = new List<string>();
    private List<string> answersSection2_2 = new List<string>();
    private List<string> answersSection3_1 = new List<string>();

    private List<int> answersCompleteS1 = new List<int>();
    private List<int> answersCompleteS2_dropdown = new List<int>();
    private List<string> answersCompleteS2_text = new List<string>();
    private List<string> answersCompleteS3 = new List<string>();
    private int subsectionCounter;
    private int sectionCounter;

    //survey answer placeholders
    private List<int> answersPlaceholder = new List<int>();
    private List<string> textPlaceholder = new List<string>();

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScKfsyKJy9iI_dbBuRMUvbPMPz42CySfYvakDMu2djr3nNJyg/formResponse";

    // Update is called once per frame
    void Update()
    {
        sectionCounter = 0;
        subsectionCounter = 0;
        subSections.Clear();
        dropdowns.Clear();
        textInput.Clear();

        //get all subsections in section
        foreach (var section in sections)
        {
            sectionCounter++;

            if(section.activeSelf)
            {
                activeSection = section;

                if(sectionCounter != sections.Length)
                {
                    nextSection = sections[sectionCounter];
                }

                foreach (Transform child in activeSection.transform)
                {

                    if (child.gameObject.tag == "SurveySubsection" && !subSections.Contains(child.gameObject))
                    {
                        subSections.Add(child.gameObject);
                    }
                }
            }
        }

        foreach (var obj in subSections)
        {
            subsectionCounter++;

            if (obj.activeSelf)
            {
                activeSubsection = obj;

                if(subsectionCounter != subSections.Count && subsectionCounter != 1)
                {
                    nextSubsection = subSections[subsectionCounter];
                    previousSubsection = subSections[subsectionCounter - 2];
                }
                else if(subsectionCounter == subSections.Count && subsectionCounter != 1)
                {
                    nextSubsection = subSections[subsectionCounter - 1];
                    previousSubsection = subSections[subsectionCounter - 2];
                }
                else if(subsectionCounter == subSections.Count && subsectionCounter == 1)
                {
                    nextSubsection = subSections[subsectionCounter - 1];
                    previousSubsection = subSections[subsectionCounter - 1];
                }
                else
                {
                    nextSubsection = subSections[subsectionCounter];
                    previousSubsection = subSections[subsectionCounter - 1];
                }

                foreach (Transform child in activeSubsection.transform)
                {
                    if(child.gameObject.tag == "Dropdown" && !dropdowns.Contains(child.gameObject))
                    {
                        dropdowns.Add(child.gameObject);
                    }
                    if(child.gameObject.tag == "TextInput" && !textInput.Contains(child.gameObject))
                    {
                        textInput.Add(child.gameObject);
                    }
                    if(child.gameObject.tag == "Error")
                    {
                        errorMessage = child.gameObject;
                    }
                }
                break;
            }
        }
    }

    public void CheckDropdownValues()
    {
        bool isError = false;

        for (int i = 0; i < dropdowns.Count; i++)
        {
            if(dropdowns[i].gameObject.GetComponent<TMP_Dropdown>().value == 0)
            {
                isError = true;
                errorMessage.SetActive(true);
            }
        }

        for (int i = 0; i < textInput.Count; i++)
        {
            if(textInput[i].gameObject.GetComponent<TMP_InputField>().text == "")
            {
                isError = true;
                errorMessage.SetActive(true);
            }
        }

        if(!isError)
        {
            SaveDropdownValues(activeSubsection.name);

            previousSubsection = activeSubsection;
            activeSubsection.SetActive(false);
            nextSubsection.SetActive(true);
        }

    }

    public void GoToNextSection()
    {
        SaveDropdownValues(activeSubsection.name);

        previousSection = activeSection;

        activeSection.SetActive(false);
        nextSection.SetActive(true);
    }

    public void GoToPreviousSection()
    {
        activeSection.SetActive(false);
        previousSection.SetActive(true);

        activeSection = previousSection;
    }

    public void GetPreviousPage()
    {
        activeSubsection.SetActive(false);
        previousSubsection.SetActive(true);
    }

    public void QuitGame()
    {
        answersCompleteS1.AddRange(answersSection1_1);
        answersCompleteS1.AddRange(answersSection1_2);
        answersCompleteS1.AddRange(answersSection1_3);
        answersCompleteS1.AddRange(answersSection1_4);
        answersCompleteS1.AddRange(answersSection1_5);
        answersCompleteS1.AddRange(answersSection1_6);

        //1 = Positive 2 = Neutral 3 = Negative
        gameType = 1;

        answersCompleteS2_dropdown.AddRange(answersSection2_1_dropdown);
        answersCompleteS2_text.AddRange(answersSection2_1_text);
        answersCompleteS2_text.AddRange(answersSection2_2);

        answersCompleteS3.AddRange(answersSection3_1);

        StartCoroutine(Post(answersCompleteS1, answersCompleteS2_dropdown, answersCompleteS2_text, answersCompleteS3, gameType));

        Application.Quit();

        Debug.Log("Section 1:" + answersCompleteS1.Count);
        Debug.Log("Section 2 dd:" + answersCompleteS2_dropdown.Count);
        Debug.Log("Section 2 text:" + answersCompleteS2_text.Count);
        Debug.Log("Section 3:" + answersCompleteS3.Count);
    }

    IEnumerator Post(List<int> answersCompleteS1, List<int> answersCompleteS2_dropdown, List<string> answersCompleteS2_text, List<string> answersCompleteS3, int gameType)
    {
        WWWForm form = new WWWForm();

        //section 1
        form.AddField("entry.1646509452", answersCompleteS1[0]);
        form.AddField("entry.123365821", answersCompleteS1[1]);
        form.AddField("entry.288170302", answersCompleteS1[2]);
        form.AddField("entry.1145481869", answersCompleteS1[3]);
        form.AddField("entry.430175792", answersCompleteS1[4]);
        form.AddField("entry.1835718405", answersCompleteS1[5]);
        form.AddField("entry.510499444", answersCompleteS1[6]);
        form.AddField("entry.779086878", answersCompleteS1[7]);
        form.AddField("entry.1784684340", answersCompleteS1[8]);
        form.AddField("entry.730286654", answersCompleteS1[9]);
        form.AddField("entry.1449589420", answersCompleteS1[10]);
        form.AddField("entry.1097438337", answersCompleteS1[11]);
        form.AddField("entry.2000936805", answersCompleteS1[12]);
        form.AddField("entry.831843712", answersCompleteS1[13]);
        form.AddField("entry.1355193469", answersCompleteS1[14]);
        form.AddField("entry.118332121", answersCompleteS1[15]);
        form.AddField("entry.1764540697", answersCompleteS1[16]);
        form.AddField("entry.1444006081", answersCompleteS1[17]);
        form.AddField("entry.590697014", answersCompleteS1[18]);
        form.AddField("entry.1745510518", answersCompleteS1[19]);
        form.AddField("entry.1117823425", answersCompleteS1[20]);
        form.AddField("entry.130668445", answersCompleteS1[21]);
        form.AddField("entry.969365196", answersCompleteS1[22]);
        form.AddField("entry.1366294132", answersCompleteS1[23]);
        form.AddField("entry.1068501694", answersCompleteS1[24]);
        form.AddField("entry.1074549080", answersCompleteS1[25]);
        form.AddField("entry.1386598348", answersCompleteS1[26]);
        form.AddField("entry.2096109895", answersCompleteS1[27]);
        form.AddField("entry.1467121606", answersCompleteS1[28]);
        form.AddField("entry.1140242539", answersCompleteS1[29]);

        // WWWForm form1 = new WWWForm();
        //Section 2
        form.AddField("entry.616429835", answersCompleteS2_dropdown[0]);
        form.AddField("entry.285754025", answersCompleteS2_text[0]);
        form.AddField("entry.447925714", answersCompleteS2_text[1]);
        form.AddField("entry.170127727", answersCompleteS2_text[2]);
        form.AddField("entry.386573068", answersCompleteS2_text[3]);
        form.AddField("entry.1131524279", answersCompleteS2_text[4]);
        form.AddField("entry.1488604533", answersCompleteS2_text[5]);
        form.AddField("entry.1477548232", answersCompleteS2_text[6]);

        //Section 3
        form.AddField("entry.1050806395", answersCompleteS3[0]);
        form.AddField("entry.995138048", answersCompleteS3[1]);
        form.AddField("entry.1597929298", answersCompleteS3[2]);


        form.AddField("entry.2110178887", gameType);



        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;

    }

    private void SaveDropdownValues(string section)
    {
        answersPlaceholder.Clear();
        textPlaceholder.Clear();

        foreach (var dropdown in dropdowns)
        {
            answersPlaceholder.Add(dropdown.gameObject.GetComponent<TMP_Dropdown>().value);
        }

        foreach (var input in textInput)
        {
            textPlaceholder.Add(input.gameObject.GetComponent<TMP_InputField>().text);
        }
        
        switch (section)
        {
            case "Section1-1":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection1_1.Add(answer);
                }
                break;
            case "Section1-2":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection1_2.Add(answer);
                }
                break;
            case "Section1-3":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection1_3.Add(answer);
                }
                break;
            case "Section1-4":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection1_4.Add(answer);
                }
                break;
            case "Section1-5":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection1_5.Add(answer);
                }
                break;
            case "Section1-6":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection1_6.Add(answer);
                }
                break;
            case "Section2-1":
                foreach (var answer in answersPlaceholder)
                {
                    answersSection2_1_dropdown.Add(answer);
                }
                foreach (var answer in textPlaceholder)
                {
                    answersSection2_1_text.Add(answer);
                }
                break;
            case "Section2-2":
                foreach (var answer in textPlaceholder)
                {
                    answersSection2_2.Add(answer);
                }
                break;
            case "Section3-1":
                foreach (var answer in textPlaceholder)
                {
                    answersSection3_1.Add(answer);
                }
                break;
            default:
                break;
        }
    }

}
