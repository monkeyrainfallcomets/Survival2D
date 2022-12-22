using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PXAnimator : MonoBehaviour
{
    //scale is how much rgb value correlates to position. So if scale is 10 and r is 10 and b is 10 then the position will be 1,1
    [SerializeField] float scale;
    [SerializeField] CharacterMap baseMap;
    [SerializeField] SerializableDictionary<string, PXAnimation> animations;
    Dictionary<string, CachedPXAnimation> cachedAnimations;
    public void Play(float speed, bool loop)
    {

    }
    //this method will put the given texture at the start position start position being left most.
    public void ModifyCharacterMap(Texture2D modification, Vector2Int startPosition)
    {
        baseMap.Modify(modification, startPosition);
    }
}

//Animation for the PX library which is mine cuz i'm cool
public struct PXAnimation
{
    [SerializeField] PXAnimationMap[] frames;

}
public struct PXAnimationMap
{

}

public struct CharacterMap
{
    [SerializeField] Texture2D texture;
    public void Modify(Texture2D modification, Vector2Int startPosition)
    {
        int modHeight = modification.height;
        int modWidth = modification.width;
        int height = texture.height;
        int width = texture.width;
        for (int y = 0; y < modHeight && y < height - startPosition.y; y++)
        {
            for (int x = 0; x < modWidth && x < width - startPosition.x; x++)
            {
                texture.SetPixel(x + startPosition.x, y + startPosition.y, modification.GetPixel(x, y));
            }
        }
    }
}
//An animation that is cached
public struct CachedPXAnimation
{

}