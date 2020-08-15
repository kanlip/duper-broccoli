/* Start Header **************************************************************/
/*!
\file       Arrow.cs
\author     Eugene Lee Yuih Chin
\StudentNo  6572595
\par        xelycx@gmail.com
\date       15.8.2020
\brief

Reproduction or disclosure of this file or its contents
without the prior written consent of author is prohibited.
*/
/* End Header ****************************************************************/

using Photon.Pun;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class Arrow : MonoBehaviourPun
    {

        public float speed = 100.0f;
        public float lifeTime = 3.0f;
        private Rigidbody rb;
        public GameObject owner;
        //private GameObject collidedGameObj;

        IEnumerator destroyArrow()
        {
            yield return new WaitForSeconds(lifeTime);
            //this.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.All);
            Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(destroyArrow());
            transform.Rotate(Vector3.right, -90);
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            //transform.Translate(Vector3.forward * Time.deltaTime * speed);
            //should be foward but arrow is rotated at x axis -90 deg
            //transform.Translate(Vector3.down * Time.deltaTime * speed);


            rb.AddForce(-rb.transform.up * Time.deltaTime * speed, ForceMode.Impulse);
        }

        /*
        public void OnTriggerEnter(Collider other)
        {
            //if (!photonView.IsMine)
            //{
            //    return;
            //}
            int damageAmount = 10;
            if (other.gameObject.tag == "Player" && other.gameObject != owner)
            {
                collidedGameObj = other.gameObject;
                collidedGameObj.GetComponent<NetworkPlayerManager>().TakeDamage(damageAmount);
                //collidedGameObj.GetPhotonView().RPC("TakeDamage", RpcTarget.All, damageAmount);
                Debug.Log("damage");
                this.gameObject.SetActive(false);
                //this.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
        }
        */

        [PunRPC]
        public void DestroyRPC()
        {
            Destroy(this.gameObject);
        }

        //[PunRPC]
        //public void TakeDamage(int damageAmount)
        //{
        //    if (collidedGameObj)
        //    {
        //        collidedGameObj.GetComponent<NetworkPlayerManager>().TakeDamage(damageAmount);
        //    }
        //}

        public void SetOwner(GameObject shooter)
        {
            owner = shooter;
        }

    }
}