using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private static LoadingScene instance;

    [SerializeField] private float fadeTime;
    [SerializeField] private string loadingSceneName;
    public static bool EndLoading { get; set; }
    public static float FadeTime => instance.fadeTime;

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(instance.LoadSceneWithLoadingScreen(sceneName));
    }

    private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0);
        SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(fadeTime);
        EndLoading = false;

        yield return new WaitUntil(() => EndLoading);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));        

        yield return new WaitForSecondsRealtime(fadeTime);
        SceneManager.UnloadSceneAsync(loadingSceneName);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
