using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneManagement {
  public class ScenePortal : MonoBehaviour {
    public int sceneIndexToLoad = 0;
    [SerializeField]
    Transform spawnPoint;
    private void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
        StartCoroutine(TransitionToScene());
      }
    }

    private IEnumerator TransitionToScene() {
      yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);
      ScenePortal portalToSpawnTo = GetSpawnToPortal();
      SetPlayerPosition(portalToSpawnTo);
      Destroy(gameObject);
    }

    private void SetPlayerPosition(ScenePortal portal) {
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player == null) {
        Debug.LogError("There is no Player GameObject in the scene!");
        return;
      }
      player.transform.position = portal.spawnPoint.position;
      player.transform.rotation = portal.spawnPoint.rotation;
    }

    private ScenePortal GetSpawnToPortal() {
      ScenePortal[] portals = FindObjectsOfType<ScenePortal>();
      foreach (ScenePortal portal in portals) {
        // This isn't smart enough. It doesn't allow for multiple portals in a scene
        if (portal.sceneIndexToLoad != this.sceneIndexToLoad) {
          return portal;
        }
      }
      return null;
    }
  }
}
