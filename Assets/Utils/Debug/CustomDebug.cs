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
}