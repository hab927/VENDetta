using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip computerGlitch;
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

    public void PlayCoinsFalling() {
        src.PlayOneShot(coinsFalling);
    }

    public void PlayUpgradePurchased() {
        src.PlayOneShot(upgradePurchased);
    }

    public void PLayVendingConfirm() {
        src.PlayOneShot(vendingConfirm);
    }

    public void PlayWhoosh() {
        src.PlayOneShot(whoosh);
    }

    public void PlayComputerStartup() {
        src.PlayOneShot(computerStartup);
    }

    public void PlayWinSound() {
        src.PlayOneShot(winSound);
    }

    public void PlayLoseSound() {
        src.PlayOneShot(loseSound);
    }

    public void PlayExpiredSting() {
        src.PlayOneShot(expiredSting);
    }

    public void PlayFreshSting() {
        src.PlayOneShot(freshSting);
    }

    public void PlayCrunch() {
        src.PlayOneShot(crunch);
    }

    public void PlayGlug() {
        src.PlayOneShot(glug);
    }
}
