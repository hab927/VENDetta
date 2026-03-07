using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuElements : MonoBehaviour
{
    public void LoadSceneOnClick(string sceneName) {
        if (sceneName == "Game") {
            GameManager.Instance.NewRun();
        }
        SoundManager.instance.PlayButtonPress();
        StartCoroutine(LoadScene(sceneName));
    }

    public IEnumerator LoadScene(string scene) {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);

        while (!load.isDone) {
            yield return null;
        }
    }

    public void QuitButton() {
        SoundManager.instance.PlayButtonPress();
        Application.Quit();
    }
}
