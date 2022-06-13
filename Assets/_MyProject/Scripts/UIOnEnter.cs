using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOnEnter : MonoBehaviour , IPointerEnterHandler
{
    // POINTERENTER BILGISI ALINCA SES OYNATIR
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.root.GetComponent<SoundManager>().PlayUISound(1f);
    }
}
