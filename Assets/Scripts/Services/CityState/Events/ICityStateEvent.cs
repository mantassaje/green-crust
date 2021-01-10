public interface ICityStateEvent
{
    string GetTitle(Biome cityBiome);
    string GetIntroudctionText(Biome cityBiome);
    string GetCursedText(Biome cityBiome);
    string GetBlessedText(Biome cityBiome);

    string CurseButtonText(Biome cityBiome);
    string BlesseButtonText(Biome cityBiome);

    bool CanHappen(Biome cityBiome);

    void Bless(Biome cityBiome);
    void Curse(Biome cityBiome);
    void ApplyIgnored(Biome cityBiome);
}
