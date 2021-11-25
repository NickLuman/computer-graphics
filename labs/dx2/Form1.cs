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
        private float phi = 0f;

        private Device device;

        private VertexBuffer vb;
        private IndexBuffer ib;
        

        public void Initialize()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);


            device.RenderState.CullMode = Cull.CounterClockwise;
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2, this.Width / this.Height, 1f, 50f);


            device.RenderState.Lighting = true;

            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.Red;
            device.Lights[0].Direction = new Vector3(1f, -1f, 1f);
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

            CustomVertex.PositionNormalColored[] vertices = new CustomVertex.PositionNormalColored[24];
            //up
            vertices[0] = new CustomVertex.PositionNormalColored(vertex0, normalUp, Color.Cyan.ToArgb());
            vertices[1] = new CustomVertex.PositionNormalColored(vertex1, normalUp, Color.Red.ToArgb());
            vertices[2] = new CustomVertex.PositionNormalColored(vertex2, normalUp, Color.Blue.ToArgb());
            vertices[3] = new CustomVertex.PositionNormalColored(vertex3, normalUp, Color.Magenta.ToArgb());
            //down
            vertices[4] = new CustomVertex.PositionNormalColored(vertex4, normalDown, Color.Cyan.ToArgb());
            vertices[5] = new CustomVertex.PositionNormalColored(vertex5, normalDown, Color.Red.ToArgb());
            vertices[6] = new CustomVertex.PositionNormalColored(vertex6, normalDown, Color.Blue.ToArgb());
            vertices[7] = new CustomVertex.PositionNormalColored(vertex7, normalDown, Color.Magenta.ToArgb());
            //front
            vertices[8] = new CustomVertex.PositionNormalColored(vertex3, normalFront, Color.Magenta.ToArgb());
            vertices[9] = new CustomVertex.PositionNormalColored(vertex0, normalFront, Color.Cyan.ToArgb());
            vertices[10] = new CustomVertex.PositionNormalColored(vertex4, normalFront, Color.Cyan.ToArgb());
            vertices[11] = new CustomVertex.PositionNormalColored(vertex7, normalFront, Color.Magenta.ToArgb());
            //back
            vertices[12] = new CustomVertex.PositionNormalColored(vertex1, normalBack, Color.Red.ToArgb());
            vertices[13] = new CustomVertex.PositionNormalColored(vertex2, normalBack, Color.Blue.ToArgb());
            vertices[14] = new CustomVertex.PositionNormalColored(vertex6, normalBack, Color.Blue.ToArgb());
            vertices[15] = new CustomVertex.PositionNormalColored(vertex5, normalBack, Color.Red.ToArgb());
            //left
            vertices[16] = new CustomVertex.PositionNormalColored(vertex0, normalLeft, Color.Cyan.ToArgb());
            vertices[17] = new CustomVertex.PositionNormalColored(vertex1, normalLeft, Color.Red.ToArgb());
            vertices[18] = new CustomVertex.PositionNormalColored(vertex5, normalLeft, Color.Red.ToArgb());
            vertices[19] = new CustomVertex.PositionNormalColored(vertex4, normalLeft, Color.Cyan.ToArgb());
            //right
            vertices[20] = new CustomVertex.PositionNormalColored(vertex2, normalRight, Color.Blue.ToArgb());
            vertices[21] = new CustomVertex.PositionNormalColored(vertex3, normalRight, Color.Magenta.ToArgb());
            vertices[22] = new CustomVertex.PositionNormalColored(vertex7, normalRight, Color.Magenta.ToArgb());
            vertices[23] = new CustomVertex.PositionNormalColored(vertex6, normalRight, Color.Blue.ToArgb());

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

            vb = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 24, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalColored.Format, Pool.Default);
            vb.SetData(vertices, 0, LockFlags.None);

            ib = new IndexBuffer(typeof(int), indices.Length, device, Usage.WriteOnly, Pool.Default);
            ib.SetData(indices, 0, LockFlags.None);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            phi += 0.01f;
            device.Transform.View = Matrix.LookAtLH(new Vector3((float)(2 * Math.Sqrt(2) * Math.Cos(phi + Math.PI / 4)), (float)Math.Sin(phi), 10f), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
            
            device.Clear(ClearFlags.Target, Color.DarkSlateBlue, 1.0f, 0);
            device.BeginScene();

            Material M = new Material();
            M.Diffuse = Color.Pink;
            M.Emissive = Color.Purple;
            M.Ambient = Color.Moccasin;
            M.Specular = Color.WhiteSmoke;
            M.SpecularSharpness = 0.5f;
            device.Material = M;

            device.VertexFormat = CustomVertex.PositionNormalColored.Format;
            device.SetStreamSource(0, vb, 0);
            device.Indices = ib;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);

            device.EndScene();
            device.Present();

            this.Invalidate();
        }

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            Initialize();
        }
    }
}
