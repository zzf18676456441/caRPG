using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TextureRepeater : MonoBehaviour
{
    public float singleWidth;
    public float desiredWidth;
    public bool leftToRight;
    public bool positiveDirection;
    public GameObject repeatMe;

    // Start is called before the first frame update
    void Start()
    {
        float neededSegements = desiredWidth / singleWidth;
        for (int i = 1; i < neededSegements; i++)
        {
            GameObject newSegment = Instantiate<GameObject>(repeatMe);
            float m = positiveDirection ? i : -i;
            newSegment.transform.position = leftToRight ? new Vector3(repeatMe.transform.position.x + singleWidth * m, repeatMe.transform.position.y, repeatMe.transform.position.z) : new Vector3(repeatMe.transform.position.x, repeatMe.transform.position.y + singleWidth * m, repeatMe.transform.position.z);
        }
    }
}