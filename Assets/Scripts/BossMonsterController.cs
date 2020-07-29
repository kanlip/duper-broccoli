
    using System;
    using UnityEngine;

    public class BossMonsterController:MonoBehaviour,IEnemy
    {
        public int Health { get; set; }
        public int Exp { get; set; }
        public int MovementSpeed { get; set; }
        public int AttackSpeed { get; set; }
        private int CurrentHealth { get; set; }
        
        private void Awake()
        {
            Health = 300;
            Exp = 10;
            CurrentHealth = Health;
        }
        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void DamageTaken(int damageTaken)
        {
            if(CurrentHealth > 0) CurrentHealth -= damageTaken;
        }
    }
