using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeCardButton : MonoBehaviour {

    public GameObject ChangeToCard;

    private MenuController MenuController;

    public void Start()
    {
        this.MenuController = FindObjectOfType<MenuController>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => this.MenuController.ChangeCard(ChangeToCard));
    }
}
