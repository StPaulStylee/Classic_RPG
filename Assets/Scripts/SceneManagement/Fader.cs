using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.SceneManagement {
  [RequireComponent(typeof(CanvasGroup))]
  public class Fader : MonoBehaviour {
    [SerializeField] float fadeTime = 2f;
    [SerializeField] float fadeWaitTime = 0f;
    CanvasGroup canvasGroup;
    private void Awake() {
      canvasGroup = GetComponent<CanvasGroup>();
    }

    IEnumerator FadeOutIn() {
      yield return FadeOut();
      yield return FadeIn();
    }

    public IEnumerator FadeWait() {
      yield return new WaitForSeconds(fadeWaitTime);
    }

    public void SetToMaxOpacity() {
      canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeIn() {
      while (canvasGroup.alpha < 1f) {
        canvasGroup.alpha += Time.deltaTime / fadeTime;
        yield return null;
      }
    }

    public IEnumerator FadeIn(float fadeTime) {
      while (canvasGroup.alpha < 1f) {
        canvasGroup.alpha += Time.deltaTime / fadeTime;
        yield return null;
      }
    }

    public IEnumerator FadeOut() {
      while (canvasGroup.alpha > 0f) {
        canvasGroup.alpha -= Time.deltaTime / fadeTime;
        yield return null;
      }
    }

    public IEnumerator FadeOut(float fadeTime) {
      while (canvasGroup.alpha > 0f) {
        canvasGroup.alpha -= Time.deltaTime / fadeTime;
        yield return null;
      }
    }
  }
}
