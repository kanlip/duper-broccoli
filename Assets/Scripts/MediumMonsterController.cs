using System;
using UnityEngine;

using Com.MyCompany.MyGame;
using System.Collections;

public class MediumMonsterController:MonoBehaviour,IEnemy
{
    [SerializeField]
    public int MaxHealth { get; set; }
    public int Exp { get; set; }
    public int MovementSpeed { get; set; }
    public EnemyAnimation animationState { get; set; }
    public int AttackSpeed { get; set; }
    [SerializeField]
    private int CurrentHealth;// { get; set; }
    private GameObject _player;
    private Transform _playerTransform;
    private Animator _enemyAnimator;
    private bool canAttack = true;
    private float attackCoolDown = 2.0f;
    
    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
            _playerTransform = _player.GetComponent<Transform>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
            Attack();
        if (_playerTransform == null)
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
            return;
        }

        var direction = _playerTransform.position - transform.position;
        var angleOfView = Vector3.Angle(direction, transform.forward);
        
        if (direction.magnitude < 10 && angleOfView < 40)
        {
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            if (direction.magnitude < 4)
            {
                if (canAttack) { Attack(); }
                    
                //transform.Translate(0,0,0.04f);
            }
            else
            {
                _enemyAnimator.SetTrigger(EnemyAnimation.Run.ToString());
                transform.Translate (0, 0, 0.2F);
                animationState = EnemyAnimation.Run;
            }
        }
        else
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
            animationState = EnemyAnimation.Idle;
        }
        //if (attackCoolDownTimer < attackCoolDown) { attackCoolDownTimer += Time.deltaTime; }
            
    }

    private void Awake()
    {
        MaxHealth = 150;
        Exp = 5;
        CurrentHealth = MaxHealth;
    }
    
    public void Attack()
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
    }

    public void DamageTaken(int damageTaken)
    {
        if(CurrentHealth > 0) 
            CurrentHealth -= damageTaken;
        else 
            _enemyAnimator.SetTrigger(EnemyAnimation.Dead.ToString());
    }
    
}

