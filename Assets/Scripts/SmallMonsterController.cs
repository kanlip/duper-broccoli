
    using System;
    using UnityEngine;

    public class SmallMonsterController:MonoBehaviour, IEnemy
    {
        public int Health { get; set; }
        public int Exp { get; set; }
        public int CurrentHealth { get; set; }
    
        
        private void Awake()
        {
            Health = 100;
            Exp = 2;
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
