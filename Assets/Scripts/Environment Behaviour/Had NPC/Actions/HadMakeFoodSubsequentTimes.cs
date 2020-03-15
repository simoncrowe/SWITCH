using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadMakeFoodSubsequentTimes : MonoBehaviour
{
    public float potFoodVolume = 0.0012f;
    public float potFoodEnergyJ = 2492624f;
    public Metabolism manMetabolism;
    public Metabolism hadMetabolism;
    public AudioClip ingestPotFoodClip;
    public HadMakeFoodFirstTime makeFoodFirstTimeScript;

    public List<string> makeFoodDialogueNodes;

    private SimpleConsumable potFood;

    void Start()
    {
        potFood = new SimpleConsumable(0, 0);

        for (int i = 0; i < makeFoodDialogueNodes.Count; i++)
        {
            EventManager.StartListening("dialogue_" + makeFoodDialogueNodes[i], InitiateAction);
        }
    }

    void InitiateAction()
    {
        if (makeFoodFirstTimeScript.FoodMade)
        {
            Debug.Log("Initiated HadMakeFoodSubsequentTImes.");
            if (potFood.Volume == 0)
            {
                Invoke("WalkToChutes", 1f);
            }
            else {
                Invoke("HadFeedMan", 1f);
            }
        }
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
        EventManager.TriggerEvent("remove_diced_rutabaga_from_cupboard");
        Invoke("WalkToStoveWithDicedRutabaga", 1.25f);
    }

    void WalkToStoveWithDicedRutabaga()
    {
        EventManager.TriggerEvent("move_had_to_stove");
        EventManager.StartListening("had_arrives_at_stove", PutDicedRutabagaInPot);
    }

    void PutDicedRutabagaInPot()
    {
        EventManager.StopListening("had_arrives_at_stove", PutDicedRutabagaInPot);
        EventManager.TriggerEvent("add_diced_rutabaga_to_pot");
        Invoke("WalkToCupboardToGetChoppedCelery", 0.4f);
    }

    void WalkToCupboardToGetChoppedCelery()
    {
        EventManager.TriggerEvent("move_had_to_cupboard");
        EventManager.StartListening("had_arrives_at_cupboard", GetChoppedCeleryFromCupboard);
    }

    void GetChoppedCeleryFromCupboard()
    {
        EventManager.StopListening("had_arrives_at_cupboard", GetChoppedCeleryFromCupboard);
        EventManager.TriggerEvent("remove_chopped_celery_from_cupboard");
        Invoke("WalkToStoveWithCelery", 0.9f);
    }

    void WalkToStoveWithCelery()
    {
        EventManager.TriggerEvent("move_had_to_stove");
        EventManager.StartListening("had_arrives_at_stove", PutChoppedCeleryInPot);
    }

    void PutChoppedCeleryInPot()
    {
        EventManager.StopListening("had_arrives_at_stove", PutChoppedCeleryInPot);
        EventManager.TriggerEvent("add_chopped_celery_to_pot");
        Invoke("WalkToChutedToGetCornflour", 0.5f);
    }

    void WalkToChutedToGetCornflour()
    {
        EventManager.TriggerEvent("move_had_to_chutes");
        EventManager.StartListening("had_arrives_at_chutes", GetCornflourFronChutes);
    }

    void GetCornflourFronChutes()
    {
        EventManager.StopListening("had_arrives_at_chutes", GetCornflourFronChutes);
        EventManager.TriggerEvent("remove_cornflour_from_chute");
        Invoke("WalkToStoveWithCornflour", 0.5f);
    }

    void WalkToStoveWithCornflour()
    {
        EventManager.TriggerEvent("move_had_to_stove");
        EventManager.StartListening("had_arrives_at_stove", PutCornflourInPot);
    }

    void PutCornflourInPot()
    {
        EventManager.StopListening("had_arrives_at_stove", PutCornflourInPot);
        EventManager.TriggerEvent("add_cornflour_to_pot");
        Invoke("CarryPotToSink", 0.4f);
    }

    void CarryPotToSink()
    {
        EventManager.TriggerEvent("move_had_to_sink");
        EventManager.StartListening("had_arrives_at_sink", PutPotInSink);
    }

    void PutPotInSink()
    {
        EventManager.StopListening("had_arrives_at_sink", PutPotInSink);
        EventManager.TriggerEvent("put_pot_in_sink");
        Invoke("TurnOnTap", 0.6f);
    }

    void TurnOnTap()
    {
        EventManager.TriggerEvent("turn_on_tap");
        EventManager.StartListening("tap_turned_on", WaitForPotToFillUp);
    }

    void WaitForPotToFillUp()
    {
        EventManager.StopListening("tap_turned_on", WaitForPotToFillUp);
        Invoke("TurnOffTap", 10f);
    }

    void TurnOffTap()
    {
        EventManager.TriggerEvent("add_water_to_pot");
        EventManager.TriggerEvent("turn_off_tap");
        EventManager.StartListening("tap_turned_off", TakeFullPotBackToStove);
    }

    void TakeFullPotBackToStove()
    {
        EventManager.StopListening("tap_turned_off", TakeFullPotBackToStove);
        EventManager.TriggerEvent("move_had_to_stove");
        EventManager.StartListening("had_arrives_at_stove", PutPotOnStove);
    }

    void PutPotOnStove()
    {
        EventManager.StopListening("had_arrives_at_stove", PutPotOnStove);
        EventManager.TriggerEvent("put_pot_on_stove");
        Invoke("TurnOnHob", 0.75f);
    }

    void TurnOnHob()
    {
        EventManager.TriggerEvent("turn_on_hob");
        Invoke("BoilPotContents", 1.5f);
    }

    void BoilPotContents()
    {
        EventManager.TriggerEvent("cook_contents_of_pot");
        EventManager.StartListening("contents_of_pot_is_cooked", TurnOffHob);
    }

    void TurnOffHob()
    {
        EventManager.StopListening("contents_of_pot_is_cooked", TurnOffHob);
        EventManager.TriggerEvent("turn_off_hob");
        Invoke("TakeFoodToMan", 0.7f);
    }

    void TakeFoodToMan()
    {
        EventManager.TriggerEvent("move_had_in_front_of_wheelchair");
        EventManager.StartListening("had_arrives_at_front_of_wheelchair", HadFeedMan);
    }

    void HadFeedMan()
    {
        EventManager.StopListening("had_arrives_at_front_of_wheelchair", HadFeedMan);
        potFood = manMetabolism.IngestSimple(new SimpleConsumable(potFoodVolume, potFoodEnergyJ),
                                                                          ingestPotFoodClip);
        Invoke("HadFeedSelf", 3f);
    }

    void HadFeedSelf()
    {
        potFood = hadMetabolism.IngestSimple(potFood, ingestPotFoodClip);
        EventManager.TriggerEvent("had_has_fed_man_and_food_finshed");
    }
}