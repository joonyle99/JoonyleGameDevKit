using UnityEngine;

namespace JoonyleGameDevKit
{
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