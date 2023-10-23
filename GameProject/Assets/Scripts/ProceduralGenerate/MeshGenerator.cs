using UnityEngine;

namespace TheIslandKOD
{
    public static class MeshGenerator
    {

        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, 
                                                   AnimationCurve heightCurve, int levelOfDetail)
        {
            int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

            AnimationCurve animationCurve = new AnimationCurve(heightCurve.keys);
            int borderedSize = heightMap.GetLength(0);
            int meshSize = borderedSize - 2 * meshSimplificationIncrement;
            int meshSizeUnsimplified = borderedSize - 2;
            
            int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

            float topLeftX = (meshSizeUnsimplified - 1) / -2f;
            float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

            MeshData meshData = new MeshData(verticesPerLine);

            int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
            int meshVertexIndex = 0;
            int borderedVertexIndex = -1;

            for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
            {
                for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
                {
                    bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

                    if (isBorderVertex)
                    {
                        vertexIndicesMap[x, y] = borderedVertexIndex;
                        borderedVertexIndex--;
                    }
                    else
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }
                }
            }

            for (int y = 0; y < borderedSize; y+= meshSimplificationIncrement)
            {
                for (int x = 0; x < borderedSize; x+= meshSimplificationIncrement)
                {
                    int vertexIndex = vertexIndicesMap[x, y];

                    Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float)meshSize,
                                                  (y - meshSimplificationIncrement) / (float)meshSize);

                    float height = animationCurve.Evaluate(heightMap[x, y]) * heightMultiplier;

                    Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y * meshSizeUnsimplified);

                    meshData.AddVertex(vertexPosition, percent, vertexIndex);

                    if (x < borderedSize - 1 && y < borderedSize - 1)
                    {
                        int a = vertexIndicesMap[x, y];
                        int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
                        int c = vertexIndicesMap[x, y + meshSimplificationIncrement];
                        int d = vertexIndicesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];

                        meshData.AddTriangle(a, d, c);
                        meshData.AddTriangle(d, a, b); 
                    }

                    
                }
            }

            return meshData;
        }

    }

    public class MeshData
    {
        private Vector3[] m_vertices;
        private int[] m_triangles;
        private Vector2[] m_uvs;
        private Vector3[] m_borderVertices;
        private int[] m_borderTriangles;

        private int m_triangleIndex;
        private int m_borderTriangleIndex;
        public MeshData(int verticesPerLine)
        {
            m_vertices = new Vector3[verticesPerLine * verticesPerLine];
            m_uvs = new Vector2[verticesPerLine * verticesPerLine];
            m_triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

            m_borderVertices = new Vector3[verticesPerLine * 4 + 4];
            m_borderTriangles = new int[verticesPerLine * 24];
        }

        public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
        {

            if (vertexIndex < 0)
            {
                m_borderVertices[-vertexIndex - 1] = vertexPosition;
            }
            else
            {
                m_vertices[vertexIndex] = vertexPosition;
                m_uvs[vertexIndex] = uv;
            }
        }

        public void AddTriangle(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0)
            {
                m_borderTriangles[m_borderTriangleIndex] = x;
                m_borderTriangles[m_borderTriangleIndex + 1] = y;
                m_borderTriangles[m_borderTriangleIndex + 2] = z;

                m_borderTriangleIndex += 3;
            }
            else
            {
                m_triangles[m_triangleIndex] = x;
                m_triangles[m_triangleIndex + 1] = y;
                m_triangles[m_triangleIndex + 2] = z;

                m_triangleIndex += 3;
            }
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = m_vertices;
            mesh.triangles = m_triangles;
            mesh.uv = m_uvs;
            mesh.normals = CalculateNoramls();
            return mesh;
        }


        private Vector3[] CalculateNoramls()
        {
            Vector3[] vertexNormals = new Vector3[m_vertices.Length];
            int triangleCount = m_triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = m_triangles[normalTriangleIndex];
                int vertexIndexB = m_triangles[normalTriangleIndex + 1];
                int vertexIndexC = m_triangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += triangleNormal;
                vertexNormals[vertexIndexB] += triangleNormal;
                vertexNormals[vertexIndexC] += triangleNormal;
            }

            int borderTriangleCount = m_borderTriangles.Length / 3;
            for (int i = 0; i < borderTriangleCount; i++)
            { 
                int normalTriangleIndex = i * 3;
                int vertexIndexA = m_borderTriangles[normalTriangleIndex];
                int vertexIndexB = m_borderTriangles[normalTriangleIndex + 1];
                int vertexIndexC = m_borderTriangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                if (vertexIndexA >= 0)
                {
                    vertexNormals[vertexIndexA] += triangleNormal;
                }
                if (vertexIndexB >= 0)
                {
                    vertexNormals[vertexIndexB] += triangleNormal;
                }
                if (vertexIndexC >= 0)
                {
                    vertexNormals[vertexIndexC] += triangleNormal;
                }
            }

            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }

        private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            Vector3 pointA = (indexA < 0) ? m_borderVertices[-indexA - 1] : m_vertices[indexA];
            Vector3 pointB = (indexB < 0) ? m_borderVertices[-indexB - 1] : m_vertices[indexB];
            Vector3 pointC = (indexC < 0) ? m_borderVertices[-indexC - 1] : m_vertices[indexC];

            Vector3 sideAB = pointB - pointA;
            Vector3 sideAC = pointC - pointA;

            return Vector3.Cross(sideAB, sideAC).normalized;
        }

    }
}