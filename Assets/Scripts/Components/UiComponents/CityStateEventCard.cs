using System.Text;
using UnityEngine.UI;

public class CityStateEventCard : ManualUpdateBehaviour
{
    private Biome _cityBiome;

    public Text Text;
    public Text Title;

    public Text BlessButtonText;
    public Text CurseButtonText;

    public Button BlessButton;
    public Button IgnoreButton;
    public Button CurseButton;
    public Button OkButton;

    private void Start()
    {
        gameObject.SetActive(false);

        BlessButton.onClick.AddListener(Bless);
        IgnoreButton.onClick.AddListener(Close);
        OkButton.onClick.AddListener(Close);
        CurseButton.onClick.AddListener(Curse);
    }

    private void Close()
    {
        gameObject.SetActive(false);
        _cityBiome = null;
    }

    private void Bless()
    {
        //Replace to PlayerActions
        _cityBiome.CityState.Event.Bless(_cityBiome);
        _cityBiome.CityState.EventStatus = CityStateEventStatus.Blessed;
    }

    private void Curse()
    {
        //Replace to PlayerActions
        _cityBiome.CityState.Event.Curse(_cityBiome);
        _cityBiome.CityState.EventStatus = CityStateEventStatus.Cursed;
    }

    public override void ManualUpdate()
    {
        if (_cityBiome.IsNull()
            || _cityBiome.CityState.Event == null
            || _cityBiome != Singles.PlayerController.SelectedBiome
            || (!Singles.World.IsSandbox && _cityBiome.Owner != Singles.PlayerController.Player))
        {
            gameObject.SetActive(false);
            return;
        }

        BlessButton.gameObject.SetActive(_cityBiome.CityState.EventStatus == CityStateEventStatus.Ignored);
        CurseButton.gameObject.SetActive(_cityBiome.CityState.EventStatus == CityStateEventStatus.Ignored);
        IgnoreButton.gameObject.SetActive(_cityBiome.CityState.EventStatus == CityStateEventStatus.Ignored);
        OkButton.gameObject.SetActive(_cityBiome.CityState.EventStatus != CityStateEventStatus.Ignored);

        Title.text = _cityBiome.CityState.Event.GetTitle(_cityBiome);
        var text = new StringBuilder();
        text.AppendLine(_cityBiome.CityState.Event.GetIntroudctionText(_cityBiome));

        if (_cityBiome.CityState.EventStatus != CityStateEventStatus.Ignored)
        {
            text.AppendLine();
            text.AppendLine(_cityBiome.CityState.EventStatus == CityStateEventStatus.Blessed 
                ? _cityBiome.CityState.Event.GetBlessedText(_cityBiome)
                : _cityBiome.CityState.Event.GetCursedText(_cityBiome));
        }

        Text.text = text.ToString();

        var blessText = _cityBiome.CityState.Event.BlesseButtonText(_cityBiome);
        BlessButtonText.text = string.IsNullOrEmpty(blessText) ? "Bless" : blessText;

        var curseText = _cityBiome.CityState.Event.CurseButtonText(_cityBiome);
        CurseButtonText.text = string.IsNullOrEmpty(curseText) ? "Curse" : curseText;
    }

    public void Open(Biome cityBiome)
    {
        _cityBiome = cityBiome;
        gameObject.SetActive(true);
        cityBiome.CityState.EventIsRead = true;
    }
}
