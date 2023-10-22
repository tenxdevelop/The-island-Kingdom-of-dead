using UnityEngine;

namespace TheIslandKOD
{
    public class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        public int lod;
        
        private MapGenerator m_mapGenerator;
        public LODMesh(int lod, MapGenerator mapGeerator)
        {
            this.lod = lod;
            
            m_mapGenerator = mapGeerator;
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            m_mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
        }

        private void OnMeshDataRecieved(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
            
        }

    }
}
