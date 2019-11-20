using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Optionen für LineRenderer, in Form von verschiedener Konstruktoren.
/// </summary>
public static class Constants
{
    public class LineRendererOption
    {
        public Material stdMaterial;
        public float stdStartWidth;
        public float stdEndWidth;
        public LineTextureMode textureMode;

        public LineRendererOption(Material material, float v1, float v2, LineTextureMode mode)
        {
            this.stdMaterial = material;
            this.stdStartWidth = v1;
            this.stdEndWidth = v2;
            this.textureMode = mode;
        }
    }
    public static LineRendererOption lineRenderOption1 = new LineRendererOption(
        Resources.Load("Material/LineMaterial1", typeof(Material)) as Material,
        0.04f,
        0.09f,
        LineTextureMode.Stretch
        );
    public static LineRendererOption lineRenderOption2 = new LineRendererOption(
        Resources.Load("Material/LineMaterial2", typeof(Material)) as Material,
        0.04f,
        0.04f,
        LineTextureMode.Stretch
        );
    public static LineRendererOption lineRenderOption3 = new LineRendererOption(
        Resources.Load("Material/LineMaterial2", typeof(Material)) as Material,
        0.1f,
        0.1f,
        LineTextureMode.Stretch
        );
    public static LineRendererOption lineRenderOption4 = new LineRendererOption(
        Resources.Load("Material/LineMaterial1", typeof(Material)) as Material,
        0.1f,
        0.1f,
        LineTextureMode.Stretch
        );

}
