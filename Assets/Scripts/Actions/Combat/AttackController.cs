using System;
using System.Collections;
using System.Collections.Generic;
using Game.Actions.Movement;
using Game.Core;
using UnityEngine;

namespace Game.Actions.Combat {
  [RequireComponent(typeof(Animator))]
  public class AttackController : MonoBehaviour, IAction {
    [Tooltip("The range (in units) the character must be within to perform an attack. Character will move to within range if Attack() is called when not within range.")]
    [SerializeField]
    private float range = 2f;
    [Tooltip("The amount of damage the character will deliver on each attack routine.")]
    [SerializeField]
    private float damage = 5f;
    [Tooltip("This set's the required delay between each attack iteration. The larger the value, the slower the attack.")]
    [SerializeField]
    private float timeBetweenAttack = 0f;
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

    private void Update() {
      timeSincelastAttack += Time.deltaTime;
      if (target != null && IsWithinRange()) {
        moveController.Cancel();
        ExecuteAttackRoutine();
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

    public bool IsWithinRange(GameObject target) {
      float distance = Vector3.Distance(transform.position, target.transform.position);
      if (distance > range) {
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

    private bool IsAttackEnabled() {
      return timeSincelastAttack >= timeBetweenAttack;
    }

    private bool IsWithinRange() {
      float distance = Vector3.Distance(transform.position, target.transform.position);
      if (distance > range) {
        return false;
      }
      return true;
    }

    // Animation Event
    private void Hit() {
      if (target == null) {
        return;
      }
      target.TakeDamage(damage);
    }
  }
}
