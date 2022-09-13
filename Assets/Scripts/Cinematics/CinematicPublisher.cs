using System.Collections;
using System.Collections.Generic;
using Game.Actions;
using Game.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cinematics {
  public class CinematicPublisher : MonoBehaviour {
    private PlayableDirector playableDirector;
    private GameObject player;
    void Awake() {
      playableDirector = GetComponent<PlayableDirector>();
      if (playableDirector == null) {
        Debug.LogError($"The {name} attempted to find a PlayableDirector but found none");
        return;
      }
      playableDirector.played += OnPlay;
      playableDirector.stopped += OnStop;
    }

    private void Start() {
      player = GameObject.FindWithTag("Player");
    }

    private void OnPlay(PlayableDirector playableDirector) {
      Debug.Log("Cinematic has begun playing.");
      player.GetComponent<ActionScheduler>().CancelCurrentAction();
      player.GetComponent<CharacterActionController>().enabled = false;
    }

    private void OnStop(PlayableDirector playableDirector) {
      Debug.Log("The cinematic has stopped.");
      player.GetComponent<CharacterActionController>().enabled = true;
    }
  }
}
