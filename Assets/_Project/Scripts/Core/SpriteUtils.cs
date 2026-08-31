using UnityEngine;

namespace Hordewood.Core
{
    public static class SpriteUtils
    {
        public static Vector2 GetPivotCenteringOffset(Sprite sprite)
        {
            float ppu = sprite.pixelsPerUnit;
            Vector2 pivotPixels = sprite.pivot;
            float width = sprite.rect.width;
            float height = sprite.rect.height;

            Vector2 center = new Vector2(width * 0.5f, height * 0.5f);
            Vector2 offsetPixels = center - pivotPixels;
            return offsetPixels / ppu;
        }
    }
}
