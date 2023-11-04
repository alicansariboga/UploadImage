using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Library for the file processes
using System.Data.SqlClient;


namespace UploadImage
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection("Add Your SQL Path for Connection");

        string imagePath;
        string NickName;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnUploadImage.Enabled = false;
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            btnUploadImage.Enabled = true;

            openFileDialog1.Title = "Upload Image";
            openFileDialog1.Filter = "JPG Dosyalari(*.jpg)|*.jpg| PNG Dosyalari(*.png)|*.png| JPEG Dosyalari(*.jpeg)|*.jpeg| GIF Dosyalari(*.gif)|*.gif| TIF Dosyalari(*.tif)|*.tif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                imagePath = openFileDialog1.FileName;
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {   if(pictureBox1.Image != null && tbxNickName.Text != "")
            {
                FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                byte[] image = binaryReader.ReadBytes((int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();

                connection.Open();
                String date = Convert.ToString(DateTime.Now);
                SqlCommand command = new SqlCommand("insert into Images(Images, NickName, Upload_Date) values (@images, @nickname, @simdi)", connection);
                command.Parameters.Add("@images", SqlDbType.Image, image.Length).Value = image;
                command.Parameters.AddWithValue("@simdi", DateTime.Parse(date));
                command.Parameters.AddWithValue("@nickname", NickName);
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Resim Yukleme islemi basariyla tamamlandi.", "Islem Tamamlandi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lutfen resmin basariyla yuklendiginden veya @NickName alanini doldugunuzdan emin olun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbxNickName_TextChanged(object sender, EventArgs e)
        {
            NickName = "@" + tbxNickName.Text;
        }
    }
}
