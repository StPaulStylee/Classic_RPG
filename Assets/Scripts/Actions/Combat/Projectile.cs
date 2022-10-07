using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions.Combat {
  public class Projectile : MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] float speed;

    private void Start() {
    }
    private void Update() {
      if (target) {
        transform.LookAt(GetLookAtPosition());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
      }
    }

    private Vector3 GetLookAtPosition() {
      CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
      if (collider) {
        // This works because of order of operations
        Vector3 position = target.position + Vector3.up * (collider.height / 2);
        return position;
      }
      return target.position;
    }
  }
}
