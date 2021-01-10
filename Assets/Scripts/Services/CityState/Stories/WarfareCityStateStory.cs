using Assets.Code.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WarfareCityStateStory: ICityStateSory
{
    private enum Situation
    {
        Savages,
        Strong,
        AverageFighters,
        Weak,
        Pacifists,

        SuccesfullFullRaids,
        SuccesfullGuerillaRaids,
        FailedFullRaids,
        FailedGuerillaRaids,
        NoInitiatedAttacks,

        RaidingOnlySmallTribes,
        RaidingNotables,

        SuccesfullDefenseFromFullRaids,
        SuccesfullDefenseFromGuerillaRaids,
        FailedDefenseFromFullRaids,
        FailedDefenseFromGuerillaRaids,
        PeaceOnWalls,

        DefendingOnlyFromTribes,

        RaidedMoreThenHalf,
        RaidedMoreThenQuarter,
        LostMoreThenHalf,
        LostMoreThenQuarter,
        ZeroBooty,

        //TODO when raid success burn some growth. Not carried to raider city state.
    }

    public string Get(Biome cityBiome)
    {
        var situations = new List<Situation>();

        if (cityBiome.CityState.Personality.Rading.Rate <= CityStateConstant.PersonalityFaded)
            situations.Add(Situation.Pacifists);
        else if (cityBiome.CityState.Personality.Rading.Rate <= CityStateConstant.PersonalityLow)
            situations.Add(Situation.Weak);
        else if (cityBiome.CityState.Personality.Rading.Rate <= CityStateConstant.PersonalityAvarage)
            situations.Add(Situation.AverageFighters);
        else if (cityBiome.CityState.Personality.Rading.Rate <= CityStateConstant.PersonalityHigh)
            situations.Add(Situation.Strong);
        else situations.Add(Situation.Savages);

        if (cityBiome.CityState.CurrentAge.RaidedCities.Any())
        {
            if (cityBiome.CityState.CurrentAge.RaidedCities.Any(value => value.RaidIsSuccess && !value.GuerrillaRaid && value.VictimBiome.CityState.IsVisible))
                situations.Add(Situation.SuccesfullFullRaids);
            if (cityBiome.CityState.CurrentAge.RaidedCities.Any(value => value.RaidIsSuccess && value.GuerrillaRaid && value.VictimBiome.CityState.IsVisible))
                situations.Add(Situation.SuccesfullGuerillaRaids);
            if (cityBiome.CityState.CurrentAge.RaidedCities.Any(value => !value.RaidIsSuccess && !value.GuerrillaRaid))
                situations.Add(Situation.FailedFullRaids);
            if (cityBiome.CityState.CurrentAge.RaidedCities.Any(value => !value.RaidIsSuccess && value.GuerrillaRaid))
                situations.Add(Situation.FailedGuerillaRaids);

            if (cityBiome.CityState.CurrentAge.RaidedCities.All(value => !value.VictimBiome.CityState.IsVisible))
                situations.Add(Situation.RaidingOnlySmallTribes);
            if (cityBiome.CityState.CurrentAge.RaidedCities.Any(value => value.VictimBiome.CityState.IsVisible))
                situations.Add(Situation.RaidingNotables);
        }
        else
        {
            situations.Add(Situation.NoInitiatedAttacks);
        }

        if (cityBiome.CityState.CurrentAge.DefendedFromCities.Any())
        {
            if (cityBiome.CityState.CurrentAge.DefendedFromCities.Any(value => value.RaidIsSuccess && !value.GuerrillaRaid))
                situations.Add(Situation.FailedDefenseFromFullRaids);
            if (cityBiome.CityState.CurrentAge.DefendedFromCities.Any(value => value.RaidIsSuccess && value.GuerrillaRaid))
                situations.Add(Situation.FailedDefenseFromGuerillaRaids);
            if (cityBiome.CityState.CurrentAge.DefendedFromCities.Any(value => !value.RaidIsSuccess && !value.GuerrillaRaid))
                situations.Add(Situation.SuccesfullDefenseFromFullRaids);
            if (cityBiome.CityState.CurrentAge.DefendedFromCities.Any(value => !value.RaidIsSuccess && value.GuerrillaRaid))
                situations.Add(Situation.SuccesfullDefenseFromGuerillaRaids);

            if (cityBiome.CityState.CurrentAge.DefendedFromCities.All(value => !value.RaiderBiome.CityState.IsNotable))
                situations.Add(Situation.DefendingOnlyFromTribes);
        }
        else
        {
            situations.Add(Situation.PeaceOnWalls);
        }

        var totalGrowth = cityBiome.CityState.CurrentAge.TotalGrowthWithoutLostGrowth;
        var booty = cityBiome.CityState.CurrentAge.BootyGrowth;
        var lost = cityBiome.CityState.CurrentAge.TotalLoss;

        var bootyRate = totalGrowth <= 0 ? 0 : booty / totalGrowth;
        var lostRate = totalGrowth <= 0 ? 0 : lost / totalGrowth;

        if (bootyRate >= 0.5f) situations.Add(Situation.RaidedMoreThenHalf);
        if (bootyRate >= 0.25f) situations.Add(Situation.RaidedMoreThenQuarter);
        if (booty < 0.01) situations.Add(Situation.ZeroBooty);

        if (lostRate >= 0.5f) situations.Add(Situation.LostMoreThenHalf);
        if (lostRate >= 0.25f) situations.Add(Situation.LostMoreThenQuarter);

        return EnumsToText(cityBiome, situations);
    }

    private string EnumsToText(Biome cityBiome, IEnumerable<Situation> situations)
    {
        var result = new StringBuilder();

        if (situations.ContainsAtLeastOne(Situation.Savages))
        {
            result.Append("People are savage and furious warriors. ");
                
        }
        else if (situations.ContainsAtLeastOne(Situation.Strong))
            result.Append("People are strong and aggressive. ");
        /*else if (situations.ContainsAtLeastOne(Situation.Weak))
            result.Append("They dislake fighting. ");*/
        else if (situations.ContainsAtLeastOne(Situation.Pacifists))
            result.Append("They are afraid to spill blood. ");

        if (situations.ContainsAtLeastOne(Situation.Savages, Situation.Strong)
            && situations.ContainsAtLeastOne(Situation.ZeroBooty))
            result.Append("Although they could not raid anything from other cities or tribes. ");

        if (situations.ContainsAtLeastOne(/*Situation.Weak,*/ Situation.Pacifists)
            && situations.ContainsAtLeastOne(Situation.LostMoreThenQuarter, Situation.LostMoreThenHalf))
            result.Append("But reluctance to fight cost them a lot. ");

        if (situations.ContainsAtLeastOne(Situation.RaidingOnlySmallTribes))
            result.Append("They raid small tribes. ");

        if (situations.ContainsAtLeastOne(Situation.SuccesfullFullRaids))
        {
            result.Append("They succesfuly make full fledge assaults on city walls. ");
            var mostNotableAttack = cityBiome.CityState.CurrentAge.RaidedCities
                .OrderByDescending(value => value.Booty)
                .FirstOrDefault(value => value.RaidIsSuccess && value.VictimBiome.CityState.IsNotable);

            if (mostNotableAttack != null)
                result.Append($"Largest spoils came from {mostNotableAttack.VictimBiome.CityState.Name}. ");

            if (situations.ContainsAtLeastOne(Situation.FailedFullRaids))
                result.Append("Although some other attacks on city walls failed. ");
        }
        else if (situations.ContainsAtLeastOne(Situation.FailedFullRaids))
            result.Append("Unfortunately all their attacks on the cities failed. ");
        else if (situations.ContainsAtLeastOne(Situation.SuccesfullGuerillaRaids))
            result.Append("They ambush traiders and samller detached patrols. And run away with spoils. ");
        else if (situations.ContainsAtLeastOne(Situation.FailedGuerillaRaids))
            //Can be changed to skirmishes
            result.Append("They attempt ambushes on traiders and samller detached patrols. But victims are well protected and ambushers retreated without much spoils. ");

        if (situations.ContainsAtLeastOne(Situation.RaidedMoreThenHalf))
            result.Append("More then half of riches they possess come from raids. ");
        else if (situations.ContainsAtLeastOne(Situation.RaidedMoreThenQuarter))
            result.Append("More then quarter of riches they possess come from raids. ");

        if (situations.ContainsAtLeastOne(Situation.DefendingOnlyFromTribes))
            result.Append("Various small tribes are terrorizing this city. ");

        if (situations.ContainsAtLeastOne(Situation.FailedDefenseFromFullRaids))
            result.Append("Devestating attack was launched on city walls. A lot of people were killed and riches were plundered. ");
        else if (situations.ContainsAtLeastOne(Situation.FailedDefenseFromGuerillaRaids))
            result.Append("It is not safe outside the walls. Ambushes happen that people can not protect them self from. ");
        else if (situations.ContainsAtLeastOne(Situation.SuccesfullDefenseFromFullRaids))
            result.Append("Major attacks on city walls were stopped. ");
        else if (situations.ContainsAtLeastOne(Situation.SuccesfullDefenseFromGuerillaRaids))
            result.Append("A lot of ambushes are stoped by patrols outside the walls. ");

        var mostNotableLoss = cityBiome.CityState.CurrentAge.DefendedFromCities
                .OrderByDescending(value => value.Booty)
                .FirstOrDefault(value => value.RaidIsSuccess && value.RaiderBiome.CityState.IsNotable);
        if (mostNotableLoss != null)
        {
            result.Append($"The worst plunderes they could not protect from are {mostNotableLoss.RaiderBiome.CityState.Name}. ");
        }

        if (situations.ContainsAtLeastOne(Situation.LostMoreThenHalf))
            result.Append("More then half of riches are lost to plunders. ");
        else if (situations.ContainsAtLeastOne(Situation.LostMoreThenQuarter))
            result.Append("More then quarter of riches are lost to plunders. ");

        if (result.Length > 0)
            result.AppendLine();

        return result.ToString();
    }
}
