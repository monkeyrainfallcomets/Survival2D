using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
[System.Serializable]
public class TilePlacementInstance : PlacementInstance
{
    Vector3Int position;
    Tilemap tilemap;
    GenTile genTile;
    int priority;
    bool[] transitions = new bool[4];
    protected TilePlacementInstance[] surroundingTiles = new TilePlacementInstance[4];

    public TilePlacementInstance(Tilemap tilemap, Vector3Int position, GenTile genTile, int priority)
    {
        this.position = position;
        this.tilemap = tilemap;
        this.genTile = genTile;
        this.priority = priority;
    }

    public override void Destroy(PlanetGenerationInstance world)
    {
        tilemap.SetTile(position, null);
    }
    public override void Place(PlanetGenerationInstance world)
    {
        for (int i = 0; i < 4; i++)
        {
            if (surroundingTiles[i] == null)
            {
                world.GetTile((Vector2Int)position + GetDirection(i), out surroundingTiles[i]);
            }
        }
        int tileIndex = GenTile.GetTransitionTileIndex(transitions);
        if (tileIndex != -1 && genTile.tile.transitionTiles != null)
        {
            Texture2D baseTexture = genTile.tile.transitionTiles[tileIndex];
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = Sprite.Create(GenerateTexture(baseTexture, world), new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);
            tilemap.SetTile(position, tile);

        }
        else
        {
            tilemap.SetTile(position, genTile.tile.baseTile);
        }

    }
    public bool SelectDetail(Vector2Int position, out Placement placement, NoiseValue noiseValues)
    {
        return genTile.SelectDetail(position, out placement, noiseValues);
    }
    public void AddTransition(Vector2Int direction, TilePlacementInstance tile)
    {
        int index = GetIndex(direction);
        transitions[index] = true;
        surroundingTiles[index] = tile;
    }

    public void CacheNeighbor(TilePlacementInstance tile, Vector2Int direction)
    {
        surroundingTiles[GetIndex(direction)] = tile;
    }
    public static void RectPaste(RectInt referenceRect, RectInt pasteRect, Texture2D baseTexture, Texture2D alphaTexture, Texture2D outputTexture, bool transparencyOnly)
    {
        Vector2Int pasteLocation = pasteRect.position;
        Vector2Int referenceLocation = referenceRect.position;
        for (int i = 0; referenceLocation.y <= referenceRect.height; i++)
        {
            if (!transparencyOnly || alphaTexture.GetPixel(pasteLocation.x, pasteLocation.y).a == 0)
            {
                if (transparencyOnly || baseTexture.GetPixel(referenceLocation.x, referenceLocation.y).a != 0)
                {
                    outputTexture.SetPixel(pasteLocation.x, pasteLocation.y, baseTexture.GetPixel(referenceLocation.x, referenceLocation.y));
                }

            }
            if (pasteLocation.x >= pasteRect.width)
            {
                pasteLocation.x = pasteRect.position.x;
                pasteLocation.y++;
            }
            else
                pasteLocation.x++;

            if (referenceLocation.x >= referenceRect.width)
            {
                referenceLocation.x = referenceRect.position.x;
                referenceLocation.y++;
            }
            else
                referenceLocation.x++;
        }
    }
    private Texture2D GenerateTexture(Texture2D baseTexture, PlanetGenerationInstance world)
    {
        Texture2D texture = new Texture2D(16, 16, TextureFormat.RGBA32, baseTexture.mipmapCount, true);
        texture.filterMode = FilterMode.Point;
        Graphics.CopyTexture(baseTexture, texture);
        bool assigned = false;
        PlanetType planet = world.GetPlanet();

        if (transitions[0])
        {
            if (transitions[1] && !transitions[3])
            {
                if ((surroundingTiles[0].GetTile().tile.GetPriority(planet) > surroundingTiles[1].GetTile().tile.GetPriority(planet)))
                {
                    RectPaste(new RectInt(12, 4, 15, 15), new RectInt(0, 4, 3, 15), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(12, 0, 15, 15), new RectInt(0, 0, 3, 15), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (!transitions[1] && transitions[3])
            {
                if (surroundingTiles[0].GetTile().tile.GetPriority(planet) > surroundingTiles[3].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(12, 0, 15, 11), new RectInt(0, 0, 3, 11), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(12, 0, 15, 15), new RectInt(0, 0, 3, 15), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (transitions[3] && transitions[1])
            {
                if (surroundingTiles[0].GetTile().tile.GetPriority(planet) > surroundingTiles[3].GetTile().tile.GetPriority(planet))
                {
                    if (surroundingTiles[0].GetTile().tile.GetPriority(planet) > surroundingTiles[1].GetTile().tile.GetPriority(planet))
                    {
                        RectPaste(new RectInt(12, 4, 15, 11), new RectInt(0, 4, 3, 11), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                    else
                    {
                        RectPaste(new RectInt(12, 0, 15, 11), new RectInt(0, 0, 3, 11), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                }
                else if (surroundingTiles[0].GetTile().tile.GetPriority(planet) > surroundingTiles[1].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(12, 4, 15, 15), new RectInt(0, 4, 3, 15), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(12, 0, 15, 15), new RectInt(0, 0, 3, 15), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else
            {
                RectPaste(new RectInt(12, 0, 15, 15), new RectInt(0, 0, 3, 15), surroundingTiles[0].GetTile().tile.baseTexture, baseTexture, texture, true);
            }
        }

        if (transitions[1])
        {
            if (transitions[0] && !transitions[2])
            {
                if (surroundingTiles[1].GetTile().tile.GetPriority(planet) > surroundingTiles[0].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(4, 12, 15, 15), new RectInt(4, 0, 15, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 12, 15, 15), new RectInt(0, 0, 15, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (!transitions[0] && transitions[2])
            {
                if (surroundingTiles[1].GetTile().tile.GetPriority(planet) > surroundingTiles[2].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(0, 12, 11, 15), new RectInt(0, 0, 11, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 12, 15, 15), new RectInt(0, 0, 15, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (transitions[0] && transitions[2])
            {
                if (surroundingTiles[1].GetTile().tile.GetPriority(planet) > surroundingTiles[2].GetTile().tile.GetPriority(planet))
                {
                    if (surroundingTiles[1].GetTile().tile.GetPriority(planet) > surroundingTiles[0].GetTile().tile.GetPriority(planet))
                    {
                        RectPaste(new RectInt(4, 12, 11, 15), new RectInt(4, 0, 11, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                    else
                    {
                        RectPaste(new RectInt(0, 12, 11, 15), new RectInt(0, 0, 11, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                }
                else if (surroundingTiles[1].GetTile().tile.GetPriority(planet) > surroundingTiles[0].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(4, 12, 15, 15), new RectInt(4, 0, 15, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 12, 15, 15), new RectInt(0, 0, 15, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else
            {
                RectPaste(new RectInt(0, 12, 15, 15), new RectInt(0, 0, 15, 3), surroundingTiles[1].GetTile().tile.baseTexture, baseTexture, texture, true);
            }
        }

        if (transitions[2])
        {
            if (transitions[1] && !transitions[3])
            {
                if (surroundingTiles[2].GetTile().tile.GetPriority(planet) > surroundingTiles[1].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(0, 4, 3, 15), new RectInt(12, 4, 15, 15), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 0, 3, 15), new RectInt(12, 0, 15, 15), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (!transitions[1] && transitions[3])
            {
                if (surroundingTiles[2].GetTile().tile.GetPriority(planet) > surroundingTiles[3].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(0, 0, 3, 11), new RectInt(12, 0, 15, 11), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 0, 3, 15), new RectInt(12, 0, 15, 15), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (transitions[3] && transitions[1])
            {
                if (surroundingTiles[2].GetTile().tile.GetPriority(planet) > surroundingTiles[3].GetTile().tile.GetPriority(planet))
                {
                    if (surroundingTiles[2].GetTile().tile.GetPriority(planet) > surroundingTiles[1].GetTile().tile.GetPriority(planet))
                    {
                        RectPaste(new RectInt(0, 4, 3, 11), new RectInt(12, 4, 15, 11), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                    else
                    {
                        RectPaste(new RectInt(0, 0, 3, 11), new RectInt(12, 0, 15, 11), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                }
                else if (surroundingTiles[2].GetTile().tile.GetPriority(planet) > surroundingTiles[1].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(0, 4, 3, 15), new RectInt(12, 4, 15, 15), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 0, 3, 15), new RectInt(12, 0, 15, 15), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else
            {
                RectPaste(new RectInt(0, 0, 3, 15), new RectInt(12, 0, 15, 15), surroundingTiles[2].GetTile().tile.baseTexture, baseTexture, texture, true);
            }
        }
        if (transitions[3])
        {
            if (transitions[0] && !transitions[2])
            {
                if (surroundingTiles[3].GetTile().tile.GetPriority(planet) > surroundingTiles[0].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(4, 0, 15, 3), new RectInt(4, 12, 15, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 0, 15, 3), new RectInt(0, 12, 15, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (!transitions[0] && transitions[2])
            {
                if (surroundingTiles[3].GetTile().tile.GetPriority(planet) > surroundingTiles[2].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(0, 0, 11, 3), new RectInt(0, 12, 11, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 0, 15, 3), new RectInt(0, 12, 15, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else if (transitions[0] && transitions[2])
            {
                if (surroundingTiles[3].GetTile().tile.GetPriority(planet) > surroundingTiles[2].GetTile().tile.GetPriority(planet))
                {
                    if (surroundingTiles[3].GetTile().tile.GetPriority(planet) > surroundingTiles[0].GetTile().tile.GetPriority(planet))
                    {
                        RectPaste(new RectInt(4, 0, 11, 3), new RectInt(4, 12, 11, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                    else
                    {
                        RectPaste(new RectInt(0, 0, 11, 3), new RectInt(0, 12, 11, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                    }
                }
                else if (surroundingTiles[3].GetTile().tile.GetPriority(planet) > surroundingTiles[0].GetTile().tile.GetPriority(planet))
                {
                    RectPaste(new RectInt(4, 0, 15, 3), new RectInt(4, 12, 15, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
                else
                {
                    RectPaste(new RectInt(0, 0, 15, 3), new RectInt(0, 12, 15, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
                }
            }
            else
            {
                RectPaste(new RectInt(0, 0, 15, 3), new RectInt(0, 12, 15, 15), surroundingTiles[3].GetTile().tile.baseTexture, baseTexture, texture, true);
            }
        }
        texture.Apply(true, true);
        return texture;
    }


    private int GetIndex(Vector2Int direction)
    {
        if (direction == Vector2Int.left)
        {
            return 0;
        }
        else if (direction == Vector2Int.down)
        {
            return 1;
        }
        else if (direction == Vector2Int.right)
        {
            return 2;
        }
        else if (direction == Vector2Int.up)
        {
            return 3;
        }
        return -1;
    }

    private Vector2Int GetDirection(int index)
    {

        if (index == 0)
        {
            return Vector2Int.left;
        }
        else if (index == 1)
        {
            return Vector2Int.down;
        }
        else if (index == 2)
        {
            return Vector2Int.right;
        }
        else if (index == 3)
        {
            return Vector2Int.up;
        }
        return Vector2Int.zero;
    }
    public bool HasTransition(int index)
    {
        return transitions[index];
    }

    public GenTile GetTile()
    {
        return genTile;
    }
}
