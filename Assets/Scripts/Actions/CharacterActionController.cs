using System.Collections;
using System.Collections.Generic;
using Game.InputActions;
using Game.Actions.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Actions.Combat;
using Game.Core;

namespace Game.Actions {
  [RequireComponent(typeof(AttackController), typeof(MoveController), typeof(HealthController))]
  public class CharacterActionController : MonoBehaviour {
    private bool isMovementEnabled;
    private AttackController attackController;
    private MoveController moveController;
    private HealthController healthController;
    private CharacterControls playerInput;
    private Camera mainCamera;

    private void Awake() {
      playerInput = new CharacterControls();
      playerInput.CharacterController.Move.performed += OnMove;
      playerInput.CharacterController.Move.canceled += OnMove;
      attackController = GetComponent<AttackController>();
      moveController = GetComponent<MoveController>();
      healthController = GetComponent<HealthController>();
    }

    void Start() {
      mainCamera = Camera.main;
      Debug.Log("CharacterActionController: " + transform.position);
    }

    private void OnDisable() {
      playerInput.Disable();
    }

    private void OnEnable() {
      playerInput.Enable();
    }

    void Update() {
      if (healthController.IsDead) {
        return;
      }
      Move();
    }

    private bool IsCombat() {
      RaycastHit[] hits = Physics.RaycastAll(GetCursorRay());
      for (int i = 0; i < hits.Length; i++) {
        RaycastHit hit = hits[i];
        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        if (target == null) {
          continue;
        }
        if (attackController.IsAttackable(target.gameObject) && !attackController.IsWithinRange(target.gameObject)) {
          moveController.StartMoveAction(target.transform.position, 1f);
          attackController.SetTarget(target.gameObject);
          return false;
        }
        if (attackController.IsAttackable(target.gameObject)) {
          attackController.Attack(target.gameObject);
          return true;
        }
      }
      return false;
    }

    private void Move() {
      // This method may need to be refactored by having the Raycast pulled out of the condition
      // so it can fire every frame. We will be using "cursor affordance" later on in order
      // to toggle the curson sprite. I am thinking I can create some sort of cursor script
      // that can do this work instead but in case not, I have made this note.
      if (isMovementEnabled) {
        bool hasHit = Physics.Raycast(GetCursorRay(), out RaycastHit hitDetails);
        if (hasHit) {
          moveController.StartMoveAction(hitDetails.point, 1f);
          return;
        }
        Debug.Log("Nothing to do.");
      }
    }

    private Ray GetCursorRay() {
      Vector2 cursorPosition = playerInput.CharacterController.CursorPosition.ReadValue<Vector2>();
      Ray ray = mainCamera.ScreenPointToRay(cursorPosition);
      return ray;
    }

    private void OnMove(InputAction.CallbackContext ctx) {
      if (ctx.performed) {
        isMovementEnabled = !IsCombat(); ;
        return;
      }
      if (ctx.canceled) {
        isMovementEnabled = false;
        return;
      }
    }
  }
}
