using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    public static Vector2 GetBezier3(float time, float period, Vector2 start, Vector2 middle, Vector2 target)
    {
        float t = time / period;
        var ab = Vector2.Lerp(start,middle,t);
        var bc = Vector2.Lerp(middle,target,t);
        return Vector2.Lerp(ab,bc,t);
    }

    public static Vector2 GetBezier2(float time, float period, Vector2 start, Vector2 startForward, Vector2 end, Vector2 endForward)
    {
        float t = time / period;
        Vector2 p0 = start;
        Vector2 p1 = p0 + startForward;
        Vector2 p3 = end;
        Vector2 p2 = p3 + endForward;
        
        // here is where the magic happens!
        return Mathf.Pow(1f-t,3f)*p0+3f*Mathf.Pow(1f-t,2f)*t*p1+3f*(1f-t)*Mathf.Pow(t,2f)*p2+Mathf.Pow(t,3f)*p3;
    }
}
