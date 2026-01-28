using UnityEngine;

namespace JoonyleGameDevKit
{
    [System.Serializable]
    public class RangeFloat
    {
        private readonly System.Random random = new System.Random();

        public float Start;
        public float End;

        public float Random()
        {
            var offset = End - Start;
            var factor = random.NextDouble();
            return Start + (float)(offset * factor);
        }
    }

    [System.Serializable]
    public class RangeInt
    {
        private readonly System.Random random = new System.Random();

        public int Start;
        public int End;

        public RangeInt()
        {
            Start = 0;
            End = 0;
        }
        public RangeInt(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }

        public int Random()
        {
            return random.Next(Start, End);
        }
    }

    [System.Serializable]
    public struct Line2D
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public Line2D(Vector2 a, Vector2 b)
        {
            pointA = a;
            pointB = b;
        }

        /// <summary>
        /// Linearly interpolates between the two points of the line.
        /// </summary>
        public static Vector2 Lerp(Line2D line, float t)
        {
            t = Mathf.Clamp01(t);

            var pointA = line.pointA;
            var pointB = line.pointB;

            return new Vector2(pointA.x + (pointB.x - pointA.x) * t, pointA.y + (pointB.y - pointA.y) * t);
        }
    }
    
    [System.Serializable]
    public struct Line3D
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public Line3D(Vector3 a, Vector3 b)
        {
            pointA = a;
            pointB = b;
        }

        /// <summary>
        /// Linearly interpolates between the two points of the line.
        /// </summary>
        public static Vector3 Lerp(Line3D line, float t)
        {
            t = Mathf.Clamp01(t);

            var pointA = line.pointA;
            var pointB = line.pointB;

            return new Vector3(pointA.x + (pointB.x - pointA.x) * t, pointA.y + (pointB.y - pointA.y) * t, pointA.z + (pointB.z - pointA.z) * t);
        }
    }
}