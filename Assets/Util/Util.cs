using UnityEngine;

namespace JoonyleGameDevKit
{
    public static class CustomDebug
    {
        private const float DEFAULT_LINE_LENGTH = 1f;

        private static readonly Vector2 vec1 = new(-DEFAULT_LINE_LENGTH, DEFAULT_LINE_LENGTH);
        private static readonly Vector2 vec2 = new(DEFAULT_LINE_LENGTH, -DEFAULT_LINE_LENGTH);
        private static readonly Vector2 vec3 = new(-DEFAULT_LINE_LENGTH, -DEFAULT_LINE_LENGTH);
        private static readonly Vector2 vec4 = new(DEFAULT_LINE_LENGTH, DEFAULT_LINE_LENGTH);

        /// <summary>
        /// Draws an X shape at the specified position.
        /// </summary>
        /// <param name="center">The center position of the X.</param>
        /// <param name="color">The color of the X. Defaults to red if not specified.</param>
        /// <param name="size">The size of the X. Defaults to 1 if not specified.</param>
        /// <param name="duration">How long the X should be visible. Defaults to 2 seconds.</param>
        public static void DrawX(Vector2 center, Color? color = null, float size = 1f, float duration = 2f)
        {
            Color _color = color ?? Color.red;
            Debug.DrawLine(center + vec1 * size, center + vec2 * size, _color, duration);
            Debug.DrawLine(center + vec3 * size, center + vec4 * size, _color, duration);
        }
    }

    public static class CustomGizmos
    {
        private const float DEFAULT_LINE_LENGTH = 1f;

        /// <summary>
        /// Draws a vertical axis at the specified origin.
        /// </summary>
        /// <param name="origin">The origin point of the vertical axis.</param>
        /// <param name="color">The color of the axis. Defaults to white if not specified.</param>
        /// <param name="size">The size multiplier of the axis. Defaults to 1 if not specified.</param>
        public static void DrawVerticalAxis(Vector3 origin, Color? color = null, float size = 1f)
        {
            Vector3 top = new Vector3(origin.x, origin.y + DEFAULT_LINE_LENGTH * size, origin.z);
            Vector3 bottom = new Vector3(origin.x, origin.y - DEFAULT_LINE_LENGTH * size, origin.z);
            Gizmos.color = color ?? Color.white;
            Gizmos.DrawLine(top, bottom);
        }
    }
}