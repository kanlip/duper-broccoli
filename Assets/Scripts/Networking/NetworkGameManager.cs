/* Start Header **************************************************************/
/*!
\file       NetworkGameManager.cs
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
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class NetworkGameManager : MonoBehaviourPunCallbacks
    {
        public static NetworkGameManager Instance;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> 'Network Game Manager' playerPrefab Reference", this);
            }
            else
            {
                if (NetworkPlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0.0f, 25.0f, 0.0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("NetworkLauncher");
        }

        /*
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }
        */

        #endregion

        
        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion
        /*
        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            //Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("Loading NetworkedRoom");
            //PhotonNetwork.LoadLevel("NetworkedRoom");
        }
        

        #endregion
        */
    }
}