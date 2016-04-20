using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CS437
{
    /// <summary>
    /// Heavily modified code based off of http://xboxforums.create.msdn.com/forums/t/62209.aspx
    /// Converts model data into a format suitable for bullet physics to use for TriangleMeshShatpe bounding models for concave shapes
    /// </summary>
    static class ModelDataExtractor
    {
        public static void ExtractModelMeshData(Model model, Matrix transform, out List<Vector3> vertices, out List<int> indices)
        {
            ModelMeshPart meshPart = model.Meshes[0].MeshParts[0];

            // Read the format of the vertex buffer  
            VertexDeclaration declaration = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] vertexElements = declaration.GetVertexElements();

            // Find the element that holds the position  
            VertexElement vertexPosition = new VertexElement();

            foreach (VertexElement vert in vertexElements)
            {
                if (vert.VertexElementUsage == VertexElementUsage.Position &&
                    vert.VertexElementFormat == VertexElementFormat.Vector3)
                {
                    vertexPosition = vert;
                    // There should only be one  
                    break;
                }
            }

            // Check the position element found is valid  
            if (vertexPosition == null ||
                vertexPosition.VertexElementUsage != VertexElementUsage.Position ||
                vertexPosition.VertexElementFormat != VertexElementFormat.Vector3)
            {
                throw new Exception("Model uses unsupported vertex format!");
            }

            // This where we store the vertices until transformed  
            Vector3[] allVertex = new Vector3[meshPart.NumVertices];
            // Read the vertices from the buffer in to the array
            meshPart.VertexBuffer.GetData(
                meshPart.VertexOffset * declaration.VertexStride + vertexPosition.Offset,
                allVertex,
                0,
                meshPart.NumVertices,
                declaration.VertexStride);

            // Transform the vertices
            for (int i = 0; i != allVertex.Length; ++i)   
            {   
                Vector3.Transform(ref allVertex[i], ref transform, out allVertex[i]);   
            }  

            // Store the transformed vertices with those from all the other meshes in this model  
            vertices = new List<Vector3>(allVertex);

            // Find out which vertices make up which triangles  
            if (meshPart.IndexBuffer.IndexElementSize != IndexElementSize.SixteenBits)
            {
                // This could probably be handled by using int in place of short but is unnecessary
                throw new Exception("Model uses 32-bit indices, which are not supported.");
            }

            // Each primitive is a triangle
            short[] indexElements = new short[meshPart.PrimitiveCount * 3];
            meshPart.IndexBuffer.GetData(
                meshPart.StartIndex * 2,
                indexElements,
                0,
                meshPart.PrimitiveCount * 3);

            // Store our indices after casting them to ints
            indices = new List<int>(Array.ConvertAll(indexElements, item => (int)item));
        }
    }
}
