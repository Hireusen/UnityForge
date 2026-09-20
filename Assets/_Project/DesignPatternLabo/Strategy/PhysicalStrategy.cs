namespace Project.DesignPatternLabo
{
    public class PhysicalStrategy : IWeapon
    {
        public void Attack(IEntity attacker, IEntity victim)
        {
            victim.Hit(attacker.Physical);
        }
    }
}
