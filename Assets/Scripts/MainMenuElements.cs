using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuElements : MonoBehaviour
{
    public void LoadSceneOnClick(string sceneName) {
        StartCoroutine(LoadScene(sceneName));
    }

    public IEnumerator LoadScene(string scene) {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);

        while (!load.isDone) {
            yield return null;
        }
    }

    public void QuitButton() {
        Application.Quit();
    }
}
