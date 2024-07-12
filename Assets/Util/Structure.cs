using UnityEngine;

namespace JoonyleGameDevKit.Structure
{
    public struct Line2
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public Line2(Vector2 a, Vector2 b)
        {
            pointA = a;
            pointB = b;
        }

        /// <summary>
        /// Linearly interpolates between the two points of the line.
        /// </summary>
        /// <param name="line">The line to interpolate along.</param>
        /// <param name="t">The interpolation factor (0 to 1).</param>
        /// <returns>The interpolated point.</returns>
        public static Vector2 Lerp(Line2 line, float t)
        {
            t = Mathf.Clamp01(t);

            var pointA = line.pointA;
            var pointB = line.pointB;

            return new Vector2(pointA.x + (pointB.x - pointA.x) * t, pointA.y + (pointB.y - pointA.y) * t);
        }
    }
    public struct Line3
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public Line3(Vector3 a, Vector3 b)
        {
            pointA = a;
            pointB = b;
        }

        /// <summary>
        /// Linearly interpolates between the two points of the line.
        /// </summary>
        /// <param name="line">The line to interpolate along.</param>
        /// <param name="t">The interpolation factor (0 to 1).</param>
        /// <returns>The interpolated point.</returns>
        public static Vector3 Lerp(Line3 line, float t)
        {
            t = Mathf.Clamp01(t);

            var pointA = line.pointA;
            var pointB = line.pointB;

            return new Vector3(pointA.x + (pointB.x - pointA.x) * t, pointA.y + (pointB.y - pointA.y) * t,
                pointA.z + (pointB.z - pointA.z) * t);
        }
    }
}