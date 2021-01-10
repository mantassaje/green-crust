using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface INatureGrowth
{
    /// <summary>
    /// Should be between 0.0 and 1.0.
    /// Is growth based on rainfall and heat 
    /// </summary>
    NatureGrowthModel GetChange(Biome biome, NatureGrowthModel model);
}
