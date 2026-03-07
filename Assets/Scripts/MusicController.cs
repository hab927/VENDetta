using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
        float savedVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        slider.value = savedVolume;
    }

    public void MusicSlider(float volume) {
        MusicManager.instance.ChangeVolume(volume);
    }
}
