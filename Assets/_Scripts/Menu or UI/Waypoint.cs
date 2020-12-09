using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int index;
    public float x { get {return transform.position.x;} }
    public float y { get {return transform.position.y;} }
}
