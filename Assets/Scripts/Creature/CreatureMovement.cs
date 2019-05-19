using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    private Vector3 startPosition;
    private Quaternion direction;

    private float startEnergy = 10f;
    private float energy;

    private bool dayGoing = false;

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

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

    private void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, 5 * Time.deltaTime);
    }

    private IEnumerator ChooseRandomDirection()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        direction = Quaternion.Euler(0, transform.eulerAngles.y + Random.Range(-60f, 60f), 0);
        StartCoroutine(ChooseRandomDirection());
    }

    private void ReturnToStartPosition()
    {
        StopCoroutine(ChooseRandomDirection());

        transform.LookAt(startPosition);
        direction = transform.rotation;
    }

    private void CheckOutOfBounds()
    {
        if (Mathf.Abs(transform.position.x) > 9 || Mathf.Abs(transform.position.z) > 9)
        {
            transform.LookAt(Vector3.up * startPosition.y);
            direction = transform.rotation;
        }
    }

    private void CheckEnergy()
    {
        if (energy > 0)
        {
            energy -= Time.deltaTime;
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

    public void StartDay()
    {
        energy = startEnergy;
        dayGoing = true;

        StartCoroutine(ChooseRandomDirection());
    }

    public void StopDay()
    {
        transform.LookAt(Vector3.up * startPosition.y);
    }

    public void Die()
    {
        StopCoroutine(ChooseRandomDirection());
    }
}
