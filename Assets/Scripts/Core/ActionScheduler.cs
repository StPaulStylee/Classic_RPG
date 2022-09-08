using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
  public class ActionScheduler : MonoBehaviour {
    public IAction CurrentAction { get; private set; }
    public void StartAction(IAction action) {
      if (CurrentAction == action) {
        return;
      }
      if (CurrentAction != null) {
        Debug.Log($"Cancelling {CurrentAction}");
        CurrentAction.Cancel();
      }
      Debug.Log($"Starting {action}");
      CurrentAction = action;
    }

    public void CancelCurrentAction() {
      StartAction(null);
    }
  }
}
