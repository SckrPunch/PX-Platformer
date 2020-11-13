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

        Application.Quit();

        Debug.Log("Section 1:" + answersCompleteS1.Count);
        Debug.Log("Section 2 dd:" + answersCompleteS2_dropdown.Count);
        Debug.Log("Section 2 text:" + answersCompleteS2_text.Count);
        Debug.Log("Section 3:" + answersCompleteS3.Count);
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
