namespace Lrw.Script.Agent.HealthSystem
{
    public interface IHealthModule
    {
        float CurrentHealth { get; set; }
        event HealthChangedHandler OnHealthChanged;
    }
}