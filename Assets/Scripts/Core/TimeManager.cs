using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviourSinqletonBase<TimeManager>
{
    private bool flow = true;

    public void StopTime()
    {
        Time.timeScale = 0f;
        flow = false;
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
        flow = true;
    }

    public bool TimeFlow()
    {
        return flow;
    }
}
