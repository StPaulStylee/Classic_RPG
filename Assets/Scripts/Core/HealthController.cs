using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
  [RequireComponent(typeof(Animator), typeof(ActionScheduler))]
  public class HealthController : MonoBehaviour {
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
  }
}
