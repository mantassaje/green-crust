using Assets.Code.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TradeCityStateStory : ICityStateSory
{
    private enum Situation
    {
        TradeHater,
        PassiveTrader,
        AverageTrader,
        ActiveTrader,
        DominantTrader,

        NoInitaidedTrading,
        PoorInitiatedTrading,
        AnyHighInitiatedTradingGrowth,
        OnlyWithTribesInitiatedTrade,

        OnlyTribesComeToMarket,
        LotsOfTribesComeToMarket,
        PopularMarket,
        NoMarket,
        PoorMarket,
        AnyHighMarketTrade,

        AnyTradeWithNotable,
        GainMoreThenQuarter,
        GainMoreThenHalf
    }

    public string Get(Biome cityBiome)
    {
        var situations = new List<Situation>();

        if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityFaded)
            situations.Add(Situation.TradeHater);
        else if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityLow)
            situations.Add(Situation.PassiveTrader);
        else if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityAvarage)
            situations.Add(Situation.AverageTrader);
        else if (cityBiome.CityState.Personality.Trading.Rate <= CityStateConstant.PersonalityHigh)
            situations.Add(Situation.ActiveTrader);
        else situations.Add(Situation.DominantTrader);

        var initiatedTraders = cityBiome.CityState.CurrentAge.Trades.Where(trade => trade.Client == cityBiome);
        if (initiatedTraders.Any())
        {
            if (initiatedTraders.All(trade => trade.ClientGain < 0.02f))
                situations.Add(Situation.PoorInitiatedTrading);
            else if (initiatedTraders.Any(trade => trade.ClientGain > 0.1f))
                situations.Add(Situation.AnyHighInitiatedTradingGrowth);

            if (initiatedTraders.All(trade => !trade.Market.CityState.IsNotable))
                situations.Add(Situation.OnlyWithTribesInitiatedTrade);
        }
        else
        {
            situations.Add(Situation.NoInitaidedTrading);
        }

        var marketTrades = cityBiome.CityState.CurrentAge.Trades.Where(trade => trade.Market == cityBiome);
        if (marketTrades.Any())
        {
            if (cityBiome.CityState.CurrentAge.MarketTradeGrowth > 0.3f)
                situations.Add(Situation.PopularMarket);

            if (marketTrades.All(trade => !trade.Market.CityState.IsNotable))
            {
                situations.Add(Situation.OnlyTribesComeToMarket);
                situations.Add(Situation.LotsOfTribesComeToMarket);
            }
            else
            {
                float tribeCount = marketTrades.Count(trade => !trade.Market.CityState.IsNotable);
                float total = marketTrades.Count();

                var tribeRate = total / tribeCount;
                if (tribeRate > 0.8f)
                    situations.Add(Situation.LotsOfTribesComeToMarket);
            }

            if (marketTrades.All(trade => trade.MarketGain < 0.02f))
                situations.Add(Situation.PoorMarket);
            else if (marketTrades.Any(trade => trade.MarketGain > 0.1f))
                situations.Add(Situation.AnyHighMarketTrade);
        }
        else
        {
            situations.Add(Situation.NoMarket);
        }

        if (initiatedTraders.Any(trade => trade.Market.CityState.IsNotable)
            || marketTrades.Any(trade => trade.Client.CityState.IsNotable))
            situations.Add(Situation.AnyTradeWithNotable);

        var totalTradeGrowth = cityBiome.CityState.CurrentAge.ClientTradeGrowth + cityBiome.CityState.CurrentAge.MarketTradeGrowth;
        var tradeRate = totalTradeGrowth / cityBiome.CityState.CurrentAge.TotalGrowthWithoutLostGrowth;

        if (tradeRate >= 0.5f)
            situations.Add(Situation.GainMoreThenHalf);
        else if (tradeRate >= 0.25f)
            situations.Add(Situation.GainMoreThenQuarter);

        return EnumsToText(cityBiome, situations);
    }

    private string EnumsToText(Biome cityBiome, IEnumerable<Situation> situations)
    {
        var result = new StringBuilder();

        if (situations.ContainsAtLeastOne(Situation.DominantTrader))
            result.Append("People live only to put something into market. ");
        else if (situations.ContainsAtLeastOne(Situation.ActiveTrader))
            result.Append("People are active traders. ");
        /*else if (situations.ContainsAtLeastOne(Situation.PassiveTrader))
            result.Append("They see little use from trading. ");*/
        else if (situations.ContainsAtLeastOne(Situation.TradeHater))
            result.Append("They think other activities are far more profitable than trade. ");

        if (situations.ContainsAtLeastOne(Situation.TradeHater/*, Situation.PassiveTrader*/))
        {
            if (situations.ContainsAtLeastOne(Situation.GainMoreThenQuarter, Situation.GainMoreThenHalf,
                Situation.AnyHighMarketTrade, Situation.AnyHighInitiatedTradingGrowth))
                result.Append("Yet city does grow from trade. ");
        }

        if (situations.ContainsAll(Situation.LotsOfTribesComeToMarket, Situation.OnlyWithTribesInitiatedTrade))
            result.Append("City trades only mostly with various tribes. ");
        else if (situations.ContainsAtLeastOne(Situation.LotsOfTribesComeToMarket))
            result.Append("Various tribes come to cities market. ");
        else if (situations.ContainsAtLeastOne(Situation.OnlyWithTribesInitiatedTrade))
            result.Append("Cities merchents only travel to various tribes. ");

        if (situations.ContainsAtLeastOne(Situation.PoorInitiatedTrading)
            && situations.ContainsAtLeastOne(Situation.DominantTrader, Situation.ActiveTrader))
        {
            result.Append("Yet they could not find good markets in other cities or tribes. ");
        }
        
        if (situations.ContainsAtLeastOne(Situation.NoMarket)
            && situations.ContainsAtLeastOne(Situation.DominantTrader, Situation.ActiveTrader))
        {
            result.Append("Nobody is interested to come to this city for trading. ");
        }
        else if (situations.ContainsAtLeastOne(Situation.PoorMarket)
            && situations.ContainsAtLeastOne(Situation.DominantTrader, Situation.ActiveTrader))
        {
            result.Append("Very little riches come from market inside city. ");
        }

        if (situations.ContainsAtLeastOne(Situation.AnyHighInitiatedTradingGrowth, Situation.AnyHighMarketTrade))
        {
            var initiatedTraders = cityBiome.CityState.CurrentAge.Trades.Where(trade => trade.Market.CityState.IsNotable && trade.Client == cityBiome);
            var marketTrades = cityBiome.CityState.CurrentAge.Trades.Where(trade => trade.Client.CityState.IsNotable && trade.Market == cityBiome);

            var highestInitiatedTrade = initiatedTraders
                .OrderByDescending(trade => trade.ClientGain)
                .FirstOrDefault();
            var highestMarketTrade = marketTrades
                .OrderByDescending(trade => trade.MarketGain)
                .FirstOrDefault();

            if (highestInitiatedTrade != null 
                && highestInitiatedTrade?.ClientGain > highestMarketTrade?.MarketGain)
                result.Append($"{highestInitiatedTrade.Market.CityState.Name} is the richest market that traders traveling to. ");
            else if (highestMarketTrade != null)
                result.Append($"{highestMarketTrade.Client.CityState.Name} is from where the richest traders come from. ");
        }

        if (situations.ContainsAtLeastOne(Situation.GainMoreThenHalf))
            result.Append("More then half of riches come from trade. ");
        else if (situations.ContainsAtLeastOne(Situation.GainMoreThenQuarter))
            result.Append("More then quarter of riches come from trade. ");

        if(result.Length > 0)
            result.AppendLine();

        return result.ToString();
    }
}
