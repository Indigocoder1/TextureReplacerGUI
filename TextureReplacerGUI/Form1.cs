using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TextureReplacerGUI
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> classIDs = new Dictionary<string, string>();
        private const string MANAGED_FOLDER_PATH = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Subnautica\\Subnautica_Data\\Managed";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(!TryGetClassIDs())
            {
                return;
            }

            SetUpClassIDList();

            variationChanceBox.Enabled = variationToggle.Checked;

            string assetFolderPath = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Subnautica\\Subnautica_Data\\StreamingAssets\\aa\\StandaloneWindows64";

            DataLoader.LoadBundlesFolder(assetFolderPath, MANAGED_FOLDER_PATH);

            //DataLoader.LoadBundleFile(assetPath, MANAGED_FOLDER_PATH);
        }

        private bool TryGetClassIDs()
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

        private void materialBox_TextChanged(object sender, EventArgs e)
        {
            if(!int.TryParse(materialBox.Text, out _))
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
    }
}
