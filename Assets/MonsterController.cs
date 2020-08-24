
using System;
using UnityEngine;

using Com.MyCompany.MyGame;
using System.Collections;

public class MonsterController : MonoBehaviour, IEnemy
{
    enum MonsterState
    {
        Idle,
        Seek,
        Attack,
        Wander,
        Dead
    }

    public int MaxHealth { get; set; }
    public int Exp { get; set; }
    public int MovementSpeed { get; set; }
    public int AttackSpeed { get; set; }
    public EnemyAnimation animationState { get; set; }

    public int CurrentHealth;// { get; set; }
    private GameObject _player;
    private Transform _playerTransform;
    private Animator _enemyAnimator;
    //private float attackCoolDownTimer = 0;
    private bool canAttack = true;
    private float attackCoolDown = 2.0f;

    private MonsterState currentState = MonsterState.Idle;

    [SerializeField]
    public GameObject enemyUIPrefab;

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
        if (currentState != MonsterState.Dead)
        {
            DoThink();
            DoState();
        }
        
        //if (attackCoolDownTimer < attackCoolDown) { attackCoolDownTimer += Time.deltaTime; }
    }

    public void DoThink()
    {
        var direction = _playerTransform.position - transform.position;
        var angleOfView = Vector3.Angle(direction, transform.forward);

        //acquire target range
        if (direction.magnitude < 10 && angleOfView < 40)
        {
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            //within attack target range
            if (direction.magnitude < 4)
            {
                //if (attackCoolDownTimer > attackCoolDown) { Attack(); }

                currentState = MonsterState.Seek;
                if (canAttack) { DoAttack(); }
                //transform.Translate(0,0,0.04f);
            }
        }
        else 
        {
            currentState = MonsterState.Idle;
        }
        //else
        //{
        //    _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
        //    CurrentBotState = EnemyAnimation.Idle;
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
    }

    public void Seek()
    {
        if (currentState != MonsterState.Dead)
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Run.ToString());
            transform.Translate(0, 0, 0.2F);
            animationState = EnemyAnimation.Run;
        }
    }

    public void Attack()
    {

    }

    public void Wander() { }

    public void Dead() 
    {
        canAttack = false;
        _player = null;
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
