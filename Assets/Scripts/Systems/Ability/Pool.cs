using UnityEngine;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Pool", menuName = "Abilities/Pool")]
    public class Pool : ScriptableObject
    {
        #region Private Fields
        [SerializeField] string _poolName;
        #endregion

        #region Public Fields
        public WeaponAbility[] Ability;
        #endregion
    }
}