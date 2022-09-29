using System;
using System.Collections;
using System.Collections.Generic;
using Game.InputActions;
using Game.Saving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.SceneManagement {
  [RequireComponent(typeof(SavingSystem))]
  public class SavingWrapper : MonoBehaviour {
    const string defaultSaveFile = "save";
    private CharacterControls playerInput;
    private SavingSystem savingSystem;
    private Fader sceneFadeInOut;
    void Awake() {
      savingSystem = GetComponent<SavingSystem>();

      playerInput = new CharacterControls();
      playerInput.QuickActionsController.Enable();
      playerInput.QuickActionsController.Save.performed += OnSave;
      playerInput.QuickActionsController.Load.performed += OnLoad;
    }

    private IEnumerator Start() {
      sceneFadeInOut = FindObjectOfType<Fader>();
      if (sceneFadeInOut == null) {
        Debug.LogError("No Fader component found in scene!");
      }
      sceneFadeInOut.SetToMaxOpacity();
      yield return savingSystem.LoadLastScene(defaultSaveFile);
      yield return sceneFadeInOut.FadeOut(1.5f);
    }

    private void OnSave(InputAction.CallbackContext ctx) {
      Save();
    }

    public void Save() {
      savingSystem.Save(defaultSaveFile);
    }

    private void OnLoad(InputAction.CallbackContext ctx) {
      Load();
    }

    public void Load() {
      savingSystem.Load(defaultSaveFile);
    }
  }
}
