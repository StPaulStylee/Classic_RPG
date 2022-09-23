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
    void Awake() {
      savingSystem = GetComponent<SavingSystem>();
      playerInput = new CharacterControls();
      playerInput.QuickActionsController.Enable();
      playerInput.QuickActionsController.Save.performed += OnSave;
      playerInput.QuickActionsController.Load.performed += OnLoad;
    }

    private void OnSave(InputAction.CallbackContext ctx) {
      Save();
    }

    private void Save() {
      savingSystem.Save(defaultSaveFile);
    }

    private void OnLoad(InputAction.CallbackContext ctx) {
      Load();
    }

    private void Load() {
      savingSystem.Load(defaultSaveFile);
    }
  }
}
