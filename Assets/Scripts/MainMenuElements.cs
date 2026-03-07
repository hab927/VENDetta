using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuElements : MonoBehaviour
{
    public void LoadSceneOnClick(string sceneName) {
        SoundManager.instance.PlayButtonPress();
        StartCoroutine(LoadScene(sceneName));
    }

    public IEnumerator LoadScene(string scene) {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);
        while (!load.isDone) {
            yield return null;
        }
        if (scene == "Game") {
            GameManager.Instance.NewRun();
        }
    }

    public void QuitButton() {
        SoundManager.instance.PlayButtonPress();
        Application.Quit();
    }
}
