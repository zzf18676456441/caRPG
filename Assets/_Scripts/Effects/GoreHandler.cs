using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreHandler : MonoBehaviour
{
    public Sprite[] splatters;
    private float maxScale;
    private float alpha;
    void Start()
    {
        print("created");
        transform.position = FindObjectOfType<Camera>().transform.position + new Vector3(UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(-30f, 30f));
        this.transform.parent = FindObjectOfType<Camera>().transform;
        GetComponent<SpriteRenderer>().sprite = splatters[UnityEngine.Random.Range(0, 3)];
        alpha = GetComponent<SpriteRenderer>().color.a;
        transform.localScale = new Vector3(0, 0, 0);
        maxScale = UnityEngine.Random.Range(95f, 150f);
    }

    void FixedUpdate()
    {
        if (transform.localScale.x < maxScale)
        {
            transform.localScale += new Vector3(7.5f, 7.5f);
        }
        else
        {
            if (alpha > .25f)
                alpha -= .025f;
            else
                alpha -= .002f;
            transform.position = new Vector3(transform.position.x, transform.position.y - .015f);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
        }
        if (alpha <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
