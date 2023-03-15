using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Map.Generation;

namespace BulletHell.Map
{
    public class RoomSpawner : MonoBehaviour
    {




        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, (GenerationUtilities.CellSize - 1) / 2);
        }
    }
}
