namespace BulletHell.Enemies.Steering
{
    public class ContextMap
    {
        #region Private Fields
        float[] _values;
        #endregion

        #region Public Fields
        public int Count => _values.Length;
        public float this[int index]
        {
            get {
                return _values[index];
            }

            set {
                _values[index] = value;
            }
        }
        public ContextMap(int resolution)
        {
            _values = new float[resolution];
        }
        #endregion

        #region Public Methods
        public void Clear()
        {
            for (int i = 0; i < _values.Length; i++) {
                _values[i] = 0;
            }
        }
        #endregion
    }
}
