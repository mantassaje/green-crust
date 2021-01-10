using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPagesData : MonoBehaviour {

    public class PageText
    {
        public string Header { get; set; }
        public string Content { get; set; }
    }

    public List<Sprite> Pages;
    public List<Sprite> ButtonSpirtes1;
    public List<Sprite> ButtonSpirtes2;

    public List<PageText> PageTexts = new List<PageText>()
    {
        new PageText()
        {
            Header = "Your lands",
            Content = "Red lines mark your borders. You can do actions only inside your borders."
        },
        new PageText()
        {
            Header = "Waters",
            Content = "Water is source of life. You can create seas by lowering flat land or dropping ice meteor. Small blue ball shows how much energy this tile generates to you each turn. Waters increase you borders. Larger borders will make your actions consume more energy."
        },
        new PageText()
        {
            Header = "Mud deserts",
            Content = "After ending turn you will find mud deserts. This is where you can create wildlife. Single tile of sea might not be enough."
        },
        new PageText()
        {
            Header = "Wildlife",
            Content = "Select biome and click on \"Grow\". This will consume some energy. Wildlife will be created. Wildlife can be biggest source of energy. But it needs to grow first. Energy is needed to win the game."
        },
        new PageText()
        {
            Header = "Abyss",
            Content = "You are surrounded by abyss. Cold comes from abyss. You can create a new land over the abyss if your borders touch the abyss."
        },
        new PageText()
        {
            Header = "Mountains",
            Content = "Mountains can block cold coming from the abyss. You can increase or lower the mountains."
        },
        new PageText()
        {
            Header = "Clouds",
            Content = "Clouds visualize the rainfall. Lands with no clouds are dry so grass biomes can be there. Lands with clouds have moderate rainfall so forest can grow there. Forests can grow bigger and generate more energy."
        },
        new PageText()
        {
            Header = "Ancients",
            Content = "Strong and old ecosystem in your biome is called ancient. It allows you to capture your opponents lands. Ancient can capture only the same biome. And there should be no existing ancient. If biome changes the ancient will disappear."
        },
        new PageText()
        {
            Header = "Goal",
            Content = "You will play against one or few opponents. Goal is written on top of the screen. The first one to collect specified amount of energy over the game will be winner. Using actions does not shrink this number."
        }
    };

    public TutorialPagesData()
    {
        Singles.TutorialPagesData = this;
    }
}
