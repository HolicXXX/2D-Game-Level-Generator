// /*
// 	Allen
// 	2018/5/21
// 	allendk@foxmail.com
// */
using System;
public static class ExtensionUtils
{
    public static float Truncate(this float value, int digits = 4)
    {
        double mult = Math.Pow(10.0, digits);
		mult = Math.Truncate(mult * value) / mult;
		return (float)mult;
    }

	public static double Truncate(this double value, int digits = 4)
    {
        double mult = Math.Pow(10.0, digits);
		return Math.Truncate(mult * value) / mult;
    }
}
