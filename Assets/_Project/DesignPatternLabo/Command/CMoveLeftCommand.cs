namespace Project.DesignPatternLabo
{
    public class CMoveLeftCommand : ACommand
    {
        private CMoveObject _player;
        public CMoveLeftCommand(CMoveObject player) => _player = player;

        public override void Execute()
        {
            _player.MoveLeft();
        }
        public override void Undo()
        {
            _player.MoveRight();
        }
    }
}
