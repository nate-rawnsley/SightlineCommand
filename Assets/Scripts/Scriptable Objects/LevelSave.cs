using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nate
/// The data for a level save.
/// Created by LevelSaveEditor.
/// </summary>
public class LevelSave : ScriptableObject {
    public List<TileData> tiles;
    public int width;
    public int height;
    public float scale;
    public float gapScale;
}
