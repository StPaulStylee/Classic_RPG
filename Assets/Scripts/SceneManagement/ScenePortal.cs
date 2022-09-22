using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Game.SceneManagement.SO;

namespace Game.SceneManagement {
  public class ScenePortal : MonoBehaviour {
    // public int sceneIndexToLoad = 0;
    // enum PortalIdentifier {
    //   A, B, C, D, E
    // }
    [SerializeField]
    ScenePortal_SO scenePortalData;
    // [SerializeField]
    // PortalIdentifier portalId;
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
      yield return SceneManager.LoadSceneAsync(scenePortalData.SceneIndexToLoad);
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
      // Setting the player position this way is necessary otherwise the NavMeshAgent can conflict
      // with where we try to set the player position using player.transform.position = ...
      player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
      player.transform.rotation = portal.spawnPoint.rotation;
    }

    private ScenePortal GetSpawnToPortal() {
      ScenePortal[] portals = FindObjectsOfType<ScenePortal>();
      foreach (ScenePortal portal in portals) {
        // Right now this would require you to spawn to another scene - no intra scene portalling
        // Seems like you could just remove the sceneIndexToLoad but right now not all of the portals
        // are getting destroyed on load and it's causing conflicts.
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
