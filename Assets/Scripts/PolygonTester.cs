using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonTester : MonoBehaviour
{
    
    public List<GameObject> usedTrailSegments = new List<GameObject>();
    public List<Vector2> newVertices = new List<Vector2>();
    Vector2[] newUv;
    int[] newTriangles;
    private Vector2 chosenSegment;

    //public void GatherNewVertices()
     void GatherNewVertices()
    {

        //Use the list of vector 2 newVertices here
        //FIGURE OUT HOW TO CONVERT THESE VECTOR 2 ARRAY POINTS INTO THE POINTS GIVEN IN THE CONVERTED newVertices LIST AND FIGURE OUT HOW TO USE THE NUMBERS IN BRACKETS WHEN THE POINTS WILL ALWAYS BE DIFFERENT
        //TRY FILTERING THROUGH THE LIST IN ORDER AND CREATING THE VECTOR2S BASED OF EACH INDIVIDUAL SEGMENT POSITION
        //FIGURE OUT HOW TO HAVE IT KNOW HOW MANY VECTOR2'S TO CREATE BASED OFF THE AMNOUNT INSIDE THE SEGMENT LIST
        //USE THE METHOD THAT CALCULATES HOW MANY SEGMENTS ARE LEFT AFTER THE ADJUSTMENT IN PLAYER SCRIPT TO DEFINE THE AMOUNT OF VECTOR2S MADE IN THIS ARRAY

        // Create Vector2 vertices

        //Vector2[] vertices2D = newVertices.ToArray();
        //Vector2[] vertices2D = new Vector2[]
        //{
        //    new Vector2(0, 0),
        //    new Vector2(0, 50),
        //    new Vector2(50, 50),
        //    new Vector2(50, 100),
        //    new Vector2(0, 100),
        //    new Vector2(0, 150),
        //    new Vector2(150, 150),
        //    new Vector2(150, 100),
        //    new Vector2(100, 100),
        //    new Vector2(100, 50),
        //    new Vector2(150, 50),
        //    new Vector2(150, 0),
        //};

         Vector2[] vertices2D = newVertices.ToArray();




        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
        //Debug.Log(vertices2D.Length);
        //Debug.Log(indices.Length);


        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        //Set up game object with mesh;
        //gameObject.AddComponent(typeof(MeshRenderer));
        MeshFilter filter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;
    }
    public void UpdateVerticesList()
    {
        //Fill vertices array with positions of the used trail gameobjects - DONE
        foreach (GameObject usedSegements in usedTrailSegments)
        {
            chosenSegment = new Vector2(usedSegements.transform.position.x, usedSegements.transform.position.z);
            newVertices.Add(chosenSegment);
        }
        GatherNewVertices();
    }
}