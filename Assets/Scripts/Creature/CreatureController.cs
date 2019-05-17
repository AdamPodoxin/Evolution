using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    private Vector3 startPosition;

    private float energy = 10f;

    private void Start()
    {
        startPosition = transform.position;
        StartDay();
    }

    private void Update()
    {
        CheckOutOfBounds();
        CheckEnergy();
    }

    private IEnumerator ChooseRandomDirection()
    {
        transform.LookAt(new Vector3(Random.Range(-1f, 1f), startPosition.y, Random.Range(-1f, 1f)));

        yield return new WaitForSeconds(1f);

        StartCoroutine(ChooseRandomDirection());
    }

    private void StartDay()
    {
        StartCoroutine(ChooseRandomDirection());
    }

    private void StopDay()
    {
        //Check for food, etc.
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void ReturnToStartPosition()
    {
        StopCoroutine(ChooseRandomDirection());
        transform.LookAt(startPosition);
    }

    private void CheckOutOfBounds()
    {
        if (Mathf.Abs(transform.position.x) > 9 || Mathf.Abs(transform.position.z) > 9)
        {
            transform.LookAt(startPosition);
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
        }
    }
}
