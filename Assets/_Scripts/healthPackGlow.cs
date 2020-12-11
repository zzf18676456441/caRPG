using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPackGlow : MonoBehaviour
{
    public Outline outline;
    private Color color;
    private bool goingDown;
    void Update()
    {
        if (goingDown)
            color = new Color(outline.outlineColor.r, outline.outlineColor.g, outline.outlineColor.b, outline.outlineColor.a -= (.25f * Time.deltaTime));
        else
            color = new Color(outline.outlineColor.r, outline.outlineColor.g, outline.outlineColor.b, outline.outlineColor.a += (.3f * Time.deltaTime));
        outline.outlineColor = color;
        outline.RefreshOutlineColor();
        if (outline.outlineColor.a <= .01f)
        {
            goingDown = false;
        }
        else if(outline.outlineColor.a >= .75f)
        {
            goingDown = true;
        }
    }
}
