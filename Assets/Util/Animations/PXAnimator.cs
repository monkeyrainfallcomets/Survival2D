using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PXAnimator : MonoBehaviour
{
    //scale is how much rgb value correlates to position. So if scale is 10 and r is 10 and b is 10 then the position will be 1,1
    [SerializeField] SpriteRenderer mainRenderer;
    [SerializeField] CharacterMap baseMap;
    [SerializeField] SerializableDictionary<string, PXAnimation>[] animations;
    [SerializeField] int scale;
    [SerializeField] int state;
    [SerializeField] Texture2D dexter;
    Rect spriteRect;
    PlayingAnimation currentAnimation;
    bool animationPlaying;
    Dictionary<string, CachedPXAnimation>[] cachedAnimations;
    void Awake()
    {
        cachedAnimations = new Dictionary<string, CachedPXAnimation>[animations.Length];
        spriteRect = mainRenderer.sprite.rect;
    }

    void Start()
    {
        Play("Idle", 2, true, 2);
    }
    public bool Play(string animation, float speed, bool loop, float loopDelay)
    {
        CachedPXAnimation cachedAnimation;
        if (cachedAnimations[state] != null)
        {
            if (cachedAnimations[state].TryGetValue(animation, out cachedAnimation))
            {
                Play(cachedAnimation, speed, loop, loopDelay);
                return true;
            }
        }
        else
        {
            cachedAnimations[state] = new Dictionary<string, CachedPXAnimation>();
        }
        if (animations[state][animation] != null)
        {
            cachedAnimation = new CachedPXAnimation(animations[state][animation], baseMap, scale, speed, spriteRect);
            cachedAnimations[state][animation] = cachedAnimation;
            Play(cachedAnimation, speed, loop, loopDelay);
        }

        animationPlaying = false;
        return false;
    }

    void Play(CachedPXAnimation animation, float speed, bool loop, float loopDelay)
    {
        if (animationPlaying)
        {
            Stop();
        }
        currentAnimation = new PlayingAnimation(animation, speed, loop, loopDelay);
        StartCoroutine("PlayAnimation");
        animationPlaying = true;
    }

    public void Stop()
    {
        if (animationPlaying)
        {
            animationPlaying = false;
            StopCoroutine("PlayAnimation");
        }
    }

    IEnumerator PlayAnimation()
    {
        bool initialCall = true;
        while (currentAnimation.loop || initialCall)
        {
            initialCall = false;
            for (int i = 0; i < currentAnimation.animation.frames.Length; i++)
            {
                mainRenderer.sprite = currentAnimation.animation.frames[i];
                yield return new WaitForSeconds(currentAnimation.animation.delay);
            }
            yield return new WaitForSeconds(currentAnimation.loopDelay);
        }

    }
    public void ChangeState(int state, string animation, float speed, bool loop, float loopDelay)
    {

    }
    //this method will put the given texture at the start position start position being left most.
    public void ModifyCharacterMap(Texture2D modification, Vector2Int startPosition)
    {
        baseMap.Modify(modification, startPosition);
    }
    public class CachedPXAnimation
    {
        public Sprite[] frames;
        public float delay;
        public CachedPXAnimation(PXAnimation animation, CharacterMap characterMap, int scale, float frameTransition, Rect spriteRect)
        {
            frames = animation.ConvertToAnimation(characterMap, scale, spriteRect);
            delay = frameTransition;
        }
    }
    [System.Serializable]
    public class PXAnimation
    {
        [SerializeField] PXAnimationMap[] frames;
        public Sprite[] ConvertToAnimation(CharacterMap characterMap, int scale, Rect spriteRect)
        {
            Sprite[] animation = new Sprite[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                animation[i] = Sprite.Create(frames[i].ConvertToTexture(characterMap, scale), spriteRect, new Vector2(0, 0));
            }
            return animation;
        }
    }
    [System.Serializable]
    public class PXAnimationMap
    {
        [SerializeField] Texture2D texture;
        public Texture2D ConvertToTexture(CharacterMap characterMap, int scale)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    newTexture.SetPixel(x, y, characterMap.GetPixel(texture.GetPixel(x, y), scale));
                }
            }
            newTexture.filterMode = FilterMode.Point;
            newTexture.Apply();
            return newTexture;
        }
    }
    [System.Serializable]
    public class CharacterMap
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
            texture.Apply();
        }
        public Color32 GetPixel(Color32 color, int scale)
        {
            Vector2Int position = new Vector2Int(color.r / scale, color.g / scale);
            Color32 pixel = texture.GetPixel(position.x, position.y);
            if (color.a == 0)
            {
                pixel = new Color32(0, 0, 0, 0);
            }
            //pixel = Color32.Lerp(pixel, new Color32(0, 0, 0, 255), color.b / 255);
            //pixel = Color32.Lerp(pixel, new Color32(255, 255, 255, 255), color.a / 255);
            return pixel;
        }
    }

    public class PlayingAnimation
    {
        public CachedPXAnimation animation;
        public int currentFrame;
        public float speed;
        public bool loop;
        public float loopDelay;
        public PlayingAnimation(CachedPXAnimation animation, float speed, bool loop, float loopDelay)
        {
            currentFrame = 0;
            this.animation = animation;
            this.speed = speed;
            this.loop = loop;
            this.loopDelay = loopDelay;
        }
    }
}



