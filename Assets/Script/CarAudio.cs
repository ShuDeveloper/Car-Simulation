using Unity.VisualScripting;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioSource engineSoundSource;

    [Tooltip("This is breake sound to Play CarController")]
    public AudioSource brakeSoundSource;
   
    public AudioClip idleClip;
    public AudioClip midClip;
    public AudioClip runningClip;   

    public float pitchMultiplier = 1.0f;
    public float volumeMultiplier = 1.0f;
    private Rigidbody carRigidbody;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        engineSoundSource.clip = idleClip;
        engineSoundSource.Play();
    }

    void Update()
    {
        float speed = carRigidbody.velocity.magnitude;
        engineSoundSource.pitch = Mathf.Lerp(1.0f, 3.0f, speed / 100.0f) * pitchMultiplier;
        engineSoundSource.volume = Mathf.Lerp(0.5f, 1.0f, speed / 100.0f) * volumeMultiplier;
    

        // Speed engine Sound
        if (speed > 5f && speed < 8 && engineSoundSource.clip != midClip && !Input.GetKey(KeyCode.Space))
        {
            engineSoundSource.clip = midClip;
            engineSoundSource.Play();
            print("mid clip");
        }
        else if (speed > 10f && engineSoundSource.clip != runningClip && !Input.GetKey(KeyCode.Space))
        {
            engineSoundSource.clip = runningClip;
            engineSoundSource.Play();
            print("running clip");

        }
        else if (speed <= 5f && engineSoundSource.clip != idleClip && !Input.GetKey(KeyCode.Space))
        {
            engineSoundSource.pitch = 2 * pitchMultiplier;
            engineSoundSource.volume = 5 * volumeMultiplier;
            engineSoundSource.clip = idleClip;
            engineSoundSource.Play();
            print("idle");
        }      
    }
}
