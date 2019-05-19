using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    public float speed = 3f;

    public float startEnergy = 10f;
    private float energy;
    private float energyLoss;

    private Vector3 startPosition;
    private Quaternion direction;

    private bool dayGoing = false;

    private GameController gameController;
    private CreatureController creatureController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        creatureController = GetComponent<CreatureController>();

        startPosition = transform.position;
        direction = transform.rotation;
    }

    private void Update()
    {
        if (dayGoing)
        {
            CheckOutOfBounds();
            CheckEnergy();
        }
    }

    private IEnumerator ChooseRandomDirectionCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        if (!CheckForFood())
        {
            ChooseRandomDirection();
            StartCoroutine(ChooseRandomDirectionCoroutine());
        }
    }

    public void ChooseRandomDirection()
    {
        direction = Quaternion.Euler(0, transform.eulerAngles.y + Random.Range(-60f, 60f), 0);
    }

    private bool CheckForFood()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, creatureController.senseRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Food"))
            {
                StopCoroutine(ChooseRandomDirectionCoroutine());
                LookAt(collider.transform.position);

                return true;
            }
        }

        return false;
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, 5 * Time.deltaTime);
    }

    private void ReturnToStartPosition()
    {
        StopCoroutine(ChooseRandomDirectionCoroutine());

        LookAt(startPosition);
    }

    private void CheckOutOfBounds()
    {
        if (Mathf.Abs(transform.position.x) > 9 || Mathf.Abs(transform.position.z) > 9)
        {
            LookAt(Vector3.up * startPosition.y);
        }
    }

    private void CheckEnergy()
    {
        if (energy > 0)
        {
            energy -= energyLoss * Time.deltaTime;
            Move();
        }
        else
        {
            ReturnToStartPosition();

            if (Vector3.Distance(startPosition, transform.position) > 0.1f)
            {
                Move();
            }
            else
            {
                dayGoing = false;
                gameController.CreatureEnded();
            }
        }
    }

    public void CalculateEnergyLoss()
    {
        energyLoss = speed / 3f;
    }

    public void LookAt(Vector3 lookDirection)
    {
        transform.LookAt(lookDirection);
        direction = transform.rotation;
    }

    public void StartDay()
    {
        energy = startEnergy;
        dayGoing = true;

        StartCoroutine(ChooseRandomDirectionCoroutine());
    }

    public void StopDay()
    {
        transform.LookAt(Vector3.up * startPosition.y);
    }

    public void Die()
    {
        StopCoroutine(ChooseRandomDirectionCoroutine());
    }
}
