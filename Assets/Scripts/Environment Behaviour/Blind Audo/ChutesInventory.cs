using UnityEngine;
using System;

public class ChutesInventory : MonoBehaviour
{

    public int celeryCount = 8;
    public int rutabagaCount = 5;
    public int cornflourCount = 10;
    
    void Start()
    {
        EventManager.StartListening("remove_celery_from_chute", RemoveCelery);
        EventManager.StartListening("remove_rutabaga_from_chute", RemoveRutabaga);
        EventManager.StartListening("remove_cornflour_from_chute", RemoveCornflour);
    }

    void RemoveCelery()
    {
        if (celeryCount > 0)
        {
            celeryCount--;
        }
        else
        {
            throw new InvalidOperationException("There are no celery sticks left to remove.");
        }
    }

    void RemoveRutabaga()
    {
        if (rutabagaCount > 0)
        {
            rutabagaCount--;
        }
        else
        {
            throw new InvalidOperationException("There are mo rutabagas left to remove.");
        }
    }

    void RemoveCornflour()
    {
        if (cornflourCount > 0)
        {
            cornflourCount--;
        }
        else
        {
            throw new InvalidOperationException("There is no cornflour left to remove.");
        }
    }
}
