/* Start Header **************************************************************/
/*!
\file       NetworkPlayerUI.cs
\author     Eugene Lee Yuih Chin, developer@exitgames.com
\StudentNo  6572595
\par        xelycx@gmail.com
\date       15.8.2020
\brief

Reproduction or disclosure of this file or its contents
without the prior written consent of author is prohibited.
*/
/* End Header ****************************************************************/

using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

namespace Com.MyCompany.MyGame
{
    public class NetworkPlayerUI : MonoBehaviour
    {
        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        public Text playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        public Slider playerHealthSlider;

        #region Private Fields

        private NetworkPlayerManager target;

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;

        #endregion
        void Awake()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        #region MonoBehaviour Callbacks
        void Update()
        {
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            // Reflect the Player Health
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.getHealth();
            }
        }

        void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }


            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        #endregion


        #region Public Methods
        public void SetTarget(NetworkPlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;

            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }

            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }

        #endregion
    }
}