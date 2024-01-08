using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using UABEANext3.AssetHandlers.Mesh;
using ConfigInfo = TextureReplacer.CustomTextureReplacer.TexturePatchConfigData;

namespace TextureReplacerGUI
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> classIDs = new Dictionary<string, string>();
        private List<ConfigInfo> configs = new List<ConfigInfo>();
        private bool meshesLoaded;

        private bool dragging;
        private Point previousMouseLocation;

        private const float CAM_SENSITIVITY = 1f;
        private const float SCROLL_SENSITIVITY = 0.2f;
        private float mouseX;
        private float mouseY;
        private float modelOffset = -2f;

        private MeshToOpenGL activeMesh;

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
            loadingLabel.Visible = false;
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

        #region GL Stuff
        private void glControl1_Load(object sender, EventArgs e)
        {
            glControl1.MouseDown += OnGLMouseDown;
            glControl1.MouseUp += OnGLMouseUp;
            glControl1.MouseMove += OnGLMouseMove;
            glControl1.MouseWheel += glControl1_Scroll;

            GL.ClearColor(Color.CadetBlue);

            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspectRatio = (float)glControl1.ClientSize.Width / glControl1.ClientSize.Height;
            Matrix4 viewMatrix = Matrix4.CreatePerspectiveFieldOfView(60f.Deg2Rad(), aspectRatio, 1f, 100f);
            GL.LoadMatrix(ref viewMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Enable(EnableCap.DepthTest);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if(activeMesh == null || !meshesLoaded)
            {
                return;
            }

            glControl1.MakeCurrent();

            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Translate(new Vector3(0, 0, modelOffset));
            GL.Rotate(mouseX, new Vector3(0, 1, 0));
            GL.Rotate(mouseY, new Vector3(1, 0, 0));

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Begin(PrimitiveType.TriangleFan);

            GL.Color3(0.48f, 0.5f, 0.59f);

            int vertexCount = activeMesh.Vertices.Length / 3;
            for (int i = 0; i < vertexCount; i++)
            {
                GL.Vertex3(activeMesh.Vertices[i * 3], activeMesh.Vertices[i * 3 + 1], activeMesh.Vertices[i * 3 + 2]);
            }

            GL.End();
            glControl1.SwapBuffers();
        }

        private void OnGLMouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left)
            {
                return;
            }
            
            previousMouseLocation = e.Location;
            dragging = true;
        }

        private void OnGLMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            dragging = false;
        }

        private void OnGLMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
            {
                return;
            }

            Point difference = e.Location - (Size)previousMouseLocation;
            mouseX += difference.X * CAM_SENSITIVITY;
            mouseY += difference.Y * CAM_SENSITIVITY;

            previousMouseLocation = e.Location;
            glControl1.Invalidate();
        }

        private void classIDDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadingLabel.Visible = false;
            if (!meshesLoaded) return;

            string key = classIDs.ElementAt(classIDDropdown.SelectedIndex).Key;
            if (!DataLoader.meshInfos.ContainsKey(key))
            {
                previewLabel.Visible = true;
                return;
            }

            modelOffset = -2f;
            activeMesh = DataLoader.meshInfos[key];
            glControl1.Invalidate();
        }

        private void loadFolderBtn_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                loadingLabel.Visible = true;

                string managedFolderPath = Path.Combine(folderBrowserDialog1.SelectedPath, "Managed");
                string assetsPath = Path.Combine(folderBrowserDialog1.SelectedPath, "StreamingAssets\\aa\\StandaloneWindows64");

                DataLoader.LoadBundlesFolderAsync(assetsPath, managedFolderPath, OnFileLoadComplete, () => { 
                    meshesLoaded = true;
                    loadingLabel.Text = "File load complete!";
                    });
            }
        }

        private void OnFileLoadComplete(object sender, DataLoader.OnFileLoaded_EventArgs e)
        {
            loadingLabel.Text = $"Loading files... This will take a while ({Math.Round(e.currentPercentage * 100, 2)}%)";
            progressBar1.Value = (int)Math.Round(e.currentPercentage * 100, 0);
        }

        private void glControl1_Scroll(object sender, MouseEventArgs e)
        {
            modelOffset = Math.Max(-1, modelOffset + e.Delta / 10f * SCROLL_SENSITIVITY);
            glControl1.Invalidate();
            Console.WriteLine(modelOffset);
        }
        #endregion

        private void SaveCfgBtn_Click(object sender, EventArgs e)
        {
            string configName = configNameBox.Text;
            int materialIndex = int.Parse(materialBox.Text);
            string fileName = texFileNameBox.Text;
            bool isVariation = variationToggle.Checked;
            float variationChance = float.Parse(variationChanceBox.Text);
            string classID = classIDs.ElementAt(classIDDropdown.SelectedIndex).Key;
            string rendererHierarchyPath = hierarchyPathBox.Text;
            string textureName = textureNameBox.Text;

            configs.Add(new ConfigInfo(configName, materialIndex, fileName, isVariation, variationChance, classID, rendererHierarchyPath, textureName, null));
        }

        private void ExportCfgBtn_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            saveFileDialog1.FileName = "Exported Config";
            saveFileDialog1.Filter = "Json Files | *.json";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string configJson = JsonConvert.SerializeObject(configs, Formatting.Indented);
                File.WriteAllText(saveFileDialog1.FileName, configJson);
            }
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

    public static class Extensions
    {
        public static float Deg2Rad(this float i) => (i * (float)Math.PI / 180f);
        public static Vector2 ToVector2(this Point point) => (new Vector2(point.X, point.Y));
    }
}
