using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
        float savedVolume = PlayerPrefs.GetFloat("VolumeKey", 1f);
        slider.value = savedVolume;
    }

    public void VolumeSlider(float volume) {
        SoundManager.instance.ChangeVolume(volume);
    }
}
