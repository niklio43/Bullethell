using UnityEngine;
using BulletHell.Stats;

namespace BulletHell
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public Pool<Projectile> Pool;
        public ProjectileData Data;

        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Animator _animator;

        [HideInInspector] public Vector2 Position;
        [HideInInspector] public Vector2 Velocity;
        [HideInInspector] public Vector2 Direction;
        [HideInInspector] public Vector2 GravityPoint;
        [HideInInspector] public float Gravity;

        [HideInInspector] public float TimeToLive;
        [HideInInspector] public float Acceleration;
        [HideInInspector] public float Speed;

        [HideInInspector] public DamageInfo Damage;

        [HideInInspector] public BoxCollider2D ProjectileCollider;


        private void Awake()
        {
            ProjectileCollider = GetComponent<BoxCollider2D>();
        }

        [ContextMenu("Test")]
        public void Test()
        {
            Initialize(Data);
        }

        public void Initialize(ProjectileData data)
        {
            Data = data;
            gameObject.SetActive(true);
            if (data.Sprite != null)
                _spriteRenderer.sprite = data.Sprite;
            if(data.Animator != null) {
                _animator.runtimeAnimatorController = data.Animator;
            }

            ProjectileCollider.offset = Data.collider.center;
            ProjectileCollider.size = Data.collider.size / 2;

            transform.localScale = Vector3.one * data.Scale;
            _spriteRenderer.color = data.Color;
        }

        private void Update()
        {
            transform.position = Position;
            var angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
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

        private void OnTriggerEnter(Collider other)
        {
            if(!Data.CollisionTags.Contains(other.gameObject.tag)) { return; }
            if(other.TryGetComponent(out Character character)) {
                if (Damage != null)
                    character.TakeDamage(Damage);
            }
        }

    }
}
