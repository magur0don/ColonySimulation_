using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public Slider BGMSlider;

    public TextMeshProUGUI BGMSliderValue;

    public Slider SESlider;

    void Start()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        SetBGMValueText();
    }

    public void SetBGMValueText()
    {
        // SliderÇÃvalueÇÕ0~1Ç»ÇÃÇ≈ÅAï™Ç©ÇËÇ‚Ç∑Ç≥èdéãÇ≈100î{ÇµÇƒÇ†Ç∞ÇÈ
        BGMSliderValue.text = $"{BGMSlider.value * 100}";
    }
}
