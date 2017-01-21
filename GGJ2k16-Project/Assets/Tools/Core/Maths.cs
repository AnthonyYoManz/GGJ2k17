using UnityEngine;

static class Maths
{
    #region Vector helpers
    public static Vector2 ComponentMultiply(Vector2 v1, Vector2 v2)
    {
        Vector2 result = new Vector2();
        result.x = v1.x * v2.x;
        result.y = v1.y * v2.y;
        return result;
    }
    public static Vector3 ComponentMultiply(Vector3 v1, Vector3 v2)
    {
        Vector3 result = new Vector3();
        result.x = v1.x* v2.x;
        result.y = v1.y* v2.y;
        result.z = v1.z * v2.z;
        return result;
    }
    public static Vector4 ComponentMultiply(Vector4 v1, Vector4 v2)
    {
        Vector4 result = new Vector4();
        result.x = v1.x * v2.x;
        result.y = v1.y * v2.y;
        result.z = v1.z * v2.z;
        result.z = v1.w * v2.w;
        return result;
    }
    #endregion

    #region Smoothing functions  
    /// <summary>
    /// Smooth x. Input must be between 0, 1.
    /// </summary>
    /// <returns></returns>
    public static float Smooth(float x)
    {
        return x * x * (3.0f - 2.0f * x);
    }
    /// <summary>
    /// Smooth x. Input must be between 0, 1.
    /// </summary>
    /// <returns></returns>
    public static double Smooth(double x)
    {
        return x * x * (3.0 - 2.0 * x);
    }
    /// <summary>
    /// Smooth x. Input components must be between 0, 1.
    /// </summary>
    /// <returns></returns>
    public static Vector2 Smooth(Vector2 x)
    {
        return ComponentMultiply(ComponentMultiply(x,x), (new Vector2(3.0f, 3.0f) - ComponentMultiply(new Vector2(2.0f, 2.0f), x)));
    }
    /// <summary>
    /// Smooth x. Input components must be between 0, 1.
    /// </summary>
    /// <returns></returns>
    public static Vector3 Smooth(Vector3 x)
    {
        return ComponentMultiply(ComponentMultiply(x, x), (new Vector3(3.0f, 3.0f, 3.0f) - ComponentMultiply(new Vector3(2.0f, 2.0f, 2.0f), x)));
    }
    /// <summary>
    /// Smooth x. Input components must be between 0, 1.
    /// </summary>
    /// <returns></returns>
    public static Vector4 Smooth(Vector4 x)
    {
        return ComponentMultiply(ComponentMultiply(x, x), (new Vector4(3.0f, 3.0f, 3.0f, 3.0f) - ComponentMultiply(new Vector4(2.0f, 2.0f, 2.0f, 2.0f), x)));
    }
    #endregion
}