using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private static LoadingScene instance;

    [SerializeField] private LoadingSceneController prefab;
    [SerializeField] private float fadeTime;

    private LoadingSceneController controller;

    public static bool IsPlayLoading { get; private set; }
    public static bool EndLoading { get; set; }
    public static float FadeTime => instance.fadeTime;

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(instance.LoadSceneWithLoadingScreen(sceneName));
    }

    private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        IsPlayLoading = true;
        yield return new WaitForSecondsRealtime(0);
        
        EndLoading = false;
        controller.gameObject.SetActive(true);
        controller.PlayLoading();
        yield return new WaitForSecondsRealtime(fadeTime);

        yield return new WaitUntil(() => EndLoading);
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSecondsRealtime(fadeTime);
        controller.gameObject.SetActive(false);
        IsPlayLoading = false;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            controller = Instantiate(prefab);
            controller.gameObject.SetActive(false);
            DontDestroyOnLoad(controller);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
