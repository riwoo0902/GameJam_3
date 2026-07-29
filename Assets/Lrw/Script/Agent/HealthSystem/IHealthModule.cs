namespace Lrw.Script.Agent.HealthSystem
{
    public interface IHealthModule
    {
        float CurrentHealth { get; set; }
        void TakeDamage(float damage);
        void Heal(float heal);
        event HealthChangedHandler OnHealthChanged;
    }
}