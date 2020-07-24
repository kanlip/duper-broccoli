

public interface IEnemy
{
    int Health { get; set; }
    int Exp { get; set; }
    int MovementSpeed { get; set; }
    int AttackSpeed { get; set; }
    void DamageTaken(int damageTaken);
    
    void Attack();
}
