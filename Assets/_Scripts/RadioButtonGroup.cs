using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButtonGroup : MonoBehaviour
{
    public GameObject defaultButtonHolder;

    private Button selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = defaultButtonHolder.GetComponent<Button>();

        RadioButtonPressed(selectedButton, selectedButton);

        Button[] buttons = this.GetComponentsInChildren<Button>();

        foreach(Button b in buttons)
        {
            b.onClick.AddListener(()=> {
                RadioButtonPressed(b, selectedButton);
                selectedButton = b;
            });
        }
    }

    public static void RadioButtonPressed(Button clickedButton, Button selectedButton)
    {
        ColorBlock setColors = clickedButton.colors;

        Color defaultNormal = setColors.normalColor;
        Color defaultHighlighted = setColors.highlightedColor;
        Color defaultPressed = setColors.pressedColor;
        Color defaultSelected = setColors.selectedColor;

        ColorBlock oldColors = selectedButton.colors;

        oldColors.normalColor = defaultNormal;
        oldColors.highlightedColor = defaultHighlighted;
        oldColors.pressedColor = defaultPressed;
        oldColors.selectedColor = defaultSelected;

        selectedButton.colors = oldColors;

        setColors.normalColor = defaultSelected;
        setColors.highlightedColor = defaultSelected;
        setColors.pressedColor = defaultSelected;

        clickedButton.colors = setColors;
    }
}
