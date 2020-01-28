using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace FileConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ArquivoExiste = File.Exists("C:\\Users\\joaog\\Desktop\\Curso Tópicos.txt");

            string arquivo = "C:\\Users\\joaog\\Desktop\\TesteCurso.txt";

            string conteudo = File.ReadAllText("C:\\Users\\joaog\\Desktop\\Curso Tópicos.txt");
            //File.WriteAllText(arquivo, conteudo);


            using (FileStream fs = new FileStream(arquivo, FileMode.OpenOrCreate))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(conteudo);

                fs.Write(bytes, 0, bytes.Length);

                fs.Flush();
            }


            using (FileStream fs = new FileStream(arquivo, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(conteudo);
                }
            }


            Bitmap bmp = new Bitmap("C:\\Users\\joaog\\Desktop\\imagem1.bmp");

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    //Color color = Color.FromArgb(Convert.ToInt32(c.R * 0.3), Convert.ToInt32(c.G * 0.5), Convert.ToInt32(c.B * 0.7));
                    int cor = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bmp.SetPixel(i, j, Color.FromArgb(cor, cor, cor));
                }
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            watch.Reset();

            Bitmap bmp1 = new Bitmap("C:\\Users\\joaog\\Desktop\\imagem1.bmp");
            watch.Start();
            bmp1 = processImage(bmp1);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            bmp.Save("C:\\Users\\joaog\\Desktop\\imagem2.bmp");

            using (FileStream fs = new FileStream(arquivo, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    string conteudo1 = reader.ReadToEnd();
                }
            }

        }

        //Para permitir código não seguro (unsafe), acessar propriedades do projeto -> Build -> Allow Unsafe Code
        public static Bitmap processImage(Bitmap image)
        {
            Bitmap returnMap = new Bitmap(image.Width, image.Height,
                                   PixelFormat.Format32bppArgb);
            BitmapData bitmapData1 = image.LockBits(new Rectangle(0, 0,
                                     image.Width, image.Height),
                                     ImageLockMode.ReadOnly,
                                     PixelFormat.Format32bppArgb);
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0,
                                     returnMap.Width, returnMap.Height),
                                     ImageLockMode.ReadOnly,
                                     PixelFormat.Format32bppArgb);
            int a = 0;
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;
                for (int i = 0; i < bitmapData1.Height; i++)
                {
                    for (int j = 0; j < bitmapData1.Width; j++)
                    {
                        // write the logic implementation here
                        a = (imagePointer1[0] + imagePointer1[1] +
                             imagePointer1[2]) / 3;
                        imagePointer2[0] = (byte)a;
                        imagePointer2[1] = (byte)a;
                        imagePointer2[2] = (byte)a;
                        imagePointer2[3] = imagePointer1[3];
                        //4 bytes per pixel
                        imagePointer1 += 4;
                        imagePointer2 += 4;
                    }//end for j
                     //4 bytes per pixel
                    imagePointer1 += bitmapData1.Stride -
                                    (bitmapData1.Width * 4);
                    imagePointer2 += bitmapData1.Stride -
                                    (bitmapData1.Width * 4);
                }//end for i
            }//end unsafe
            returnMap.UnlockBits(bitmapData2);
            image.UnlockBits(bitmapData1);
            return returnMap;
        }//end processImage
    }
}
