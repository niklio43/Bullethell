using UnityEngine;

namespace BulletHell
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public Pool<Projectile> Pool;
        public ProjectileData Data;

        [SerializeField] SpriteRenderer spriteRenderer;

        [HideInInspector] public Vector2 Position;
        [SerializeField] public Vector2 Velocity;
        [HideInInspector] public Vector2 Gravity;

        [HideInInspector] public float TimeToLive;
        [HideInInspector] public float Acceleration;
        [HideInInspector] public float Speed;

        public void Initialize(ProjectileData data)
        {
            Data = data;

            if (data.Sprite != null)
                spriteRenderer.sprite = data.Sprite;

            transform.localScale = Vector3.one * data.Scale;

            spriteRenderer.color = data.Color;
            name = $"{data.name}";
        }

        private void Update()
        {
            transform.position = Position;

            if (TimeToLive <= 0) {
                ResetObject();
            }
        }

        public void ResetObject()
        {
            Data = null;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            Pool.Release(this);
        }
    }
}
