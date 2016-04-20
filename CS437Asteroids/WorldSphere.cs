using System;
using System.Collections.Generic;
using BulletSharp;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CS437
{
    class WorldSphere : Actor
    {
        private List<int> indices;
        private List<Vector3> vertices;

        public WorldSphere(DynamicsWorld DynamicsWorld,
            Func<string, Model> loadModel,
            float scale = 1f) : base(DynamicsWorld, null, null, null, GamePhysics.CollisionTypes.Everything, GamePhysics.CollisionTypes.Everything, scale, 0)
        {
            _model = loadModel("Models/worldsphere");

            ModelDataExtractor.ExtractModelMeshData(_model, Matrix.CreateScale(scale), out vertices, out indices);

            StridingMeshInterface mesh = new TriangleIndexVertexArray(indices, vertices);

            CollisionShape = new BvhTriangleMeshShape(mesh, true);

            Body.Restitution = 1f;
            // Body.SetDamping(0f, 0f);
            // Body.Friction = 0;
            // Body.RollingFriction = 0;
        }

        public override void Draw(Camera camera)
        {
            GraphicsDevice graphics = _model.Meshes[0].Effects[0].GraphicsDevice;
            RasterizerState originalState = graphics.RasterizerState;
            RasterizerState state = new RasterizerState();
            state.FillMode = FillMode.WireFrame;
            graphics.RasterizerState = state;

            base.Draw(camera);

            graphics.RasterizerState = originalState;
        }
    }
}
