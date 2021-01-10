public class CityStateConstant
{
    public const float InpassableHeight = 3f;

    public const float NatureGrowthMultiplyer = 0.015f;
    public const float BonusPerPopMultiplyer = 0.5f;
    public const float PopulationConsumptionMultiplyer = 0.2f;

    public const float BaseTradeMultiplyer = 0.30f;
    public const float InitiatedTradeMultiplyer = 1f;
    public const float MarketTradeMultiplyer = 1f;

    /// <summary>
    /// Of raiders personality rate and raiders population.
    /// </summary>
    public const float GuerrillaRaidCarryBootyMultiplyer = 0.6f;
    /// <summary>
    /// Of raiders personality rate and raiders population.
    /// </summary>
    public const float FullRaidCarryBootyMultiplyer = 0.9f;

    /// <summary>
    /// Off victims growth.
    /// </summary>
    public const float GuerrillaRaidBootyMultiplyer = 0.2f;
    /// <summary>
    /// Off victims growth.
    /// </summary>
    public const float RaidBurnMultiplyer = 0.3f;
    /// <summary>
    /// Off victims growth.
    /// </summary>
    public const float FullRaidBootyMultiplyer = 0.4f;

    public const float AttackMultiplyer = 1f;
    public const float DefenseMultiplyer = 1.3f;
    public const float FullRaidVictimPopModifier = 0.75f;
    public const float FullRaidRaiderPopModifier = 0.9f;

    public const float WorshiperManaMultiplyer = 3f;

    public static float PersonalityAxisCount { get; private set; }

    /// <summary>
    /// Low below or equal. Above average.
    /// </summary>
    public static float PersonalityLow { get; private set; }

    /// <summary>
    /// Avarage below or equal.
    /// </summary>
    public static float PersonalityAvarage { get; private set; }

    /// <summary>
    /// Faded below or equal. Above low.
    /// </summary>
    public static float PersonalityFaded { get; private set; }

    /// <summary>
    /// High below or equal.
    /// Dominant above.
    /// </summary>
    public static float PersonalityHigh { get; private set; }

    public static float PersonalityBigChange { get; private set; }
    public static float PersonalityMidChange { get; private set; }
    public static float PersonalityLowChange { get; private set; }

    static CityStateConstant()
    {
        /*PersonalityAxisCount = new CityStatePersonality().AxisCount;

        PersonalityAvarage = 1f / PersonalityAxisCount;
        PersonalityFaded = PersonalityAvarage * 0.4f;
        PersonalityDominant = 1f - PersonalityAvarage;

        PersonalityEventChange = PersonalityAvarage * 0.8f;
        PersonalityLowEventChange = PersonalityFaded * 0.5f;*/

        PersonalityAxisCount = new CityStatePersonality().AxisCount;

        //0 Faded 0.2 Low 0.4 Average 0.6 High 0.8 Dominant 1
        PersonalityFaded = 0.2f;
        PersonalityLow = 0.4f;
        PersonalityAvarage = 0.6f;
        PersonalityHigh = 0.8f;

        PersonalityBigChange = 0.4f;
        PersonalityMidChange = 0.3f;
        PersonalityLowChange = 0.2f;
    }
}