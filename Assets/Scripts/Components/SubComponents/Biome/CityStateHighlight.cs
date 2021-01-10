using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CityStateHighlight : MonoBehaviour
{
    private SpriteRenderer Sprite;

    public SpriteRenderer Icon1;
    public SpriteRenderer Icon2;
    public SpriteRenderer EventIsPresentIcon;
    public Color ExploredColor;

    public Sprite MapIcon;
    public Sprite TradeIcon;
    public Sprite SuccesfullFightIcon;
    public Sprite FailedFightIcon;
    public Sprite SuccesfullCityAttackIcon;
    public Sprite FailedCityAttackIcon;

    public Biome BiomeWithIcons;

    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Sprite.enabled = false;
        Icon1.enabled = false;
        Icon2.enabled = false;
        EventIsPresentIcon.enabled = false;

        if (BiomeWithIcons.CityState.IsVisible
            && BiomeWithIcons.CityState.Event != null
            && BiomeWithIcons.CityState.EventStatus == CityStateEventStatus.Ignored
            && (BiomeWithIcons.Owner == Singles.PlayerController.Player || Singles.World.IsSandbox))
        {
            EventIsPresentIcon.enabled = true;
            EventIsPresentIcon.color = BiomeWithIcons.CityState.EventIsRead ? Color.white : Color.red;
        }

        if (Singles.PlayerController.SelectedBiome.IsNotNull()
            && Singles.PlayerController.SelectedBiome.CityState.IsVisible
            && BiomeWithIcons.CityState.IsVisible
            && BiomeWithIcons != Singles.PlayerController.SelectedBiome)
        {
            var raided = Singles.PlayerController.SelectedBiome.CityState.CurrentAge.RaidedCities
                .FirstOrDefault(value => value.VictimBiome == BiomeWithIcons);
            var defendedFrom = Singles.PlayerController.SelectedBiome.CityState.CurrentAge.DefendedFromCities
                .FirstOrDefault(value => value.RaiderBiome == BiomeWithIcons);

            var trader = Singles.PlayerController.SelectedBiome.CityState.CurrentAge.Trades
                .FirstOrDefault(trade => trade.Client == BiomeWithIcons);
            if (trader == null) trader = Singles.PlayerController.SelectedBiome.CityState.CurrentAge.Trades
                .FirstOrDefault(trade => trade.Market == BiomeWithIcons);

            if (raided != null
                && !raided.GuerrillaRaid
                && raided.RaidIsSuccess)
            {
                Icon2.enabled = true;
                Icon2.sprite = SuccesfullCityAttackIcon;
            }
            else if (raided != null
                && !raided.GuerrillaRaid
                && !raided.RaidIsSuccess)
            {
                Icon2.enabled = true;
                Icon2.sprite = FailedCityAttackIcon;
            }
            else if (raided != null && raided.RaidIsSuccess
                || defendedFrom != null && !defendedFrom.RaidIsSuccess)
            {
                Icon2.enabled = true;
                Icon2.sprite = SuccesfullFightIcon;
            }
            else if (raided != null && !raided.RaidIsSuccess
                || defendedFrom != null && defendedFrom.RaidIsSuccess)
            {
                Icon2.enabled = true;
                Icon2.sprite = FailedFightIcon;
            }
            else if (trader != null)
            {
                Icon2.enabled = true;
                Icon2.sprite = TradeIcon;
            }

            if (Singles.PlayerController.SelectedBiome.CityState.Explored
                .SelectMany(explored => explored.GetNearbyBiomesCache())
                .Any(expolred => expolred == BiomeWithIcons))
            {
                Icon1.enabled = true;
                Icon1.sprite = MapIcon;
            }
        }
    }
}
