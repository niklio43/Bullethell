namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorInt : ActionSetAnimatorVariableBase
    {
        #region Private Fields
        private readonly string _paramName;
        private readonly int _value;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Set Animator Int";

        public ActionSetAnimatorInt(string paramName, int value)
        {
            _paramName = paramName;
            _value = value;
        }
        #endregion

        #region Private Methods
        protected override void OnEnter()
        {
            _animator.SetInteger(_paramName, _value);
        }
        #endregion
    }
}
