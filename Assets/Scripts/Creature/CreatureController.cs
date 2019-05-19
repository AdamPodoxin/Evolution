using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public GameObject creature;

    private int foodCollected = 0;

    private GameController gameController;
    private CreatureMovement creatureMovement;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        creatureMovement = GetComponent<CreatureMovement>();

        gameController.startDay += StartDay;
        gameController.stopDay += StopDay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Food"))
        {
            Destroy(other.gameObject);
            foodCollected++;
        }
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
        Instantiate(creature, transform.position + transform.right, transform.rotation);
        //"creature.stats = stats"
    }
}
