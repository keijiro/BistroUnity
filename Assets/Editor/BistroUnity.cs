using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

namespace BistroUnity {

public static class TextureUtil
{
    [MenuItem("Assets/Bistro/Remove Unsupported Textures")]
    public static void RemoveInvalidTextures()
    {
        Debug.Log("Removing unsupported texture files...");
        var dir = new DirectoryInfo("Assets/Bistro_v5_2/Textures");
        foreach (var file in dir.GetFiles("*.dds"))
        {
            if (file.Length == 144)
            {
                file.Delete();
                Debug.Log($"Removed: {file.Name}");
            }
        }
    }
}

public class BistroMaterialPostprocessor : AssetPostprocessor
{
    public void OnPreprocessMaterialDescription
      (MaterialDescription description,
       Material material,
       AnimationClip[] materialAnimation)
    {
        if (!description.materialName.StartsWith("Foliage")) return;

        var shader = material.shader;
        var keyword1 = new LocalKeyword(shader, "_ALPHATEST_ON");
        var keyword2 = new LocalKeyword(shader, "_DOUBLESIDED_ON");

        material.EnableKeyword(keyword1);
        material.EnableKeyword(keyword2);

        material.SetFloat("_AlphaCutoffEnable", 1);
        material.SetFloat("_CullMode", 0);
        material.SetFloat("_CullModeForward", 0);
        material.SetFloat("_DoubleSidedEnable", 1);
        material.SetFloat("_ZTestGBuffer", 3);

        material.SetOverrideTag("RenderType", "TransparentCutout");
        material.EnableKeyword("_ALPHATEST_ON");
        material.renderQueue = 2475;

        Debug.Log($"Alpha Cutout Enabled: {description.materialName}");
    }
}

} // namespace BistroUnity
