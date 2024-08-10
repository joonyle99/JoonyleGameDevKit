using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace JoonyleGameDevKit
{
    public static class ExtensionMethod
    {
        // Vector
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
        public static Vector2 ToVector2XZ(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }
        public static Vector3 ToVector3(this Vector2 vec, float z = 0)
        {
            return new Vector3(vec.x, vec.y, z);
        }
        public static Vector3 ToVector3XZ(this Vector2 vec, float y = 0)
        {
            return new Vector3(vec.x, y, vec.y);
        }

        public static Vector3 Combine(this Vector2 v2, Vector3 v3)
        {
            return new Vector3(v2.x + v3.x, v2.y + v3.y, v3.z);
        }
        public static Vector3 Combine(this Vector3 v3, Vector2 v2)
        {
            return new Vector3(v3.x + v2.x, v3.y + v2.y, v3.z);
        }

        // Random
        public static int RangeExcept(this System.Random random, int minInclusive, int maxExclusive, int except, int limitCount = 10)
        {
            if (minInclusive < 0 || maxExclusive < 0 || minInclusive >= maxExclusive)
            {
                Debug.LogError($"Invalid minInclusive or maxExclusive\n{StackTraceUtility.ExtractStackTrace()}");
                return except;
            }

            var currentCount = 0;
            var result = except;

            while (result == except)
            {
                if (currentCount >= limitCount)
                {
                    result = random.Next(minInclusive, maxExclusive);
                    break;
                }

                currentCount++;

                result = random.Next(minInclusive, maxExclusive);
            }

            return result;
        }

        // LayerMask
        public static int GetLayerNumber(this int layerMaskValue)
        {
            if (layerMaskValue == 0)
                return -1;

            int layerNumber = 0;

            while (layerMaskValue > 1)      // 1�� �Ǹ� ����
            {
                layerMaskValue = layerMaskValue >> 1;
                layerNumber++;
            }

            return layerNumber;
        }
        public static int GetLayerValue(this int layerMaskNumber)
        {
            if (layerMaskNumber is < 0 or > 31)
                return -1;

            return 1 << layerMaskNumber;
        }

        public static int ExtractNumber(this string str)
        {
            // ���ڿ����� ���ӵ� ���ڸ� ã�´�
            Match match = Regex.Match(str, @"\d+");

            if (match.Success)
            {
                // ã�� ���� ���ڿ��� ������ ��ȯ�Ѵ�
                if (int.TryParse(match.Value, out int result))
                {
                    return result;
                }
            }

            // ���ڸ� ã�� ���߰ų� ��ȯ�� ������ ��� ���ܸ� ������
            throw new ArgumentException("No valid number found in the input string.");
        }
    }
}