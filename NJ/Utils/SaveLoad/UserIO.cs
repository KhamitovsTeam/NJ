using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Xml.Serialization;
using Debug = System.Diagnostics.Debug;

namespace Chip
{
    public static class UserIO
    {
        #region Public properties

        public static bool Saving { get; private set; }

        public static bool SavingResult { get; private set; }

        #endregion

        private const string SavePath = "Saves";
        private const string Extension = ".chip";
        private static bool savingFile;
        private static bool savingSettings;
        private static byte[] savingFileData;
        private static byte[] savingSettingsData;
        private static Action action;
        private static bool indicator;

        public static bool Delete(string path)
        {
            var handle = GetHandle(path);
            if (!File.Exists(handle))
                return false;
            File.Delete(handle);
            return true;
        }

        public static void Close()
        {
        }

        public static void SaveHandler(bool file, bool settings, Action action = null, bool indicator = true)
        {
            Saving = true;
            savingFile = file;
            savingSettings = settings;
            UserIO.action = action;
            UserIO.indicator = indicator;
            if (savingFile)
            {
                SaveData.Instance.BeforeSave();
                savingFileData = Serialize(SaveData.Instance);
            }
            if (savingSettings)
                savingSettingsData = Serialize(Settings.Instance);
            SavingResult = false;
            RunThread.Start(SaveThread, "USER_IO");
        }

        private static string GetHandle(string name)
        {
            return Path.Combine(SavePath, name + Extension);
        }

        public static bool Open(Mode mode)
        {
            return true;
        }

        private static bool Save(string name, byte[] data)
        {
#if XBOXONE
            StorageDevice device;
            var filename = name + Extension;

            StorageDevice.BeginShowSelector((result) => {
                device = StorageDevice.EndShowSelector(result);
                if (device != null && device.IsConnected)
                {
                    IAsyncResult r = device.BeginOpenContainer(SavePath, null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = device.EndOpenContainer(r);
                    if (container.FileExists(filename))
                        container.DeleteFile(filename);
                    using (var fileStream = container.CreateFile(filename))
                    {
                        fileStream.Write(data, 0, data.Length);
                    }
                    container.Dispose();
                    result.AsyncWaitHandle.Close();
                }
            }, null);
            return true;
#else
            var handle = GetHandle(name);
            var flag = false;
            try
            {
                var directory = new FileInfo(handle).Directory;
                if (directory != null && !directory.Exists)
                    directory.Create();
                using (var fileStream = File.Open(handle, FileMode.Create, FileAccess.Write))
                    fileStream.Write(data, 0, data.Length);
                flag = true;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR: " + ex);
            }

            if (!flag)
                Log.Error("Save Failed");
            return flag;
#endif
        }

        public static T Load<T>(string name) where T : class
        {
#if XBOXONE
            int state = -1;
            var obj = default(T);
            StorageDevice device;
            var filename = "0.chip";// name + Extension;

            StorageDevice.BeginShowSelector((result) => {
                device = StorageDevice.EndShowSelector(result);
                IAsyncResult r = device.BeginOpenContainer(SavePath, null, null);
                result.AsyncWaitHandle.WaitOne();
                StorageContainer container = device.EndOpenContainer(r);
                result.AsyncWaitHandle.Close();
                if (container.FileExists(filename))
                {
                    using (Stream fileStream = container.OpenFile(filename, FileMode.Open))
                    {
                        obj = Deserialize<T>(fileStream);
                    }
                    container.Dispose();
                    state = 0;
                }
                else
                {
                    state = 0;
                }
            }, null);

            while (true)
            {
                if (state == 0)
                {
                    return obj;
                }
            }
#else
            var handle = GetHandle(name);
            var obj = default(T);
            try
            {
                if (File.Exists(handle))
                {
                    using (var fileStream = TitleContainer.OpenStream(handle))
                        obj = Deserialize<T>(fileStream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return obj;
#endif
        }

        private static byte[] Serialize<T>(T instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                new XmlSerializer(typeof(T)).Serialize(memoryStream, instance);
                return memoryStream.ToArray();
            }
        }

        private static T Deserialize<T>(Stream stream) where T : class
        {
            var obj = default(T);
            try
            {
                obj = (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                // ignored
            }

            if (obj != null) return obj;
            stream.Position = 0L;
            obj = (T)new BinaryFormatter().Deserialize(stream);

            return obj;
        }

        private static void SaveThread()
        {
            if (indicator)
                SaveLoadIcon.Show();
            SavingResult = false;
            if (!Open(Mode.Write)) return;
            SavingResult = true;
            if (savingFile)
                SavingResult &= Save(SaveData.GetFilename(), savingFileData);
            if (savingSettings)
                SavingResult &= Save("settings", savingSettingsData);
            Close();
            action?.Invoke();
            Saving = false;
            Thread.Sleep(700);
            if (indicator)
                SaveLoadIcon.Hide();
        }

        public enum Mode
        {
            Read,
            Write
        }
    }
}