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

            //Get the mesh with the aramid fibers classID
            MeshToOpenGL mesh = DataLoader.meshInfos.FirstOrDefault(i => i.Key == "2c4a802e-a6d4-4280-a803-02fc7555caf1").Value;

            Console.WriteLine($"Indices length = {mesh.Indices.Length}");
            Console.WriteLine($"Vertex length = {mesh.Vertices.Length}");

            string vertList = "";
            for (int i = 0; i < mesh.Vertices.Length; i += 3)
            {
                //string vertexNum = i > mesh.Vertices.Length - 1 ? "" : $"{mesh.Vertices[i]} (Index {i})";
                vertList += $"Vertex at index {i} is {mesh.Vertices[i]}\n";
            }

            File.WriteAllText("C:\\Users\\caleb\\Downloads\\indexInfo.txt", vertList);
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

            GL.Begin(PrimitiveType.Triangles);

            //Get the mesh with the aramid fibers classID
            MeshToOpenGL mesh = DataLoader.meshInfos.FirstOrDefault(i => i.Key == "2c4a802e-a6d4-4280-a803-02fc7555caf1").Value;
            Vector3[] vertices = new Vector3[mesh.Vertices.Length / 3];
            for (int i = 0; i < mesh.Vertices.Length / 3; i += 3)
            {
                float x = mesh.Vertices[i];
                float y = mesh.Vertices[i + 1];
                float z = mesh.Vertices[i + 2];
                vertices[i] = new Vector3(x, y, z);
            }

            for (int i = 0; i < mesh.Indices.Length; i++)
            {
                //int originalIndex = i == 0 ? 0 : mesh.Indices[i] + 2;
                //Console.WriteLine($"Current index is {i} | Indice at i is {mesh.Indices[i]} | Vertex at mesh.Indices[i] is {mesh.Vertices[originalIndex]}");
                GL.Vertex3(vertices[mesh.Indices[i]]);
            }

            GL.End();
            glControl1.SwapBuffers();
        }

        private void tickTimer_Tick(object sender, EventArgs e)
        {
            theta  = (theta + 1) % 360;
            glControl1.Invalidate();
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
    }

    public static class IntExtensions
    {
        public static float Deg2Rad(this float i) => (i * (float)Math.PI / 180f);
    }
}
