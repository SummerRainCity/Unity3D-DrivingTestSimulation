using UnityEngine;

/// <summary>
/// 【反光镜】摄像机镜像
/// </summary>
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Retroreflector_MirrorFlip : MonoBehaviour
{
    new Camera camera;
    public bool flipHorizontal;
    void Awake()
    {
        camera = this.GetComponent<Camera>();
    }
    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        GL.invertCulling = flipHorizontal;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}