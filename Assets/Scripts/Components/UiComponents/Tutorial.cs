using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : ManualUpdateBehaviour
{

    private int _page;

    public Text Header;
    public Text Content;
    public Image Image;
    public Image ButtonImage1;
    public Image ButtonImage2;
    public Button NextButton;
    public Button PreviousButton;
    public Button Exit;

    public bool IsOpen
    {
        get
        {
            return gameObject.activeSelf;
        }
    }


    void Start ()
    {
        NextButton.onClick.AddListener(Next);
        PreviousButton.onClick.AddListener(Previous);
        Exit.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public override void ManualUpdate()
    {
        Header.text = Singles.TutorialPagesData.PageTexts[_page].Header;
        Content.text = Singles.TutorialPagesData.PageTexts[_page].Content;
        Image.sprite = Singles.TutorialPagesData.Pages[_page];
        ButtonImage1.sprite = Singles.TutorialPagesData.ButtonSpirtes1[_page];
        ButtonImage2.sprite = Singles.TutorialPagesData.ButtonSpirtes2[_page];

        ButtonImage1.gameObject.SetActive(ButtonImage1.sprite != null);
        ButtonImage2.gameObject.SetActive(ButtonImage2.sprite != null);

        NextButton.gameObject.SetActive(_page < Singles.TutorialPagesData.Pages.TopIndex());
        PreviousButton.gameObject.SetActive(_page > 0);
    }

    private void Next()
    {
        _page++;
        _page = _page.GetMinMax(0, Singles.TutorialPagesData.Pages.TopIndex());
    }

    private void Previous()
    {
        _page--;
        _page = _page.GetMinMax(0, Singles.TutorialPagesData.Pages.TopIndex());
    }
}
