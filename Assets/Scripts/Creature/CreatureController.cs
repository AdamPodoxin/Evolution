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
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            foodCollected++;

            StartCoroutine(creatureMovement.ChooseRandomDirection());
        }
    }

    private IEnumerator CheckForFood()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Food"))
            {
                creatureMovement.LookAt(collider.transform.position);
                StopCoroutine(creatureMovement.ChooseRandomDirection());
            }
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(CheckForFood());
    }

    private void StartDay()
    {
        foodCollected = 0;
        StartCoroutine(CheckForFood());

        creatureMovement.StartDay();
    }

    private void StopDay()
    {
        StopCoroutine(CheckForFood());
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
