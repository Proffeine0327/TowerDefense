using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeController : MonoBehaviour
{
    [SerializeField] private Image leftClosePart;
    [SerializeField] private Image rightClosePart;
    [SerializeField] private Image middleFadeImage;
    [SerializeField] private Image progressFrame;
    [SerializeField] private Image progressBar;
    [Header("Easing")]
    [SerializeField] private AnimationCurve quadIn;
    [SerializeField] private AnimationCurve quadOut;

    private float progress;

    public void PlayLoading(bool playLoading) => StartCoroutine(LoadingDisplayRoutine(playLoading));

    private IEnumerator LoadingDisplayRoutine(bool playLoading)
    {
        var unscaledWait = new WaitForSecondsRealtime(0);
        var lStart = leftClosePart.rectTransform.anchoredPosition;
        var rStart = rightClosePart.rectTransform.anchoredPosition;

        for (float t = 0; t < ScreenFade.FadeTime; t += Time.unscaledDeltaTime)
        {
            leftClosePart.rectTransform.anchoredPosition =
                Vector2.Lerp(lStart, Vector2.zero, quadOut.Evaluate(t / ScreenFade.FadeTime));

            rightClosePart.rectTransform.anchoredPosition =
                Vector2.Lerp(rStart, Vector2.zero, quadOut.Evaluate(t / ScreenFade.FadeTime));
            yield return unscaledWait;
        }

        yield return new WaitForSecondsRealtime(1f);

        if (playLoading)
        {
            progress = 0;

            for (float t = 0; t < 1; t += Time.unscaledDeltaTime)
            {
                middleFadeImage.color = new Color(1, 1, 1, t / 1);
                yield return unscaledWait;
            }

            for (float t = 0; t < 1; t += Time.unscaledDeltaTime)
            {
                progressBar.color = new Color(1, 1, 1, t / 1);
                progressFrame.color = new Color(1, 1, 1, t / 1);
                yield return unscaledWait;
            }

            yield return new WaitForSecondsRealtime(0.5f);

            while (progress < 1)
            {
                progress += 0.1f * Random.Range(0.2f, 1f);
                yield return new WaitForSecondsRealtime(Random.Range(0.1f, 0.8f));
            }

            yield return new WaitForSecondsRealtime(0.5f);

            for (float t = 1; t > 0; t -= Time.unscaledDeltaTime)
            {
                progressBar.color = new Color(1, 1, 1, t / 1);
                progressFrame.color = new Color(1, 1, 1, t / 1);
                middleFadeImage.color = new Color(1, 1, 1, t / 1);
                yield return unscaledWait;
            }
        }

        ScreenFade.EndLoading = true;

        for (float t = 0; t < ScreenFade.FadeTime; t += Time.unscaledDeltaTime)
        {
            leftClosePart.rectTransform.anchoredPosition =
                Vector2.Lerp(Vector2.zero, lStart, quadIn.Evaluate(t / ScreenFade.FadeTime));

            rightClosePart.rectTransform.anchoredPosition =
                Vector2.Lerp(Vector2.zero, rStart, quadIn.Evaluate(t / ScreenFade.FadeTime));
            yield return unscaledWait;
        }
    }

    private void Update()
    {
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, Time.unscaledDeltaTime * 5f);
    }
}
