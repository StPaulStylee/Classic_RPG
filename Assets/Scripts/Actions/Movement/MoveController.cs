using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Actions.Movement {
  [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(HealthController))]
  public class MoveController : MonoBehaviour, IAction, ISaveable {
    private ActionScheduler actionScheduler;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private HealthController healthController;
    [SerializeField]
    private float maximumSpeed = 6f;
    private int forwardSpeedHash;
    private void Awake() {
      actionScheduler = GetComponent<ActionScheduler>();
      animator = GetComponent<Animator>();
      healthController = GetComponent<HealthController>();
      navMeshAgent = GetComponent<NavMeshAgent>();
      forwardSpeedHash = Animator.StringToHash("forwardSpeed");
    }

    void Update() {
      navMeshAgent.enabled = !healthController.IsDead;
      UpdateAnimator();
    }

    public void MoveToPosition(Vector3 position, float maxSpeedModifier) {
      navMeshAgent.isStopped = false;
      navMeshAgent.speed = maximumSpeed * maxSpeedModifier;
      SetDestination(position);
    }

    // This method is necessary because the AttackController needs to call MoveToPosition
    // when combat engages. We don't want that call to call the action scheduler.
    public void StartMoveAction(Vector3 position, float maxSpeedModifier) {
      actionScheduler.StartAction(this);
      MoveToPosition(position, maxSpeedModifier);
    }

    public void Cancel() {
      navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator() {
      Vector3 velocity = navMeshAgent.velocity;
      // InverseTransformDirection takes a direction of global value and converts it to a local value
      // This is necessary to get the proper values for our blend tree threshold values
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.z;
      animator.SetFloat(forwardSpeedHash, speed);
    }

    private void SetDestination(Vector3 point) {
      navMeshAgent.SetDestination(point);
    }

    public object CaptureState() {
      return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state) {
      SerializableVector3 position = (SerializableVector3)state;
      // Sometimes the nav mesh will mess with setting the positoin so disabling 
      // and enabling it fixes that.
      navMeshAgent.enabled = false;
      transform.position = position.ToVector();
      navMeshAgent.enabled = true;
    }
  }
}
