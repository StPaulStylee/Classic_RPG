using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cinematics {
  public class CinematicPublisher : MonoBehaviour {
    private PlayableDirector playableDirector;
    void Awake() {
      playableDirector = GetComponent<PlayableDirector>();
      if (playableDirector == null) {
        Debug.LogError($"The {name} attempted to find a PlayableDirector but found none");
        return;
      }
      playableDirector.played += OnPlay;
      playableDirector.stopped += OnStop;
    }

    private void OnPlay(PlayableDirector playableDirector) {
      Debug.Log("Cinematic has begun playing.");
    }

    private void OnStop(PlayableDirector playableDirector) {
      Debug.Log("The cinematic has stopped.");
    }
  }
}
