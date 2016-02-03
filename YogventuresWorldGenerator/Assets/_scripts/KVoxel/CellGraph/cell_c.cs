using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BenTools.Mathematics;
using LibNoise.Unity.Generator;

using KVoxel;

public class cell_c : MonoBehaviour
{
    private Cell cell;

    public Cell Cell
    {
        get { return cell; }
        set
        {
            cell = value;
            Initialize();
        }
    }

    public double Max;
    public bool Visible = false;
    private Vector3 Site; //Delauny Points (sites)
    private List<Vector3> BluePoints; // Voronoi Graph Vertex(s)
    private Vector3[] GreenPoints; // Polygon Ordered Edges
    private List<GameObject> myLines = new List<GameObject>();
    public MeshRenderer rMesh;
    public MeshFilter fMesh;
    public Mesh mesh;
    public MeshCollider cMesh;
    public Material cell_color_material;
    public Material cell_decoration_material;
    private List<Vector3> meshVerts = new List<Vector3>();
    private List<Vector3> meshNormals = new List<Vector3>();
    private List<Vector2> meshUvs = new List<Vector2>();
    private List<int> meshTris = new List<int>();

    // Use this for initialization
    void Awake()
    {
        rMesh = gameObject.GetComponent<MeshRenderer>();
        fMesh = gameObject.GetComponent<MeshFilter>();
        cMesh = gameObject.GetComponent<MeshCollider>();
        mesh = new Mesh();
    }

    void Initialize()
    {
        Reset();
    }

    public void Reset()
    {
        meshVerts.Clear();
        meshNormals.Clear();
        meshUvs.Clear();
        meshTris.Clear();
        RebuildCell();
    }

    void RebuildCell()
    {
        //Debug.Log ("Cell: " + cell.site);
        foreach (VoronoiEdge edge in cell.voronoiEdges)
        {

            //Debug.Log ("edge: " + edge.VVertexA.AsVector3() + ":" + edge.VVertexB.AsVector3());
            Vector3 a = edge.VVertexA.AsVector3();
            a.x = Mathf.Clamp(a.x, 0, (float)Max);
            a.z = Mathf.Clamp(a.z, 0, (float)Max);

            Vector3 b = edge.VVertexB.AsVector3();
            b.x = Mathf.Clamp(b.x, 0, (float)Max);
            b.z = Mathf.Clamp(b.z, 0, (float)Max);

            Vector3 c = cell.site.AsVector3();

            // depending on the vert locations compared to the center of the polygon we wind differntly.
            Vector3 AToB = b - a;
            Vector3 BToC = c - a;
            Vector3 crossProduct = Vector3.Cross(AToB, BToC).normalized;

            if (crossProduct.y > 0.0f)
            {
                // clockwise
                meshVerts.Add(a);
                meshVerts.Add(b);
                meshVerts.Add(c);
            }
            else {
                // counter-clockwise.  Need to reverse the order of our vertices.
                meshVerts.Add(a);
                meshVerts.Add(c);
                meshVerts.Add(b);

            }

        }
        int i = 0;
        for (i = 0; i < meshVerts.Count; i++)
        {
            meshNormals.Add(Vector3.up);
            meshUvs.Add(Vector2.zero);
            meshTris.Add(i);
        }
        RedrawCell();
    }

    void RedrawCell()
    {
        mesh = null;
        mesh = new Mesh();

        mesh.vertices = meshVerts.ToArray();
        mesh.normals = meshNormals.ToArray();
        mesh.uv = meshUvs.ToArray();
        mesh.triangles = meshTris.ToArray();

        UpdateMaterial();

        fMesh.sharedMesh = null;
        fMesh.sharedMesh = mesh;

        RebuildCollider();
    }

    void RebuildCollider()
    {
        cMesh.sharedMesh = null;
        cMesh.sharedMesh = mesh;
    }

    void OnTriggerEnter(Collider other)
    {
        paintbrush pBrush = other.gameObject.GetComponent<paintbrush>();
        if (pBrush != null)
        {
            if (pBrush.IsPainting)
            {
                if (Cell.biome != pBrush.CurrentBiome)
                {
                    SetCellBiome(pBrush.CurrentBiome, true);
                }
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        paintbrush pBrush = other.gameObject.GetComponent<paintbrush>();
        if (pBrush != null)
        {
            if (pBrush.IsPainting)
            {
                if (Cell.biome != pBrush.CurrentBiome)
                {
                    SetCellBiome(pBrush.CurrentBiome, true);
                }
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        paintbrush pBrush = other.gameObject.GetComponent<paintbrush>();
        if (pBrush != null)
        {
            if (pBrush.IsPainting)
            {
                if (Cell.biome != pBrush.CurrentBiome)
                {
                    SetCellBiome(pBrush.CurrentBiome, true);
                }
            }
        }
    }

    public void SetCellBiome(eBiome b, bool IsOverride)
    {
        Cell.BiomeOverride = IsOverride;
        Cell.biome = b;
        SetCellMaterial(false, BiomeHelper.GetMaterialFromBiome(b));
    }

    /// <summary>
    /// Helper method to let the map push color into the cell
    /// </summary>
    /// <param name="decoration">Change the Base or the Decoration Material <c>true</c> decoration.</param>
    public void SetCellMaterial(bool decoration, Material newMaterial)
    {
        if (decoration)
        {
            cell_decoration_material = newMaterial;
        }
        else {
            cell_color_material = newMaterial;
        }
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        gameObject.GetComponent<Renderer>().material = cell_color_material;
    }

    public void DestroyYouself()
    {
        GameObject.DestroyImmediate(gameObject);
    }

}
