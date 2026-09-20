using System.Diagnostics;

namespace Project.DesignPatternLabo
{
    public class PlayerStrategyHub
    {
        private IWeapon _weapon;

        public void SetStrategy(IWeapon weapon)
        {
            UnityEngine.Debug.Log($"플레이어의 전략을 {weapon.GetType().Name}로 바꿨습니다.");
            _weapon = weapon;
        }

        public void Attack(IEntity attacker, IEntity victim)
        {
            if(_weapon == null) return;
            _weapon.Attack(attacker, victim);
        }
    }
}
