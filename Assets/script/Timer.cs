using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    private Text timerText;
    private float m_Time = 0;
    private bool timerRunning = false;

    private void OnEnable()
    {
        SimpleCarController.carParkingStart += TimerStart;
        SimpleCarController.carParkingStop += TimerStop;
    }
    private void OnDisable()
    {
        SimpleCarController.carParkingStart -= TimerStart;
        SimpleCarController.carParkingStop -= TimerStop;
    }
    private void TimerStart()
    {
        if (!timerRunning)
            m_Time = 0;
        timerRunning= true;
    }
    private void TimerStop() 
    {
        timerRunning= false;

    }

    private void Update()
    {
        if(timerRunning)
            m_Time+= Time.deltaTime;
        GetComponent<Text>().text = m_Time.ToString();
    }
}
