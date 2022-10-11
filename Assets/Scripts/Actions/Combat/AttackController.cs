using System;
using System.Collections;
using System.Collections.Generic;
using Game.Actions.Movement;
using Game.Core;
using Game.Saving;
using UnityEngine;

namespace Game.Actions.Combat {
  [RequireComponent(typeof(Animator))]
  public class AttackController : MonoBehaviour, IAction {
    // public delegate void ProjectileHitDelegate();
    // public static ProjectileHitDelegate OnProjectileHit = delegate { };
    [Tooltip("The location of the weapon")]
    [SerializeField] private Transform rightHandTransform = null;
    [SerializeField] private Transform leftHandTransform = null;

    [Tooltip("The weapon scriptable object to assign to the character")]
    [SerializeField] private Weapon defaultWeaponSO = null;
    [SerializeField] private Weapon currentWeaponSO = null;
    private float timeSincelastAttack = Mathf.Infinity;
    private HealthController target;
    private MoveController moveController;
    private ActionScheduler actionScheduler;
    private Animator animator;
    private int onAttackHash;
    private int onStopAttackHash;

    private void Awake() {
      actionScheduler = GetComponent<ActionScheduler>();
      animator = GetComponent<Animator>();
      moveController = GetComponent<MoveController>();
      onAttackHash = Animator.StringToHash("onAttack");
      onStopAttackHash = Animator.StringToHash("onStopAttack");
      if (moveController == null) {
        Debug.LogError($"{gameObject.name} attempting to assign instance field in {name} but it isn't assigned.");
      }
    }

    private void Start() {
      EquipWeapon(defaultWeaponSO);
    }

    private void Update() {
      timeSincelastAttack += Time.deltaTime;
      if (target != null && IsWithinRange()) {
        moveController.Cancel();
        ExecuteAttackRoutine();
      }
    }

    public void EquipWeapon(Weapon weaponSO) {
      DestroyCurrentWeapon();
      if (weaponSO && (rightHandTransform || leftHandTransform)) {
        currentWeaponSO = weaponSO;
        weaponSO.Spawn(rightHandTransform, leftHandTransform, animator);
      }
    }

    public void Attack(GameObject attackTarget) {
      actionScheduler.StartAction(this);
      target = attackTarget.GetComponent<HealthController>();
    }

    public void Attack() {
      if (target == null) {
        Debug.LogError($"Calling attack via {transform.name} but no target is set!");
        return;
      }
      actionScheduler.StartAction(this);
    }

    public void Cancel() {
      animator.ResetTrigger(onAttackHash);
      animator.SetTrigger(onStopAttackHash);
      target = null;
    }

    public bool IsWithinRange(GameObject target) {
      float distance = Vector3.Distance(transform.position, target.transform.position);
      if (distance > currentWeaponSO.Range) {
        return false;
      }
      return true;
    }

    public bool IsAttackable(GameObject target) {
      if (target != null && !target.GetComponent<HealthController>().IsDead) {
        return true;
      }
      return false;
    }

    public void SetTarget(GameObject target) {
      HealthController targetHealthCtrl = target.GetComponent<HealthController>();
      if (targetHealthCtrl) {
        this.target = targetHealthCtrl;
      }
    }

    private void DestroyCurrentWeapon() {
      if (currentWeaponSO) {
        currentWeaponSO.Remove();
      }
    }

    private void ExecuteAttackRoutine() {
      if (target.GetComponent<HealthController>().IsDead) {
        Cancel();
        return;
      }
      if (IsAttackEnabled()) {
        Attack();
        animator.ResetTrigger(onStopAttackHash);
        transform.LookAt(target.transform);
        animator.SetTrigger(onAttackHash);
        timeSincelastAttack = 0f;
      }
    }

    private bool IsAttackEnabled() {
      return timeSincelastAttack >= currentWeaponSO.TimeBetweenAttack;
    }

    private bool IsWithinRange() {
      float distance = Vector3.Distance(transform.position, target.transform.position);
      if (distance > currentWeaponSO.Range) {
        return false;
      }
      return true;
    }

    // Animation Events
    private void Hit() {
      if (target == null) {
        return;
      }
      target.TakeDamage(currentWeaponSO.Damage);
    }

    private void Shoot() {
      if (target == null) {
        return;
      }
      if (currentWeaponSO.HasProjectile) {
        currentWeaponSO.FireProjectile(rightHandTransform, leftHandTransform, target);
      }
    }
  }
}
