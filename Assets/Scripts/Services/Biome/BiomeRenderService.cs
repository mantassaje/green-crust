using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class BiomeRenderService
{
    public static Color GetCloudColor(Biome biome)
    {
        var zoomAlphaScale = (Singles.CameraControlls.Camera.orthographicSize / 40f).GetMinMax(0f, 1f);
        return new Color(1, 1, 1, zoomAlphaScale);
    }

    public static Color GetGlowColor(Biome biome)
    {
        if (biome.ClimateVisual == BomeClimateVisual.Wait) return Color.white;
        if (biome.Weather.HeatType == HeatTypes.Hot)
        {
            var heatColor = new Color(1f, 0.9f, 0.8f);
            if (biome.ClimateVisual == BomeClimateVisual.Brighten)
            {
                return Color.Lerp(Color.white, heatColor, biome.ClimateVisualTiming.TimeLeftUntilBeginTimeRate);
            }
            if (biome.ClimateVisual == BomeClimateVisual.Fade)
            {
                return Color.Lerp(Color.white, heatColor, 1f - biome.ClimateVisualTiming.TimeLeftUntilBeginTimeRate);
            }
        }

        return Color.white;
    }

    public static Color GetGlowColorToAdd(Biome biome)
    {
        var glowColor = GetGlowColor(biome);
        return (Color.white - glowColor) / 2f;
    }

    public static Color GetHeatFilter(Biome biome)
    {
        return Color.Lerp(
                Color.white,
                new Color(0.9f, 0.85f, 0.7f),
                1f - biome.Weather.GetHeatRate());
    }

    public static Color GetGroundColor(Biome biome)
    {
        if (biome.Spec is FrozenDesertBiomeSpec) return biome.Spec.BaseColor;
        var desertColor = Color.Lerp(
                Singles.Colors.ColdDesert,
                Singles.Colors.Desert,
                biome.Weather.GetHeatRate());
        if (biome.Spec is HotDesertBiomeSpec) return desertColor;
        return Color.Lerp(desertColor, Singles.Colors.Uninhabited, 0.6f);

    }

    public static void SetUiLines(Biome biome)
    {
        if (ReferenceEquals(Singles.PlayerController.SelectedBiome, biome))
        {
            biome.UiLines.SetLevel(1);
            biome.UiLines.SetColor(Color.yellow);
        }
        else if (ReferenceEquals(Singles.PlayerController.HoverBiome, biome))
        {
            biome.UiLines.SetLevel(1);
            biome.UiLines.SetColor(Color.white);
        }
        else
        {
            biome.UiLines.SetLevel(0);
        }
    }

    public static string GetManaGainInfo(Biome biome)
    {
        var texts = ManaService.ManaBonuses
            .Where(bonus => bonus.GetManaBonus(biome) != 0)
            .Select(bonus => bonus.GetText(biome));

        if (texts.Any())
        {
            return string.Format("Energy{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, texts.ToArray()));
        }

        return null;
    }

    public static string GetBiomeDebugText(Biome biome)
    {
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine("Energy gain " + biome.GetManaOutput());
        strBuilder.AppendLine();
        strBuilder.AppendLine("Nature cap " + BiomeNatureService.GetNatureCap(biome));
        strBuilder.AppendLine("Nature growth " + BiomeNatureService.GetNatureGrowth(biome));
        strBuilder.AppendLine("Nature " + biome.Nature.Population.GetRounded());
        strBuilder.AppendLine("Nature maturity " + biome.Nature.Maturity.GetRounded());
        strBuilder.AppendLine();
        strBuilder.AppendLine("Rainfall " + biome.Weather.Humidity.GetRounded());
        strBuilder.AppendLine("Heat " + biome.Weather.Heat.GetRounded());
        strBuilder.AppendLine("Height " + biome.Crust.Height);
        strBuilder.AppendLine();
        if (!biome.CityState.IsDead)
        {
            strBuilder.AppendLine("City pop " + biome.CityState.Population.GetRounded());
            strBuilder.AppendLine("City growth " + CityStateInfoService.GetBaseGrowth(biome).GetRounded());
            strBuilder.AppendLine("Civ " + biome.CityState.Name);
        }
        return strBuilder.ToString();
    }

    public static void BorderDrawLogic(Biome biome)
    {
        if (biome.Owner.IsNull() || Singles.World.IsSandbox)
        {
            biome.DrawTopBorder = false;
            biome.DrawBotBorder = false;
            biome.DrawTopLeftBorder = false;
            biome.DrawTopRightBorder = false;
            biome.DrawBotLeftBorder = false;
            biome.DrawBotRightBorder = false;
            return;
        }

        var topB = Singles.Grid.GetTop(biome);
        var botB = Singles.Grid.GetBot(biome);
        var topleftB = Singles.Grid.GetTopLeft(biome);
        var botleftB = Singles.Grid.GetBotLeft(biome);
        var toprightB = Singles.Grid.GetTopRight(biome);
        var botrightB = Singles.Grid.GetBotRight(biome);

        biome.DrawTopBorder = topB.IsNull() ? false : !topB.IsOwner(biome.Owner);
        biome.DrawBotBorder = botB.IsNull() ? false : !botB.IsOwner(biome.Owner);
        biome.DrawTopLeftBorder = topleftB.IsNull() ? false : !topleftB.IsOwner(biome.Owner);
        biome.DrawTopRightBorder = toprightB.IsNull() ? false : !toprightB.IsOwner(biome.Owner);
        biome.DrawBotLeftBorder = botleftB.IsNull() ? false : !botleftB.IsOwner(biome.Owner);
        biome.DrawBotRightBorder = botrightB.IsNull() ? false : !botrightB.IsOwner(biome.Owner);
    }

    public static void BorderDraw(Biome biome)
    {
        if (biome.DrawBotBorder)
            biome.BotBorder.SetLevel(1);
        else biome.BotBorder.SetLevel(0);

        if (biome.DrawTopBorder)
            biome.TopBorder.SetLevel(1);
        else biome.TopBorder.SetLevel(0);

        if (biome.DrawTopLeftBorder)
            biome.TopLeftBorder.SetLevel(1);
        else biome.TopLeftBorder.SetLevel(0);

        if (biome.DrawBotLeftBorder)
            biome.BotLeftBorder.SetLevel(1);
        else biome.BotLeftBorder.SetLevel(0);

        if (biome.DrawTopRightBorder)
            biome.TopRightBorder.SetLevel(1);
        else biome.TopRightBorder.SetLevel(0);

        if (biome.DrawBotRightBorder)
            biome.BotRightBorder.SetLevel(1);
        else biome.BotRightBorder.SetLevel(0);

        if (biome.Owner != null)
        {
            biome.TopBorder.SetColor(biome.Owner.Color);
            biome.BotBorder.SetColor(biome.Owner.Color);
            biome.TopLeftBorder.SetColor(biome.Owner.Color);
            biome.BotLeftBorder.SetColor(biome.Owner.Color);
            biome.TopRightBorder.SetColor(biome.Owner.Color);
            biome.BotRightBorder.SetColor(biome.Owner.Color);
        }
    }

    public static string GetCelsius(Biome biome)
    {
        var cels = (int)(biome.Weather.Heat - 2) * 5;
        if (cels < 0) return cels.ToString() + " C";
        else if (cels > 0) return "+" + cels + " C";
        else return "0 C";
    }

    public static string GetRainfallCm(Biome biome)
    {
        var cm = (int)(biome.Weather.Humidity) * 25;
        return cm + " cm";
    }

    public static Sprite GetCloudSprite(Biome biome)
    {
        var clouds = Singles.Sprites.Clouds.GetSafe((int)biome.Weather.RainfallType);

        if (clouds == null || clouds.Count == 0) return null;

        return clouds[biome.Weather.Sorter % clouds.Count];
    }

    public static void HideAllUiElements(Biome biome)
    {
        biome.ManaGain.SetLevel(0);
        biome.Ancients.enabled = false;

        biome.DrawTopBorder = false;
        biome.DrawBotBorder = false;
        biome.DrawTopLeftBorder = false;
        biome.DrawTopRightBorder = false;
        biome.DrawBotLeftBorder = false;
        biome.DrawBotRightBorder = false;

        BorderDraw(biome);

        biome.UiLines.SetLevel(0);
    }
}

