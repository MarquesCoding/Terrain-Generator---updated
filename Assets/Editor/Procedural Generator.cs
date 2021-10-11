using UnityEngine;
using UnityEditor;

public class ProceduralGenerator : EditorWindow
{

    //All the local and global variables that are used throughout the project.
    Mesh mesh; //Initializes a mesh inside the project.
    Material m_material; //Initializes a material for the mesh.
    Vector3[] vertices; // Creates an 3D vector that controls the vertices in an array.
    int[] triangles; // Creates an array for the triangles for the mesh.
    string debug = ""; // This is used for debugging if there is an error when creating the terrain.
    bool showdebug = false; // This is a local variable used for displaying any errors.
    int selection = 0; // This is used for local variables for the current selection 'type' of biome.
    int xSize; // This is used for the width of the generated mesh.
    int zSize; //This is used for the depth of the generated mesh.
    float height;
    bool useselect;
    float select; // This is used for the selection for the generation of the mesh.
    string[] biome = new string[3] { "Grasslands", "Desert", "Water" }; // This is a local variable (array) used for the listing of 'types' of biomes for the mesh.

    [MenuItem("Window/Generation")]
    public static void ShowWindow() // This is for the initialization of the editor for the user.
    {
        GetWindow<ProceduralGenerator>("Procedural Generator"); // Gives it a title.
    }
    private void OnGUI() // This is used for all the GUI elements within the EditorWindow. (Technologies, 2020)
    {
        GUILayout.Label("Terrain", EditorStyles.boldLabel); //This is used to create labels within the editor.



        selection = EditorGUILayout.Popup("Biome", selection, biome); // This is used to create a dropdown menu for the user to select their biome.
        xSize = EditorGUILayout.IntField("Width", xSize); // This is used for a slider for the selection of the width of the mesh.
        zSize = EditorGUILayout.IntField("Depth", zSize); // This is used for a slider for the selection of the depth of the mesh.
        GUILayout.Label("Additions", EditorStyles.boldLabel); // This is used to create labels within the editor.
        useselect = EditorGUILayout.Toggle("Use Custom Height?", useselect);
        height = EditorGUILayout.Slider(height, 1, 3); // This is used for a slider for the selection of the depth of the mesh.
        if (showdebug)
        {
            GUILayout.Label("Error:" + " " + debug); // This is used for displaying any error messages.
        }
        if (GUILayout.Button("Generate"))
        { // This is used for the button that is the main create function of the mesh.
            Generater(); // References and calls for the 'Generater' function.
        }
    }
    void Generater()
    { //After the user presses the 'generate' button this function is called.

        if (zSize < 0 || xSize < 0)
        { //Checks to see if the user hasn't inputted any information about their sizing.
            showdebug = true; //Sets the error text to visible.
            debug = "You cannot render a terrain smaller than 0."; // Sets the error text to 'You cannot render a terrain smaller than 0.'
        }
        else
        {
            if (selection == 0)
            { //This is used for the biome selection 'Grasslands'.
                select = .3f;  //This is used for the amount of noise for the perlin generator.
            }
            if (selection == 1)
            { //This is used for the biome selection 'Desert'.
                select = .1f; //This is used for the amount of noise for the perlin generator.
            }
            if (selection == 2)
            { //This is used for the biome selection 'Water'.
                select = .05f; //This is used for the amount of noise for the perlin generator.
            }
            showdebug = false; //Sets the error text to not visible.
            mesh = new Mesh(); //Creates a new mesh and assigns it to 'mesh'.
            var terrain_mesh = new GameObject("Terrain"); // Creates a new variable 'terrain_mesh' and applies a new gameobject called 'terrain'.
            terrain_mesh.AddComponent<MeshFilter>(); // Adds the component 'MeshFilter' to the new terrain.
            terrain_mesh.AddComponent<MeshRenderer>(); // Adds the component 'MeshRenderer' to the new terrain.
            m_material = terrain_mesh.GetComponent<Renderer>().material; // Sets the material from the new generated material to the new terrains material.
            terrain_mesh.GetComponent<MeshFilter>().mesh = mesh; // Get's the new terrains mesh and then assigns it to the generated mesh.
            createShape(); //Calls the function 'CreateShape'.
            UpdateMesh(); //Calls the function 'UpdateMesh'.
        }


    }

    void createShape() // This is used for the function of creating the initial shape and the terrain generation.
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; // This is setting the verticies to a new vector 3 from the size that the user is inputting.
        int i = 0; // Creates a local int called i.
        for (int z = 0; z <= zSize; z++) // (Terrain Generator 2020. from line 88 to 140)
        {
            for (int x = 0; x <= xSize; x++)
            {
                if (useselect)
                {
                    float y = Mathf.PerlinNoise(x * height, z * height) * 2f;
                    vertices[i] = new Vector3(x, y, z);
                    i++;
                }
                else
                {
                    float y = Mathf.PerlinNoise(x * select, z * select) * 2f;
                    vertices[i] = new Vector3(x, y, z);
                    i++;
                }

            }
        }
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }


    }
    void UpdateMesh()
    {

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


}


//References that were used:

//Terrain Generator 2020. [online] Available at: <https://www.youtube.com/watch?v=64NblGkAabk> [Accessed 24 April 2020].
//Technologies, U., 2020. Unity - Manual: Editor Windows. [online] Docs.unity3d.com. Available at: <https://docs.unity3d.com/Manual/editor-EditorWindows.html> [Accessed 24 April 2020].
//
//Technologies, U.
//Technologies, U. (2020) Unity - Manual: Mesh Renderer, Docs.unity3d.com.Available at: https://docs.unity3d.com/Manual/class-MeshRenderer.html (Accessed: 7 May 2020).
//
//