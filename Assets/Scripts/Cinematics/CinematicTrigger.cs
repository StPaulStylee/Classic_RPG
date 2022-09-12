using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cinematics {
  public class CinematicTrigger : MonoBehaviour {
    [Tooltip("Declares whether the PlayableDirector will play the cinematic or not")]
    [SerializeField]
    private bool isActive = true;
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.CompareTag("Player")) {
        PlayableDirector cinematic = GetComponent<PlayableDirector>();
        if (cinematic && isActive) {
          cinematic.Play();
          isActive = false;
          return;
        }
        Debug.LogError("Attempting to call Play on PlayableDirector but no PlayableDirector was found.");
      }
    }
  }
}
