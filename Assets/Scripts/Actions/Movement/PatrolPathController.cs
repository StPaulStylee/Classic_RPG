using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actions.Movement {
  public class PatrolPathController : MonoBehaviour {
    [SerializeField]
    private float gizmoRadius = 1f;
    private void OnDrawGizmos() {
      for (int i = 0; i < transform.childCount; i++) {
        int j = 0;
        if (i < transform.childCount - 1) {
          j = GetNextIndex(i);
        }
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(GetWaypoint(i), gizmoRadius);
        Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
      }
    }

    public int GetNextIndex(int i) {
      if (i == transform.childCount - 1) {
        return 0;
      }
      return i + 1;
    }

    public Vector3 GetWaypoint(int i) {
      return transform.GetChild(i).position;
    }
  }
}
