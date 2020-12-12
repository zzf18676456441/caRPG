using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScript : MonoBehaviour
{
    public GameObject boss;
    public SpriteRenderer color;
    public Transform hpbarInner;
    private Enemy bossScript;
    private float maxHP;
    void Awake()
    {
        bossScript = boss.GetComponent<Enemy>();
        maxHP = bossScript.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null){
            gameObject.SetActive(false);
        }
        transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y + 6);
        hpbarInner.localScale = new Vector3(1 * (bossScript.health / maxHP), 1);
        color.color = new Color((.5f / (bossScript.health / maxHP)), (1f * (bossScript.health / maxHP)), .1f);
    }
}
