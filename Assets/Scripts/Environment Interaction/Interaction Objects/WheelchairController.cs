using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class WheelchairController : InteractionObject
{

    public FirstPersonController userController;
    public InGameGUIMotile currentGUI;
    public GameObject userControler;
    public GameObject userCamera;
    public GameObject pushingCollider;
    public GameObject pushingCamera;

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float minStepPauseDuration = 0.3f;
    public float maxStepPauseDuration = 0.5f;

    public bool isPushing { get { return pushing; } }
    public Rigidbody getRigitBody { get { return rigidBody; } }

    private GameObject currentuserController;
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private bool pushing = false;
    public float averageVelocity;
    public bool movingForward = false;

    protected void Awake()
    {
        rigidBody = (Rigidbody)GetComponent(typeof(Rigidbody));
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (IsSelected)
        {
            if (Input.GetButtonDown("Interact"))
            {
                userControler.SetActive(false);
                userCamera.SetActive(false);
                pushingCollider.SetActive(true);
                pushingCamera.SetActive(true);
                userController.LockMovement = true;
                currentGUI.CurrentCursor = InGameGUIMotile.CursorType.None;
                pushing = true;
            }
        }
        if (pushing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                userControler.transform.position = pushingCollider.transform.position;
                userControler.transform.rotation = pushingCollider.transform.rotation;
                pushingCollider.SetActive(false);
                pushingCamera.SetActive(false);
                userController.LockMovement = false;
                userControler.SetActive(true);
                userCamera.SetActive(true);
                pushing = false;
            }
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform

    void FixedUpdate()
    {
        if (pushing)
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
            }
        }
        averageVelocity = Mathf.Abs((GetComponent<Rigidbody>().velocity.x
            + GetComponent<Rigidbody>().velocity.y
            + GetComponent<Rigidbody>().velocity.z) / 3f);

        var localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);

        if (localVelocity.z < 0)
        {
            movingForward = false;
        }
        else
        {
            movingForward = true;
        }

        if (Mathf.Abs(localVelocity.magnitude) > 0.1)
        {
            audioSource.volume = Mathf.Abs(localVelocity.magnitude) * 0.4f;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
}
