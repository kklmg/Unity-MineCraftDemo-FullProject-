using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NWorld;
using Assets.Scripts.NNoise;


[Serializable]
public struct BlockLayer
{
    [SerializeField]
    public int Top;
    public int Bottom;

    [SerializeField]
    public Block blockType;
}

[Serializable]
public class LayerData
{
    [SerializeField]
    private List<BlockLayer> m_Layers;

    private Dictionary<int,Block> m_LayerTable;

    [SerializeField]
    private Block DefaultBlock;
    [SerializeField]
    private Block baseBlock;

    [SerializeField]
    private byte baseBlockAltitude;

    public void Init()
    {
        m_LayerTable = new Dictionary<int, Block>();
        foreach (var layer in m_Layers)
        {
            for (int i = layer.Top; i < layer.Bottom; ++i)
            {
                m_LayerTable.Add(i,layer.blockType);
            }
        }
    }

    public Block GetBlock(int CurAltitude,int TopAltitude)
    {
        if (CurAltitude > TopAltitude) return null;
        if (CurAltitude < baseBlockAltitude) return baseBlock;

        if (m_LayerTable.ContainsKey(TopAltitude - CurAltitude))
        {
            return m_LayerTable[TopAltitude - CurAltitude];
        }
        else return DefaultBlock;
    }
}


[CreateAssetMenu(menuName ="Biome")]
public class Biome : ScriptableObject
{
    [SerializeField]
    private LayerData m_LayerData;

    [SerializeField]
    private float m_Offset;

    [SerializeField]
    private float m_Frequency;

    [SerializeField]
    private int m_LowestAltitude;

    [SerializeField]
    private float m_HighestAltitude;
    
    public LayerData Layer { get { return m_LayerData; } }

    public void Init()
    {
        m_LayerData.Init();
    }

    public int GetHeight(Vector2Int vector2Int,INoiseMaker noiseMaker,int octave)
    {
        return (int)noiseMaker.MakeOctave_2D(vector2Int/*coord*/, m_Frequency
                , m_HighestAltitude - m_LowestAltitude, octave) + m_LowestAltitude;    //Height
    }
}

[System.Serializable]
public struct BiomeProportion
{
    [Range(0.0f,1.0f)]
    public float m_Min;
    [Range(0.0f, 1.0f)]
    public float m_Max;

    public Biome m_Biome;

    public BiomeProportion(float _min,float _max,Biome _biome)
    {
        m_Min = _min;
        m_Max = _max;
        m_Biome = _biome;
    }
}


