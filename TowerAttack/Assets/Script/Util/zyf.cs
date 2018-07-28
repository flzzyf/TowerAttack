using UnityEngine;

public class zyf
{
    public static void Out(string _msg)
    {
        Debug.Log(_msg);
    }

    public static void Out(int _msg)
    {
        Debug.Log(_msg);
    }

    //获取实际游戏世界屏幕尺寸
    public static Vector2 GetWorldScreenSize()
    {
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //the up right corner
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        float width = rightBorder - leftBorder;
        float height = topBorder - downBorder;

        return new Vector2(width, height);
    }

    //a分之一概率事件
    public static bool IfItWins(int _a)
    {
        return Random.Range(1, _a) == 1;
    }
}