using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }
    [SerializeField] private AudioSource src;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start() {
        src.volume = 1.0f;
    }

    public void ChangeVolume(float value) {
        src.volume = value;
        PlayerPrefs.SetFloat("MusicKey", value);
    }
}
