/* Start Header **************************************************************/
/*!
\file       NetworkPlayerAnimatorManager.cs
\author     Eugene Lee Yuih Chin, developer@exitgames.com
\StudentNo  6572595
\par        xelycx@gmail.com
\date       15.8.2020
\brief

Reproduction or disclosure of this file or its contents
without the prior written consent of author is prohibited.
*/
/* End Header ****************************************************************/

using Photon.Pun;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
	public class NetworkPlayerAnimatorManager : MonoBehaviourPun 
	{
        #region Private Fields
        public float fireCoolDown = 0.5f;
        public float moveSpeed = 1.5f;
        public float attackSpeed = 1.5f;
        #endregion

        #region Private Fields
        [SerializeField]
        private float elapsedTime = 0.0f;
        private float directionDampTime = 0.25f;
        Animator animator;
        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start () 
	    {
	        animator = GetComponent<Animator>();
	    }
	        
		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
	    void Update () 
	    {
            elapsedTime += Time.deltaTime;
            // Prevent control is connected to Photon and represent the localPlayer
            if ( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
	        {
	            return;
	        }

			// failSafe is missing Animator component on GameObject
	        if (!animator)
	        {
				return;
			}

            if (Input.GetButtonDown("Fire1") && elapsedTime > fireCoolDown)
            {
                animator.SetTrigger("Shoot");
                elapsedTime = 0;
            }

            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                //if (Input.GetButtonDown("Jump"))
                //    animator.SetTrigger("Jump");
                animator.speed = moveSpeed;
            }
            else if (stateInfo.IsName("Base Layer.Shoot"))
            {
                animator.speed = attackSpeed;
            }
            else { animator.speed = 1; }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (Mathf.Abs(v) < 0.1f)
            {
                v = 0;
            }

            animator.SetFloat("Speed", v * v + h * h, directionDampTime, Time.deltaTime);
            animator.SetFloat("Vertical_f", v, directionDampTime, Time.deltaTime);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }

		#endregion

	}
}