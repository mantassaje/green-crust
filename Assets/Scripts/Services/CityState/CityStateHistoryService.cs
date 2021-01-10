using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class CityStateHistoryService
{
    public static string GetHistoryText(CityState city)
    {
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine("Civs history:");
        for (int i = city.History.Count - 1; i >= 0; i--)
        {
            strBuilder.AppendLine(city.History[i].GetFullText());
        }
        return strBuilder.ToString();
    }

    public static void BecomeNotable(CityState city)
    {
        throw new NotImplementedException();
        AddHistory(city, "A notable king " + city.Name + " has emerged in these lands");
    }

    public static void BecomeIndepended(CityState city, CityState oldCity)
    {
        throw new NotImplementedException();
        AddHistory(city, "We become independed from the king " + oldCity.Name);
    }

    public static void AddHistory(CityState city, string story)
    {
        throw new NotImplementedException();
        var hst = new CityStateHistory()
        {
            Story = story,
            Turn = Singles.World.Turn
        };
        city.History.Add(hst);
    }

    public static void GoldenAge(CityState city)
    {
        throw new NotImplementedException();
        AddHistory(city, "With help of our genius this was a great age for our nation");
    }


    public static void BecomeNotableByGodlenAge(CityState city)
    {
        throw new NotImplementedException();
        AddHistory(city, "All hail our new king " + city.Name + " who united people of this land. It was a great age for our nation");
    }

    public static void DebugAttackDefensePpints(CityState city, float attack, float defeanse)
    {
        throw new NotImplementedException();
        AddHistory(city, "Attack points: " + attack + " Defeanse points: " + defeanse);
    }
}

