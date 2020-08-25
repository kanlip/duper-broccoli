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
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
	public class NetworkPlayerAnimatorManager : MonoBehaviourPun 
	{
        #region Private Fields
        public float fireCoolDown = 0.3f;
        public float moveSpeed = 1.5f;
        public float attackSpeed = 1.5f;
        #endregion

        #region Private Fields
        [SerializeField]
        private float elapsedTime = 0.0f;
        private float directionDampTime = 0.25f;
        Animator animator;

        GameObject leftJoystick;
        GameObject attackButton;
        private float vMobileMove;
        private float hMobileMove;
        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start () 
	    {
	        animator = GetComponent<Animator>();

            leftJoystick = GameObject.FindWithTag("GameController");
            attackButton = GameObject.FindWithTag("AttackButton");
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
#if UNITY_ANDROID || UNITY_IOS
            //attack button is pressed for mobile
            attackButton.GetComponent<Button>().onClick.AddListener(attackButtonPressed);

#else
            //pc input
            if (Input.GetButtonDown("Fire1") && elapsedTime > fireCoolDown)
            {
                animator.SetTrigger("Shoot");
                elapsedTime = 0;
            }
#endif
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

#if UNITY_ANDROID || UNITY_IOS

            //mobile input for movement of the player
            hMobileMove = leftJoystick.GetComponent<FixedJoystick>().Horizontal;
            vMobileMove = leftJoystick.GetComponent<FixedJoystick>().Vertical;

            animator.SetFloat("Speed", vMobileMove * vMobileMove + hMobileMove * hMobileMove, directionDampTime, Time.deltaTime);
            animator.SetFloat("Vertical_f", vMobileMove, directionDampTime, Time.deltaTime);
            animator.SetFloat("Direction", hMobileMove, directionDampTime, Time.deltaTime);

#else
            //pc input for movement of the player
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (Mathf.Abs(v) < 0.1f)
            {
                v = 0;
            }

            animator.SetFloat("Speed", v * v + h * h, directionDampTime, Time.deltaTime);
            animator.SetFloat("Vertical_f", v, directionDampTime, Time.deltaTime);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
#endif
        }

        void attackButtonPressed()
        {
            if (elapsedTime > fireCoolDown)
            {
                animator.SetTrigger("Shoot");
                elapsedTime = 0;
            }
        }

        public void DoDeadAnimation()
        {
            animator.SetTrigger("Dead");
        }
#endregion

    }
}