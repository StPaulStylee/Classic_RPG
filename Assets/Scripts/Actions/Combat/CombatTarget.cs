using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Actions.Combat {
  [RequireComponent(typeof(HealthController))]
  public class CombatTarget : MonoBehaviour {
    // private bool IsWithinRange() {
    //   float distance = Vector3.Distance(transform.position, target.transform.position);
    //   if (distance > range) {
    //     return false;
    //   }
    //   return true;
    // }
  }
}
