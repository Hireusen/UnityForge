namespace Project.DesignPatternLabo
{
    public class SelfHarmStrategy : IWeapon
    {
        public void Attack(IEntity attacker, IEntity victim)
        {
            attacker.Hit(attacker.Physical);
        }
    }
}
