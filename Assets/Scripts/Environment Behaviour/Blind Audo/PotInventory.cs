using UnityEngine;
using System;

public class PotInventory : MonoBehaviour
{
    bool containsChoppedCelery = false;
    bool containsDicedRutabaga = false;
    bool containsCornflour = false;
    bool containsWater = false;

    bool contentsIsCooked = false;

    private void Start()
    {
        EventManager.StartListening("add_diced_rutabaga_to_pot", AddDicedRutabaga);
        EventManager.StartListening("add_chopped_celery_to_pot", AddChoppedCelery);
        EventManager.StartListening("add_cornflour_to_pot", AddCornflour);
        EventManager.StartListening("add_water_to_pot", AddWater);
        EventManager.StartListening("cook_contents_of_pot", CookContents);
        EventManager.StartListening("eat_from_pot", EmptyPot);
        EventManager.StartListening("empty_pot", EmptyPot);
    }
    void AddDicedRutabaga()
    {
        if (!containsDicedRutabaga)
        {
            containsDicedRutabaga = true;
            EventManager.TriggerEvent("play_drop_diced_rutabaga_in_pot_sound");
        }
        else
        {
            throw new InvalidOperationException("There is already diced rutabaga in the pot.");
        }
    }

    void AddChoppedCelery()
    {
        if (!containsChoppedCelery)
        {
            EventManager.TriggerEvent("play_drop_chopped_celery_in_pot_sound");
            containsChoppedCelery = true;
        }
        else
        {
            throw new InvalidOperationException("There is already chopped celery in the pot.");
        }
    }

    void AddCornflour()
    {
        if (!containsCornflour)
        {
            EventManager.TriggerEvent("put_cornflour_in_pot");
            containsCornflour = true;
        }
        else
        {
            throw new InvalidOperationException("There is already cornflour in the pot.");
        }
    }

    void AddWater()
    {
        if (!containsWater)
        {
            EventManager.TriggerEvent("put_water_in_pot");
            containsWater = true;
        }
        else
        {
            throw new InvalidOperationException("There is already water in the pot.");
        }
    }

    void CookContents()
    {
        if (!contentsIsCooked)
        {
            if (containsWater)
            {
                if (containsCornflour || containsChoppedCelery || containsDicedRutabaga)
                {
                    EventManager.TriggerEvent("play_pot_boiling_sound");
                    Invoke("FinishedCooking", 30f);
                }
                else
                {
                    throw new InvalidOperationException(
                        "You must cook something in addition to water."
                    );
                }
            }
            else
            {
                throw new InvalidOperationException("You cannot cook without water.");
            }
        }
        else
        {
            throw new InvalidOperationException("The contents of the pot is alreay cooded.");
        }
    }

    void FinishedCooking()
    {
        contentsIsCooked = true;
    }

    void EmptyPot()
    {
        contentsIsCooked = false;
        containsChoppedCelery = false;
        containsCornflour = false;
        containsDicedRutabaga = false;
        containsWater = false;
    }
    void EatFromPot()
    {
        EventManager.TriggerEvent("play_eat_from_pot_sound");
    }
}
