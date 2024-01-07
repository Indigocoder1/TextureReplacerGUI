using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Forms;
using UABEANext3.AssetHandlers.Mesh;

namespace TextureReplacerGUI
{
    internal class DataLoader
    {
        public static Dictionary<string, MeshToOpenGL> meshInfos = new();

        public static void LoadAssetsFile(string filePath, string managedFolderPath)
        {
            var manager = new AssetsManager();
            manager.MonoTempGenerator = new MonoCecilTempGenerator(managedFolderPath);
            
            manager.LoadClassPackage("C:\\VisualStudioProjects\\TextureReplacerGUI\\lz4.tpk");

            var aFileInst = manager.LoadAssetsFile(filePath, true);
            var aFile = aFileInst.file;

            manager.LoadClassDatabaseFromPackage(aFile.Metadata.UnityVersion);

            foreach (var goInfo in aFile.GetAssetsOfType(AssetClassID.GameObject))
            {
                var goBase = manager.GetBaseField(aFileInst, goInfo);
                string name = goBase["m_Name"].AsString;
                Console.WriteLine(name);
                
                var components = goBase["m_Component.Array"];
                foreach (var data in components)
                {
                    var componentPointer = data["component"];
                    var componentExtInfo = manager.GetExtAsset(aFileInst, componentPointer);

                    var componentType = (AssetClassID)componentExtInfo.info.TypeId;
                    Console.WriteLine($"Component type = {componentType}");

                    if (componentType == AssetClassID.MeshFilter)
                    {
                        Console.WriteLine(componentExtInfo.info.PathId);
                    }
                }
            }
        }

        public static async void LoadBundlesFolder(string folderPath, string managedFolderPath, EventHandler<OnFileLoaded_EventArgs> OnFileLoaded, Action OnLoadComplete)
        {
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Invalid folder path!");
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.bundle");
            int filesRead = 0;
            foreach (string file in files)
            {
                if(new FileInfo(file).Length == 0)
                {
                    filesRead++;
                    continue;
                }

                Console.WriteLine($"Progress at {filesRead / (float)files.Length * 100}%");

                ClassIDMeshInfo info = await LoadBundleFile(file, managedFolderPath);
                if(string.IsNullOrEmpty(info.classID))
                {
                    continue;
                }
                
                meshInfos.Add(info.classID, info.mesh);
                OnFileLoaded?.Invoke(null, new OnFileLoaded_EventArgs(filesRead / (float)files.Length));

                filesRead++;
            }

            OnLoadComplete?.Invoke();
        }

        public static async Task<ClassIDMeshInfo> LoadBundleFile(string filePath, string managedFolderPath)
        {
            var manager = new AssetsManager();
            manager.LoadClassPackage("C:\\VisualStudioProjects\\TextureReplacerGUI\\lz4.tpk");
            manager.MonoTempGenerator = new MonoCecilTempGenerator(managedFolderPath);

            FileStream stream = new FileStream(filePath, FileMode.Open);
            var bunInst = manager.LoadBundleFile(stream, true);
            var aFileInst = manager.LoadAssetsFileFromBundle(bunInst, 0, false);
            var aFile = aFileInst.file;

            manager.LoadClassDatabaseFromPackage(aFile.Metadata.UnityVersion);

            string classID = null;
            bool hasClassID = await Task.Run(() => TryGetClassID(aFileInst, manager, out classID));
            if(!hasClassID)
            {
                return ClassIDMeshInfo.Empty;
            }

            MeshToOpenGL mesh = null;
            bool hasMesh = await Task.Run(() => TryGetMesh(aFileInst, manager, out mesh));
            if(!hasMesh)
            {
                return ClassIDMeshInfo.Empty;
            }
            
            return new ClassIDMeshInfo(classID, mesh);
        }

        private static bool TryGetClassID(AssetsFileInstance aFileInstance, AssetsManager manager, out string classID)
        {
            var aFile = aFileInstance.file;
            var scriptInfos = AssetHelper.GetAssetsFileScriptInfos(manager, aFileInstance);

            foreach (var goInfo in aFile.GetAssetsOfType(AssetClassID.GameObject))
            {
                var goBase = manager.GetBaseField(aFileInstance, goInfo);

                var components = goBase["m_Component.Array"];
                foreach (var data in components)
                {
                    var componentPointer = data["component"];
                    var componentExt = manager.GetExtAsset(aFileInstance, componentPointer);

                    ushort scriptIndex = componentExt.file.file.GetScriptIndex(componentExt.info);
                    if (!scriptInfos.ContainsKey(scriptIndex))
                    {
                        continue;
                    }

                    var script = scriptInfos[scriptIndex];
                    if (!(script.ClassName == "PrefabIdentifier"))
                    {
                        continue;
                    }

                    classID = componentExt.baseField["classId"].AsString;
                    return true;
                }
            }

            classID = null;
            return false;
        }
        
        private static bool TryGetMesh(AssetsFileInstance aFileInstance, AssetsManager manager, out MeshToOpenGL mesh)
        {
            mesh = null;
            var aFile = aFileInstance.file;

            foreach (var filterInfo in aFile.GetAssetsOfType(AssetClassID.Mesh))
            {
                var meshBase = manager.GetBaseField(aFileInstance, filterInfo);

                try
                {
                    mesh = new MeshToOpenGL(aFileInstance, meshBase);
                    return true;
                }
                catch(Exception e)
                {
                    MessageBox.Show($"Error loading mesh data for {meshBase["m_Name"].AsString}!\n{e.Message}");
                }
            }

            return false;
        }

        public struct ClassIDMeshInfo
        {
            public string classID;
            public MeshToOpenGL mesh;

            public ClassIDMeshInfo(string classID, MeshToOpenGL mesh)
            {
                this.classID = classID;
                this.mesh = mesh;
            }

            public static ClassIDMeshInfo Empty = new ClassIDMeshInfo(null, null);
        }

        public class OnFileLoaded_EventArgs : EventArgs
        {
            public float currentPercentage;

            public OnFileLoaded_EventArgs(float currentPercentage)
            {
                this.currentPercentage = currentPercentage;
            }
        }
    }
}