using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuManager : MonoBehaviour
{
    public GameObject upgradesButtonHolder;
    public GameObject upgradesItemGroup;

    public GameObject attachmentsButtonHolder;
    public GameObject attachmentsItemGroup;

    private Button activeButton;

    // Start is called before the first frame update
    void Start()
    {
        Button upgradesButton = upgradesButtonHolder.GetComponent<Button>();
        Button attachmentsButton = attachmentsButtonHolder.GetComponent<Button>();
        Button selectedButton = upgradesButton;

        RadioButtonGroup.RadioButtonPressed(upgradesButton, selectedButton);
        attachmentsItemGroup.SetActive(false);

        upgradesButton.onClick.AddListener(() => {
            RadioButtonGroup.RadioButtonPressed(upgradesButton, selectedButton);
            selectedButton = upgradesButton;
            attachmentsItemGroup.SetActive(false);
            upgradesItemGroup.SetActive(true);
        });

        attachmentsButton.onClick.AddListener(() => {
            RadioButtonGroup.RadioButtonPressed(attachmentsButton, selectedButton);
            selectedButton = attachmentsButton;
            upgradesItemGroup.SetActive(false);
            attachmentsItemGroup.SetActive(true);
        });
    }
}
