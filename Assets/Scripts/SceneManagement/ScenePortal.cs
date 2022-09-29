using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Game.SceneManagement.SO;
using Game.Saving;

namespace Game.SceneManagement {
  public class ScenePortal : MonoBehaviour, ISaveable {
    [SerializeField] private ScenePortal_SO scenePortalData;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] Fader sceneFadeInOut;

    private SavingWrapper savingWrapper;
    private void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    private void Start() {
      savingWrapper = FindObjectOfType<SavingWrapper>();
      if (savingWrapper == null) {
        Debug.LogError("The saving wrapper is not available in the scene!");
      }
      if (sceneFadeInOut == null) {
        sceneFadeInOut = FindObjectOfType<Fader>();
      }
    }

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
        StartCoroutine(TransitionToScene());
      }
    }

    private IEnumerator TransitionToScene() {
      // StartCoroutine(sceneFadeInOut.FadeIn());
      yield return sceneFadeInOut.FadeIn();
      // Finding all of the portals in the current scene here so they can all be properly destroyed 
      // once the scene transition is finished.
      ScenePortal[] portals = FindObjectsOfType<ScenePortal>();
      savingWrapper.Save();
      yield return SceneManager.LoadSceneAsync(scenePortalData.SceneIndexToLoad);
      savingWrapper.Load();
      ScenePortal portalToSpawnTo = GetSpawnToPortal();
      SetPlayerPosition(portalToSpawnTo);
      savingWrapper.Save();
      yield return sceneFadeInOut.FadeWait();
      yield return sceneFadeInOut.FadeOut();
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

    public object CaptureState() {
      throw new NotImplementedException();
    }

    public void RestoreState(object state) {
      throw new NotImplementedException();
    }
  }
}
