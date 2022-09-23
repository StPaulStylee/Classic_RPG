using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
  public class PersistentObjectsSpawner : MonoBehaviour {
    [SerializeField] GameObject persistentObjectsPrefab;
    static bool hasSpawned = false;
    private void Awake() {
      if (hasSpawned) {
        return;
      }
      DontDestroyOnLoad(SpawnPersistentObjects());
      hasSpawned = true;
    }

    private GameObject SpawnPersistentObjects() {
      return Instantiate(persistentObjectsPrefab);
    }
  }
}
