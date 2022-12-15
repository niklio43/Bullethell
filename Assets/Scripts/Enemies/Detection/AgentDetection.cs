using UnityEngine;

namespace BulletHell.Enemies.Detection
{
    [RequireComponent(typeof(AgentFOV))]
    public class AgentDetection : MonoBehaviour
    {
        public bool ShowGizmo = true;

        public float ObstacleDetectionRadius = 1;

        [HideInInspector] public DetectionData Data = new DetectionData();

        AgentFOV _agentFOV;

        private void Awake()
        {
            _agentFOV = GetComponent<AgentFOV>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(Detect), 0, .05f);
        }

        public void Detect()
        {
            Data.Obstacles = DetectObstacles();
            Data.Players = DetectPlayers();
            Data.Friendlies = DetectFriendlies();
        }

        public Collider2D[] DetectPlayers()
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Player");
            LayerMask obstacleMask = 1 << LayerMask.NameToLayer("Obstacle");

            return _agentFOV.Detect(mask, obstacleMask);
        }

        public Collider2D[] DetectFriendlies()
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Enemy");
            LayerMask obstacleMask = 1 << LayerMask.NameToLayer("Obstacle");

            return _agentFOV.Detect(mask, obstacleMask);
        }

        public Collider2D[] DetectObstacles()
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Obstacle");
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, ObstacleDetectionRadius, mask);

            return obstacles;
        }
    }
}