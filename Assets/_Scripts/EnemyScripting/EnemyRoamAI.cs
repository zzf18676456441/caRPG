using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoamAI : MonoBehaviour
{


    private Vector3 startPosition;
    private Vector3 roamPosition;
    public bool isColide = false;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        roamPosition = GetRandomPosition();
        GetComponent<SinglePointMovement>().MoveToLocation(roamPosition);
    }

    void FixedUpdate()
    {
        float reachedPositionDis = 1f;
        if ((Vector3.Distance(transform.position, roamPosition) < reachedPositionDis) || isColide)
        {
            roamPosition = GetRandomPosition();
            isColide = false;
            StartCoroutine(Wait());
            GetComponent<SinglePointMovement>().MoveToLocation(roamPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isColide = true;
        }
    }

    private static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private Vector3 GetRandomPosition()
    {
        return startPosition + GetRandomDir() * Random.Range(10f, 15f);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }
}