using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public static class Helper
{

    public static float radiusToVolume(float radius)
    {
        float volume = Mathf.Pow(radius, 3) * 4 * Mathf.PI / 3;
        return volume;
    }

    public static float volumeToRadius(float volume)
    {
        float radius = Mathf.Pow( (3 * volume / (4 * Mathf.PI)), (1.0f/3.0f));
        return radius;
    }

    public static String FormatAs10Power(decimal val)
    {
        String SuperscriptDigits = "\u2070\u00b9\u00b2\u00b3\u2074\u2075\u2076\u2077\u2078\u2079";
        String expstr = String.Format("{0:0.##E0}", val);

        var numparts = expstr.Split('E');
        char[] powerchars = numparts[1].ToArray();
        for (int i = 0; i < powerchars.Length; i++)
        {
            powerchars[i] = (powerchars[i] == '-') ? '\u207b' : SuperscriptDigits[powerchars[i] - '0'];
        }
        numparts[1] = new String(powerchars);
        return String.Join(" x 10", numparts);
    }

}