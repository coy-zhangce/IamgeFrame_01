using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTool_CreateMesh : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    public Material mat;



    void Start()
    {
        // 创建一个新的网格对象
        mesh = new Mesh();

        // 设置网格的顶点
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 10),
            new Vector3(10, 0, 10),
            new Vector3(10, 0, 0)
        };

        mesh.vertices = vertices;

        // 设置网格的三角形（每个三角形由三个顶点的索引构成）
        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 3, 0
        };

        mesh.triangles = triangles;

        // 设置网格的法线（可选，但通常用于光照计算）
        Vector3[] normals = new Vector3[]
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        mesh.normals = normals;

        // 设置网格的UV坐标（可选，但通常用于纹理映射）
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        mesh.uv = uvs;

        // 创建一个网格过滤器组件，并将网格分配给它
        meshFilter = GetComponent<MeshFilter>();
        if (!meshFilter)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshFilter.mesh = mesh;

        // 创建一个网格渲染器组件，以便在场景中显示网格
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer.material = mat;
    }
}
