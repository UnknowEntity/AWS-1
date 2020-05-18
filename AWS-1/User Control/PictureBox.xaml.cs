using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Label = Amazon.Rekognition.Model.Label;

namespace AWS_1.User_Control
{
    /// <summary>
    /// Interaction logic for PictureBox.xaml
    /// </summary>
    public partial class PictureBox : UserControl
    {
        string filePath = null;
        public PictureBox()
        {
            InitializeComponent();
        }

        public void LoadPicture(string tempFilePath)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(tempFilePath));
            imgPictureFrame.Source = bitmap;
            imgPictureFrame.Width = bitmap.Width;
            imgPictureFrame.Height = bitmap.Height;
            this.Height = bitmap.Height;
            this.Width = bitmap.Width;
            filePath = tempFilePath;
        }

        public void FindObject()
        {
            Amazon.Rekognition.Model.Image image = new Amazon.Rekognition.Model.Image();
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = null;
                    data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    image.Bytes = new MemoryStream(data);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load file " + filePath);
                return;
            }

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
            {
                Image = image,
                MaxLabels = 10,
                MinConfidence = 77F
            };

            try
            {
                DetectLabelsResponse detectLabelsResponse = rekognitionClient.DetectLabels(detectlabelsRequest);

                double width = imgPictureFrame.Width;
                double height = imgPictureFrame.Height;

                foreach (Label label in detectLabelsResponse.Labels)
                {
                    List<Instance> instances = label.Instances;
                    foreach (Instance instance in instances)
                    {
                        string data = $"{label.Name}: {Math.Round(instance.Confidence,2)}%";
                        BoundingBox boundingBox = instance.BoundingBox;
                        BindingBox bindingBox = new BindingBox(width*boundingBox.Width,height*boundingBox.Height,height*boundingBox.Top,width*boundingBox.Left,data);
                        gContainer.Children.Add(bindingBox);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
