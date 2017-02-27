using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerExitHandler
{

    public GameObject GbRef;

    public event_Gb ActivationRequest, DeActivationRequest;

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
                this.GetComponentInChildren<Text>().fontSize = 30;
                break;
            case "But2":
                this.ActivationRequest.Invoke(this.GbRef);
                break;
            case "But3":
                break;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        switch (this.gameObject.tag)
        {
            case "But1":
                this.GetComponentInChildren<Text>().fontSize = 25;
                break;
            case "But2":
                this.DeActivationRequest.Invoke(this.GbRef);
                break;
            case "But3":
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
