namespace Project.DesignPatternLabo
{
    public class CMoveRightCommand : ACommand
    {
        private CMoveObject _player;
        public CMoveRightCommand(CMoveObject player) => _player = player;

        public override void Execute()
        {
            _player.MoveRight();
        }
        public override void Undo()
        {
            _player.MoveLeft();
        }
    }
}
