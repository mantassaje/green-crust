using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BiomeInfoCard : ManualUpdateBehaviour
{

    public Text TitleText;
    public Text InfoText;
    public Text NatureCountText;
    public Text AncientsText;
    public Text ManaGainText;
    public Text OwnerText;
    public Text HeatText;
    public Text RainfallText;

    public Image AncientImage;

    public ProgressBar NatureProgressBar;
    public ProgressBar AncientsProgressBar;
    public ProgressBar HeatBar;
    public ProgressBar HumidityBar;

    public override void ManualUpdate()
    {
        var biome = Singles.PlayerController.SelectedBiome;
        if (biome.IsNotNull())
        {
            if (biome.Owner.IsNotNull()) OwnerText.text = biome.Owner.Name;
            else OwnerText.text = null;

            TitleText.text = biome.Spec.Name;
            InfoText.text = biome.Spec.GetDescription(biome);

            HeatText.text = $"Climate {biome.Weather.HeatType.ToString()}";
            RainfallText.text = $"Rainfall {biome.Weather.RainfallType.ToString()}";

            AncientImage.sprite = biome.Spec.Ancient;
            AncientImage.gameObject.SetActive(biome.Spec.Ancient != null);

            HeatBar.BarProgressRate = biome.Weather.Heat / 10f;
            HumidityBar.BarProgressRate = BiomeService.GetHeatRainDiff(biome) / 10f;

            var natureCap = (int)BiomeNatureService.GetNatureCap(biome);
            var naturePop = (int)biome.Nature.Population;
            NatureCountText.text = string.Format("Wildlife pop. {0} / {1}", naturePop, natureCap);

            if(biome.Nature.Population > 0)
            {
                NatureCountText.gameObject.SetActive(true);
                NatureProgressBar.gameObject.SetActive(true);

                if (biome.Nature.Population <= 0) NatureProgressBar.BarProgressRate = 0;
                else if (natureCap <= naturePop) NatureProgressBar.BarProgressRate = 1;
                else NatureProgressBar.BarProgressRate = biome.Nature.Population - naturePop;
            }
            else
            {
                NatureCountText.gameObject.SetActive(false);
                NatureProgressBar.gameObject.SetActive(false);
            }

            if (biome.IsWater || biome.Nature.Population >= 1f)
            {
                AncientsText.gameObject.SetActive(true);
                AncientsProgressBar.gameObject.SetActive(true);
                AncientsProgressBar.BarProgressRate = biome.Nature.Maturity / BiomeConstant.AncientMax;
            }
            else
            {
                AncientsText.gameObject.SetActive(false);
                AncientsProgressBar.gameObject.SetActive(false);
            }

            ManaGainText.text = BiomeRenderService.GetManaGainInfo(biome);
        }
    }
}
