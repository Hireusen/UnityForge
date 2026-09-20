namespace Project.DesignPatternLabo
{
    public class UltimateStrategy : IWeapon
    {
        public void Attack(IEntity attacker, IEntity victim)
        {
            victim.Hit(attacker.Physical * attacker.Magical);
        }
    }
}
