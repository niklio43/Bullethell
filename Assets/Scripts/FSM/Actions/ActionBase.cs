namespace BulletHell.FiniteStateMachine
{
    public abstract class ActionBase : IAction
    {
        private bool _init;

        public virtual string Name { get; set; } = "Untitled";
        public IState ParentState { get; set; }

        private void Init()
        {
            if (_init) return;

            OnInit();
            _init = true;
        }

        protected virtual void OnInit() { }

        public void Enter()
        {
            Init();
            OnEnter();
        }

        protected virtual void OnEnter() { }

        public void Exit()
        {
            OnExit();
        }

        protected virtual void OnExit() { }

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        public void Transition(string id)
        {
            ParentState.Transition(id);
        }
    }
}
