using UnityEngine;
using UnityEngine.InputSystem;

public class HornSound : MonoBehaviour
{
    bool hornPress;
  

    public AudioSource AudioSource;
    void OnHorn(InputValue value)
    {
        hornPress = value.isPressed;
    }

    private void Update()
    {
        if (hornPress && !AudioSource.isPlaying)
        {               
                AudioSource.Play();
                hornPress=false;        
        }
      

    }
}
