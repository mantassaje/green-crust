using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class ManaService
{
    public static List<IManaBonus> ManaBonuses { get; private set; }

    static ManaService()
    {
        ManaBonuses = new List<IManaBonus>
        {
            new WorshiperManaBonus(),
            new AncientsManaBonus(),
            new NatureManaBonus(),
            new SeaManaBonus(),
            new DeepSeaManaBonus(),
            new MountainManaBonus()
        };
    }

    public static int GetManaBonuses(Biome biome)
    {
        var mana = 0;
        foreach(var bonus in ManaBonuses)
        {
            mana += bonus.GetManaBonus(biome);
        }
        return mana;
    }
}

