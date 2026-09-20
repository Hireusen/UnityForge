namespace Project.DesignPatternLabo
{
    public interface IEntity
    {
        int Health { get; }
        int Physical { get; }
        int Magical { get; }

        void Hit(int damage);
    }
}
