using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    public AudioClip buttonPress;           // joedeshon
    public AudioClip computerGlitch;        // bassimat
    public AudioClip coinInsert;            // krokulator
    public AudioClip coinsFalling;          // moogleoftheages
    public AudioClip upgradePurchased;      // vilkas-sound
    public AudioClip vendingConfirm;        // Emma7073
    public AudioClip whoosh;                // hitrison
    public AudioClip computerStartup;       // juskiddink
    public AudioClip winSound;              // EVRetro
    public AudioClip loseSound;             // Fupicat

    public AudioClip expiredSting;          // jerry.berumen
    public AudioClip freshSting;            // jerry.berumen
    public AudioClip crunch;                // qubodup
    public AudioClip glug;                  // deleted_user_2104797

    public AudioSource src;

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
        PlayerPrefs.SetFloat("VolumeKey", value);
    }

    public void PlayButtonPress() {
        src.PlayOneShot(buttonPress);
    }
    public void PlayCoinsFalling() {
        src.PlayOneShot(coinsFalling);
    }

    public void PlayCoinInsert() {
        src.PlayOneShot(coinInsert);
    }

    public void PlayUpgradePurchased() {
        src.PlayOneShot(upgradePurchased);
    }

    public void PlayVendingConfirm() {
        src.PlayOneShot(vendingConfirm);
    }

    public void PlayWhoosh() {
        src.PlayOneShot(whoosh);
    }

    public void PlayComputerStartup() {
        src.PlayOneShot(computerStartup);
    }

    public void PlayComputerGlitch() {
        src.PlayOneShot(computerGlitch);
    }

    public void PlayWinSound() {
        src.PlayOneShot(winSound);
    }

    public void PlayLoseSound() {
        src.PlayOneShot(loseSound);
    }

    public void PlayExpiredSting() {
        src.PlayOneShot(expiredSting, 0.6f);
    }

    public void PlayFreshSting() {
        src.PlayOneShot(freshSting, 0.6f);
    }

    public void PlayCrunch() {
        src.PlayOneShot(crunch);
    }

    public void PlaySlurp() {
        src.PlayOneShot(glug);
    }
}
