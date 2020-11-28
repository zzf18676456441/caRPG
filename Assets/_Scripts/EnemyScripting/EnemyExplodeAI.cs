using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplodeAI : MonoBehaviour
{
    private bool destory = false;
    public bool startChase = false;
    //if player out of this range, enemy will back to randomly roaming otherwise it will chase enemy and attack.
    public float chaseRange = 30f;
    public float stopChaseRange = 50f;

    public float fieldOfImpact;

    public GameObject explosion;

    public LayerMask LayerToHit;

    private Transform player;
    private SpriteRenderer sprite;
    public float explodeTime = 3f;



    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = controller.GetCar().transform;
        gameObject.GetComponent<SinglePointMovement>().maxSpeed /= 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!destory)
        {
            if (startChase)
            {
                gameObject.GetComponent<SinglePointMovement>().Chase(player);
            }
        }
    }

    private void FixedUpdate()
    {
        if (destory)
        {
            gameObject.GetComponent<EnemyRoamAI>().enabled = false;
        }
        else
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        if ((!startChase) && Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            startChase = true;
            gameObject.GetComponent<EnemyRoamAI>().enabled = false;
            gameObject.GetComponent<SinglePointMovement>().maxSpeed *= 5;
            return;
        }

        if (startChase && Vector2.Distance(transform.position, player.position) > stopChaseRange)
        {
            startChase = false;
            gameObject.GetComponent<SinglePointMovement>().maxSpeed /= 5;
            gameObject.GetComponent<EnemyRoamAI>().enabled = true;
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SinglePointMovement>().Stop();
            destory = true;
            Explode();
        }
    }

    private void Explode()
    {
        FlashRed(explodeTime);
    }

    private void ApplyExplode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 dir = obj.transform.position - transform.position;
            IDamagable damagable = (IDamagable)obj.GetComponent(typeof(IDamagable));
            if (damagable != null)
            {
                DamageSystem.ApplyDamage(this.GetComponent<Enemy>(), damagable, dir);
            }

        }


        Destroy(gameObject);
    }

    void FlashRed(float time)
    {
        Debug.Log(time);
        if (time <= 0.1f)
        {
            GameObject g = Instantiate(explosion, transform.position, Quaternion.identity);
            g.GetComponent<ParticleSystem>().Play();
            ApplyExplode();
        }
        else
        {
            sprite.color = Color.red;
            StartCoroutine(Tick(time / 3));
            sprite.color = Color.white;
            FlashRed(time - time / 3);
        }

    }

    IEnumerator Tick(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}
