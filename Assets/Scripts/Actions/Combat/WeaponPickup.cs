using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions.Combat {
  [RequireComponent(typeof(BoxCollider), typeof(Weapon))]
  public class WeaponPickup : MonoBehaviour {
    [SerializeField] private Weapon weaponSO;
    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
        other.GetComponent<AttackController>().EquipWeapon(weaponSO);
        // Maybe we just disable it instead?
        Destroy(gameObject);
      }
    }
  }
}
