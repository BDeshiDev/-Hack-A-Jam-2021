namespace Core.Combat
{
    public class HealthComponent : ResourceComponent
    {
        private void Awake()
        {
            fullyRestore();
        }
    }
}