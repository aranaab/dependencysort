// See https://aka.ms/new-console-template for more information

/* Create a C# program that solves the following dependency problem:
 
A person needs to figure out which order his/her clothes need to be put on. 
The person creates a file that contains the dependencies.
 
This input is a declared array of dependencies with the [0] index being the dependency and the [1] index being the item. 
 
A simple input would be:
 
                var input = new string[,]
                               {
                                                                //dependency    //item
                                               {"t-shirt",             "dress shirt"},
                                                        {"dress shirt", "pants"},
                                                        {"dress shirt", "suit jacket"},
                                                        {"tie",                           "suit jacket"},
                                                        {"pants",     "suit jacket"},
                                                        {"belt",         "suit jacket"},
                                                        {"suit jacket", "overcoat"},
                                                        {"dress shirt", "tie"},
                                                        {"suit jacket", "sun glasses"},
                                                        {"sun glasses", "overcoat"},
                                                        {"left sock",                "pants"},
                                                        {"pants",     "belt"},
                                                        {"suit jacket", "left shoe"},
                                                        {"suit jacket", "right shoe"},
                                                        {"left shoe",               "overcoat"},
                                                        {"right sock",             "pants"},
                                                        {"right shoe",            "overcoat"},
                                                        {"t-shirt",    "suit jacket"}
                                       };
 
In this example, it shows that they must put on their left sock before their pants. Also, 
they must put on their pants before their belt.
 
From this data, write a program that provides the order that each object needs to be put on.
 
The output should be a line-delimited list of objects. If there are multiple objects that
can be done at the same time, list each object on the same line, alphabetically 
sorted, comma separated.
 
Therefore, the output for this sample file would be:
 
left sock,right sock, t-shirt
dress shirt
pants, tie
belt
suit jacket
left shoe, right shoe, sun glasses
overcoat
 
Evaluation Criteria
 
You will be evaluated on the following criteria:
 
1.            Correctness of the solution
2.            Algorithmic, logic, and programming skills
3.            Performance considerations
4.            Design and code structure (modular, etc)
5.            Coding style
6.            Usability
7.            Testability
8.            Documentation
 */

using System;
using System.Reflection.Metadata.Ecma335;
        
        namespace DependencySort
        {
    class Dependency {         
        

        static void Main(string[] args)
        {
                string[,] input = new string[,] 
                { 
                    {"t-shirt","dress shirt"},
                    {"dress shirt", "pants"},
                    {"dress shirt", "suit jacket"},
                    {"tie", "suit jacket"},
                    {"pants","suit jacket"},
                    {"belt", "suit jacket"},
                    {"suit jacket", "overcoat"},
                    {"dress shirt", "tie"},
                    {"suit jacket", "sun glasses"},
                    {"sun glasses", "overcoat"},
                    {"left sock", "pants"},
                    {"pants", "belt"},
                    {"suit jacket", "left shoe"},
                    {"suit jacket", "right shoe"},
                    {"left shoe", "overcoat"},
                    {"right sock", "pants"},
                    {"right shoe", "overcoat"},
                    {"t-shirt", "suit jacket"}
};


{

};


            if (input.Length == 0)
               {
               Console.WriteLine("Array Is Empty"); 
               return; //stop code execution if array is empty
               }
               else
               {
                //Console.WriteLine("Array (0) has " + input.GetLength(0) + " elements");
                //Console.WriteLine("Array (1) has " + input.GetLength(1) + " elements");
               }

               //Make a new array with list of items
               string[] arrayItems = new string[18];
               //Make a new array with list of dependency items
               string[] arrayDependencies = new string[18];

               for (int i = 0; i < input.GetLength(0); i++)
               {
                arrayItems[i] = input[i,1];
                arrayDependencies[i] = input[i,0];
               }
               //remove duplicates from the list of items
               string[] listItems = arrayItems.Distinct().ToArray();
               string[] listDependencies = arrayDependencies.Distinct().ToArray();
               //Console.WriteLine("New list array has " + listItems.Length + " unique elements");
               //Console.WriteLine("New dependency array has " + listDependencies.Length + " unique elements");
               

                //traverse the list of items and add all dependencies

               List<Item> items = new List<Item>();
               var dependencyDictionary = new List<string>();

               
               for (int i = 0; i < listItems.Length; i++)
               {
                
                items.Add(new Item() { 
                    Name = listItems[i] 
                    //DependsOn = dependencyDictionary(i)
                    });

                for (int j = 0; j < input.GetLength(0); j++)
                {
                    //Console.WriteLine("Search item:" + listItems[i] + "/ Found: " + input[j,1] + "/ Dependency: " +  input[j,0] );
                    if (listItems[i] == input[j,1])
                    {
                    
                    dependencyDictionary.Add(input[j,0]);
                    //Console.WriteLine(" **** MATCH FOUND" + "Search item:" + listItems[i] + "/ Found: " + input[j,1] + "/ Dependency: " +  input[j,0] );
                    //Console.WriteLine(input[j,0]);
                    };
                        
                    }
                    items[i].DependsOn = dependencyDictionary.ToArray();
                    dependencyDictionary.Clear();
                
                }
                
               
//Get topological sort done

 int[] sortOrder = getTopologicalSortOrder(items);
 
        for (int i = sortOrder.Length -1 ; i >= 0; i--)
        {
            var field = items[sortOrder[i]];
            Console.WriteLine(field.Name);
            if (field.DependsOn != null)
                foreach (var item in field.DependsOn)
                {
                    Console.WriteLine(" -{0}", item);
                }
        }
            
        }
        
        private static int[] getTopologicalSortOrder(List<Item> fields)
    {
        TopologicalSorter g = new TopologicalSorter(fields.Count);
        Dictionary<string, int> _indexes = new Dictionary<string, int>();

        //add vertices
        for (int i = 0; i < fields.Count; i++)
        {
            _indexes[fields[i].Name.ToLower()] = g.AddVertex(i);
        }

        //add edges
        for (int i = 0; i < fields.Count; i++)
        {
            if (fields[i].DependsOn != null)
            {
                for (int j = 0; j < fields[i].DependsOn.Length; j++)
                {

                    try
                    {
                    g.AddEdge(i,
                        _indexes[fields[i].DependsOn[j].ToLower()]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        int[] result = g.Sort();
        return result;

    }
    }
     class Item
    {
        public string Name { get; set; }
        public string[] DependsOn { get; set; }
    }
    
    class TopologicalSorter
{
    #region - Private Members -

    private readonly int[] _vertices; // list of vertices
    private readonly int[,] _matrix; // adjacency matrix
    private int _numVerts; // current number of vertices
    private readonly int[] _sortedArray;

    #endregion

    #region - CTors -

    public TopologicalSorter(int size)
    {
        _vertices = new int[size];
        _matrix = new int[size, size];
        _numVerts = 0;
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                _matrix[i, j] = 0;
        _sortedArray = new int[size]; // sorted vert labels
    }

    #endregion

    #region - Public Methods -

    public int AddVertex(int vertex)
    {
        _vertices[_numVerts++] = vertex;
        return _numVerts - 1;
    }

    public void AddEdge(int start, int end)
    {
        _matrix[start, end] = 1;
    }

    public int[] Sort() // toplogical sort
    {
        while (_numVerts > 0) // while vertices remain,
        {
            // get a vertex with no successors, or -1
            int currentVertex = noSuccessors();
            if (currentVertex == -1) // must be a cycle                
                throw new Exception("Graph has cycles");

            // insert vertex label in sorted array (start at end)
            _sortedArray[_numVerts - 1] = _vertices[currentVertex];

            deleteVertex(currentVertex); // delete vertex
        }

        // vertices all gone; return sortedArray
        return _sortedArray;
    }

    #endregion

    #region - Private Helper Methods -

    // returns vert with no successors (or -1 if no such verts)
    private int noSuccessors()
    {
        for (int row = 0; row < _numVerts; row++)
        {
            bool isEdge = false; // edge from row to column in adjMat
            for (int col = 0; col < _numVerts; col++)
            {
                if (_matrix[row, col] > 0) // if edge to another,
                {
                    isEdge = true;
                    break; // this vertex has a successor try another
                }
            }
            if (!isEdge) // if no edges, has no successors
                return row;
        }
        return -1; // no
    }

    private void deleteVertex(int delVert)
    {
        // if not last vertex, delete from vertexList
        if (delVert != _numVerts - 1)
        {
            for (int j = delVert; j < _numVerts - 1; j++)
                _vertices[j] = _vertices[j + 1];

            for (int row = delVert; row < _numVerts - 1; row++)
                moveRowUp(row, _numVerts);

            for (int col = delVert; col < _numVerts - 1; col++)
                moveColLeft(col, _numVerts - 1);
        }
        _numVerts--; // one less vertex
    }

    private void moveRowUp(int row, int length)
    {
        for (int col = 0; col < length; col++)
            _matrix[row, col] = _matrix[row + 1, col];
    }

    private void moveColLeft(int col, int length)
    {
        for (int row = 0; row < length; row++)
            _matrix[row, col] = _matrix[row, col + 1];
    }

    #endregion
}
   
        }

           
           
                   





