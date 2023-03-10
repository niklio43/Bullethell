using BulletHell.Map.Generation;
using UnityEngine;


namespace BulletHell.Map
{
    public class Room : MonoBehaviour
    {
        public RoomCell[] Cells;
        public Color colorCoding;

        const int _cellSize = 20;

        public bool IsOverLapping()
        {
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < Cells.Length; i++) {
                Vector2 pos = (Vector2)transform.position + (Cells[i].Pos * _cellSize);

                Gizmos.DrawWireCube(pos, Vector2.one * _cellSize);
            }
        }
    }

    [System.Serializable]
    public class RoomCell
    {
        [SerializeField] Vector2Int _pos = Vector2Int.zero;
        public Door[] Doors;

        public Vector2Int Pos => _pos;
        public int x => _pos.x;
        public int y => _pos.y;
    }
}
