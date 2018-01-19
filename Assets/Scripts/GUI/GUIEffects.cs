using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIEffects : MonoBehaviour {

    private CanvasRenderer canvasTemplate;
    private EventTrigger eventTrigg;
    //-------- Metodos API-----------------
    //Al iniciar, se consiguen los componentes del canvas GUI y del event trigger para luego activar
    //Los nuevos tipos de eventListener, que son el PointEnter y el PointExit, que pueden ser anexados a los 
    //objetos del GUI que necesiten este tipo de comportamiento en el OnEnter y OnExit.
    private void Start()
    {
        canvasTemplate = this.GetComponent<CanvasRenderer>();
        eventTrigg = this.GetComponent<EventTrigger>();

        AddEventTriggerListener(eventTrigg, EventTriggerType.PointerEnter,OnEnter);
        AddEventTriggerListener(eventTrigg, EventTriggerType.PointerExit, OnExit);
    }

    //Cabe destacar que no se pierden los eventos nativos seteados por el editor
    //Simplemente estos se agregan a el componente.
    public static void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }

    //-----------Metodos Custom-------------
    // Callback function 
    //Esto es para  que haya focus de alfa en los textos al pasar sobre ellos, aumentandolo cuando el mouse esté sobre él.
    void OnEnter(BaseEventData eventData)
    {
        canvasTemplate.SetAlpha(255);
    }

    void OnExit(BaseEventData eventData)
    {
        canvasTemplate.SetAlpha(0.5f);
    }

}
