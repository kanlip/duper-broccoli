
    using System;
    using UnityEngine;

    public class SmallMonsterController:MonoBehaviour, IEnemy
    {
        public int Health { get; set; }
        public int Exp { get; set; }
        public int MovementSpeed { get; set; }
        public int AttackSpeed { get; set; }
        public EnemyAnimation CurrentBotState { get; set; }
        private int CurrentHealth { get; set; }
        private GameObject _player;
        private Transform _playerTransform;
        private Animator _enemyAnimator;
        private void Start()
        {
            _enemyAnimator = GetComponent<Animator>();
            _player = GameObject.FindGameObjectWithTag("Player");
            if(_player != null)
                _playerTransform = _playerTransform.GetComponent<Transform>();
        
        }
        private void Awake()
        {
            Health = 100;
            Exp = 2;
            CurrentHealth = Health;
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
                if (direction.magnitude < 1)
                {
                    Attack();
                    transform.Translate(0,0,0.04f);
                }
                else
                {
                    _enemyAnimator.SetTrigger(EnemyAnimation.Run.ToString());
                    transform.Translate (0, 0, 0.2F);
                    CurrentBotState = EnemyAnimation.Run;
                }
            }
            else
            {
                _enemyAnimator.SetTrigger(EnemyAnimation.Idle.ToString());
                CurrentBotState = EnemyAnimation.Idle;
            }
        }

        public void Attack()
        {
            _enemyAnimator.SetTrigger(EnemyAnimation.Attack.ToString());
        
            CurrentBotState = EnemyAnimation.Attack;
        }
    
        public void DamageTaken(int damageTaken)
        {
            if(CurrentHealth > 0) 
                CurrentHealth -= damageTaken;
            else 
                _enemyAnimator.SetTrigger(EnemyAnimation.Dead.ToString());
        }
    }
