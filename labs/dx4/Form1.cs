using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.VisualC;

namespace dx
{
    public partial class Form1 : Form
    {
        private Device device;
		
        private VertexBuffer vb = null;
        private IndexBuffer ib = null;
		
        private float phi = 0f;
		
        Texture texture0;
        Texture texture1;

        public void InitialD()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.EnableAutoDepthStencil = true;
            presentParams.AutoDepthStencilFormat = DepthFormat.D16;

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);


            device.RenderState.CullMode = Cull.CounterClockwise;
			device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2, this.Width / this.Height, 1f, 50f);
			

            Bitmap b2 = (Bitmap)Image.FromFile("Resources/texture.bmp");
            texture0 = new Texture(device, b2, 0, Pool.Managed);

            Bitmap b = (Bitmap)Image.FromFile("Resources/textureRR.bmp");
            texture1 = new Texture(device, b, 0, Pool.Managed);
        
		
			device.RenderState.Lighting = true;

            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.Orange;
            device.Lights[0].Direction = new Vector3(-1f, -1f, 1f);
            device.Lights[0].Enabled = true;
        
		
            Vector3 vertex4 = new Vector3(0f, -2f, 0f);
            Vector3 vertex5 = new Vector3(-3f, 0f, 0f);
            Vector3 vertex6 = new Vector3(0f, 4f, 0f);
            Vector3 vertex7 = new Vector3(5f, 0f, 0f);
            Vector3 vertex0 = vertex4 + new Vector3(1f, 1f, 5f);
            Vector3 vertex1 = vertex5 + new Vector3(1f, 1f, 5f);
            Vector3 vertex2 = vertex6 + new Vector3(1f, 1f, 5f);
            Vector3 vertex3 = vertex7 + new Vector3(1f, 1f, 5f);

            Vector3 normalUp = -Vector3.Cross(vertex1 - vertex0, vertex1 - vertex2); normalUp.Normalize();
            Vector3 normalDown = -Vector3.Cross(vertex5 - vertex4, vertex6 - vertex5); normalDown.Normalize();
            Vector3 normalLeft = -Vector3.Cross(vertex1 - vertex0, vertex5 - vertex1); normalLeft.Normalize();
            Vector3 normalRight = -Vector3.Cross(vertex3 - vertex2, vertex7 - vertex3); normalRight.Normalize();
            Vector3 normalFront = -Vector3.Cross(vertex0 - vertex3, vertex4 - vertex0); normalFront.Normalize();
            Vector3 normalBack = -Vector3.Cross(vertex2 - vertex1, vertex6 - vertex2); normalBack.Normalize();

            CustomVertex.PositionNormalTextured[] vertices = new CustomVertex.PositionNormalTextured[48];
            //up
            vertices[0] = new CustomVertex.PositionNormalTextured(vertex0, normalUp, 0f, 0f);
            vertices[1] = new CustomVertex.PositionNormalTextured(vertex1, normalUp, 1f, 0f);
            vertices[2] = new CustomVertex.PositionNormalTextured(vertex2, normalUp, 1f, 1f);
            vertices[3] = new CustomVertex.PositionNormalTextured(vertex3, normalUp, 0f, 1f);
            //down
            vertices[4] = new CustomVertex.PositionNormalTextured(vertex4, normalDown, 0f, 0f);
            vertices[5] = new CustomVertex.PositionNormalTextured(vertex5, normalDown, 1f, 0f);
            vertices[6] = new CustomVertex.PositionNormalTextured(vertex6, normalDown, 1f, 1f);
            vertices[7] = new CustomVertex.PositionNormalTextured(vertex7, normalDown, 0f, 1f);
            //front
            vertices[8] = new CustomVertex.PositionNormalTextured(vertex3, normalFront,  0f, 0f);
            vertices[9] = new CustomVertex.PositionNormalTextured(vertex0, normalFront,  1f, 0f);
            vertices[10] = new CustomVertex.PositionNormalTextured(vertex4, normalFront, 1f, 1f);
            vertices[11] = new CustomVertex.PositionNormalTextured(vertex7, normalFront, 0f, 1f);
            //back
            vertices[12] = new CustomVertex.PositionNormalTextured(vertex1, normalBack, 0f, 0f);
            vertices[13] = new CustomVertex.PositionNormalTextured(vertex2, normalBack, 1f, 0f);
            vertices[14] = new CustomVertex.PositionNormalTextured(vertex6, normalBack, 1f, 1f);
            vertices[15] = new CustomVertex.PositionNormalTextured(vertex5, normalBack, 0f, 1f);
            //left
            vertices[16] = new CustomVertex.PositionNormalTextured(vertex0, normalLeft, 0f, 0f);
            vertices[17] = new CustomVertex.PositionNormalTextured(vertex1, normalLeft, 1f, 0f);
            vertices[18] = new CustomVertex.PositionNormalTextured(vertex5, normalLeft, 1f, 1f);
            vertices[19] = new CustomVertex.PositionNormalTextured(vertex4, normalLeft, 0f, 1f);
            //right
            vertices[20] = new CustomVertex.PositionNormalTextured(vertex2, normalRight, 0f, 0f);
            vertices[21] = new CustomVertex.PositionNormalTextured(vertex3, normalRight, 1f, 0f);
            vertices[22] = new CustomVertex.PositionNormalTextured(vertex7, normalRight, 1f, 1f);
            vertices[23] = new CustomVertex.PositionNormalTextured(vertex6, normalRight, 0f, 1f);

            int[] indices = new int[]{
                //up
                0, 3, 2,
                0, 2, 1,
                //down
                4, 5, 6,
                4, 6, 7,
                //front
                16, 17, 18,
                16, 18, 19,
                //back
                20, 21, 22,
                20, 22, 23,
                //left
                8, 9, 10,
                8, 10, 11,
                //right
                12, 13, 14,
                12, 14, 15
            };

            for (int i = 24; i < 48; i++)
            {
                vertices[i] = new CustomVertex.PositionNormalTextured(vertices[i - 24].X + 2, vertices[i - 24].Y + 2, vertices[i - 24].Z - 1, vertices[i - 24].Nx, vertices[i - 24].Ny, vertices[i - 24].Nz, vertices[i - 24].Tu, vertices[i - 24].Tv);
            }

			vb = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 48, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
            vb.SetData(vertices, 0, LockFlags.None);

            ib = new IndexBuffer(typeof(int), indices.Length, device, Usage.WriteOnly, Pool.Default);
            ib.SetData(indices, 0, LockFlags.None);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            phi += 0.01f;
            device.Transform.View = Matrix.LookAtLH(new Vector3((float)(2 * Math.Sqrt(2) * Math.Cos(phi + Math.PI / 4)), (float)Math.Sin(phi), 10f), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
			
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DarkSlateBlue, 1.0f, 0);
            device.BeginScene();

            Material M = new Material();
            M.Diffuse = Color.Pink;
            M.Emissive = Color.Purple;
            M.Ambient = Color.Moccasin;
            M.Specular = Color.WhiteSmoke;
            M.SpecularSharpness = 0.5f;
            device.Material = M;

            device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
			device.Indices = ib;
            device.SetStreamSource(0, vb, 0);
            device.SetTexture(0, texture0);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
            device.SetTexture(0, texture1);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 24, 0, 24, 0, 12);

            device.EndScene();
            device.Present();
            this.Invalidate();

        }

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitialD();
        }
    }
}
