using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions.Combat {
  [CreateAssetMenu(fileName = "Weapon_SO", menuName = "Weapons/New Weapon", order = 0)]
  public class Weapon : ScriptableObject {
    [SerializeField] private AnimatorOverrideController overrideController;
    [Tooltip("The weapon that the character is wielding. null is equal to no weapon")]
    [SerializeField] private GameObject weaponPrefab = null;
    [Tooltip("The range (in units) the character must be within to perform an attack. Character will move to within range if Attack() is called when not within range.")]
    [field: SerializeField] public float Range { get; private set; }

    [Tooltip("The amount of damage the character will deliver on each attack routine.")]
    [field: SerializeField] public float Damage { get; private set; }

    [field: Tooltip("This set's the required delay between each attack iteration. The larger the value, the slower the attack.")]
    [field: SerializeField] public float TimeBetweenAttack { get; private set; }

    public void Spawn(Transform handTransform, Animator animator) {
      if (weaponPrefab && handTransform) {
        Instantiate(weaponPrefab, handTransform);
      }
      if (overrideController) {
        animator.runtimeAnimatorController = overrideController;
      }
    }
  }
}
