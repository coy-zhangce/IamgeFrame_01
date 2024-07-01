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
        // ����һ���µ��������
        mesh = new Mesh();

        // ��������Ķ���
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 10),
            new Vector3(10, 0, 10),
            new Vector3(10, 0, 0)
        };

        mesh.vertices = vertices;

        // ��������������Σ�ÿ��������������������������ɣ�
        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 3, 0
        };

        mesh.triangles = triangles;

        // ��������ķ��ߣ���ѡ����ͨ�����ڹ��ռ��㣩
        Vector3[] normals = new Vector3[]
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        mesh.normals = normals;

        // ���������UV���꣨��ѡ����ͨ����������ӳ�䣩
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        mesh.uv = uvs;

        // ����һ������������������������������
        meshFilter = GetComponent<MeshFilter>();
        if (!meshFilter)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshFilter.mesh = mesh;

        // ����һ��������Ⱦ��������Ա��ڳ�������ʾ����
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer.material = mat;
    }
}
