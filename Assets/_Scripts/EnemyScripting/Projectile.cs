using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float duration = 5;
    public GameObject explosion;
    public void FireAt(Vector2 FixedTarget)
    {
        duration *= 50f;
        gameObject.GetComponent<SinglePointMovement>().MoveByVector(FixedTarget);
    }

    public void FireAt(Transform MovingTarget)
    {
        duration *= 50f;
        gameObject.GetComponent<SinglePointMovement>().Chase(MovingTarget);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        duration--;
        if (duration == 0) DestroyProjectile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            GameObject detonated = Instantiate(explosion, this.transform.position, Quaternion.identity);
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
