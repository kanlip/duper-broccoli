using System;

public interface IEnemy
{
    int MaxHealth { get; set; }
    int Exp { get; set; }
    int MovementSpeed { get; set; }
    EnemyAnimation animationState { get; set; }
    int AttackSpeed { get; set; }
    void DamageTaken(int damageTaken);
    void Attack();
}
