﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

/// <summary>
/// 98% of the skybox code is credited to RB Whitaker.
/// This code is taken from his tutorial on skyboxes which can be found at http://rbwhitaker.wikidot.com/skyboxes-1
/// </summary>
namespace CS437
{
    /// <summary>
    /// Handles all of the aspects of working with a skybox.
    /// </summary>
    public class Skybox
    {
        /// <summary>
        /// The skybox model, which will just be a cube
        /// </summary>
        public Model skyBox;

        /// <summary>
        /// The actual skybox texture
        /// </summary>
        private TextureCube skyBoxTexture;

        /// <summary>
        /// The effect file that the skybox will use to render
        /// </summary>
        private Effect skyBoxEffect;

        /// <summary>
        /// The size of the cube, used so that we can resize the box
        /// for different sized environments.
        /// </summary>
        private float size = 5000f;

        /// <summary>
        /// Creates a new skybox
        /// </summary>
        /// <param name="skyboxTexture">the name of the skybox texture to use</param>
        public Skybox(ContentManager Content, string skyboxTexture)
        {
            skyBox = Content.Load<Model>("Models/cube");
            skyBoxTexture = Content.Load<TextureCube>(skyboxTexture);
            skyBoxEffect = Content.Load<Effect>("Effects/skybox");
        }

        /// <summary>
        /// Does the actual drawing of the skybox with our skybox effect.
        /// There is no world matrix, because we're assuming the skybox won't
        /// be moved around.  The size of the skybox can be changed with the size
        /// variable.
        /// </summary>
        /// <param name="view">The view matrix for the effect</param>
        /// <param name="projection">The projection matrix for the effect</param>
        /// <param name="cameraPosition">The position of the camera</param>
        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            // Go through each pass in the effect, but we know there is only one...
            foreach (EffectPass pass in skyBoxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (ModelMesh mesh in skyBox.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(
                            Matrix.CreateScale(size) * Matrix.CreateTranslation(cameraPosition));
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(skyBoxTexture);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    }

                    mesh.Draw();
                }
            }
        }
    }
}
