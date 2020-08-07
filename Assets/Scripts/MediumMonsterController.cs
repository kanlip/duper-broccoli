using System;
using UnityEngine;

public enum EnemyAnimation
{
    Attack,
    Run,
    Dead,
    Idle
    
}
public class MediumMonsterController:MonoBehaviour,IEnemy
{   
    public int Health { get; set; }
    public int Exp { get; set; }
    public int MovementSpeed { get; set; }
    public int AttackSpeed { get; set; }
    private int CurrentHealth { get; set; }
    private GameObject player;
    private Transform playerTransform;
    private Animator enemyAnimator;
    
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerTransform.GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            Attack();
    }          

    private void Awake()
    {
        Health = 150;
        Exp = 5;
        CurrentHealth = Health;
    }
    
    public void Attack()
    {
        enemyAnimator.SetTrigger(EnemyAnimation.Attack.ToString());
    }
    
    public void DamageTaken(int damageTaken)
    {
        if(CurrentHealth > 0) CurrentHealth -= damageTaken;
        else enemyAnimator.SetTrigger(EnemyAnimation.Dead.ToString());
    }
}

