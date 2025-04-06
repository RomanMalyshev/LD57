using UnityEngine;

namespace Rom
{
    public static class MathHelper
    {
        public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        // Utility function to map a value from one range to another
        public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            // Basic check to prevent division by zero
            if (Mathf.Approximately(fromMax, fromMin))
            {
                Debug.LogWarning("Map function: fromMin and fromMax are the same, returning toMin.");
                return toMin;
            }
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }
    }


}
