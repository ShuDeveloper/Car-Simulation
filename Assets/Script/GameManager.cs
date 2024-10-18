using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class GameManager : MonoBehaviour
{
    public Slider BritenessSlider;
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    public GameObject[] OnLight;
    private void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        colorAdjustments.postExposure.value = BritenessSlider.value;
    }
    
    public void Exit()
    {
        Application.Quit();
    }

}
