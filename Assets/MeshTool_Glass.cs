using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeshTool_Glass : MonoBehaviour
{
    public float width = 10;
    public float height = 10;
    public float radius = 1;

    private Mesh mesh;
    private MeshFilter meshFilter;
    public Material mat;
    public GameObject MyGameObject = null;

    public List<string> imageAddress = new List<string>();

    private int cornersize = MeshTool_Util.CORNERSIZE;
    void Start()
    {
        LoadConfig();
    }

    void InitMesh(ref List<Vector3> vbs, ref List<int> ibs, ref List<Vector3> nls, ref List<Vector2> uvs)
    {
        var vb = vbs.ToArray();
        Vector3[] vertices = new Vector3[vbs.Count];
        for (int i = 0; i < vbs.Count; i++)
            vertices[i] = vbs[i];

        var ib = ibs.ToArray();
        int[] indexbuff = new int[ib.Length];
        for (int i = 0; i < ibs.Count; i++)
            indexbuff[i] = ibs[i];

        var nl = nls.ToArray();
        Vector3[] nlbuff = new Vector3[nls.Count];
        for (int i = 0; i < ibs.Count; i++)
            indexbuff[i] = ibs[i];


        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indexbuff;
        mesh.normals = nls.ToArray();
        mesh.uv = uvs.ToArray();

        if (MyGameObject != null)
            GameObject.Destroy(MyGameObject);
        MyGameObject = new GameObject("FrameOneSide");
        MyGameObject.transform.parent = gameObject.transform;
        MyGameObject.transform.localPosition = Vector3.zero;
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

    List<Vector3> vbs = new List<Vector3>();
    List<int> ibs = new List<int>();
    List<Vector3> nls = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    void LoadConfig()
    {
        vbs.Clear();
        nls.Clear();
        uvs.Clear();
        ibs.Clear();

        float v0_w = width - radius * 2;
        float v0_h = height - radius * 2;

        //new Vector3(0, 0, 0),            new Vector3(0, 0, 10),            new Vector3(10, 0, 10),            new Vector3(10, 0, 0)
        FillVertices(ref vbs);  // = 12
        FillVertices_Zone(ref vbs, 0, 11, 4);  // = 12 + cornersize
        FillVertices_Zone(ref vbs, 1, 5, 6);  // = 12 + cornersize * 2
        FillVertices_Zone(ref vbs, 2, 7, 8);  // = 12 + cornersize * 3
        FillVertices_Zone(ref vbs, 3, 9, 10);  // = 12 + cornersize * 4


        // 设置网格的三角形（每个三角形由三个顶点的索引构成）

        // 设置网格的法线（可选，但通常用于光照计算）
        for (int i = 0; i < 12 + cornersize * 4; i++)
            nls.Add(Vector3.up);


        // 设置网格的UV坐标（可选，但通常用于纹理映射）
        float rad_u = radius / width;
        float rad_v = radius / height;
        FillUV(ref uvs, rad_u, rad_v);
        FillUV_Zone(ref uvs, 0, 11, 4);
        FillUV_Zone(ref uvs, 1, 5, 6);
        FillUV_Zone(ref uvs, 2, 7, 8);
        FillUV_Zone(ref uvs, 3, 9, 10);
        FileIndex(ref ibs);
        FillIndex_Zone(ref ibs, 12 + cornersize * 0, 0, 11, 4);
        FillIndex_Zone(ref ibs, 12 + cornersize * 1, 1, 5, 6);
        FillIndex_Zone(ref ibs, 12 + cornersize * 2, 2, 7, 8);
        FillIndex_Zone(ref ibs, 12 + cornersize * 3, 3, 9, 10);

        //mesh.vertices = vertices;
        //mesh.triangles = triangles;
        //mesh.normals = normals;
        //mesh.uv = uvs;
        InitMesh(ref vbs, ref ibs, ref nls, ref uvs);

    }

    int FillVertices(ref List<Vector3> vertices)
    {
        vertices.Add(new Vector3(radius, 0, radius));
        vertices.Add(new Vector3(width - radius, 0, radius));
        vertices.Add(new Vector3(width - radius, 0, height - radius));
        vertices.Add(new Vector3(radius, 0, height - radius));
        vertices.Add(new Vector3(radius, 0, 0));
        vertices.Add(new Vector3(width - radius, 0, 0));
        vertices.Add(new Vector3(width, 0, radius));
        vertices.Add(new Vector3(width, 0, height - radius));
        vertices.Add(new Vector3(width - radius, 0, height));
        vertices.Add(new Vector3(radius, 0, height));
        vertices.Add(new Vector3(0, 0, height - radius));
        vertices.Add(new Vector3(0, 0, radius));
        return vertices.Count;
    }
    void FillVertices_Zone(ref List<Vector3> vertices, int i0, int i1, int i2)
    {
        Vector3 start0 = vertices[i0];  // 0,1,2,3
        Vector3 dir_U = vertices[i1] - start0;
        Vector3 dir_V = vertices[i2] - start0;
        float step = Mathf.PI / (cornersize + 1) * 0.5f;
        for (int i = 0; i < cornersize; i++)
        {
            vertices.Add(start0 + Mathf.Cos(step * (i + 1)) * dir_U + Mathf.Sin(step * (i + 1)) * dir_V);
        }
    }

    void FillUV(ref List<Vector2> uv, float rad_u, float rad_v)
    {
        uv.Add(new Vector2(rad_u, 1.0f - rad_v));
        uv.Add(new Vector2(1.0f - rad_u, 1.0f - rad_v));
        uv.Add(new Vector2(1.0f - rad_u, rad_v));
        uv.Add(new Vector2(rad_u, rad_v));
        uv.Add(new Vector2(rad_u, 1));
        uv.Add(new Vector2(1.0f - rad_u, 1));
        uv.Add(new Vector2(1.0f, 1.0f - rad_v));
        uv.Add(new Vector2(1.0f, rad_v));
        uv.Add(new Vector2(1.0f - rad_u, 0.0f));
        uv.Add(new Vector2(rad_u, 0.0f));
        uv.Add(new Vector2(0, rad_v));
        uv.Add(new Vector2(0, 1.0f - rad_v));
    }

    void FillUV_Zone(ref List<Vector2> uv, int i0, int i1, int i2)
    {
        Vector2 corner = uv[i0];
        Vector2 dir_U = uv[i1] - corner;
        Vector2 dir_V = uv[i2] - corner;
        float step = Mathf.PI / (cornersize + 1) * 0.5f;
        for (int i = 0; i < cornersize; i++)
        {
            uv.Add(corner + Mathf.Cos(step * (i + 1)) * dir_U + Mathf.Sin(step * (i + 1)) * dir_V);
        }
    }

    void FileIndex(ref List<int> ib)
    {
        AddIndexBuffer(ref ib, 0, 1, 2);
        AddIndexBuffer(ref ib, 2, 3, 0);
        AddIndexBuffer(ref ib, 4, 5, 1);
        AddIndexBuffer(ref ib, 1, 0, 4);
        AddIndexBuffer(ref ib, 1, 6, 7);
        AddIndexBuffer(ref ib, 7, 2, 1);
        AddIndexBuffer(ref ib, 3, 2, 8);
        AddIndexBuffer(ref ib, 8, 9, 3);
        AddIndexBuffer(ref ib, 11, 0, 3);
        AddIndexBuffer(ref ib, 3, 10, 11);
    }
    void FillIndex_Zone(ref List<int> ib, int startIndex, int i0, int i1, int i2)
    {
        AddIndexBuffer( ref ib, i1, startIndex, i0);

        for (int i = 0; i < cornersize - 1; i++)
            AddIndexBuffer(ref ib, startIndex + i, startIndex + i + 1, i0);

        AddIndexBuffer(ref ib, startIndex + cornersize - 1, i2, i0);
    }

    void AddIndexBuffer(ref List<int> ib, Vector3Int tri)
    {
        ib.Add(tri.x);
        ib.Add(tri.y);
        ib.Add(tri.z);
    }

    void AddIndexBuffer(ref List<int> ib, int x, int y, int z)
    {
        ib.Add(x);
        ib.Add(y);
        ib.Add(z);
    }

    int imageIndex = -1;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 40, 100, 20), "Texture"))
        {
            imageIndex++;
            if(imageIndex == imageAddress.Count)
            {
                imageIndex = 0;
            }
            StartCoroutine(ImageChange(imageAddress[imageIndex]));
        }


        if (GUI.Button(new Rect(20, 70, 100, 20), "Reset"))
        {
            LoadConfig();
        }
    }

    IEnumerator ImageChange(string address)
    {
        UnityWebRequest www = UnityWebRequest.Get(address);
        var downloadTexture = new DownloadHandlerTexture(true);
        www.downloadHandler = downloadTexture;
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error + "," + address);
        }
        else
        {
            Texture2D texture2d = DownloadHandlerTexture.GetContent(www);

            Material material = new Material(mat);



            // 修改第一个材质的颜色
            material.SetTexture("_MainTex", texture2d);

            MeshRenderer meshRenderer = MyGameObject.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.material = material;
            }

        }


    }
}
