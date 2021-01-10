using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject ActiveCard;

    public void ChangeCard(GameObject card)
    {
        if (ActiveCard != null)
        {
            ActiveCard.SetActive(false);
        }

        card.SetActive(true);
        ActiveCard = card;
    }



    void OnServerInitialized()
    {
        var test = FindObjectsOfType<ClientRpcAction>();
    }
}
