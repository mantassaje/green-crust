using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : ManualUpdateBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Text;
    public Vector3 transformChange;

    public override void ManualUpdate()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Singles.UiController.Tooltip.Text.text = Text;
        Singles.UiController.Tooltip.gameObject.SetActive(true);

        Singles.UiController.Tooltip.transform.position = this.transform.position + transformChange;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Singles.UiController.Tooltip.gameObject.SetActive(false);
    }
}
