using UnityEngine;

public class BiomeClimateDebug : MonoBehaviour {
    public Biome Biome;
    public SpriteRenderer QuantityVisual;

    private Vector3 _originalQuantityVisualSize;

    private void Start()
    {
        _originalQuantityVisualSize = QuantityVisual.transform.localScale;
    }

    private void Update()
    {
        if (Singles.World.ClimateDebugMode == ClimateDebugMode.None)
        {
            QuantityVisual.gameObject.SetActive(false);
        }
        else if (Singles.World.ClimateDebugMode == ClimateDebugMode.Coldness)
        {
            QuantityVisual.gameObject.SetActive(true);
            QuantityVisual.transform.localScale = _originalQuantityVisualSize * (Biome.Weather.Cold / Singles.World.AbyssColdBase);
            QuantityVisual.color = Color.magenta;
        }
        else
        {
            QuantityVisual.gameObject.SetActive(true);
            QuantityVisual.transform.localScale = _originalQuantityVisualSize * Biome.Weather.Humidity / Singles.World.OceanHumidityBase;
            QuantityVisual.color = Color.blue;
        }
    }
}
