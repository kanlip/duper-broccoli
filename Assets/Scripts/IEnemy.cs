

public interface IEnemy
{
    public int Health { get; set; }
    public int Exp { get; set; }
    public void DamageTaken(int damageTaken);
    public void Attack();
}
