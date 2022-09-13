using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneManagement {
  public class ScenePortal : MonoBehaviour {
    [SerializeField]
    int sceneIndexToLoad = 0;
    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
        SceneManager.LoadScene(sceneIndexToLoad);
      }
    }
  }
}
