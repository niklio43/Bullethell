using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "SeekBehaviour", menuName = "Enemies/Steering/Seek Behaviour", order = 1)]
    public class SeekBehaviour : SteeringBehaviour
    {
        public float Weight = 0;

        public override void GetSteering(AgentSteering steering, EnemyMovement movement)
        {
            Transform transform = movement.transform;
            Enemy enemy = movement.Enemy;

            if (enemy.Target == null) { return; }

            float distance = Vector2.Distance(enemy.Target.position, transform.position);

            if (distance < enemy.PreferredDistance && enemy.TargetInLineOfSight(transform.position)) { return; }

            EnemyPathFinder path = movement.PathFinder;
            path.UpdatePathTraversal();
            Vector2 towardsVector = path.GetCurrentPathNode() - transform.position;

            float distanceWeight = Mathf.Clamp01(distance / 3);

            for (int i = 0; i < steering.Directions.Length; i++) {
                float result = Vector2.Dot(towardsVector.normalized, steering.Directions[i]);
                result = result * Weight * distanceWeight;

                result = Mathf.Clamp01(result);

                if (result > steering.Interest[i])
                    steering.Interest[i] = result;
            }
        }
    }
}