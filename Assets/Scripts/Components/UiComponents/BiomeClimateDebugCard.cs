using System.Collections;
using UnityEngine.UI;

public class BiomeClimateDebugCard : ManualUpdateBehaviour
{
    private IEnumerator rainfallEnum;
    private IEnumerator coldEnum;

    public Button ShowRainButton;
    public Button ShowColdButton;
    public Button HideButton;
    public Button ResetButton;
    public Button NextButton;

    private void Start()
    {
        ShowRainButton.onClick.AddListener(() =>
        {
            Singles.World.ClimateDebugMode = ClimateDebugMode.Rainfall;
        });

        ShowColdButton.onClick.AddListener(() =>
        {
            Singles.World.ClimateDebugMode = ClimateDebugMode.Coldness;
        });

        HideButton.onClick.AddListener(() =>
        {
            Singles.World.ClimateDebugMode = ClimateDebugMode.None;
        });

        ResetButton.onClick.AddListener(() =>
        {
            RainfallEmitService.ResetWorldHumidity();
            HeatService.ResetWorldHeat();

            rainfallEnum = new RainfallEmitHandler().DoEmisionsCoroutine();
            coldEnum = new ColdEmitHandler().DoEmisionsCoroutine();
        });

        NextButton.onClick.AddListener(() =>
        {
            rainfallEnum.MoveNext();
            coldEnum.MoveNext();
        });
    }
}
