using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RaidData
{
    public Biome VictimBiome { get; set; }
    public Biome RaiderBiome { get; set; }
    public bool RaidIsSuccess { get; set; }
    public bool GuerrillaRaid { get; set; }
    public float Booty { get; set; }
}
