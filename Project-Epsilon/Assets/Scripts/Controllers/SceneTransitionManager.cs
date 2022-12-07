using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public FadeScreen fadeScreen;

        /// <summary>
        /// Opens scene at specified index
        /// </summary>
        /// <param name="sceneIndex">The build index of the scene to load</param>
        public void GoToScene(int sceneIndex)
        {
            StartCoroutine(GoToSceneRoutine(sceneIndex));
        }
        
        IEnumerator GoToSceneRoutine(int sceneIndex)
        {
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(fadeScreen.fadeDuration);
            PhotonNetwork.LoadLevel(sceneIndex);
        }
    }
}
