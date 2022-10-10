using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Actions.Combat {
  public class Projectile : MonoBehaviour {
    [SerializeField] private HealthController target;
    [SerializeField] private float speed;
    [SerializeField] private TrailRenderer trailRenderer;
    public float Damage { private get; set; }
    private float timeUntilDestroy = 0f;

    private void Awake() {
      trailRenderer = GetComponentInChildren<TrailRenderer>();
      if (trailRenderer == null) {
        Debug.LogWarning($"No trail render found on {gameObject.name}!");
      } else {
        timeUntilDestroy = trailRenderer.time;
      }
    }

    private void Update() {
      if (target) {
        transform.LookAt(GetLookAtPosition());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
      }
    }

    private void OnTriggerEnter(Collider other) {
      if (other.GetComponent<HealthController>() == target) {
        target.TakeDamage(Damage);
        StartCoroutine(DestroyAfterDelay());
        // Destroy(gameObject);
      }
    }

    private IEnumerator DestroyAfterDelay() {
      yield return new WaitForSeconds(timeUntilDestroy);
      Destroy(gameObject);
    }

    public void SetTarget(HealthController target) {
      this.target = target;
    }

    private Vector3 GetLookAtPosition() {
      CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
      if (collider) {
        // This works because of order of operations
        Vector3 position = target.transform.position + Vector3.up * (collider.height / 2);
        return position;
      }
      return target.transform.position;
    }
  }
}
