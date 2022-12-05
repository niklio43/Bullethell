using UnityEngine;

namespace BulletHell.Emitters
{
    [CreateAssetMenu(fileName = "EmitterData", menuName = "EmitterData")]
    public class EmitterData : ScriptableObject
    {
        [HideInInspector] public Vector2 direction = Vector2.up;

    [Header("Projectile")]
    public Projectile projectilePrefab;
    public float timeToLive = 5;
    [Range(0.01f, 100f)]
    public float speed = 1;

        [Header("Projectile")]
        public Projectile projectilePrefab;
        public float timeToLive = 5;
        [Range(0.01f, 10f)]
        public float speed = 1;

        [Header("Emission Data")]
        [Range(1, 40)]
        public int emitterPoints = 1;
        [Range(0, 10)]
        public float radius = 0;
        [Range(-180, 180)]
        public float spread = 0;
        [Range(-180, 180)]
        public float pitch = 0;
        [Range(-180, 180)]
        public float centerRotation = 0;
    }
}
