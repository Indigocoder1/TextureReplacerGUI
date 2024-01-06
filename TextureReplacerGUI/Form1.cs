using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using UABEANext3.AssetHandlers.Mesh;

namespace TextureReplacerGUI
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> classIDs = new Dictionary<string, string>();

        private const string MANAGED_FOLDER_PATH = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Subnautica\\Subnautica_Data\\Managed";
        private string assetFolderPath = "D:\\Program Files (x86)\\Steam\\steamapps" +
            "\\common\\Subnautica\\Subnautica_Data\\StreamingAssets\\aa\\StandaloneWindows64";
        private bool glControlLoaded;

        private float theta;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(!TryRetrieveClassIDs())
            {
                return;
            }

            SetUpClassIDList();

            variationChanceBox.Enabled = variationToggle.Checked;

            //DataLoader.LoadBundlesFolder(assetFolderPath, MANAGED_FOLDER_PATH);
            DataLoader.LoadBundleFile(Path.Combine(assetFolderPath, "aramidfibers.prefab_038a9d818af3f295e9592596c08a081d.bundle"), MANAGED_FOLDER_PATH);
        }

        private bool TryRetrieveClassIDs()
        {
            WebClient client = new();
            try
            {
                string jsonString = client.DownloadString(
                "https://raw.githubusercontent.com/SubnauticaModding/Nautilus/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json");
                classIDs = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                //MessageBox.Show("ClassID download complete!");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error downloading classID list!\n{e.Message}");
                return false;
            }
        }

        private void SetUpClassIDList()
        {
            foreach (string value in classIDs.Values)
            {
                string[] split = value.Split('/');
                string lastWord = split[split.Length - 1];
                classIDDropdown.Items.Add(lastWord);
            }
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            glControlLoaded = true;

            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspectRatio = (float)glControl1.ClientSize.Width / glControl1.ClientSize.Height;
            Matrix4 frustumMatrix = Matrix4.CreatePerspectiveFieldOfView(60f.Deg2Rad(), aspectRatio, 1f, 100f);

            GL.LoadMatrix(ref frustumMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Enable(EnableCap.DepthTest);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();

            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Translate(0, 0, -2f);
            GL.Rotate(theta, 1f, 0, 0);
            GL.Rotate(theta, 1f, 0, 1f);

            GL.Begin(PrimitiveType.TriangleStrip);

            //Get the mesh with the aramid fibers classID
            MeshToOpenGL mesh = DataLoader.meshInfos.FirstOrDefault(i => i.Key == "2c4a802e-a6d4-4280-a803-02fc7555caf1").Value;

            int vertexCount = mesh.Vertices.Length / 3;
            int skip = mesh.Normals.Length / vertexCount;
            for (int i = 0; i < vertexCount; i++)
            {
                //GL.Normal3(new Vector3(-mesh.Normals[i * skip], mesh.Normals[i * skip + 1], mesh.Normals[i * skip + 2]));
                GL.Vertex3(mesh.Vertices[i * 3], mesh.Vertices[i * 3 + 1], mesh.Vertices[i * 3 + 2]);
            }

            GL.End();
            glControl1.SwapBuffers();
        }

        private void tickTimer_Tick(object sender, EventArgs e)
        {
            theta  = (theta + 1) % 360;
            glControl1.Invalidate();

            Console.WriteLine(classIDDropdown.PointToClient(new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y)));
        }

        #region OnChange Verifications
        private void materialBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(materialBox.Text, out _))
            {
                materialBox.Text = "";
            }
        }
        private void variationChanceBox_TextChanged(object sender, EventArgs e)
        {
            if (!float.TryParse(variationChanceBox.Text, out _))
            {
                variationChanceBox.Text = "";
            }
        }
        private void variationToggle_CheckedChanged(object sender, EventArgs e)
        {
            variationChanceBox.Enabled = variationToggle.Checked;
        }
        #endregion

        private void classIDDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(classIDDropdown.SelectedItem);
            Console.WriteLine(classIDs.ElementAt(classIDDropdown.SelectedIndex).Key);
        }
    }

    public static class IntExtensions
    {
        public static float Deg2Rad(this float i) => (i * (float)Math.PI / 180f);
    }
}
