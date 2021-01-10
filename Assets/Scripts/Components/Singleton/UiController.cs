using Assets.Scripts.Ai;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    public Tooltip Tooltip;
    public Popup Popup;
    public InGameMenuCard Menu;
    public Tutorial Tutorial;
    public CityStateEventCard CityStateEventCard;

    public GameObject BiomeButtonsCard;
    public GameObject BiomeInfoCard;
    public GameObject GoalCard;
    public GameObject ManaCard;
    public GameObject CityStateInfoCard;

    public CanvasGroup UiCanvas;

    public UiController()
    {
        Singles.UiController = this;
    }

    void Start()
    {
        Menu.gameObject.SetActive(false);
        Tooltip.gameObject.SetActive(false);
        HideAll();
    }

    private void Update()
    {
        if (Singles.World == null) return;

        GoalCard.SetActive(!Singles.World.IsSandbox);
        ManaCard.SetActive(!Singles.World.IsSandbox);

        CityStateInfoCard.SetActive(Singles.PlayerController.SelectedBiome.IsNotNull()
            && Singles.PlayerController.SelectedBiome.CityState.IsVisible);
    }

    public void ShowPopup(string text)
    {
        Popup.gameObject.SetActive(true);
        Popup.Text.text = text;
    }

    public void HideAll()
    {
        UiCanvas.alpha = 0f; //this makes everything transparent
        UiCanvas.blocksRaycasts = false; //this prevents the UI element to receive input events
    }

    public void ShowAll()
    {
        UiCanvas.alpha = 1f;
        UiCanvas.blocksRaycasts = true;
    }
}