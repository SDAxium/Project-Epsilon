using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class FadeScreen : MonoBehaviour
    {
        public bool fadeOnStart = true;
        
        /// <summary>
        /// How long it takes the fade to complete
        /// Default: 2 seconds
        /// </summary>
        public float fadeDuration = 2;

        /// <summary>
        /// The color that the screen will fade to
        /// </summary>
        public Color fadeColor;
        
        /// <summary>
        /// Reference to the renderer of the fade screen gameObject
        /// </summary>
        private Renderer rend;
        
        // Start is called before the first frame update
        void Start()
        {
            rend = GetComponent<MeshRenderer>();
            if (fadeOnStart) FadeIn();
            
        }

        public void FadeIn()
        {
            Fade(1,0);
        }
        
        public void FadeOut()
        {
            Fade(0,1);
        }
        
        public void Fade(float alphaIn, float alphaOut)
        {
            StartCoroutine(FadeRoutine(alphaIn, alphaOut));
        }

        public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
        {
            float timer = 0;
            while (timer <= fadeDuration)
            {
                Color newColor = fadeColor;
                newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
                rend.material.color = newColor;
                timer += Time.deltaTime;
                yield return null;
            }
            Color newColor2 = fadeColor;
            newColor2.a = alphaOut;
            rend.material.color = newColor2;
        }
    }
}
