using BulletSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CS437
{
    class GamePhysics
    {
        private BroadphaseInterface Broadphase { get; set; }
        private CollisionConfiguration Configuration { get; set; }
        private CollisionDispatcher Dispatcher { get; set; }
        private ConstraintSolver Solver { get; set; }

        public DiscreteDynamicsWorld DynamicsWorld { get; set; }

        public GamePhysics()
        {
            // Build the broadphase
            Broadphase = new DbvtBroadphase();

            // Set up the collision configuration and dispatcher
            Configuration = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(Configuration);

            // The actual physics solver
            Solver = new SequentialImpulseConstraintSolver();

            // The world
            DynamicsWorld = new DiscreteDynamicsWorld(Dispatcher, Broadphase, Solver, Configuration);
            DynamicsWorld.Gravity = Vector3.Zero;
        }

        public void Update(float elapsedTime)
        {
            DynamicsWorld.StepSimulation(elapsedTime);
        }

        public void AddRigidBody(RigidBody body)
        {
            DynamicsWorld.AddRigidBody(body);
        }

        public class PhysicsDebugDraw : DebugDraw
        {
            GraphicsDevice device;

            DebugDrawModes _debugMode;
            public override DebugDrawModes DebugMode
            {
                get { return _debugMode; }
                set { _debugMode = value; }
            }

            public PhysicsDebugDraw(GraphicsDevice device, BasicEffect effect)
            {
                this.device = device;
            }

            public override void Draw3dText(ref Vector3 location, string textString)
            {
                throw new NotImplementedException();
            }

            public override void DrawContactPoint(ref Vector3 pointOnB, ref Vector3 normalOnB, float distance, int lifeTime, Color color)
            {
                VertexPositionColor[] vertices = new VertexPositionColor[2];
                vertices[0] = new VertexPositionColor(pointOnB, color);
                vertices[1] = new VertexPositionColor(pointOnB + normalOnB, color);
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);
            }

            public override void DrawLine(ref Vector3 from, ref Vector3 to, Color color)
            {
                VertexPositionColor[] vertices = new VertexPositionColor[2];
                vertices[0] = new VertexPositionColor(from, color);
                vertices[1] = new VertexPositionColor(to, color);
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);
            }

            public void DrawDebugWorld(DynamicsWorld world)
            {
                world.DebugDrawWorld();
            }

            public override void ReportErrorWarning(string warningString)
            {
                throw new NotImplementedException();
            }
        }
    }


}
