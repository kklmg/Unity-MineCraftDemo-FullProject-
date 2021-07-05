using UnityEngine;

namespace Assets.Scripts.NData
{
    [System.Serializable]
    public struct AreaRect
    {
        public int left;
        public int right;
        public int top;
        public int bottom;

        public AreaRect(Vector2Int cneter, int extend)
        {
            left = cneter.x - extend;
            right = cneter.x + extend;
            top = cneter.y + extend;
            bottom = cneter.y - extend;
        }
        public AreaRect(int _left, int _right, int _top, int _bottom)
        {
            left = _left;
            right = _right;
            top = _top;
            bottom = _bottom;
        }

        public void Move(int x, int y)
        {
            left += x;
            right += x;
            top += y;
            bottom += y;
        }
        public void Move(Vector2Int offset)
        {
            left += offset.x;
            right += offset.x;
            top += offset.y;
            bottom += offset.y;
        }
        public bool IsInvolvePoint(Vector2Int point)
        {
            return
                (point.x >= left && point.x <= right
                && point.y >= bottom && point.y <= top);
        }
    }
}
