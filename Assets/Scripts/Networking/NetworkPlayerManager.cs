/* Start Header **************************************************************/
/*!
\file       NetworkPlayerManager.cs
\author     Eugene Lee Yuih Chin, Oh Hui Chin, developer@exitgames.com
\StudentNo  6572595, 6213303
\par        xelycx@gmail.com, huichin339@hotmail.com
\date       21.8.2020
\brief

Reproduction or disclosure of this file or its contents
without the prior written consent of author is prohibited.
*/
/* End Header ****************************************************************/

using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
#pragma warning disable 649

    /// <summary>
    /// Network Player manager.
    /// Handles fire Input
    /// </summary>
    public class NetworkPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields
        public GameObject arrowPrefab;
        public Transform arrowSpawn;
        public float fireCoolDown = 0.3f;

        //camera left right
        public float SENS_HOR = 3.0f;
        public float SENS_VER = 2.0f;

        //[Tooltip("The current Health of our player")]
        //public float Health = 1f;
        public bool isDead = false;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields
        float elapsedTime = 0.0f;
        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject playerUiPrefab;

        //True, when the user is firing
        bool IsFiring;

        GameObject rightJoystick;
        GameObject attackButton;

        GameObject potionButton;
        GameObject amountOfPotion;
        GameObject shootSound;

        GameObject gameOverPanel;
        GameObject cameraMove;

        [SerializeField]
        private float Health;
        [SerializeField]
        private float maxHealth;
        private float restoreHp = 10f;
        private int playerPotionAmt = 5;

        NetworkPlayerAnimatorManager npam;
        UnityStandardAssets.Cameras.FreeLookCam flc;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        public void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);

            //set the current health to 100
            this.Health = 100f;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        public void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }

                Camera cam = Camera.main;
                if (cam != null)
                {
#if !(UNITY_ANDROID || UNITY_IOS)
                    if(this.GetComponent<TPSCamera>() && photonView.IsMine)
                        this.GetComponent<TPSCamera>().enabled = true;

#endif


                }
            }
            else
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
            }

            // Create the UI
            if (this.playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(this.playerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            rightJoystick = GameObject.FindWithTag("RotateCam");
            attackButton = GameObject.FindWithTag("AttackButton");


            potionButton = GameObject.FindWithTag("HealthPotion");
            amountOfPotion = GameObject.FindWithTag("HealthPotionAmount");

            //display the amount of potion
            amountOfPotion.GetComponent<Text>().text = playerPotionAmt.ToString();

            //for when player click on the potion button
            potionButton.GetComponent<Button>().onClick.AddListener(potionRestoreHp);

            //set the max health to current health
            maxHealth = this.Health;

            npam = GetComponent<NetworkPlayerAnimatorManager>();
            flc = GetComponent<UnityStandardAssets.Cameras.FreeLookCam>();

            //get the game over panel
            
            gameOverPanel = GameObject.Find("GameOverPanel");
            if (gameOverPanel)
                gameOverPanel.SetActive(false);

            cameraMove = GameObject.Find("Pivot");

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }


        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();

#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// Process Inputs if local player.
        /// Watch for end of game, when local player health is 0.
        /// </summary>
        public void Update()
        {
            elapsedTime += Time.deltaTime;


            // we only process Inputs and check health if we are the local player
            if (!isDead && photonView.IsMine)
            {
                this.ProcessInputs();

                if (this.Health <= 0f)
                {
                    //play dead
                    isDead = true;

                    if (npam)
                    {
                        npam.DoDeadAnimation();
                        npam.isDead = true;
                    }
                }
            }

            if(isDead)
            {
                if (flc && photonView.IsMine)
                {
                    flc.enabled = false;
                }
                //show the game over panel
                if (gameOverPanel)
                    gameOverPanel.SetActive(true);

                //NetworkGameManager.Instance.LeaveRoom();
            }
        }

        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player
        /// </summary>
        public void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            //if (other.tag == "Arrow" && Health >= 0
            //&& other.GetComponent<Arrow>().owner != this.gameObject)
            //{
            //    this.Health -= 10.0f;
            //    other.gameObject.SetActive(false);
            //}


            //if (other.name.Contains("Arrow") && other.gameObject.GetComponent<Arrow>().owner != this.gameObject)
            //{
            //    this.Health -= 10.0f;
            //    //other.gameObject.SetActive(false);
            //}
        }

        /// <summary>
        /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        /// We're going to affect health while the colliders are interesting the player
        /// </summary>
        /// <param name="other">Other.</param>
        public void OnTriggerStay(Collider other)
        {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine)
            {
                return;
            }
        }


#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif


        /// <summary>
        /// MonoBehaviour method called after a new level of index 'level' was loaded.
        /// We recreate the Player UI because it was destroy when we switched level.
        /// Also reposition the player if outside the current arena.
        /// </summary>
        /// <param name="level">Level index loaded</param>
        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

#endregion

#region Private Methods


#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif
        [PunRPC]
        public void Shoot(Quaternion shootRotation)
        {
            
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawn.position, arrowSpawn.rotation);
            arrow.transform.rotation = shootRotation;
            arrow.GetComponent<Arrow>().SetOwner(this.gameObject);
            //shooting sound
            shootSound = GameObject.FindWithTag("ShootSound");
            shootSound.GetComponent<AudioSource>().Play();
        }

        //[PunRPC]
        //private void Shoot()//Vector3 pos, Vector3 dir)
        //{
        //    GameObject arrow = PhotonNetwork.Instantiate(arrowPrefab.name, arrowSpawn.position, arrowSpawn.transform.rotation);
        //    arrow.GetComponent<Arrow>().SetOwner(this.gameObject);

        //    if(!photonView.IsMine)
        //    {
        //        this.photonView.RPC("Fire", RpcTarget.All, pos, dir);
        //    }
        //}

        /// <summary>
        /// Processes the inputs. This MUST ONLY BE USED when the player has authority over this Networked GameObject (photonView.isMine == true)
        /// </summary>
        /// 
        //GameObject rightJoystick;
        GameObject playerObject;
        void ProcessInputs()
        {

#if (UNITY_ANDROID || UNITY_IOS)
            //press button to fire at the enemy

            if (attackButton)
                attackButton.GetComponent<Button>().onClick.AddListener(attackButtonPressed);

            //enable the jostick to be able to drag around freely
            Cursor.lockState = CursorLockMode.None;
            GameObject.Find("MobileManager").GetComponent<MobileManager>().mobileCanvas.SetActive(true);

            rightJoystick = GameObject.FindWithTag("RotateCam");

            float touchX = rightJoystick.GetComponent<FixedJoystick>().Horizontal * 5.0f;
            transform.Rotate(0, touchX, 0);

#else
            if (Input.GetButtonDown("Fire1") && elapsedTime > fireCoolDown)
            {
                //Camera cam = Camera.main;

                //RaycastHit hitInfo;
                //Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);


                //if (Physics.Raycast(arrowSpawn.transform.position, arrowSpawn.transform.forward, out hitInfo))
                //{
                //    arrowSpawn.transform.LookAt(hitInfo.point);
                //}

                //if (Physics.Raycast(rayOrigin, out hitInfo))
                //{
                //    arrowSpawn.transform.LookAt(hitInfo.point);
                //}


                //if (Physics.Raycast(arrowSpawn.transform.position, arrowSpawn.transform.up, out hitInfo, Mathf.Infinity))
                //{
                //    lookDirection = Quaternion.LookRotation(rayOrigin.direction);
                //}

                this.GetComponent<PhotonView>().RPC("Shoot", RpcTarget.All, GameObject.Find("Pivot").transform.rotation);
                elapsedTime = 0;
            }

            /*
            if (Input.GetButtonDown("Fire2"))
            {
                // we don't want to fire when we interact with UI buttons for example. IsPointerOverGameObject really means IsPointerOver*UI*GameObject
                // notice we don't use on on GetbuttonUp() few lines down, because one can mouse down, move over a UI element and release, which would lead to not lower the isFiring Flag.
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    //	return;
                }

                if (!this.IsFiring)
                {
                    this.IsFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire2"))
            {
                if (this.IsFiring)
                {
                    this.IsFiring = false;
                }
            }
            */
            
            var mouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseMove = Vector2.Scale(mouseMove, new Vector2(SENS_HOR, SENS_VER));

            transform.Rotate(0, mouseMove.x, 0);
            
            //transform.Rotate(-mouseMove.y, 0, 0);
            //Camera.main.transform.Rotate(-mouseMove.y, 0, 0);

#endif
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;
        }

#endregion

#region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.IsFiring);
                stream.SendNext(this.Health);
            }
            else
            {
                // Network player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }

#endregion

        void attackButtonPressed()
        {
            if (photonView.IsMine && !isDead)
            {
                if (elapsedTime > fireCoolDown)
                {
                    this.GetComponent<PhotonView>().RPC("Shoot", RpcTarget.All);
                    elapsedTime = 0;
                }
            }
        }

        //when player clicks on potion button it will restore the health
        void potionRestoreHp()
        {
            //check if player health is lesser than max health and if player have enough potion
            if (this.Health < maxHealth && playerPotionAmt > 0)
            {
                this.Health += restoreHp;

                //set the restore health to the max health so it wont overshot 100
                if(this.Health > maxHealth)
                {
                    this.Health = maxHealth;
                }

                //if player have potion then player can use it
                if(playerPotionAmt >= 0)
                {
                    playerPotionAmt -= 1;

                    //if player left 0 potion set it to 0
                    if(playerPotionAmt < 0)
                    {
                        playerPotionAmt = 0;
                    }

                    //display the amount of potion
                    amountOfPotion.GetComponent<Text>().text = playerPotionAmt.ToString();
                }
            }
        }

        public void setHealth(int hp)
        {
            this.Health = hp;
        }

        public float getHealth()
        {
            return this.Health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        //[PunRPC]
        //public void TakeDamage(int damage)
        //{
        //    if (photonView.IsMine)
        //    {
        //        Health -= damage;
        //    }
        //}
    }
}