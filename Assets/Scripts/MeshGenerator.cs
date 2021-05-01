using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;


//[CustomEditor (typeof(MeshGenerator))]
public class MeshGenerator : MonoBehaviour
{
    public List<GameObject> usedTrailSegments = new List<GameObject>();
    //Vector3[] newVertices;
    public List<Vector2> newVertices = new List<Vector2>();
    Vector2[] newUv;
    int[] newTriangles;
    private Vector3 chosenSegment;


    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //this below needs to work, update use .ToArray to get it to work however this may cause issues in run time
        //mesh.vertices = newVertices.ToArray();
        mesh.uv = newUv;
        mesh.triangles = newTriangles;

    }

    //public static bool Traingulate(Vector2[] vertices, out int[] triangles, out string errorMessage)
    //{
    //    triangles = null;
    //    errorMessage = string.Empty;

    //    if (vertices is null)
    //    {
    //        errorMessage = "The vertix list is null.";
    //        return false;
    //    }

    //    if (vertices.Length < 3)
    //    {
    //        errorMessage = "The vertex list must have at least 3 vertices";
    //        return false;
    //    }

    //    if (vertices.Length > 500)
    //    {
    //        errorMessage = "The max vertex lenght is 500";
    //        return false;
    //    }

    //    //if (!MeshGenerator.IsSimplePolygon(vertices))
    //    //{
    //    //    errorMessage = "The vertex list does not define a simple polygon";
    //    //    return false;
    //    //}


    //}
    //public static bool IsSimplePolygon(Vector2[] vertices)
    //{
    //    print("NotImplementedException");
    //    //throw new NotImplementedException();
        
    //}


   

    // Update is called once per frame
    void Update()
    {
        //Fill vertices array with positions of the used trail gameobjects


        // use ear cutting algorithm to go through each point around the shape and create triangles using triangulisation comparing three points in a clockwise motion 
        //create a mesh based off the triangles that now form the inside of the shape
        //upon shape reset, re-assign the verticy array with the new collection of trail segments and repeat the whole process 
    }

    public void UpdateVerticesList()
    {
        //Fill vertices array with positions of the used trail gameobjects - DONE
        foreach (GameObject usedSegements in usedTrailSegments)
        {
            chosenSegment = usedSegements.transform.position;
            newVertices.Add(chosenSegment);
        }
        
    }
}
