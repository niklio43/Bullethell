namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorBool : ActionSetAnimatorVariableBase
    {
        #region Private Fields
        private readonly string _paramName;
        private readonly bool _value;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Set Animator Bool";

        public ActionSetAnimatorBool(string paramName, bool value)
        {
            _paramName = paramName;
            _value = value;
        }
        #endregion

        #region Private Methods
        protected override void OnEnter()
        {
            _animator.SetBool(_paramName, _value);
        }
        #endregion
    }
}
