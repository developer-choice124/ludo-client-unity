using UnityEngine;

    [CreateAssetMenu(menuName = "ThemeBase")]
    public class ThemeConfig : ScriptableObject
    {
        public ColorModes color;
        public Sprite  baseSprite, tileSprite, goalSprite, tokenSprite, diceSprite;
        // Some fields
    }

