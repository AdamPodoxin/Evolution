using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void DayEvent();
    public DayEvent startDay;
    public DayEvent stopDay;

    private int creaturesEnded = 0;

    private void Start()
    {
        startDay.Invoke();
    }

    private IEnumerator RestartDay()
    {
        creaturesEnded = 0;
        stopDay.Invoke();

        yield return new WaitForSeconds(1);

        startDay.Invoke();
    }

    public void CreatureEnded()
    {
        creaturesEnded++;

        if (creaturesEnded >= FindObjectsOfType<CreatureController>().Length)
        {
            StartCoroutine(RestartDay());
        }
    }
}
