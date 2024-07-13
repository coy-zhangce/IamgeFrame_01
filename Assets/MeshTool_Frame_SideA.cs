using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTool_Frame_SideA : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    public Material mat;

    public GameObject MyGameObject = null;
    private const int CORNERSIZE = 10;

    public float width = 10;
    public float height = 10;
    public float radius = 1;

    public Vector3 posOffset = Vector3.zero;

    //          4, 5
    //      11, 0, 1, 6 
    //      10, 3, 2, 7
    //          9, 8
    void Start()
    {
        float v0_w = width - radius * 2;
        float v0_h = height - radius * 2;

        //new Vector3(0, 0, 0),            new Vector3(0, 0, 10),            new Vector3(10, 0, 10),            new Vector3(10, 0, 0)
        Vector3[] vertices = new Vector3[12 + CORNERSIZE * 4];
        {
            FillVertices(ref vertices);
            FillVertices_Zone(ref vertices, 12, 0, 11, 4);
            FillVertices_Zone(ref vertices, 12 + CORNERSIZE, 1, 5, 6);
            FillVertices_Zone(ref vertices, 12 + CORNERSIZE * 2, 2, 7, 8);
            FillVertices_Zone(ref vertices, 12 + CORNERSIZE * 3, 3, 9, 10);
        }


        // 设置网格的三角形（每个三角形由三个顶点的索引构成）
        int[] triangles = new int[ 5 * 6 + 12 * (CORNERSIZE + 2)];


        // 设置网格的法线（可选，但通常用于光照计算）
        Vector3[] normals = new Vector3[12 + CORNERSIZE * 4];
        {
            for (int i = 0; i < 12 + CORNERSIZE * 4; i++)
                normals[i] = Vector3.up;
        };


        // 设置网格的UV坐标（可选，但通常用于纹理映射）
        Vector2[] uvs = new Vector2[12 + CORNERSIZE * 4];
        {
            float rad_u = radius / width;
            float rad_v = radius / height;
            FillUV(ref uvs, rad_u, rad_v);
            FillUV_Zone(ref uvs, 12, 0, 11, 4);
            FillUV_Zone(ref uvs, 12 + CORNERSIZE, 1, 5, 6);
            FillUV_Zone(ref uvs, 12 + CORNERSIZE * 2, 2, 7, 8);
            FillUV_Zone(ref uvs, 12 + CORNERSIZE * 3, 3, 9, 10);
        }
        FileIndex(ref triangles);
        FillIndex_Zone(ref triangles, 0, 11, 4);
        FillIndex_Zone(ref triangles, 1, 5, 6);
        FillIndex_Zone(ref triangles, 2, 7, 8);
        FillIndex_Zone(ref triangles, 3, 9, 10);

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        if (MyGameObject != null)
            GameObject.Destroy(MyGameObject);
        MyGameObject = new GameObject("FrameOneSide");
        MyGameObject.transform.parent = gameObject.transform;
        MyGameObject.transform.localPosition = posOffset;
        MyGameObject.transform.localRotation = Quaternion.identity;
        MyGameObject.transform.localScale = Vector3.one;



        meshFilter = MyGameObject.GetComponent<MeshFilter>();
        if (!meshFilter)
        {
            meshFilter = MyGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
        MeshRenderer meshRenderer = MyGameObject.GetComponent<MeshRenderer>();
        if (!meshRenderer)
        {
            meshRenderer = MyGameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = mat;
            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

    }

    void FillVertices(ref Vector3[] vertices)
    {
        vertices[0] = new Vector3(radius, 0, radius);
        vertices[1] = new Vector3(width - radius, 0, radius);
        vertices[2] = new Vector3(width - radius, 0, height - radius);
        vertices[3] = new Vector3(radius, 0, height - radius);
        vertices[4] = new Vector3(radius, 0, 0);
        vertices[5] = new Vector3(width - radius, 0, 0);
        vertices[6] = new Vector3(width, 0, radius);
        vertices[7] = new Vector3(width, 0, height - radius);
        vertices[8] = new Vector3(width - radius, 0, height);
        vertices[9] = new Vector3(radius, 0, height);
        vertices[10] = new Vector3(0, 0, height - radius);
        vertices[11] = new Vector3(0, 0, radius);
    }
    void FillVertices_Zone(ref Vector3[] vertices, int startIndex, int i0, int i1, int i2)
    {
        Vector3 start0 = vertices[i0];  // 0,1,2,3
        Vector3 dir_U = vertices[i1] - start0;
        Vector3 dir_V = vertices[i2] - start0;
        float step = Mathf.PI / (CORNERSIZE + 1) * 0.5f;
        for (int i = 0; i < CORNERSIZE ; i++)
        {
            vertices[startIndex + i] = start0 + Mathf.Cos(step * (i + 1)) * dir_U + Mathf.Sin(step * (i + 1)) * dir_V;
        }
    }

    void FillUV(ref Vector2[] uv, float rad_u, float rad_v)
    {
        uv[0] = new Vector2(rad_u, rad_v);
        uv[1] = new Vector2(1.0f - rad_u, rad_v);
        uv[2] = new Vector2(1.0f - rad_u, 1.0f - rad_v);
        uv[3] = new Vector2(rad_u, 1.0f - rad_v);
        uv[4] = new Vector2(rad_u, 0);
        uv[5] = new Vector2(1.0f - rad_u, 0);
        uv[6] = new Vector2(1.0f, rad_v);
        uv[7] = new Vector2(1.0f, 1.0f - rad_v);
        uv[8] = new Vector2(1.0f - rad_u, 1.0f);
        uv[9] = new Vector2(rad_u, 1.0f);
        uv[10] = new Vector2(0, 1.0f - rad_v);
        uv[11] = new Vector2(0, rad_v);
    }

    void FillUV_Zone(ref Vector2[] uv, int startIndex, int i0, int i1, int i2)
    {
        Vector2 corner = uv[i0];
        Vector2 dir_U = uv[i1] - corner;
        Vector2 dir_V = uv[i2] - corner;
        float step = Mathf.PI / (CORNERSIZE + 1) * 0.5f;
        for (int i = 0; i < CORNERSIZE; i++)
        {
            uv[startIndex + i] = corner + Mathf.Cos(step * (i + 1)) * dir_U + Mathf.Sin(step * (i + 1)) * dir_V;
        }
    }

    void FileIndex(ref int[] ib)
    {
        int[] indexb = {
            0, 2, 1,   2, 0, 3,            4, 1, 5,   1, 4, 0,             1, 7, 6,   7, 1, 2,
            3, 8, 2,   8, 3, 9,            11, 3, 0,  3, 11, 10,
        };
        for (int i = 0; i< 30; i++)
        {
            ib[i] = indexb[i];
        }
    }

    void FillIndex_Zone(ref int[] ib, int i0, int i1, int i2)
    {
        // 左边三角面
        ib[30 + i0 * 3 * (CORNERSIZE + 2)] = i1;   
        ib[32 + i0 * 3 * (CORNERSIZE + 2)] = 12 + CORNERSIZE * i0;
        ib[31 + i0 * 3 * (CORNERSIZE + 2)] = i0;

        for (int i = 0; i < CORNERSIZE ; i++)
        {
            ib[33 + i0 * 3 * (CORNERSIZE + 2) + i * 3] = 12 + CORNERSIZE * i0 + i;
            ib[35 + i0 * 3 * (CORNERSIZE + 2) + i * 3] = 12 + CORNERSIZE * i0 + i + 1;
            ib[34 + i0 * 3 * (CORNERSIZE + 2) + i * 3] = i0;
        }

        // 右侧三角面
        ib[30 + i0 * 3 * (CORNERSIZE + 2) + CORNERSIZE * 3] = 12 + CORNERSIZE * i0 + CORNERSIZE - 1;
        ib[32 + i0 * 3 * (CORNERSIZE + 2) + CORNERSIZE * 3] = i2;
        ib[31 + i0 * 3 * (CORNERSIZE + 2) + CORNERSIZE * 3] = i0;

    }
}
