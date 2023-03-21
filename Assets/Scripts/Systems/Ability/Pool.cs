using UnityEngine;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Pool", menuName = "Abilities/Pool")]
    public class Pool : ScriptableObject
    {
        [SerializeField] string _poolName;
        public WeaponAbility[] Ability;
    }
}