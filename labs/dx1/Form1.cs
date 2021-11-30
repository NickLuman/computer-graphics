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

        public void Initialize()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            
			device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);


            device.RenderState.CullMode = Cull.CounterClockwise;
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 8, (float)this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(20f, 20f, 20f), new Vector3(0, 0, 0), new Vector3(0, 0, 1));


			device.RenderState.Lighting = true;
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.Cyan;
            device.Lights[0].Direction = new Vector3(-1f, -1f, -1f);
            device.Lights[0].Enabled = true;
  

            Vector3 normalUp = new Vector3(0f, 0f, 1f);

            Vector3 vertex0 = new Vector3(0f, -2f, 0f);
            Vector3 vertex1 = new Vector3(-3f, 0f, 0f);
            Vector3 vertex2 = new Vector3(0f, 4f, 0f);
            Vector3 vertex3 = new Vector3(5f, 0f, 0f);
			
			CustomVertex.PositionNormalColored[] vertices = new CustomVertex.PositionNormalColored[8];
            vertices[0] = new CustomVertex.PositionNormalColored(vertex0, normalUp, Color.Cyan.ToArgb());
            vertices[1] = new CustomVertex.PositionNormalColored(vertex1, normalUp, Color.Red.ToArgb());
            vertices[2] = new CustomVertex.PositionNormalColored(vertex2, normalUp, Color.Blue.ToArgb());
            vertices[3] = new CustomVertex.PositionNormalColored(vertex3, normalUp, Color.Magenta.ToArgb());
            vertices[4] = new CustomVertex.PositionNormalColored(vertex0, -normalUp, Color.Cyan.ToArgb());
            vertices[5] = new CustomVertex.PositionNormalColored(vertex1, -normalUp, Color.Red.ToArgb());
            vertices[6] = new CustomVertex.PositionNormalColored(vertex2, -normalUp, Color.Blue.ToArgb());
            vertices[7] = new CustomVertex.PositionNormalColored(vertex3, -normalUp, Color.Magenta.ToArgb());

            int[] indices = new int[]{
                0, 3, 2,
                0, 2, 1,
                4, 5, 6,
                4, 6, 7
            };

            ib = new IndexBuffer(typeof(int), indices.Length, device, Usage.WriteOnly, Pool.Default);
            ib.SetData(indices, 0, LockFlags.None);
			
			vb = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 8, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalColored.Format, Pool.Default);
            vb.SetData(vertices, 0, LockFlags.None);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
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
			device.Indices = ib;
            device.SetStreamSource(0, vb, 0);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 4);

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
