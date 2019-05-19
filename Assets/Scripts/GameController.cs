using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void DayEvent();
    public DayEvent startDay;
    public DayEvent stopDay;

    public GameObject foodPrefab;

    private int creaturesEnded = 0;

    [SerializeField]
    private int currentFoodAmount = 20;

    private void Start()
    {
        GenerateFood();

        startDay.Invoke();
    }

    private IEnumerator RestartDay()
    {
        creaturesEnded = 0;
        stopDay.Invoke();

        GenerateFood();

        yield return new WaitForSeconds(1);

        startDay.Invoke();
    }

    private void GenerateFood()
    {
        int amountOfFoodInMap = GameObject.FindGameObjectsWithTag("Food").Length;
        for (int i = 0; i < currentFoodAmount - amountOfFoodInMap; i++)
        {
            Instantiate(foodPrefab, new Vector3(Random.Range(-6f, 6f), 0.5f, Random.Range(-6f, 6f)), Quaternion.identity);
        }
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
