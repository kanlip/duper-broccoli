

public interface IEnemy
{
    int Health { get; set; }
    int Exp { get; set; }
    void DamageTaken(int damageTaken);
    void Attack();
}
