public interface IDamageable
{
    float Health { get; }
    void ApplyDamage(float damage);
}