using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Actions.Combat {
  [CreateAssetMenu(fileName = "Weapon_SO", menuName = "Weapons/New Weapon", order = 0)]
  public class Weapon : ScriptableObject {
    public bool HasProjectile => projectile != null;

    [SerializeField] private AnimatorOverrideController overrideController;
    [Tooltip("The weapon that the character is wielding. null is equal to no weapon")]
    [SerializeField] private GameObject weaponPrefab = null;
    [Tooltip("The weapons projectile prefab, if it needs one.")]
    [SerializeField] private Projectile projectile;
    [Tooltip("Is this weapon going to be held in the right hand?")]
    [SerializeField] private bool isRightHanded = true;
    [Tooltip("The range (in units) the character must be within to perform an attack. Character will move to within range if Attack() is called when not within range.")]
    [field: SerializeField] public float Range { get; private set; }

    [Tooltip("The amount of damage the character will deliver on each attack routine.")]
    [field: SerializeField] public float Damage { get; private set; }

    [field: Tooltip("This set's the required delay between each attack iteration. The larger the value, the slower the attack.")]
    [field: SerializeField] public float TimeBetweenAttack { get; private set; }

    public void FireProjectile(Transform rightHand, Transform leftHand, HealthController target) {
      Transform handTransform = isRightHanded ? rightHand : leftHand;
      Projectile projectile = Instantiate(this.projectile, handTransform.position, Quaternion.identity);
      projectile.Damage = Damage;
      projectile.SetTarget(target);
    }

    public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
      if (weaponPrefab) {
        Transform handTransform = isRightHanded ? rightHand : leftHand;
        Instantiate(weaponPrefab, handTransform);
      }
      if (overrideController) {
        animator.runtimeAnimatorController = overrideController;
      }
    }
  }
}
