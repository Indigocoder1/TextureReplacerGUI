using AssetsTools.NET;
using System;
using System.Collections.Generic;

[Obsolete("This is obsolete. Use MeshToOpenGL instead", true)]
public class Mesh
{
    public enum ChannelUsage
    {
        Vertex,
        Normal,
        Tangent,
        Color,
        TexCoord0,
        TexCoord1,
        TexCoord2,
        TexCoord3,
        TexCoord4,
        TexCoord5,
        TexCoord6,
        TexCoord7,
        BlendWeights,
        BlendIndices,
    };

    public enum ChannelType
    {
        Float,
        Float16,
        UNorm8,
        SNorm8,
        UNorm16,
        SNorm16,
        UInt8,
        SInt8,
        UInt16,
        SInt16,
        UInt32,
        SInt32,
    };

    public class Channel
    {
        public ChannelUsage Usage;
        public ChannelType Type;
        public int Dimension;
    }

    public string Name { get; init; }
    public uint StreamDataSize { get; init; }
    public int SubMeshes { get; init; }
    public int BlendShapes { get; init; }
    public int Bones { get; init; }
    public int Indices { get; init; }
    public uint Vertices { get; init; }
    public int Compression { get; init; }
    public bool RwEnabled { get; init; }

    public IReadOnlyList<Channel> Channels { get; init; }

    public int VertexSize { get; init; }

    private static readonly int[] s_ChannelTypeSizes =
    {
        4,  // Float
        2,  // Float16
        1,  // UNorm8
        1,  // SNorm8
        2,  // UNorm16
        2,  // SNorm16
        1,  // UInt8
        1,  // SInt8
        2,  // UInt16
        2,  // SInt16
        4,  // UInt32
        4,  // SInt32
    };

    private Mesh() { }

    public static Mesh Read(AssetTypeValueField baseField)
    {
        var name = baseField["m_Name"].AsString;
        var compression = baseField["m_MeshCompression"].AsByte;
        var channels = new List<Channel>();
        int indices;
        uint vertices;
        uint streamDataSize = 0;
        int vertexSize = 0;

        if (compression == 0)
        {
            var bytesPerIndex = baseField["m_IndexFormat"].AsInt == 0 ? 2 : 4;

            indices = baseField["m_IndexBuffer.Array"].AsByteArray.Length / bytesPerIndex; //Null error
            vertices = baseField["m_VertexData.m_VertexCount"].AsUInt;

            // If vertex data size is 0, data is stored in a stream file.
            if (baseField["m_VertexData.m_DataSize"].IsDummy || baseField["m_VertexData.m_DataSize"].AsByteArray.Length == 0)
            {
                streamDataSize = baseField["m_StreamData.size"].AsUInt;
            }

            int i = 0;
            foreach (var channel in baseField["m_VertexData.m_Channels.Array"])
            {
                int dimension = channel["dimension"].AsByte;

                if (dimension != 0)
                {
                    // The dimension can be padded. In that case, the real dimension
                    // is encoded in the top nibble.
                    int originalDim = (dimension >> 4) & 0xF;
                    if (originalDim != 0)
                    {
                        dimension = originalDim;
                    }

                    var c = new Channel()
                    {
                        Dimension = dimension,
                        Type = (ChannelType)channel["format"].AsByte,
                        Usage = (ChannelUsage)i,
                    };

                    channels.Add(c);
                    vertexSize += dimension * s_ChannelTypeSizes[(int)c.Type];
                }

                i++;
            }
        }
        else
        {
            vertices = baseField["m_CompressedMesh.m_Vertices.m_NumItems"].AsUInt / 3;
            indices = baseField["m_CompressedMesh.m_Triangle.m_NumItemss"].AsInt;
        }

        var boneHashPtr = baseField["m_BoneNameHashes.Array"];

        return new Mesh()
        {
            Name = name,
            Vertices = vertices,
            Indices = indices,
            StreamDataSize = streamDataSize,
            SubMeshes = baseField["m_SubMeshes.Array"].AsArray.size,
            BlendShapes = baseField["m_Shapes.shapes.Array"].AsArray.size,
            Bones = boneHashPtr.IsDummy ? 0 : boneHashPtr.AsArray.size,
            RwEnabled = baseField["m_IsReadable"].AsBool,
            Channels = channels,
            VertexSize = vertexSize,
        };
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}