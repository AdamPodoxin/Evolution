using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public GameObject creature;
    public float senseRadius = 2f;

    [SerializeField] private CreatureStats creatureStats = new CreatureStats();

    private int foodCollected = 0;


    private GameController gameController;
    private CreatureMovement creatureMovement;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        creatureMovement = GetComponent<CreatureMovement>();

        gameController.startDay += StartDay;
        gameController.stopDay += StopDay;

        ApplyStats();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            foodCollected++;

            creatureMovement.ChooseRandomDirection();
        }
    }

    private void ApplyStats()
    {
        creatureMovement.speed = creatureStats.speed;
        creatureMovement.startEnergy = creatureStats.energy;
        senseRadius = creatureStats.senseRadius;

        creatureMovement.CalculateEnergyLoss();
    }

    private void StartDay()
    {
        foodCollected = 0;

        creatureMovement.StartDay();
    }

    private void StopDay()
    {
        creatureMovement.StopDay();

        if (foodCollected == 0)
        {
            Die();
        }
        else if (foodCollected >= 2)
        {
            Reproduce();
        }
    }

    private void Die()
    {
        gameController.startDay -= StartDay;
        gameController.stopDay -= StopDay;

        creatureMovement.Die();

        Destroy(gameObject);
    }

    private void Reproduce()
    {
        Instantiate(creature, transform.position + transform.right, transform.rotation).GetComponent<CreatureController>().creatureStats = creatureStats;
    }
}

[System.Serializable]
public class CreatureStats
{
    public float speed = 3f;
    public float energy = 10f;
    public float senseRadius = 2f;

    public CreatureStats()
    {

    }

    public CreatureStats(float speed, float energy, float senseRadius)
    {
        this.speed = speed;
        this.energy = energy;
        this.senseRadius = senseRadius;
    }
}
