using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadMovement : MonoBehaviour
{
    public float movementRate;
    public float minFootfallInterval;
    public float maxFootfallInterval;

    public AudioClip[] footfallSounds;

    public Vector3 atDoorPosition;
    public Vector3 atChutesPosition;
    public Vector3 atSinkPosition;
    public Vector3 atStovePosition;
    public Vector3 atCupboardPosition;
    public Vector3 inFrontOfWheelchairPosition;
    public Vector3 toRightOfWheelchairPosition;
    public Vector3 behindWheelchairPosition;
    public Vector3 toLeftOfWheelchairPosition;

    public Dialogue dialogue;

    private AudioSource audioSource;
    private Queue<Vector3> futurePostions;
    private float nextFootFall = 0f;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.position = this.atDoorPosition;
        futurePostions = new Queue<Vector3>();

        EventManager.StartListening("move_had_to_door", MoveToDoor);
        EventManager.StartListening("move_had_to_chutes", MoveToChutes);
        EventManager.StartListening("move_had_to_sink", MoveToSink);
        EventManager.StartListening("move_had_to_stove", MoveToStove);
        EventManager.StartListening("move_had_to_cupboard", MoveToCupboard);
        EventManager.StartListening("move_had_in_front_of_wheelchair", MoveInFrontOfWheelchair);
        EventManager.StartListening("move_had_to_right_of_wheelchair", MoveToRightOfWheelchair);
        EventManager.StartListening("move_had_behind_wheelchair", MoveBehindWheelchair);
        EventManager.StartListening("move_had_to_left_of_wheelchair", MoveToLeftOfWheelchair);
    }

    void Update()
    {
        if (Move()) {
            ProcessFootfalls();
        }
        if (transform.position == inFrontOfWheelchairPosition)
        {
            if (!dialogue.conversing)
            {
                dialogue.conversing = true;
            }
        }
        else
        {
            if (dialogue.conversing)
            {
                dialogue.conversing = false;
            }
        }

    }

    bool Move()
    {
        if (futurePostions.Count > 0)
        {
            if (transform.position != futurePostions.Peek())
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, futurePostions.Peek(), movementRate * Time.deltaTime
                );
            }
            else
            {
                var position = futurePostions.Dequeue();
                Debug.Log("Finished moving towards " + position);

                if (position == atDoorPosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_door");
                }
                else if (position == atChutesPosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_chutes");
                }
                else if (position == atSinkPosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_sink");
                }
                else if (position == atStovePosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_stove");
                }
                else if (position == atCupboardPosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_cupboard");
                }
                else if (position == inFrontOfWheelchairPosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_front_of_wheelchair");
                }
                else if (position == toRightOfWheelchairPosition)
                {
                    EventManager.TriggerEvent("had_arrives_right_of_wheelchair");
                }
                else if (position == behindWheelchairPosition)
                {
                    EventManager.TriggerEvent("had_arrives_behind_wheelchair");
                }
                else if (position == toLeftOfWheelchairPosition)
                {
                    EventManager.TriggerEvent("had_arrives_at_left_of_wheelchair");
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    void ProcessFootfalls()
    {
        if (Time.time > nextFootFall) {
            audioSource.PlayOneShot(footfallSounds[Random.Range(0, footfallSounds.Length)]);
            nextFootFall = Time.time + Random.Range(minFootfallInterval, maxFootfallInterval);
        }
    }

    void MoveToDoor()
    {
        if ((transform.position == atSinkPosition) || 
            (transform.position == inFrontOfWheelchairPosition))
        {
            futurePostions.Enqueue(toRightOfWheelchairPosition); 
        }
        else if (transform.position == atStovePosition)
        {
            futurePostions.Enqueue(toLeftOfWheelchairPosition);
        }
        futurePostions.Enqueue(atDoorPosition);

    }
    void MoveToChutes()
    {
        if (transform.position == behindWheelchairPosition)
        {
            futurePostions.Enqueue(toRightOfWheelchairPosition);
        }
        futurePostions.Enqueue(atChutesPosition);
    }
    void MoveToSink()
    {
        if (transform.position == atDoorPosition)
        {
            futurePostions.Enqueue(toRightOfWheelchairPosition);
        }
        else if (transform.position == behindWheelchairPosition)
        {
            futurePostions.Enqueue(toLeftOfWheelchairPosition);
        }
        futurePostions.Enqueue(atSinkPosition);
    }
    void MoveToStove()
    {
        if ((transform.position == atDoorPosition) || 
            (transform.position == behindWheelchairPosition))
        {
            futurePostions.Enqueue(toLeftOfWheelchairPosition);
        }
        futurePostions.Enqueue(atStovePosition);
    }
    void MoveToCupboard()
    {
        if ((transform.position == atDoorPosition) ||
            (transform.position == behindWheelchairPosition))
        {
            futurePostions.Enqueue(toLeftOfWheelchairPosition);
        }
        else if (transform.position == toRightOfWheelchairPosition)
        {
            futurePostions.Enqueue(inFrontOfWheelchairPosition);
        }
        futurePostions.Enqueue(atCupboardPosition);
    }
    void MoveInFrontOfWheelchair()
    {
        if (transform.position == atDoorPosition)
        {
            futurePostions.Enqueue(toRightOfWheelchairPosition);
        }
        else if (transform.position == behindWheelchairPosition)
        {
            futurePostions.Enqueue(toLeftOfWheelchairPosition);
        }
        futurePostions.Enqueue(inFrontOfWheelchairPosition);
    }
    void MoveToRightOfWheelchair()
    {
        if (transform.position == atCupboardPosition)
        {
            futurePostions.Enqueue(inFrontOfWheelchairPosition);
        }
        else if (transform.position == toLeftOfWheelchairPosition)
        {
            futurePostions.Enqueue(behindWheelchairPosition);
        }
        futurePostions.Enqueue(toRightOfWheelchairPosition);
    }
    void MoveBehindWheelchair()
    {
        if ((transform.position == atStovePosition) ||
            (transform.position == inFrontOfWheelchairPosition))
        {
            futurePostions.Enqueue(toLeftOfWheelchairPosition);
        }
        else if (transform.position == atSinkPosition)
        {
            futurePostions.Enqueue(toRightOfWheelchairPosition);
        }
        futurePostions.Enqueue(behindWheelchairPosition);
    }
    void MoveToLeftOfWheelchair()
    {
        if ((transform.position == atChutesPosition) ||
            (transform.position == atSinkPosition))
        {
            futurePostions.Enqueue(inFrontOfWheelchairPosition);
        }
        else if (transform.position == toRightOfWheelchairPosition)
        {
            futurePostions.Enqueue(behindWheelchairPosition);
        }
        futurePostions.Enqueue(toLeftOfWheelchairPosition);
    }
}
