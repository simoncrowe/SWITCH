using UnityEngine;
using System.Collections;
using System;

public class CupboardInventory : MonoBehaviour
{
    private int wholeRutabagaCount = 0;
    private int halfRutabagaCount = 0;
    private int quarterRutabagaCount = 0;
    private int dicedRutabagaCount = 0;
    private int wholeCeleryCount = 0;
    private int choppedCeleryCount = 0;

    private void Start()
    {
        EventManager.StartListening("add_celery_to_cupboard", AddCelery);
        EventManager.StartListening("add_rutabaga_to_cupboard", AddRutabaga);

        EventManager.StartListening("remove_diced_rutabaga_from_cupboard", RemoveDicedRutabaga);
        EventManager.StartListening("remove_chopped_celery_from_cupboard", RemoveChoppedCelery);
    }
    void AddRutabaga()
    {
        wholeRutabagaCount++;
        EventManager.TriggerEvent("drop_rutabaga_on_surface");
    }

    void AddCelery()
    {
        wholeCeleryCount++;
        EventManager.TriggerEvent("drop_celery_on_surface");
    }

    void RemoveDicedRutabaga()
    {
        if (dicedRutabagaCount == 0)
        {
            CreateDicedRutabaga();
        }
        dicedRutabagaCount--;
    }

    void CreateDicedRutabaga()
    {
        if (quarterRutabagaCount == 0)
        {
            CreateQuarterRutabaga();
        }
        quarterRutabagaCount--;
        dicedRutabagaCount++;
        EventManager.TriggerEvent("chop_quarter_rutabaga_into_pieces");
    }

    void CreateQuarterRutabaga()
    {
        if (halfRutabagaCount == 0)
        {
            CreateHalfRutabaga();
        }
        halfRutabagaCount--;
        quarterRutabagaCount += 2;
        EventManager.TriggerEvent("chop_half_rutabaga_into_quarters");
    }
    void CreateHalfRutabaga()
    {
        if (wholeRutabagaCount > 0)
        {
            wholeRutabagaCount--;
            halfRutabagaCount += 2;
            EventManager.TriggerEvent("chop_rutabaga_in_half");
        }
        else
        {
            throw new InvalidOperationException("There are no whole rutabaga to chop.");
        }
    }

    void RemoveChoppedCelery()
    {
        {
            if (choppedCeleryCount == 0)
            {
                CreateChoppedCelery();
            }
            choppedCeleryCount--;
        }
    }

    void CreateChoppedCelery()
    {
        if (wholeCeleryCount > 0)
        {
            wholeCeleryCount--;
            choppedCeleryCount++;
            EventManager.TriggerEvent("chop_celery_into_pieces");
        }
        else
        {
            throw new InvalidOperationException("There are no whole celery to chop.");
        }
    }
}
