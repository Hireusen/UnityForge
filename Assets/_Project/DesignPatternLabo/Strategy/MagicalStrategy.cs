namespace Project.DesignPatternLabo
{
    public class MagicalStrategy : IWeapon
    {
        public void Attack(IEntity attacker, IEntity victim)
        {
            victim.Hit(attacker.Magical);
        }
    }
}
