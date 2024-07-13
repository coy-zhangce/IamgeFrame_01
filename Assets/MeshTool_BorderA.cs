using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTool_Border : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    public Material mat;

    public GameObject MyGameObject = null;
    public int cSize = MeshTool_Util.CORNERSIZE;

    public float width = 10;
    public float height = 10;
    public float rIn  = 1;
    public float rOut = 2;

    public Vector3 posOffset = Vector3.zero;

//             8, 12 
//             0, 4,
//      15, 7,        1, 9
//      11, 3,        5, 13
//             6,  2,
//            14, 10  
    void Start()
    {
        float v0_w = width - rOut * 2;
        float v0_h = height - rOut * 2;

        //new Vector3(0, 0, 0),            new Vector3(0, 0, 10),            new Vector3(10, 0, 10),            new Vector3(10, 0, 0)
        Vector3[] vertices = new Vector3[12 + cSize * 4];
        {
            FillVertices(ref vertices);
            FillVertices_Zone(ref vertices, new Vector3(rOut, 0, rOut), 16, 4, 12, 1, 9);
            FillVertices_Zone(ref vertices, new Vector3(width - rOut, 0, rOut), 16 + cSize * 2, 5, 13, 2, 10);
            FillVertices_Zone(ref vertices, new Vector3(width - rOut, 0, height - rOut), 16 + cSize * 4, 6, 14, 3, 11);
            FillVertices_Zone(ref vertices, new Vector3(rOut, 0, height - rOut), 16 + cSize * 6, 7, 15, 0, 8);
        }


        // 设置网格的三角形（每个三角形由三个顶点的索引构成）
        int[] triangles = new int[5 * 6 + 12 * (cSize + 2)];


        // 设置网格的法线（可选，但通常用于光照计算）
        Vector3[] normals = new Vector3[12 + cSize * 4];
        {
            for (int i = 0; i < 12 + cSize * 4; i++)
                normals[i] = Vector3.up;
        };


        // 设置网格的UV坐标（可选，但通常用于纹理映射）
        Vector2[] uvs = new Vector2[12 + cSize * 4];
        {
            float rad_u = rOut / width;
            float rad_v = rOut / height;
            FillUV(ref uvs, rad_u, rad_v);
            FillUV_Zone(ref uvs, 12, 0, 11, 4);
            FillUV_Zone(ref uvs, 12 + cSize, 1, 5, 6);
            FillUV_Zone(ref uvs, 12 + cSize * 2, 2, 7, 8);
            FillUV_Zone(ref uvs, 12 + cSize * 3, 3, 9, 10);
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
        }
    }

    void FillVertices(ref Vector3[] vertices)
    {
        vertices[0] = new Vector3(rOut, 0, rOut - rIn);
        vertices[4] = new Vector3(width - rOut, 0, rOut - rIn);
        vertices[8] = new Vector3(rOut, 0, 0);
        vertices[12] = new Vector3(width - rOut, 0, 0);

        vertices[1] = new Vector3(width - rOut + rIn, 0, rOut);
        vertices[5] = new Vector3(width - rOut + rIn, 0, height - rOut);
        vertices[9] = new Vector3(width, 0, rOut);
        vertices[13] = new Vector3(width, 0, height - rOut);

        vertices[2] = new Vector3(width - rOut, 0, height - rOut + rIn);
        vertices[6] = new Vector3(rOut, 0, height - rOut + rIn);
        vertices[10] = new Vector3(width - rOut, 0, 0);
        vertices[14] = new Vector3(rOut, 0, 0);

        vertices[3] = new Vector3(rOut - rIn, 0, height - rOut);
        vertices[7] = new Vector3(rOut - rIn, 0, rOut);
        vertices[11] = new Vector3(0, 0, height - rOut);
        vertices[15] = new Vector3(0, 0, rOut);

    }
    void FillVertices_Zone(ref Vector3[] vertices, Vector3 start, int startIndex, int i0, int o0, int i1, int o1)
    {
        Vector3 dir_U = vertices[i0] - start;
        Vector3 dir_UO = vertices[o0] - start;
        Vector3 dir_V = vertices[i1] - start;
        Vector3 dir_VO = vertices[i1] - start;
        float step = Mathf.PI / (cSize + 1) * 0.5f;
        for (int i = 0; i < cSize; i++)
        {
            vertices[startIndex + i*2] = start + Mathf.Cos(step * (i + 1)) * dir_U + Mathf.Sin(step * (i + 1)) * dir_V;
            vertices[startIndex + i*2 +1] = start + Mathf.Cos(step * (i + 1)) * dir_UO + Mathf.Sin(step * (i + 1)) * dir_VO;
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
        float step = Mathf.PI / (cSize + 1) * 0.5f;
        for (int i = 0; i < cSize; i++)
        {
            uv[startIndex + i] = corner + Mathf.Cos(step * (i + 1)) * dir_U + Mathf.Sin(step * (i + 1)) * dir_V;
        }
    }

    void FileIndex(ref int[] ib)
    {
        int[] indexb = {
            0, 1, 2,   2, 3, 0,             4, 5,1,    1,  0,4,             1, 6,7,    7, 2,1,
            3, 2, 8,    8, 9, 3,             11, 0,3,    3, 10,11,
        };
        for (int i = 0; i < 30; i++)
        {
            ib[i] = indexb[i];
        }
    }

    void FillIndex_Zone(ref int[] ib, int i0, int i1, int i2)
    {
        // 左边三角面
        ib[30 + i0 * 3 * (cSize + 2)] = i1;
        ib[31 + i0 * 3 * (cSize + 2)] = 12 + cSize * i0;
        ib[32 + i0 * 3 * (cSize + 2)] = i0;

        for (int i = 0; i < cSize; i++)
        {
            ib[33 + i0 * 3 * (cSize + 2) + i * 3] = 12 + cSize * i0 + i;
            ib[34 + i0 * 3 * (cSize + 2) + i * 3] = 12 + cSize * i0 + i + 1;
            ib[35 + i0 * 3 * (cSize + 2) + i * 3] = i0;
        }

        // 右侧三角面
        ib[30 + i0 * 3 * (cSize + 2) + cSize * 3] = 12 + cSize * i0 + cSize - 1;
        ib[31 + i0 * 3 * (cSize + 2) + cSize * 3] = i2;
        ib[32 + i0 * 3 * (cSize + 2) + cSize * 3] = i0;

    }
}
