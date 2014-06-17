#region Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Build.DataLayer.Enum;
using Build.DataLayer.Model;

#endregion

namespace DragDropPhoneApp.Service
{
    public static class DataService
    {
        #region Public Methods and Operators

        public static byte[] ConvertToBytes(this BitmapImage bitmapImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap btmMap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);

                btmMap.SaveJpeg(ms, bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 100);

                return ms.ToArray();
            }
        }

        public static BitmapImage FetchImage(Activity photo)
        {
            BitmapImage image = null;

            image = new BitmapImage();

            using (var imageStream = LoadImage(photo.ImageSource))
            {
                if (imageStream != null)
                {
                    image.SetSource(imageStream);
                }
            }

            return image;
        }

        public static Activity GetImage(string imgName)
        {
            DateTime start = new DateTime(2010, 1, 1);
            ActivityType activityType = new ActivityType { Type = imgName };

            Activity imageData = new Activity
                                     {
                                         ActivityType = activityType, 
                                         ImageSource = imgName, 
                                         Title = imgName, 
                                         TimeStamp = start
                                     };

            imageData.Image = FetchImage(imageData);
            return imageData;
        }

        public static async Task<List<Activity>> GetImages()
        {
            List<Activity> imageList = new List<Activity>();
            DateTime start = new DateTime(2010, 1, 1);
            foreach (var imgName in GetImagesNamesList(true))
            {
                ActivityType activityType = new ActivityType { Type = imgName.Split('\\', '.')[2] };

                Activity imageData = new Activity
                                         {
                                             ActivityType = activityType, 
                                             ImageSource = imgName, 
                                             Title = imgName, 
                                             TimeStamp = start
                                         };
                imageData.Image = FetchImage(imageData);
                imageList.Add(imageData);
            }

            return imageList;
        }

        public static List<string> GetImagesNamesList(bool withPath)
        {
            IsolatedStorageFile storeFile = IsolatedStorageFile.GetUserStoreForApplication();
            string imageFolder =ResourceStrings.Strings.ImageFolder;
            if (!storeFile.DirectoryExists(ResourceStrings.Strings.ImageFolderSlash))
            {
                storeFile.CreateDirectory(ResourceStrings.Strings.ImageFolderSlash);
            }

            List<string> fileList = new List<string>(storeFile.GetFileNames(ResourceStrings.Strings.ImageFolderSlash));
            List<string> imgNameList = new List<string>();
            if (withPath)
            {
                foreach (string file in fileList)
                {
                    imgNameList.Add(Path.Combine(imageFolder, file));
                }
            }
            else
            {
                return fileList;
            }

            return imgNameList;
        }

        public static Stream LoadImage(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentException("one of parameters is null");
            }

            Stream stream = null;

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists(filename))
                {
                    stream = isoStore.OpenFile(filename, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    var file1 = Path.Combine(ResourceStrings.Strings.ImageFolder, filename);
                    if (isoStore.FileExists(file1))
                    {
                        stream = isoStore.OpenFile(filename, FileMode.Open, FileAccess.Read);
                    }
                }
            }

            return stream;
        }

        public static bool SaveImage(string filename, byte[] byteContent)
        {
            if (filename == null || byteContent.Length == 0)
            {
                throw new ArgumentException("one of parameters is null");
            }

            string imageFolder = ResourceStrings.Strings.ImageFolder;

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isoStore.DirectoryExists(ResourceStrings.Strings.ImageFolderSlash))
                {
                    isoStore.CreateDirectory(ResourceStrings.Strings.ImageFolderSlash);
                }

                var file1 = Path.Combine(imageFolder, filename);

                if (isoStore.FileExists(file1))
                {
                    return false;
                }

                using (IsolatedStorageFileStream stream1 = isoStore.CreateFile(file1))
                {
                    stream1.Write(byteContent, 0, byteContent.Length);
                }

                return true;
            }
        }

        #endregion
    }
}