using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public List<Slider> sliders;

    public TextMeshProUGUI timerText;

    public float time;

    private bool stopTimer = true;
    private TargetController _targetController;

    // Start is called before the first frame update
    void Start()
    {
        _targetController = transform.GetChild(2).GetComponent<TargetController>();
        Init();
    }

    void Init()
    {
        stopTimer = true;
        foreach (Slider slider in sliders)
        {
            slider.maxValue = time;
            slider.value = time;
            slider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
        }

        timerText.color = Color.white;
    }

    public void StartTimer()
    {
        stopTimer = false;
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            GameObject.Find("Canvas").SetActive(false);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        time -= Time.deltaTime;
        
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        string textTime = $"{minutes:0}:{seconds:00}";

        if (time <= 10)
        {
            foreach (Slider slider in sliders)
            {
                slider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
            }

            timerText.color = Color.red;
        }
        
        if(stopTimer) return;
        
        if (time <= 0)
        {
            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                GameObject.Find("Canvas").SetActive(true);   
            }
            stopTimer = true;
            time = 0;
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                transform.GetChild(2).GetComponent<SinglePlayerTargetController>().EndSimulation();
                GameObject.Find("ScoreBoard").transform.GetChild(2).gameObject.SetActive(true);
                time = 30;
                Init();
            }
            else
            {
                _targetController.EndSimulation();    
            }
            
            return;
        }
        
        timerText.text = textTime;
        foreach (Slider slider in sliders)
        {
            slider.value = time;   
        }
    }
}
