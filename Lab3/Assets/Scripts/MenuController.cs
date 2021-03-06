using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 0.0f;
    }

    void Start()
    {
        foreach (Transform eachChild in transform)
        {
            if (eachChild.name != "Score")
            {
                Debug.Log("Child found. Name: " + eachChild.name);
                // disable them
                eachChild.gameObject.SetActive(true);
                Time.timeScale = 1.0f;
            }
        }
    }

    public void StartButtonClicked()
    {
        foreach (Transform eachChild in transform)
        {
            if (eachChild.name != "Score")
            {
                Debug.Log("Child found. Name: " + eachChild.name);
                // disable them
                eachChild.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    public void Restart()
    {
        foreach (Transform eachChild in transform)
        {
            if (eachChild.name != "Score")
            {
                Debug.Log("Child found. Name: " + eachChild.name);
                // enable them
                eachChild.gameObject.SetActive(true);
                Time.timeScale = 1.0f;
            }
        }
    }
}
