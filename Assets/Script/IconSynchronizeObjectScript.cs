using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IconSynchronizeObjectScript : MonoBehaviour {

    // temporary location
    private Vector3 position;
    // screen cord to world coord
    private Vector3 screenToWorldPointPosition;
    // the Icon Object
    public RectTransform icon;

    // get the list of track-points
    private List<Vector3> vlist = new List<Vector3>();
    // line process width
    public float width;
    // mesh
    private Mesh mesh;
    // target
    private GameObject targetObject;

    private void CreateMesh(Mesh mesh, List<Vector3> vlist)
    {
        mesh.Clear();

        var vCnt = vlist.Count;
        var vertices = new List<Vector3>();
        for (int i=0; i < vCnt-1; i++)
        {
            var currentPos = vlist[i];
            var nextPos = vlist[i + 1];
            var vec = currentPos - nextPos;//今と、一つ先のベクトルから、進行方向を得る
            if (vec.magnitude < 0.01f) continue;  //あまり頂点の間が空いてないのであればスキップする
            var v = new Vector3(-vec.y, vec.x, 1f).normalized * width;

            vertices.Add(currentPos - v);
            vertices.Add(currentPos + v);
        }

        var indices = new List<int>();
        for (int index = 0; index < vertices.Count - 2; index += 2)
        {
            indices.Add(index);
            indices.Add(index + 2);
            indices.Add(index + 3);
            indices.Add(index + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Quads, 0);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            targetObject = new GameObject("MeshObject");
            var meshRenderer = targetObject.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
            meshRenderer.sharedMaterial = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
#else
            meshRenderer.sharedMaterial = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
#endif
            var meshFilter = targetObject.AddComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;
            vlist.Clear();
        }
        if (Input.GetMouseButton(0) && targetObject != null)
        {
            // get the icon coord with Vector3
            position = icon.position;
            // repair z-axis
            position.z = 8f;
            // convert icon coord to world coord
            screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
            // add the 3d-position to the list
            vlist.Add(screenToWorldPointPosition);

            if (Physics2D.CircleCast(screenToWorldPointPosition, 0.2f, Vector2.zero))
            {
                Finish();
                return;
            }

            vlist.Add(screenToWorldPointPosition);
            CreateMesh(mesh, vlist);
        }
        if (Input.GetMouseButtonUp(0) && targetObject != null)
        {
            Finish();
        }
    }

    private void Finish()
    {
        if (mesh.vertexCount < 4)
        {
            Destroy(targetObject);
            return;
        }
        var rigibody = targetObject.AddComponent<Rigidbody>();
        var polyColliderPos = CreateMeshToPolyCollider(mesh);
        var polyCollider = targetObject.AddComponent<PolygonCollider2D>();
        polyCollider.SetPath(0, polyColliderPos.ToArray());
        targetObject = null;
    }

    private List<Vector2> CreateMeshToPolyCollider(Mesh mesh)
    {
        var polyColliderPos = new List<Vector2>();
        //偶数を小さい順に
        for (int index = 0; index < mesh.vertices.Length; index += 2)
        {
            var pos = mesh.vertices[index];
            polyColliderPos.Add(pos);
        }
        //奇数を大きい順に
        for (int index = mesh.vertices.Length - 1; index > 0; index -= 2)
        {
            var pos = mesh.vertices[index];
            polyColliderPos.Add(pos);
        }
        return polyColliderPos;
    }

}
