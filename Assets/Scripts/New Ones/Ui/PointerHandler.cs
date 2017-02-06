using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ISubmitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    { 
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>().PlaySound(1, 1);
        this.GetComponentInChildren<Text>().fontSize = 30;
    }

    public void OnDeselect(BaseEventData eventData)
    {
    
        this.GetComponentInChildren<Text>().fontSize = 25;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Cliccato");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("Cliccato");
    }
}
