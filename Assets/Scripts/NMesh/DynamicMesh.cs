using Assets.Scripts.NWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NMesh
{
    [System.Serializable]
    public class DynamicMesh
    {
        [SerializeField]
        public List<Vector3> _Vertices;    //vertex lsit
        [SerializeField]
        public List<int> _Indicies;        //index list
        [SerializeField]
        public List<Vector2> _Texture;     //texture list

        //constructor
        public DynamicMesh()
        {
            Reset();
        }

        //Add quad Mesh
        public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            //add quad indicies
            int size = _Vertices.Count;

            _Indicies.Add(size + 0);
            _Indicies.Add(size + 1);
            _Indicies.Add(size + 2);
            _Indicies.Add(size + 0);
            _Indicies.Add(size + 2);
            _Indicies.Add(size + 3);

            //add quad verties
            _Vertices.Add(v1);
            _Vertices.Add(v2);
            _Vertices.Add(v3);
            _Vertices.Add(v4);

            //add Texture

            _Texture.Add(new Vector2(0, 0));
            _Texture.Add(new Vector2(0, 1));
            _Texture.Add(new Vector2(1, 1));
            _Texture.Add(new Vector2(1, 0));
        }

        //put mesh data to unity mesh filter
        public void ToMeshFilter(MeshFilter Filter)
        {
            Filter.mesh.vertices = _Vertices.ToArray();
            Filter.mesh.triangles = _Indicies.ToArray();
            Filter.mesh.uv = _Texture.ToArray();
            Filter.mesh.RecalculateNormals();
        }

        public Mesh GenerateUnityMesh()
        {
            Mesh UnityMesh = new Mesh();

            UnityMesh.vertices = _Vertices.ToArray();
            UnityMesh.triangles = _Indicies.ToArray();

            UnityMesh.uv = _Texture.ToArray();
            UnityMesh.RecalculateNormals();

            return UnityMesh;
        }

        public IEnumerator SaveToUnityMesh_Coro(Mesh mesh)
        {
            mesh.vertices = _Vertices.ToArray();
            yield return null;

            mesh.triangles = _Indicies.ToArray();
            yield return null;

            mesh.uv = _Texture.ToArray();
            yield return null;

            mesh.RecalculateNormals();
        }



        //Combine with other mesh
        public void Add(DynamicMesh ohterMesh)
        {
            if (ohterMesh == null) return;
            int size = _Vertices.Count;

            _Vertices.AddRange(ohterMesh._Vertices);
            _Texture.AddRange(ohterMesh._Texture);

            //add Indicies
            _Indicies.Capacity = _Indicies.Count + ohterMesh._Indicies.Count;
            foreach (int i in ohterMesh._Indicies)
            {
                _Indicies.Add(i + size);
            }
        }

        public void Translate(Vector3 translation)
        {
            for (int i = 0; i < _Vertices.Count; ++i)
            {
                _Vertices[i] += translation;
            }
        }
        public void Rotate(Vector3 eular)
        {
            for (int i = 0; i < _Vertices.Count; ++i)
            {
                _Vertices[i] = Quaternion.Euler(eular) * _Vertices[i];
            }
        }
        public void Rotate(float angle, Vector3 axis)
        {
            for (int i = 0; i < _Vertices.Count; ++i)
            {
                _Vertices[i] = Quaternion.AngleAxis(angle, axis) * _Vertices[i];
            }
        }

        //set Texture data
        public void SetUV_quad(Frame frame)
        {
            for (int i = 0; i < _Texture.Count; i += 4)
            {
                _Texture[i] = new Vector2(frame.left, frame.bottom);
                _Texture[i + 1] = new Vector2(frame.left, frame.top);
                _Texture[i + 2] = new Vector2(frame.right, frame.top);
                _Texture[i + 3] = new Vector2(frame.right, frame.bottom);
            }
        }

        public void ReverseFace()
        {
            int temp;
            for (int i = 0; i < _Indicies.Count; i += 3)
            {
                temp = _Indicies[i + 1];
                _Indicies[i + 1] = _Indicies[i + 2];
                _Indicies[i + 2] = temp;
            }
        }

        public DynamicMesh Clone()
        {
            DynamicMesh clone = new DynamicMesh();
            clone._Vertices = new List<Vector3>(_Vertices);
            clone._Indicies = new List<int>(_Indicies);
            clone._Texture = new List<Vector2>(_Texture);
            
            return clone;
        }

        public void Reset()
        {
            _Vertices = new List<Vector3>();
            _Indicies = new List<int>();
            _Texture = new List<Vector2>();
        }

        public void Clear()
        {
            _Vertices.Clear();
            _Indicies.Clear();
            _Texture.Clear();
        }
    }
}