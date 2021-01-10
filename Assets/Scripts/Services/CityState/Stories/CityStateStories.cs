using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NewLineCityStateStory : ICityStateSory
{
    public string Get(Biome cityBiome)
    {
        return Environment.NewLine;
    }
}

public class IntroNameCityStateStory: ICityStateSory
{
    public string Get(Biome cityBiome)
    {
        var result = new StringBuilder();
        var netGrowth = CityStateInfoService.GetNetGrowth(cityBiome);

        if (netGrowth < -0.4f) result.Append("Fast shrinking city state in calamity with starving population ");
        else if (netGrowth < -0.1f) result.Append("Shrinking city state with starving population ");
        else if (netGrowth > 0.4f) result.Append("Prosperous city state with quickly growing population ");
        else if (netGrowth > 0.2f) result.Append("City state with quickly growing population ");
        else if (netGrowth > 0.1f) result.Append("City state with slowly growing population ");
        else result.Append("City state with population ");

        if (cityBiome.CityState.Population < 1f) result.Append("of less then a thousand people. ");
        else result.Append($"of around { (int)cityBiome.CityState.Population } thousand people. ");

        if (result.Length > 0)
            result.AppendLine();

        return result.ToString();
    }
}

public class CommonFolkCityStateStory : ICityStateSory
{
    public string Get(Biome cityBiome)
    {
        if (cityBiome.CityState.Personality.CommonFolk.Rate > CityStateConstant.PersonalityAvarage)
            return "City of common folk boasting simple life. ";
        if (cityBiome.CityState.Personality.CommonFolk.Rate > CityStateConstant.PersonalityLow)
            return "When perople are not working or on duty they live a simple common folk life. ";
        /*if (cityBiome.CityState.Personality.CommonFolk.Rate < CityStateConstant.PersonalityFaded)
            return "City of busy people. ";*/
        return "";
    }
}

public class ReligionCityStateStory : ICityStateSory
{
    public string Get(Biome cityBiome)
    {
        var result = "";

        if (cityBiome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityHigh)
            result = "Fanatical worshipers devouting all their life to religion. ";

        if (cityBiome.CityState.Personality.Worshiper.Rate <= CityStateConstant.PersonalityFaded)
            result += "Questions of gods do not occupy their minds a lot. ";
        /*else if (cityBiome.CityState.Personality.Worshiper.Rate <= CityStateConstant.PersonalityLow)
            result += "They only sometimes think about the gods. ";*/

        if (cityBiome.CityState.Personality.Worshiper.Rate > CityStateConstant.PersonalityAvarage
            && cityBiome.CityState.Personality.Worshiper.Rate <= CityStateConstant.PersonalityHigh)
            result += "People are devout worshipers. ";

        if (result.Length > 0)
            result += Environment.NewLine;

        return result;
    }
}

public class GrowthStateStory : ICityStateSory
{
    public string Get(Biome cityBiome)
    {
        var result = "";

        var totalGrwoth = cityBiome.CityState.CurrentAge.TotalGrowthWithoutLostGrowth;

        var naturalGrowthRate = cityBiome.CityState.CurrentAge.NaturalGrowth / totalGrwoth;

        if (naturalGrowthRate > 0.9f) result += "People live of nature. They grow and build everything themself. ";

        if (result.Length > 0)
            result += Environment.NewLine;

        return result;
    }
}

public class ExplorationCityStateStory : ICityStateSory
{
    public string Get(Biome cityBiome)
    {
        int totalExplored = cityBiome.CityState.Explored.Count;
        int watersCount = cityBiome.CityState.Explored.Count(explored => explored.IsWater);
        int landsCount = totalExplored - watersCount;

        var canDoList = new List<string>();
        if (cityBiome.CityState.Personality.Rading.Rate > CityStateConstant.PersonalityAvarage)
            canDoList.Add("raid");
        if (cityBiome.CityState.Personality.Trading.Rate > CityStateConstant.PersonalityFaded)
            canDoList.Add("trade with");
        var canDo = string.Join(" or ", canDoList);

        if (watersCount > 10 && landsCount > 10)
        {
            var result = "Explorers mapped large area of seas and lands. ";
            return SetCanDo(result, canDo);
        }
        else if (watersCount > 10)
        {
            var result = "Explorers mapped large area of seas. ";
            return SetCanDo(result, canDo);
        }
        else if (landsCount > 10)
        {
            var result = "Explorers mapped large area of lands. ";
            return SetCanDo(result, canDo);
        }

        return "";
    }

    private string SetCanDo(string result, string canDo)
    {
        if (!String.IsNullOrEmpty(canDo))
            result += $"They can {canDo} cities far away. ";

        return result;
    }
}