using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Game.SceneManagement.SO;

namespace Game.SceneManagement {
  public class ScenePortal : MonoBehaviour {
    [SerializeField]
    ScenePortal_SO scenePortalData;
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
      // Finding all of the portals in the current scene here so they can all be properly destroyed 
      // once the scene transition is finished. Another way to do this possibly is to just have the 
      // spawnTo position data in the SO.
      ScenePortal[] portals = FindObjectsOfType<ScenePortal>();
      yield return SceneManager.LoadSceneAsync(scenePortalData.SceneIndexToLoad);
      ScenePortal portalToSpawnTo = GetSpawnToPortal();
      SetPlayerPosition(portalToSpawnTo);
      foreach (ScenePortal portal in portals) {
        Destroy(portal.gameObject);
      }
    }

    private void SetPlayerPosition(ScenePortal portal) {
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player == null) {
        Debug.LogError("There is no Player GameObject in the scene!");
        return;
      }
      // Setting the player position this way is necessary otherwise the NavMeshAgent can conflict
      // with where we try to set the player position using player.transform.position = ...
      player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
      player.transform.rotation = portal.spawnPoint.rotation;
    }

    private ScenePortal GetSpawnToPortal() {
      ScenePortal[] portals = FindObjectsOfType<ScenePortal>();
      foreach (ScenePortal portal in portals) {
        if (portal == this) {
          continue;
        }
        if (portal.scenePortalData != scenePortalData.PortalPosition) {
          continue;
        }
        return portal;
      }
      return null;
    }
  }
}
