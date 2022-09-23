using System.Collections;
using System.Collections.Generic;
using Game.Saving;
using Game.Saving.Types;
using UnityEngine;

namespace Game.Core {
  [RequireComponent(typeof(Animator), typeof(ActionScheduler))]
  public class HealthController : MonoBehaviour, ISaveable {
    [Tooltip("The amount of health/hp given to the assiged game object")]
    [SerializeField]
    private float currentHealth = 100f;
    public bool IsDead { get; private set; } = false;
    private Animator animator;
    private ActionScheduler actionScheduler;
    private int onDieHash;
    private void Awake() {
      animator = GetComponent<Animator>();
      actionScheduler = GetComponent<ActionScheduler>();
      onDieHash = Animator.StringToHash("onDie");
    }

    public void TakeDamage(float damage) {
      currentHealth = Mathf.Clamp(currentHealth - damage, 0, currentHealth);
      // Debug.Log(currentHealth);
      if (currentHealth == 0) {
        Die();
      }
    }

    private void Die() {
      if (IsDead) {
        return;
      }
      IsDead = true;
      animator.SetTrigger(onDieHash);
      actionScheduler.CancelCurrentAction();
    }

    private void SetAnimationState() {
      if (IsDead) {
        animator.SetTrigger(onDieHash);
      }
    }

    public object CaptureState() {
      HealthData data = new HealthData();
      data.CurrentHealth = currentHealth;
      data.IsDead = IsDead;
      return data;
    }

    public void RestoreState(object state) {
      HealthData data = (HealthData)state;
      currentHealth = data.CurrentHealth;
      IsDead = data.IsDead;
      SetAnimationState();
    }
  }
}
