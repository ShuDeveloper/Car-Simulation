using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Fornt,
        Rear
    }
    //Wheel properties
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem wheellDust;
        public Axel axel;
    }
    //Physics variable for wheel of car
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;
    public float turnSensitivity = 1f;

    private Rigidbody carRigidBody;
    private CarAudio audios;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    Vector2 moveInput;
    bool handBrakeInput;

    bool frontLighton;

    //Light varialbe
    public GameObject carMesh;
    public Material BK_LightOn;
    public Material BK_LightOff;
    public Material fruntLinghtOn;
    public Material fruntLinghtOff;
    public Material LightOn;
    public Material LightOff;
    public GameObject[] BkLights;
    public GameObject[] frontLights;
    private Material[] material;
    private MeshRenderer meshRenderer;


    public InputActionAsset primaryActions;
    private InputActionMap ActionMap;
    private InputAction movementInput;
    private InputAction lightInput;
    private InputAction brakeInput;

    public Button lightButton;
    private void Awake()
    {
        ActionMap = primaryActions.FindActionMap("Basic Movement");

        movementInput = ActionMap.FindAction("Move");
        movementInput.performed += moveX;
        movementInput.canceled += moveX;

        lightInput = ActionMap.FindAction("FrontLight");
        lightInput.performed += context => Light();

        brakeInput = ActionMap.FindAction("HandBrake");
        brakeInput.performed += Brake;
        brakeInput.canceled += Brake;
    }

    private void Start()
    {
        carRigidBody = GetComponent<Rigidbody>();
        audios = GetComponent<CarAudio>();

        //set car center of mass 
        carRigidBody.centerOfMass = centerOfMass;

        //chenge material
        meshRenderer = carMesh.GetComponent<MeshRenderer>();
        material = meshRenderer.materials;

        //Dust particale effect stop
        foreach (Wheel wheel in wheels)
        {
            wheel.wheellDust.Stop();
        }

        wheels[0].wheelCollider.ConfigureVehicleSubsteps(5, 5, 1);

    }
    private void OnEnable()
    {
        movementInput.Enable();
        lightInput.Enable();
        brakeInput.Enable();
    }
    private void OnDisable()
    {
        movementInput.Disable();
        lightInput.Disable();
        brakeInput.Disable();
    }

    void moveX(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    void Brake(InputAction.CallbackContext context)
    {
        if (context.performed) { handBrakeInput = true; }
        else if (context.canceled) { handBrakeInput = false; }
    }

    private void Update()
    {
        carMovements();
        meshRenderer.materials = material;

    }


    void carMovements()
    {
        foreach (var wheel in wheels)
        {
            //move car horizontal
            wheel.wheelCollider.motorTorque = moveInput.y * maxAcceleration;

            //move steering
            if (wheel.axel == Axel.Fornt)
            {
                var _steerAngle = moveInput.x * turnSensitivity;
                wheel.wheelCollider.steerAngle = _steerAngle;
            }


            //Wheel rotation Anitmation
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;

            //Break and sound , particals, Light
            if (handBrakeInput)
            {
                print("is brake down");
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
                material[1] = BK_LightOn;
                foreach (var light in BkLights)
                {
                    light.SetActive(true);
                }

                //play dust paricale effect and brake sound
                if (carRigidBody.velocity.magnitude > 5)
                {
                    wheel.wheellDust.Emit(1);
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                    if (!audios.brakeSoundSource.isPlaying)
                    {
                        audios.brakeSoundSource.Play();
                        audios.engineSoundSource.Stop();
                    }
                }
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;

                if (audios.brakeSoundSource.isPlaying)
                {
                    audios.brakeSoundSource.Stop();
                    audios.engineSoundSource.Play();
                }

                wheel.wheelCollider.brakeTorque = 0;
                material[1] = BK_LightOff;

                foreach (var light in BkLights)
                {
                    light.SetActive(false);
                }

            }
        }
    }
    //Front Light on and off
    void Light()
    {
        frontLighton = !frontLighton;
        if (frontLighton)
        {
            print("Light ON");
            material[2] = fruntLinghtOn;
            ChangeButtonColor(Color.yellow);
            foreach (var light in frontLights)
                light.SetActive(true);

        }
        else
        {
            print("Light off");
            material[2] = fruntLinghtOff;

            ChangeButtonColor(Color.white);

            foreach (var light in frontLights)
                light.SetActive(false);
        }
    }
    //Change Ui Buttone Color 
    public void ChangeButtonColor(Color color)
    {
        ColorBlock buttonColors = lightButton.colors;
        buttonColors.normalColor = color;
        buttonColors.highlightedColor = color;
        buttonColors.pressedColor = color;
        buttonColors.selectedColor = color;
        buttonColors.disabledColor = color;
        lightButton.colors = buttonColors;
    }

}
