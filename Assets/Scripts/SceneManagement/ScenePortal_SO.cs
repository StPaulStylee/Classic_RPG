using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SceneManagement.SO {
  [CreateAssetMenu(fileName = "New Scene Portal SO", menuName = "Scene Portal")]
  public class ScenePortal_SO : ScriptableObject {
    [SerializeField]
    int sceneIndexToLoad;
    public int SceneIndexToLoad => sceneIndexToLoad;

    [SerializeField]
    ScenePortal_SO portalPosition;
    public ScenePortal_SO PortalPosition => portalPosition;
  }
}
