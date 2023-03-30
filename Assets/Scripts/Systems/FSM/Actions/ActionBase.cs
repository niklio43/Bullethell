namespace BulletHell.FiniteStateMachine
{
    public abstract class ActionBase : IAction
    {
        #region Private Fields
        private bool _init;
        #endregion

        #region Public Fields
        public virtual string Name { get; set; } = "Untitled";
        public IState ParentState { get; set; }
        #endregion

        #region Private Methods
        private void Init()
        {
            if (_init) return;

            OnInit();
            _init = true;
        }

        protected virtual void OnInit() { }
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnUpdate() { }
        #endregion

        #region Public Methods
        public void Enter()
        {
            Init();
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        public void Update()
        {
            OnUpdate();
        }

        public void Transition(string id)
        {
            ParentState.Transition(id);
        }
        #endregion
    }
}
