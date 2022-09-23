using System.Collections;
using System.Collections.Generic;
using Game.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cinematics {
  public class CinematicTrigger : MonoBehaviour, ISaveable {
    [Tooltip("Declares whether the PlayableDirector will play the cinematic or not")]
    [SerializeField]
    private bool isActive = true;

    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.CompareTag("Player")) {
        PlayableDirector cinematic = GetComponent<PlayableDirector>();
        if (cinematic == null) {
          Debug.LogError("Attempting to call Play on PlayableDirector but no PlayableDirector was found.");
          return;
        }
        if (isActive) {
          cinematic.Play();
          isActive = false;
          return;
        }
      }
    }

    public object CaptureState() {
      return isActive;
    }

    public void RestoreState(object state) {
      bool active = (bool)state;
      isActive = active;
    }
  }
}
