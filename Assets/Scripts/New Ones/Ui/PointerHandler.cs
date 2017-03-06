using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerExitHandler
{

    public GameObject GbRef;

    public event_Gb ActivationRequest, DeActivationRequest;

    private int personalFontSize;

    private void Awake()
    {
        switch (this.gameObject.tag)
        {
            case "But1":
                this.personalFontSize = this.GetComponentInChildren<Text>().fontSize;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>().PlaySound(1, 1);
     
        switch (this.gameObject.tag)
        {
            case "But1":
                this.GetComponentInChildren<Text>().fontSize = (int)(this.personalFontSize * 1.2f);
                break;
            case "But2":
                this.ActivationRequest.Invoke(this.GbRef);
                break;
            case "But3":
                this.ActivationRequest.Invoke(this.GbRef);
                break;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        switch (this.gameObject.tag)
        {
            case "But1":
                this.GetComponentInChildren<Text>().fontSize = this.personalFontSize;
                break;
            case "But2":
                this.DeActivationRequest.Invoke(this.GbRef);
                break;
            case "But3":
                this.DeActivationRequest.Invoke(this.GbRef);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
