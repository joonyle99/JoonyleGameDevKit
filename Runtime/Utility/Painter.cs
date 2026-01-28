using UnityEngine;

namespace JoonyleGameDevKit
{
    public static class Painter
    {
        private const float DEFAULT_LINE_LENGTH = 1f;

        private static readonly Vector2 EAST_VEC = new(DEFAULT_LINE_LENGTH, 0f);
        private static readonly Vector2 WEST_VEC = new(-DEFAULT_LINE_LENGTH, 0f);
        private static readonly Vector2 SOUTH_VEC = new(0f, -DEFAULT_LINE_LENGTH);
        private static readonly Vector2 NORTH_VEC = new(0f, DEFAULT_LINE_LENGTH);

        private static readonly Vector2 SE_VEC = new(DEFAULT_LINE_LENGTH, -DEFAULT_LINE_LENGTH);
        private static readonly Vector2 SW_VEC = new(-DEFAULT_LINE_LENGTH, -DEFAULT_LINE_LENGTH);
        private static readonly Vector2 NW_VEC = new(-DEFAULT_LINE_LENGTH, DEFAULT_LINE_LENGTH);
        private static readonly Vector2 NE_VEC = new(DEFAULT_LINE_LENGTH, DEFAULT_LINE_LENGTH);

        public static void DebugDrawX(Vector2 center, Color? color = null, float lengthMultiplier = 1f, float duration = 2f)
        {
            Color drawColor = color ?? Color.red;
            Debug.DrawLine(center + NW_VEC * lengthMultiplier, center + SE_VEC * lengthMultiplier, drawColor, duration);
            Debug.DrawLine(center + SW_VEC * lengthMultiplier, center + NE_VEC * lengthMultiplier, drawColor, duration);
        }
        public static void DebugDrawPlus(Vector2 center, Color? color = null, float lengthMultiplier = 1f, float duration = 2f)
        {
            Color drawColor = color ?? Color.red;
            Debug.DrawLine(center + WEST_VEC * lengthMultiplier, center + EAST_VEC * lengthMultiplier, drawColor, duration);
            Debug.DrawLine(center + SOUTH_VEC * lengthMultiplier, center + NORTH_VEC * lengthMultiplier, drawColor, duration);
        }

        public static void GizmosDrawVerticalAxis(Vector3 center, Color? color = null, float lengthMultiplier = 1f)
        {
            Gizmos.color = color ?? Color.white;
            Gizmos.DrawLine(center.CombineWith(SOUTH_VEC * lengthMultiplier), center.CombineWith(NORTH_VEC * lengthMultiplier));
        }
        public static void GizmosDrawHorizontalAxis(Vector3 center, Color? color = null, float lengthMultiplier = 1f)
        {
            Gizmos.color = color ?? Color.white;
            Gizmos.DrawLine(center.CombineWith(WEST_VEC * lengthMultiplier), center.CombineWith(EAST_VEC * lengthMultiplier));
        }
        public static void GizmosDrawArrow(Vector3 start, Vector2 dir, float arrowAngle = 20f, Color? bodyColor = null, Color? headColor = null, float bodyLength = 3f, float headLength = 1f)
        {
            Gizmos.color = bodyColor ?? Color.white;

            Vector3 centerPoint = start.CombineWith(dir * bodyLength);
            Gizmos.DrawLine(start, centerPoint);

            Gizmos.color = headColor ?? Color.white;

            var oppositeDir = (-1) * dir;

            Vector3 leftPoint = centerPoint + Quaternion.Euler(0f, 0f, -arrowAngle) * oppositeDir * headLength;
            Vector3 rightPoint = centerPoint + Quaternion.Euler(0f, 0f, arrowAngle) * oppositeDir * headLength;

            Gizmos.DrawLine(centerPoint, leftPoint);
            Gizmos.DrawLine(centerPoint, rightPoint);
        }
    }
}