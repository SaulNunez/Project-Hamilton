using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlowerTypes
{
    public const string Sunflower = "Sunflower";
    public const string Daisy = "Daisy";
    public const string Tulip = "Tulip";
    public const string Roses = "Roses";

    public static readonly List<string> Types = new List<string>
    {
        Sunflower, Daisy, Tulip, Roses
    };
}
