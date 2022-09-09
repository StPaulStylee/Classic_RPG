using System.Collections;
using System.Collections.Generic;
using Game.Actions.Combat;
using Game.Actions.Movement;
using Game.Core;
using UnityEngine;

namespace Game.Actions {
  [RequireComponent(typeof(AttackController), typeof(MoveController), typeof(HealthController))]
  public class AIActionController : MonoBehaviour {
    [Header("Patrol Data")]
    [Tooltip("The radius (in Unity units) that will trigger chase of player when entered into by player")]
    [SerializeField]
    private float chaseRadius = 5f;
    [Tooltip("The fraction of the AI's max speed they will move while on patrol.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float patrolSpeedModifier = 0.2f;
    [Tooltip("The amount of time a patrol will linger after giving chase to the player")]
    [SerializeField]
    private float suspicionInterval = 3f;
    [Tooltip("The PatrolPathController reference that will indicate this AIs patrol path.")]
    [SerializeField]
    private PatrolPathController patrolPathController;
    [SerializeField]
    private int currentPatrolPathIndex = -1;
    [Tooltip("The distance the AI object must reach from its current patrol waypoint before it will go to next waypoint")]
    [SerializeField]
    private float patrolWaypointTolerance = 1f;
    [Tooltip("The amount of time the AI object will wait at a waypoint before moving to the next one")]
    [SerializeField]
    private float waypointWaitInterval = 1f;
    public Vector3 guardPatrolPosition;
    private float timeSinceLastSawPlayer = Mathf.Infinity;
    private float timeAtPatrolWaypoint = Mathf.Infinity;
    private GameObject playerInstance;
    private ActionScheduler actionScheduler;
    private AttackController attackController;
    private MoveController moveController;
    private HealthController healthController;

    #region Monobehavior Methods
    private void Awake() {
      actionScheduler = GetComponent<ActionScheduler>();
      attackController = GetComponent<AttackController>();
      moveController = GetComponent<MoveController>();
      healthController = GetComponent<HealthController>();
      SetInitialPosition();
    }

    private void Start() {
      playerInstance = GameObject.FindWithTag("Player");
      if (playerInstance == null) {
        Debug.LogError($"{name} did not locate a player in the scene!");
      }
      if (patrolPathController == null) {
        Debug.LogWarning($"This {name} does not have an associated PatrolPathController. This must be assigned in the editor!");
      }
    }

    private void Update() {
      if (healthController.IsDead) {
        return;
      }
      ExecutePatrolBehavior();
    }

    private void OnDrawGizmos() {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
    #endregion

    private void ExecutePatrolBehavior() {
      if (IsInChaseRange() && attackController.IsAttackable(playerInstance) && !attackController.IsWithinRange(playerInstance)) {
        timeSinceLastSawPlayer = 0f;
        moveController.StartMoveAction(playerInstance.transform.position, 0.8f);
        return;
      }
      if (IsInChaseRange() && attackController.IsAttackable(playerInstance)) {
        timeSinceLastSawPlayer = 0f;
        attackController.Attack(playerInstance);
        return;
      }
      if (!IsInChaseRange() && IsInSuspicionMode()) {
        timeSinceLastSawPlayer += Time.deltaTime;
        actionScheduler.CancelCurrentAction();
        return;
      }
      timeSinceLastSawPlayer += Time.deltaTime;
      SetCurrentPosition();
    }

    private bool IsInChaseRange() {
      float distance = Vector3.Distance(transform.position, playerInstance.transform.position);
      return distance <= chaseRadius;
    }

    private bool IsInSuspicionMode() {
      return timeSinceLastSawPlayer <= suspicionInterval;
    }

    private void SetCurrentPosition() {
      if (patrolPathController == null) {
        moveController.StartMoveAction(guardPatrolPosition, patrolSpeedModifier);
        return;
      }
      if (IsAtPatrolWaypoint() && IsInWaypointWaitMode()) {
        timeAtPatrolWaypoint += Time.deltaTime;
        return;
      }
      if (IsAtPatrolWaypoint()) {
        SetNextPatrolWaypoint();
        timeAtPatrolWaypoint = 0f;
      }
      moveController.StartMoveAction(guardPatrolPosition, patrolSpeedModifier);
    }

    private bool IsAtPatrolWaypoint() {
      return Vector3.Distance(transform.position, guardPatrolPosition) <= patrolWaypointTolerance;
    }

    private bool IsInWaypointWaitMode() {
      return timeAtPatrolWaypoint <= waypointWaitInterval;
    }

    private void SetNextPatrolWaypoint() {
      if (patrolPathController == null) {
        Debug.LogError($"{name} is calling 'GetNextPatrolWaypoint' when it doesn't have a pathPatrolPathController assigned to it!");
      }
      currentPatrolPathIndex = patrolPathController.GetNextIndex(currentPatrolPathIndex);
      guardPatrolPosition = patrolPathController.GetWaypoint(currentPatrolPathIndex);
    }

    private void SetInitialPosition() {
      if (guardPatrolPosition != null) {
        guardPatrolPosition = transform.position;
      }
    }
  }
}
