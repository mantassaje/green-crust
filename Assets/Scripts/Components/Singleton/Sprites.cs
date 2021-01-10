using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour {

    public Sprites()
    {
        Singles.Sprites = this;
    }

    public Sprite RainforestTop1;
    public Sprite RainforestTop2;
    public Sprite RainforestTop3;
    public Sprite RainforestBot1;
    public Sprite RainforestBot2;
    public Sprite RainforestBot3;

    public Sprite TropicsTop1;
    public Sprite TropicsTop2;
    public Sprite TropicsTop3;
    public Sprite TropicsBot1;
    public Sprite TropicsBot2;
    public Sprite TropicsBot3;

    public Sprite SavanaTop1;
    public Sprite SavanaTop2;
    public Sprite SavanaBot1;
    public Sprite SavanaBot2;

    public Sprite WetlandTop1;
    public Sprite WetlandTop2;
    public Sprite WetlandTop3;
    public Sprite WetlandBot1;
    public Sprite WetlandBot2;
    public Sprite WetlandBot3;

    public Sprite LeafForestTop1;
    public Sprite LeafForestTop2;
    public Sprite LeafForestTop3;
    public Sprite LeafForestBot1;
    public Sprite LeafForestBot2;
    public Sprite LeafForestBot3;

    public Sprite GrasslandTop1;
    public Sprite GrasslandTop2;
    public Sprite GrasslandBot1;
    public Sprite GrasslandBot2;

    public Sprite TaigaTop1;
    public Sprite TaigaTop2;
    public Sprite TaigaTop3;
    public Sprite TaigaBot1;
    public Sprite TaigaBot2;
    public Sprite TaigaBot3;

    public Sprite TundraTop1;
    public Sprite TundraTop2;
    public Sprite TundraBot1;
    public Sprite TundraBot2;

    public Sprite Hills1;
    public Sprite Mountains1;
    public Sprite Mountains2;
    public Sprite Mountains3;

    public List<Sprite> Cloud1;
    public Sprite Cloud2;
    public Sprite Cloud3;

    public Sprite ColdDesert;
    public Sprite ColdMud;
    public Sprite DeadCold;
    public Sprite DeadHot;
    public Sprite DeepOcean;
    public Sprite ShallowSea;
    public Sprite ForestGround;
    public Sprite GrasslandGround;
    public Sprite HotDesert;
    public Sprite HotMud;
    public Sprite IceCap;
    public Sprite WarmMud;
    public Sprite RainforestGround;
    public Sprite SavanaGround;
    public Sprite TaigaGround;
    public Sprite TropicGround;
    public Sprite TundraGround;
    public Sprite WarmDesert;
    public Sprite WetlandGround;

    public Sprite Ancient_Beaber;
    public Sprite Ancient_Bison;
    public Sprite Ancient_WinterWolf;
    public Sprite Ancient_Deer;
    public Sprite Ancient_Elephant;
    public Sprite Ancient_Lion;
    public Sprite Ancient_WinterBear;
    public Sprite Ancient_TreePython;
    public Sprite Ancient_PolarBear;
    public Sprite Ancient_PolarFox;
    public Sprite Ancient_UnderwaterTurtle;
    public Sprite Ancient_Whale;
    public Sprite Ancient_Roots;

    public SafeGetList<Sprite> NoSprite { get; private set; }
    public SafeGetList<Sprite> MountainSprites { get; private set; }
    public SafeGetList<Sprite> TaigaForestTopSprites { get; private set; }
    public SafeGetList<Sprite> TaigaForestBotSprites { get; private set; }
    public SafeGetList<Sprite> TundraGrassTopSprites { get; private set; }
    public SafeGetList<Sprite> TundraGrassBotSprites { get; private set; }
    public SafeGetList<Sprite> LeafForestSpriteTop { get; private set; }
    public SafeGetList<Sprite> LeafForestSpriteBotttom { get; private set; }
    public SafeGetList<Sprite> GrasslandSpriteTop { get; private set; }
    public SafeGetList<Sprite> GrasslandSpriteBotttom { get; private set; }
    public SafeGetList<Sprite> SavanaSpriteTop { get; private set; }
    public SafeGetList<Sprite> SavanaSpriteBotttom { get; private set; }
    public SafeGetList<Sprite> TropicSpriteTop { get; private set; }
    public SafeGetList<Sprite> TropicSpriteBotttom { get; private set; }
    public SafeGetList<SafeGetList<Sprite>> Clouds { get; private set; }

    private void Start()
    {
        NoSprite = new SafeGetList<Sprite>
        {
            null
        };

        MountainSprites = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.Hills1,
            Singles.Sprites.Mountains1,
            Singles.Sprites.Mountains2,
            Singles.Sprites.Mountains3,
        };

        TaigaForestTopSprites = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.TaigaTop1,
            Singles.Sprites.TaigaTop2,
            Singles.Sprites.TaigaTop3
        };

        TaigaForestBotSprites = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.TaigaBot1,
            Singles.Sprites.TaigaBot2,
            Singles.Sprites.TaigaBot3
        };

        TundraGrassTopSprites = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.TundraTop1,
            Singles.Sprites.TundraTop2,
        };

        TundraGrassBotSprites = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.TundraBot1,
            Singles.Sprites.TundraBot2,
        };

        LeafForestSpriteTop = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.LeafForestTop1,
            Singles.Sprites.LeafForestTop2,
            Singles.Sprites.LeafForestTop3,
        };

        LeafForestSpriteBotttom = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.LeafForestBot1,
            Singles.Sprites.LeafForestBot2,
            Singles.Sprites.LeafForestBot3,
        };

        TropicSpriteTop = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.TropicsTop1,
            Singles.Sprites.TropicsTop2,
            Singles.Sprites.TropicsTop3,
        };

        TropicSpriteBotttom = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.TropicsBot1,
            Singles.Sprites.TropicsBot2,
            Singles.Sprites.TropicsBot3,
        };

        GrasslandSpriteTop = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.GrasslandTop1,
            Singles.Sprites.GrasslandTop2,
        };

        GrasslandSpriteBotttom = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.GrasslandBot1,
            Singles.Sprites.GrasslandBot2,
        };

        SavanaSpriteTop = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.SavanaTop1,
            Singles.Sprites.SavanaTop2,
        };

        SavanaSpriteBotttom = new SafeGetList<Sprite>
        {
            null,
            Singles.Sprites.SavanaBot1,
            Singles.Sprites.SavanaBot2,
        };

        Clouds = new SafeGetList<SafeGetList<Sprite>>
        {
            null,//None,
            null,//DeadDry,
            null,//Dry,
            new SafeGetList<Sprite>(Singles.Sprites.Cloud1),//Humid,
            new SafeGetList<Sprite>(Singles.Sprites.Cloud3),//Wet
        };
    }
}
