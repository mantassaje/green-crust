using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CityStateService
{
    public static void CreateNew(Biome cityBiome)
    {
        cityBiome.CityState.Name = GenerateName();
        cityBiome.CityState.Population = 0;

        cityBiome.CityState.Personality = new CityStatePersonality();

        //Scrambler
        cityBiome.CityState.Personality.CommonFolk.Add(UnityEngine.Random.Range(-0.2f, 0.2f));
        cityBiome.CityState.Personality.Trading.Add(UnityEngine.Random.Range(-0.2f, 0.2f));
        cityBiome.CityState.Personality.Worshiper.Add(UnityEngine.Random.Range(-0.2f, 0.2f));
        cityBiome.CityState.Personality.Rading.Add(UnityEngine.Random.Range(-0.2f, 0.2f));

        var randomNumber = UnityEngine.Random.Range(0f, 1f);
        if(randomNumber < 0.25f)
            cityBiome.CityState.Personality.CommonFolk.Add(UnityEngine.Random.Range(0f, 0.8f));
        else if (randomNumber < 0.5f)
            cityBiome.CityState.Personality.Trading.Add(UnityEngine.Random.Range(0f, 0.8f));
        else if (randomNumber < 0.75f)
            cityBiome.CityState.Personality.Worshiper.Add(UnityEngine.Random.Range(0f, 0.8f));
        else cityBiome.CityState.Personality.Rading.Add(UnityEngine.Random.Range(0f, 0.8f));

        cityBiome.CityState.History = new List<CityStateHistory>();

        cityBiome.CityState.Explored = new List<Biome>();
        cityBiome.CityState.Explored.Add(cityBiome);

        cityBiome.CityState.LastAge = new CityStateData(0, 0, 0);
        cityBiome.CityState.CurrentAge = new CityStateData(0, 0, 0);
    }

    public static string GenerateName()
    {
        var str = new List<String>()
        {
            "Abu", "Al", "Ardi", "Af",
            "Blo", "Bru", "Bege",
            "Cardi", "Can", "Cru", "Cay",
            "Deo", "Dans", "Dele", "Dom",
            "Ete", "Etio", "Etel", "El",
            "For", "Fran", "Fi",
            "Gab", "Geor", "Gu",
            "Hio", "Hab", "Ha", "Hus",
            "Ir", "Inc", "Ip", "It",
            "Jero", "Juc", "Ja",
            "Kar", "Kut", "Ke",
            "Lep", "Litu", "Li",
            "Mou", "Myan", "Me",
            "Nil", "Num", "No",
            "Ofre", "Oman", "Op",
            "Petro", "Par", "Pa",
            "Rio", "Rus", "Ra", "Rah",
            "Qu", "Qa",
            "Sab", "Sic", "Sed", "Sey",
            "Tur", "Tan", "Tog", "Thai",
            "Ug", "Ur", "Un",
            "Van", "Vien", "Vem", 
            "Wait", "Wana",
            "Ye",
            "Zam", "Zu"
        };

        var name = str.PickRandom(1).First() + str.PickRandom(1).First().ToLower();
        return name;
    }

    public static void ValidateExploredBiomes(Biome cityToValidate)
    {
        var removeExplored = new List<Biome>();

        foreach (var explored in cityToValidate.CityState.Explored)
        {
            if(!CityStateInfoService.CanBeExplored(explored))
            {
                removeExplored.Add(explored);
            }
        }

        removeExplored.ForEach(remove => cityToValidate.CityState.Explored.Remove(remove));
        
        RemoveUnlinkedBiomes(cityToValidate);
    }

    /// <summary>
    /// Find all city states that have this biome as explored and remove it from their explored list.
    /// </summary>
    public static void RemoveAsExplored(Biome biomeToRemove)
    {
        var cityBiomes = Singles.Grid.GetAllBiomes()
            .Where(biome => !biome.CityState.IsDead
                && biomeToRemove != biome);

        foreach(var cityBiome in cityBiomes)
        {
            var isRemoved = cityBiome.CityState.Explored.Remove(biomeToRemove);

            if (isRemoved) RemoveUnlinkedBiomes(cityBiome);
        }
    }

    /// <summary>
    /// Find all explored biomes that are not linked to city state and remove them from explored list.
    /// </summary>
    public static void RemoveUnlinkedBiomes(Biome cityBiome)
    {
        //Create new list
        var toVisitBiome = cityBiome.CityState.Explored.ToList();

        var visitedBiomes = new List<Biome>();
        var nearby = cityBiome.GetNearbyBiomesCache().Where(near => cityBiome.CityState.Explored.Contains(near));
        visitedBiomes.AddRange(nearby);
        visitedBiomes.ForEach(remove => toVisitBiome.Remove(remove));

        Biome newVisit = null;

        do
        {
            newVisit = VisitNew(toVisitBiome, visitedBiomes);

            if (newVisit.IsNotNull())
            {
                toVisitBiome.Remove(newVisit);
                visitedBiomes.Add(newVisit);
            }
        }
        while (newVisit.IsNotNull());

        //Remove all biomes from explore list that were not visited by path finding.
        toVisitBiome.ForEach(remove => cityBiome.CityState.Explored.Remove(remove));
    }

    private static Biome VisitNew(List<Biome> toVisitBiome, List<Biome> visitedBiomes)
    {
        return toVisitBiome.FirstOrDefault(
            toVisit => visitedBiomes
                .Any(visited => IsConnected(visited, toVisit)
            )
        );
    }

    private static bool IsConnected(Biome biome1, Biome biome2)
    {
        return biome1.IsWater == biome2.IsWater && biome1.GetNearbyBiomesCache().Contains(biome2);
    }
}

