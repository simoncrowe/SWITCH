using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadMakeFoodFirstTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("dialogue_5", InitiateAction);
    }
    
    void InitiateAction()
    {
        Debug.Log("Initiated HadMakeFood.");
        Invoke("GoToWorkSurface", 1f);
    }

    void GoToWorkSurface()
    {
        EventManager.TriggerEvent("move_had_to_cupboard");
        EventManager.StartListening("had_arrives_at_cupboard", OpenBottomDrawer);

    }

    void OpenBottomDrawer()
    {
        EventManager.StopListening("had_arrives_at_cupboard", OpenBottomDrawer);
        EventManager.TriggerEvent("open_bottom_drawer");
        Invoke("OpenTopDrawer", 1.25f);
    }

    void OpenTopDrawer()
    {
        EventManager.TriggerEvent("open_top_drawer");
        Invoke("CloseTopDrawer", 1.5f);

    }

    void CloseTopDrawer()
    {
        EventManager.TriggerEvent("close_top_drawer");
        Invoke("CloseBottomDrawer", 0.3f);

    }

    void CloseBottomDrawer()
    {
        EventManager.TriggerEvent("close_bottom_drawer");
        Invoke("OpenRightCupboardDoor", 0.5f);
    }

    void OpenRightCupboardDoor()
    {
        EventManager.TriggerEvent("open_cupboard_door");
        Invoke("OpenLeftCupboardDoor", 0.05f);
    }

    void OpenLeftCupboardDoor()
    {
        EventManager.TriggerEvent("open_cupboard_door");
        Invoke("CloseCupboardDoor", 2.5f);
    }

    void CloseCupboardDoor()
    {
        EventManager.TriggerEvent("close_cupboard_door");
        Invoke("WalkToChutes", 0.75f);
    }

    void WalkToChutes()
    {
        EventManager.TriggerEvent("move_had_to_chutes");
        EventManager.StartListening("had_arrives_at_chutes", PickUpCeleryAndRutabega);
    }

    void PickUpCeleryAndRutabega()
    {
        EventManager.StopListening("had_arrives_at_chutes", PickUpCeleryAndRutabega);
        Invoke("WalkBackToCupboard", 5f);
    }

    void WalkBackToCupboard()
    {
        EventManager.TriggerEvent("move_had_to_cupboard");
        EventManager.StartListening("had_arrives_at_cupboard", PutDownRutabagaOnSurface);
    }

    void PutDownRutabagaOnSurface()
    {
        EventManager.StopListening("had_arrives_at_cupboard", PutDownRutabagaOnSurface);
        EventManager.TriggerEvent("add_rutabaga_to_cupboard");
        Invoke("DropCeleryOnSurface", 0.75f);
    }

    void DropCeleryOnSurface()
    {
        EventManager.TriggerEvent("add_celery_to_cupboard");
        Invoke("GetDicedRutabagaFromSurface", 1f);
    }

    void GetDicedRutabagaFromSurface()
    {
        EventManager.TriggerEvent("remove_diced_rutabada_from_cupboard");
        Invoke("GetChoppedCeleryFromSurface", 1.25f);
    }

    void GetChoppedCeleryFromSurface()
    {
        EventManager.TriggerEvent("remove_diced_rutabada_from_cupboard");
        Invoke("WalkToStoveWithVegetables", 1.25f);
    }

    void WalkToStoveWithVegetables()
    {
        EventManager.TriggerEvent("move_had_to_stove");
        EventManager.StartListening("had_arrived_at_stove", PutVegetablesInPot);
    }

    void PutVegetablesInPot()
    {
        EventManager.StopListening("had_arrived_at_stove", PutVegetablesInPot);

    }
}
