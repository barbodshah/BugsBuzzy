using UnityEngine;
using UnityEngine.UI;

public class SliderColor : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public Image fillImage; // Reference to the Image component of the Slider's Fill area

    void Start()
    {
        if (slider != null)
        {
            // Add a listener to the slider's value change event
            slider.onValueChanged.AddListener(UpdateSliderColor);
            // Set initial color
            UpdateSliderColor(slider.value);
        }
    }

    void UpdateSliderColor(float value)
    {
        // Interpolate between white and red based on the slider value
        Color newColor = Color.Lerp(Color.white, Color.red, value);
        if (fillImage != null)
        {
            fillImage.color = newColor;
        }
    }

    void OnDestroy()
    {
        if (slider != null)
        {
            // Remove listener to avoid memory leaks
            slider.onValueChanged.RemoveListener(UpdateSliderColor);
        }
    }
}
