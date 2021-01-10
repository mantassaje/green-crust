using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : ManualUpdateBehaviour
{

    public Text Text;
    public Button CloseButton;

    private void Start()
    {
        CloseButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });

        this.gameObject.SetActive(false);
    }
}