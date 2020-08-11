using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class PlayerAnimatorManager : MonoBehaviour//MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.25f;

        static int waveState = Animator.StringToHash("Arms Layer.Recoil");
        #endregion

        #region MonoBehaviour Callbacks

        private Animator animator;
        
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            //{
            //    return;
            //}

            if (!animator)
            {
                return;
            }
            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            // only allow jumping if we are running.
            //if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Shoot");
                }

                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("Jump");
                }
            }

            if (Input.GetButtonUp("Fire3"))
            {
                animator.SetBool("Wave", true);
            }
            AnimatorStateInfo stateInfo2 = animator.GetCurrentAnimatorStateInfo(1);
            if (stateInfo2.fullPathHash == waveState)
            {
                animator.SetBool("Wave", false);
            }


            if (!animator)
            {
                return;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (Mathf.Abs(v) < 0.1f)
            {
                v = 0;
            }
            
            animator.SetFloat("Speed",  v*v+h*h, directionDampTime, Time.deltaTime);
            animator.SetFloat("Vertical_f", v, directionDampTime, Time.deltaTime);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }

        #endregion
    }
}