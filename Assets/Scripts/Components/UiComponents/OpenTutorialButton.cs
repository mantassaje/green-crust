using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenTutorialButton : ManualUpdateBehaviour
{

    public Tutorial Tutorial;

    void Start () {
        gameObject.GetComponent<Button>()
            .onClick
            .AddListener(() => Tutorial.gameObject.SetActive(true));
	}
}
