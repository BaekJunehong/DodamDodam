using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class curve : MonoBehaviour
{
    public GameObject objectToSlice; // 슬라이스할 오브젝트
    public GameObject scissors;
    private float moveSpeed = 0.05f;

    private GameObject positiveSideObject;
    private GameObject negativeSideObject;

    private Mesh Positive;
    private Mesh Nagetive;


    private Vector3 CurrentPos;
    private Vector3 PreviousPos;
    private bool isPreviousPosSet = false;
    private float timeCount = 0f;

    private SlicesMetadata slicer;
    private Vector3[] positiveVertices;
    private int[] positiveTriangles;
    private Vector2[] positiveUVs;
    private Vector3[] positiveNormals;

    
    private Vector3[] negativeVertices;
    private int[] negativeTriangles;
    private Vector2[] negativeUVs;
    private Vector3[] negativeNormals;

    private List<int> PositiveIndex = new List<int>();
    private List<int> NegativeIndex = new List<int>();



    // Update is called once per frame
    void Start() {
        Positive = new Mesh();
        Nagetive = new Mesh();
        Switching();
    }
    void Update()
    {
        UpdatePosition();
        TryMoveVertices();
        cuttingComplete();
    }
    private void UpdatePosition() {
        CurrentPos = scissors.transform.position;
        if (!isPreviousPosSet) {
            PreviousPos = CurrentPos;
            isPreviousPosSet = true;
        }
    }
    private void TryMoveVertices() {
        
        Vector3 movementDirection = (CurrentPos - PreviousPos).normalized;
        MoveVertices(movementDirection);
        PreviousPos = CurrentPos;
    }
    private void cuttingComplete(){
        if(CurrentPos.x <= -8 && timeCount < 0.1){
            Vector3 positiveTargetPosition = positiveSideObject.transform.position + Vector3.up * 10f * Time.deltaTime; // 예를 들어, 5 유닛만큼 위로
            Vector3 negativeTargetPosition = negativeSideObject.transform.position + Vector3.down * 10f * Time.deltaTime; // 예를 들어, 5 유닛만큼 아래로
            positiveSideObject.transform.position = positiveTargetPosition;
            negativeSideObject.transform.position = negativeTargetPosition;
            timeCount += Time.deltaTime;
        }
    }



    private void SliceObject(Vector3 planeNormal, Vector3 planePoint)
    {
        if (objectToSlice != null)
        {
            Mesh mesh = objectToSlice.GetComponent<MeshFilter>().mesh;
            Plane slicingPlane = new Plane(planeNormal, planePoint);

            slicer = new SlicesMetadata(slicingPlane, mesh, true);

            positiveVertices = slicer.PositiveSideMesh.vertices;
            positiveTriangles = slicer.PositiveSideMesh.triangles;
            positiveUVs = slicer.PositiveSideMesh.uv;
            positiveNormals = slicer.PositiveSideMesh.normals;
            negativeVertices = slicer.NegativeSideMesh.vertices;
            negativeTriangles = slicer.NegativeSideMesh.triangles;
            negativeUVs = slicer.NegativeSideMesh.uv;
            negativeNormals = slicer.NegativeSideMesh.normals;

            objectToSlice.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void SaveIndex(Vector3 A, Vector3 B){
        PositiveIndex = FindIndexOnLine(positiveVertices, A, B);
        NegativeIndex = FindIndexOnLine(negativeVertices, A, B);
    }

    private void MoveVertices(Vector3 direction){
            foreach (int i in PositiveIndex)
            {
                if(checkDistance(positiveSideObject, i, true)){
                    positiveVertices[i] += Vector3.up * moveSpeed;
                    positiveUVs[i] += new Vector2(0, 1) * moveSpeed;
                }
                
            }
            foreach (int i in NegativeIndex)
            {
                if(checkDistance(negativeSideObject, i, false)){
                    negativeVertices[i] += Vector3.down * moveSpeed;
                    negativeUVs[i] += new Vector2(0, -1) * moveSpeed;
                }
            }

            SetMesh(Positive, positiveVertices, positiveTriangles, positiveUVs, positiveNormals);
            SetMesh(Nagetive, negativeVertices, negativeTriangles, negativeUVs, negativeNormals);
            
            slicer.PositiveSideMesh = Positive;
            slicer.NegativeSideMesh = Nagetive;

            UpdateMesh(positiveSideObject, true);
            UpdateMesh(negativeSideObject, false);
        }

    private bool checkDistance(GameObject Object, int i, bool flag){
        Matrix4x4 localToWorld = Object.transform.localToWorldMatrix;
        Vector3 world;
        if(flag == true){world = localToWorld.MultiplyPoint3x4(positiveVertices[i]);}
        else{world = localToWorld.MultiplyPoint3x4(negativeVertices[i]);}
        float distance = Vector3.Distance(scissors.transform.position, world);
        if(distance<0.1f){
            return true;
        }
        else{
            return false;
        }
    }

    private void RePosition(){
            positiveSideObject= new GameObject("PositiveSide");
            positiveSideObject.AddComponent<MeshFilter>().mesh = slicer.PositiveSideMesh;
            positiveSideObject.AddComponent<MeshRenderer>().materials = objectToSlice.GetComponent<MeshRenderer>().materials;
            positiveSideObject.transform.position = objectToSlice.transform.position;
            positiveSideObject.transform.localScale = gameObject.transform.localScale;
            
            negativeSideObject = new GameObject("NegativeSide");
            negativeSideObject.AddComponent<MeshFilter>().mesh = slicer.NegativeSideMesh;
            negativeSideObject.AddComponent<MeshRenderer>().materials = objectToSlice.GetComponent<MeshRenderer>().materials;
            negativeSideObject.transform.position = objectToSlice.transform.position;
            negativeSideObject.transform.localScale = gameObject.transform.localScale;
    }

    private void UpdateMesh(GameObject objectToUpdate, bool isPositiveSide)
    {
        MeshFilter meshFilter = objectToUpdate.GetComponent<MeshFilter>();

        if (meshFilter != null) // MeshFilter 컴포넌트가 존재하면
        {
        // isPositiveSide 플래그에 따라 적절한 메시를 선택합니다.
            Mesh newMesh = isPositiveSide ? slicer.PositiveSideMesh : slicer.NegativeSideMesh;

        // MeshFilter의 메시를 새로운 메시로 설정합니다.
            meshFilter.mesh = newMesh;
        }
        else // MeshFilter 컴포넌트가 존재하지 않는 경우 (선택적으로 처리)
        {
        // MeshFilter 컴포넌트가 없으면 추가하고 메시 설정
            meshFilter = objectToUpdate.AddComponent<MeshFilter>();
            meshFilter.mesh = isPositiveSide ? slicer.PositiveSideMesh : slicer.NegativeSideMesh;
        }
    }

    // Mesh 데이터를 설정하는 함수
    public void SetMesh(Mesh mesh, Vector3[] vertices, int[] triangles, Vector2[] uvs, Vector3[] normals)
    {
        mesh.Clear();

        // 메쉬 구성 요소 설정
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;

        // 메쉬의 경계를 재계산
        mesh.RecalculateBounds();
    }

    private void Switching(){
        Vector3 slicePoint = Vector3.zero;
        Vector3 sliceDirection = Vector3.zero;
        Vector3 saveIndexA = Vector3.zero;
        Vector3 saveIndexB = Vector3.zero;
        switch(objectToSlice.name) {
            case "curve2":
            case "curve6":
                slicePoint = new Vector3(0, 0, 0);
                sliceDirection = new Vector3(-0.5f, 1, 0);
                saveIndexA = new Vector3(-1, -0.5f, 0);
                saveIndexB = new Vector3(1, 0.5f, 0);
                break;
            case "curve3":
            case "curve7":
                slicePoint = new Vector3(0, 0.5f, 0);
                sliceDirection = new Vector3(0, 1, 0);
                saveIndexA = new Vector3(-1, 0.5f, 0);
                saveIndexB = new Vector3(1, 0.5f, 0);
                break;
            case "curve1":
            case "curve5":
                slicePoint = new Vector3(0, -0.5f, 0);
                sliceDirection = new Vector3(0, 1, 0);
                saveIndexA = new Vector3(-1, -0.5f, 0);
                saveIndexB = new Vector3(1, -0.5f, 0);
                break;
            case "curve4":
                sliceDirection = new Vector3(0.25f, 1, 0);
                saveIndexA = new Vector3(-1, 0.25f, 0);
                saveIndexB = new Vector3(1, -0.25f, 0);
                break;
        }
        SliceObject(sliceDirection, slicePoint);
        SaveIndex(saveIndexA, saveIndexB);
        RePosition();
    }

    private static bool IsPointOnLine(Vector3 A, Vector3 B, Vector3 C){
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 crossProduct = Vector3.Cross(AB, AC);

        //직선 AB 위에 D가 있는가?
        return crossProduct == Vector3.zero;
    }
    private static List<int> FindIndexOnLine(Vector3[] vertices, Vector3 linePoint1, Vector3 linePoint2)
    {
        List<int> indices = new List<int>();
        if(vertices != null){
            for (int i = 0; i < vertices.Length; i++)
            {
                if (IsPointOnLine(linePoint1, linePoint2, vertices[i]))
                {
                    indices.Add(i);
                }
            }
            return indices;
        }
        return indices;
    }
    
    
}
