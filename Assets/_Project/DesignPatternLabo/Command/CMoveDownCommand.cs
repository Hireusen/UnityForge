namespace Project.DesignPatternLabo
{
    public class CMoveDownCommand : ACommand
    {
        private CMoveObject _player;
        public CMoveDownCommand(CMoveObject player) => _player = player;

        public override void Execute()
        {
            _player.MoveBack();
        }
        public override void Undo()
        {
            _player.MoveForward();
        }
    }
}
