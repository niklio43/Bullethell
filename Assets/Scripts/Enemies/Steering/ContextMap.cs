namespace BulletHell.Enemies.Steering
{
    public class ContextMap
    {
        float[] _values;
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

        public void Clear()
        {
            for (int i = 0; i < _values.Length; i++) {
                _values[i] = 0;
            }
        }
    }
}
