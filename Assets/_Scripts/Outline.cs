using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public SpriteRenderer mainSprite;
    public List<SpriteRenderer> outline;
    public Color outlineColor;

    void Start()
    {
        RefreshOutlineColor();
    }

    void Update()
    {
        foreach (SpriteRenderer s in outline)
        {
            s.sprite = mainSprite.sprite;
        }
    }

    public void RefreshOutlineColor()
    {
        Shader shaderGUItext;
        foreach (SpriteRenderer s in outline)
        {
            shaderGUItext = Shader.Find("GUI/Text Shader");
            s.material.shader = shaderGUItext;
            s.sortingOrder = mainSprite.sortingOrder - 1;
            s.color = outlineColor;
        }
    }
}
