using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace redimensionar_imagem
{
    internal class program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new Thread(Redimensioar);
            thread.Start();

            Console.Read();
        }

        static void Redimensioar()
        {
            #region "Diretorios"
            string diretorio_entrada = "Arquivos_Entrada";
            string diretorio_redimensionado = "Arquivos_Redimensionar";
            string diretorio_finalizado = "Arquivos_Finalizados";

            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }

            if (!Directory.Exists(diretorio_redimensionado))
            {
                Directory.CreateDirectory(diretorio_redimensionado);
            }

            if (!Directory.Exists(diretorio_finalizado))
            {
                Directory.CreateDirectory(diretorio_finalizado);
            }

            FileStream fileStream;
            FileInfo fileInfo;
            #endregion
            while (true)
            {
                //Meru programa vai olhar para a pasta de entrada
                //Se tiver arquivo ele ira redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);

                //ler o tamanho que irá redimensionar
                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada)
                {

                     fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite); /// usado para ler o arquivo de varias formas
                     fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionado 
                        + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name;

                    //Redimensiona + Copia os arquivos redimensionados para a pasta de redimensionados
                    Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                    //Fecha o arquivo
                    fileStream.Close();

                    //move o arquivo de entrada para a pasta de finalizados
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(caminhoFinalizado);
                }

                Thread.Sleep(new TimeSpan(0, 0, 5));
            }

            static void Redimensionador(Image imagem, int altura, string caminho)
            {
                double ratio = (double)altura / imagem.Height;
                int novaLargura = (int)(imagem.Width * ratio);
                int novaAltura = (int)(imagem.Height * ratio);

                Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

                using(Graphics g = Graphics.FromImage(novaImage))
                {
                    g.DrawImage(imagem, 0, 0, novaLargura, novaAltura); // zerando posição da img
                }

                novaImage.Save(caminho);
                imagem.Dispose(); // tira da memória
            }
        }
    }
}