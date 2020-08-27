/* Start Header **************************************************************/
/*!
\file       NetworkPlayerAnimatorManager.cs
\author     Eugene Lee Yuih Chin, Sukphasuth Lipipan (Kan), developer@exitgames.com
\StudentNo  6572595
\par        xelycx@gmail.com
\date       15.8.2020
\brief

Reproduction or disclosure of this file or its contents
without the prior written consent of author is prohibited.
*/
/* End Header ****************************************************************/


using UnityEngine;

using Com.MyCompany.MyGame;
using System.Collections;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterController : MonoBehaviourPun
{
    public enum MonsterState
    {
        Idle,
        Seek,
        Attack,
        Wander
    }

    public int CurrentHealth;
    public int MaxHealth;
    public int Exp;
    public int MovementSpeed;
    public int AttackSpeed;
    public EnemyAnimation animationState { get; set; }

    private GameObject _player;
    private Transform _playerTransform;
    private Animator _enemyAnimator;
    private float attackCoolDownTimer = 0;
    private bool canAttack = true;
    private float attackCoolDown = 2.0f;
    public bool isDead = false;

    public MonsterState currentState = MonsterState.Idle;

    [SerializeField]
    public GameObject enemyUIPrefab;


    private NavMeshAgent agent;
    GameObject playerGO;
    public GameObject targetGO;

    float timeElapsed = 0;
    float wanderTimer = 0;
    float destroyTimer = 0;

    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
            _playerTransform = _player.GetComponent<Transform>();

        // Create the UI
        if (this.enemyUIPrefab != null)
        {
            GameObject _uiGo = Instantiate(this.enemyUIPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        agent = GetComponent<NavMeshAgent>();
    }
    private void Awake()
    {
        MaxHealth = 100;
        Exp = 2;
        CurrentHealth = MaxHealth;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha0)) Attack();

        //if (_playerTransform == null)
        //{
        //    _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
        //    return;
        //}


        timeElapsed += Time.deltaTime;
        wanderTimer += Time.deltaTime;
        attackCoolDownTimer += Time.deltaTime;
        if (isDead)
            destroyTimer += Time.deltaTime;

        if (PhotonNetwork.IsMasterClient && destroyTimer > 5)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (agent.velocity.magnitude > 0.1f) //&& !MonsterState.Attack)
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Run.ToString());
            animationState = EnemyAnimation.Run;
        }
        else
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
            animationState = EnemyAnimation.Idle;
        }

        if (timeElapsed > 1)

        {
            if (!isDead)
            {
                DoThink();
                DoState();
            }
            timeElapsed = 0;
        }

        if (attackCoolDownTimer > attackCoolDown) { canAttack = true; }

        if (CurrentHealth <= 0 && !isDead)
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Dead.ToString());
            isDead = true;
            GetComponent<NavMeshAgent>().enabled = false;
            canAttack = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
        Seek();
    }

    public void DoThink()
    {
        //var direction = _playerTransform.position - transform.position;
        //var angleOfView = Vector3.Angle(direction, transform.forward);

        //acquire target range
        AcquireTargetPlayer();
        if (!targetGO)
        {
            
            if (currentState != MonsterState.Wander)
            {
                currentState = MonsterState.Idle;
            }
        }

        //if (direction.magnitude < 20)//&& angleOfView < 40)
        //{
        //    if (agent)
        //        agent.isStopped = false;
        //    direction.y = 0;
        //    transform.rotation = Quaternion.LookRotation(direction);
        //    currentState = MonsterState.Seek;

        //    //within attack target range
        //    if (direction.magnitude < 2.5)
        //    {
        //        //if (attackCoolDownTimer > attackCoolDown) { Attack(); }
        //        if (canAttack) { DoAttack(); }
 
        //        if (agent)
        //            agent.isStopped = true;
        //    }
        //}
        //else
        //{
        //    if (currentState != MonsterState.Wander)
        //    {
        //        currentState = MonsterState.Idle;
        //    }
        //}
    }

    public void DoState()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Seek:
                Seek();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Wander:
                Wander();
                break;
            default:
                Idle();
                break;
        }
    }

    public void Idle()
    {
        _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
        animationState = EnemyAnimation.Idle;

        if (Random.Range(0, 100) > 90)
        {
            currentState = MonsterState.Wander;
        }
    }

    public void AcquireTargetPlayer()
    {
        GameObject[] playerGOs = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i<playerGOs.Length; i++)
        {
            if(playerGOs[i] && agent)
            {
                Vector3 direction = playerGOs[i].transform.position - transform.position;
                if (direction.magnitude < 20)
                {
                    agent.isStopped = false;
                    targetGO = playerGOs[i];
                    direction.y = 0;
                    transform.rotation = Quaternion.LookRotation(direction);
                    currentState = MonsterState.Seek;

                    //within attack target range
                    if (direction.magnitude < 2.5)
                    {
                        currentState = MonsterState.Attack;
                    }
                }
                else
                {
                    targetGO = null;
                }
            }
        }
    }

    public void Seek()
    {
        if (targetGO && agent)
        {
            //Debug.Log("move");
            agent.SetDestination(targetGO.transform.position);
        }
    }

    public void Attack()
    {
        if (!targetGO) return;
        if (canAttack) { DoAttack(); }
        if (agent)
            agent.isStopped = true;

    }

    public void Wander()
    {
        if (wanderTimer > 5)
        {
            Vector3 randomNearbyPosition = new Vector3(transform.position.x + Random.Range(-20, 20),
                                                        100,
                                                        transform.position.z + Random.Range(-20, 20));
            RaycastHit hitInfo;
            //check Position To Spawn, raycast hit the ground below then allow spawn
            if (Physics.Raycast(randomNearbyPosition, Vector3.down, out hitInfo, Mathf.Infinity))
            {
                randomNearbyPosition.y = hitInfo.point.y;
                if(agent)
                    agent.SetDestination(randomNearbyPosition);
            }
            wanderTimer = 0;
        }
    }

    public void DoAttack()
    {
        _enemyAnimator.SetTrigger(EnemyAnimation.Attack.ToString());

        animationState = EnemyAnimation.Attack;

        if (targetGO)
        {
            if(targetGO.GetComponent<NetworkPlayerManager>())
                targetGO.GetComponent<NetworkPlayerManager>().TakeDamage(10);
        }
        canAttack = false;
        attackCoolDownTimer = 0.0f;


        //StartCoroutine(RestoreAttackYield());
    }

    //IEnumerator RestoreAttackYield()
    //{
    //    yield return new WaitForSeconds(attackCoolDown);
    //    canAttack = true;
    //    currentState = MonsterState.Idle;
    //    StopCoroutine(RestoreAttackYield());
    //}

    public void DamageTaken(int damageTaken)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damageTaken;
        }
        else
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Dead.ToString());
        }
    }

    //IEnumerator Destroy()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        yield return new WaitForSeconds(5);
    //        //StopCoroutine(Destroy());
    //        PhotonNetwork.Destroy(gameObject);
    //    }
    //}
}
