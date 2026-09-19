namespace Project.DesignPatternLabo
{
    public class CMoveUpCommand : ACommand
    {
        private CMoveObject _player;
        public CMoveUpCommand(CMoveObject player) => _player = player;

        public override void Execute()
        {
            _player.MoveForward();
        }
        public override void Undo()
        {
            _player.MoveBack();
        }
    }
}
