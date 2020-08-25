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

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        Idle,
        Seek,
        Attack,
        Wander,
        Dead
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
    //private float attackCoolDownTimer = 0;
    private bool canAttack = true;
    private float attackCoolDown = 2.0f;

    public MonsterState currentState = MonsterState.Idle;

    [SerializeField]
    public GameObject enemyUIPrefab;


    private NavMeshAgent agent;
    GameObject playerGO;

    float timeElapsed = 0;
    float wanderTimer = 0;

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
            if (currentState != MonsterState.Dead)
            {
                DoThink();
                DoState();
            }
            timeElapsed = 0;
        }


        //Seek();


        //if (attackCoolDownTimer < attackCoolDown) { attackCoolDownTimer += Time.deltaTime; }
    }

    public void DoThink()
    {
        var direction = _playerTransform.position - transform.position;
        var angleOfView = Vector3.Angle(direction, transform.forward);

        //acquire target range



        if (direction.magnitude < 20 )//&& angleOfView < 40)
        {
            if(agent)
                agent.isStopped = false;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            currentState = MonsterState.Seek;

            //within attack target range
            if (direction.magnitude < 2.5)
            {
                //if (attackCoolDownTimer > attackCoolDown) { Attack(); }
                if (canAttack) { DoAttack(); }
                //transform.Translate(0,0,0.04f);
                if (agent)
                    agent.isStopped = true;
            }
        }
        else 
        {
            if (currentState != MonsterState.Wander)
            {
                currentState = MonsterState.Idle;
            }
        }
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
            case MonsterState.Dead:
                Dead();
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

    public void Seek()
    {
        if (currentState != MonsterState.Dead)
        {

            playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO && agent)
            {
                Debug.Log("move");
                agent.SetDestination(playerGO.transform.position);
            }
        }
    }

    public void Attack()
    {

    }

    public void Wander()
    {
        if (wanderTimer > 5)
        {
            Vector3 randomNearbyPosition =  new Vector3(transform.position.x + Random.Range(-20, 20),
                                                        transform.position.y,
                                                        transform.position.z + Random.Range(-20, 20));
            agent.SetDestination(randomNearbyPosition);

            wanderTimer = 0;
        }
    }


    public void Dead() 
    {
        canAttack = false;
        _player = null;
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void DoAttack()
    {
        _enemyAnimator.SetTrigger(EnemyAnimation.Attack.ToString());

        animationState = EnemyAnimation.Attack;
        _player.GetComponent<NetworkPlayerManager>().TakeDamage(10);
        canAttack = false;
        //attackCoolDownTimer = 0.0f;
        StartCoroutine(RestoreAttackYield());
    }

    IEnumerator RestoreAttackYield()
    {
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
        currentState = MonsterState.Idle;
    }

    public void DamageTaken(int damageTaken)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damageTaken;
        }
        else
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Dead.ToString());
            currentState = MonsterState.Dead;
        }

    }
}
