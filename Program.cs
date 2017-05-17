using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetInfo
{
    class Program
    {
        const int BMP_HEADER_BYTE_NUMBER = 14;
        const int BMP_HEADER_AND_INFO_BYTE_NUMBER = 54;
        /// <summary>
        /// Affiche le tableau de bytes entré en paramètres de façon lisible
        /// </summary>
        /// <param name="tab"> tableau de bytes que l'on souhaite afficher </param>
        static void Affiche_Lecture(byte[] tab)
        {
            Console.WriteLine(" \n HEADER \n");

            for (int i = 0; i < BMP_HEADER_BYTE_NUMBER; i++)
            {
                Console.Write(tab[i] + " ");
            }
            Console.WriteLine("\n HEADER INFO \n");

            for (int i = BMP_HEADER_BYTE_NUMBER; i < BMP_HEADER_AND_INFO_BYTE_NUMBER; i++)
            {
                Console.Write(tab[i] + " ");
            }
            Console.WriteLine("\n IMAGE \n");
            for (int i = BMP_HEADER_AND_INFO_BYTE_NUMBER; i < tab.Length; i++)
            {
                Console.Write(tab[i] + "\t");
            }

        }
        static void Main(string[] args)
        {
            ImageBitmap image = new ImageBitmap("Test003.bmp");
            string name;
            bool menu = false;
            while (menu == false)
            {
                try
                {
                    Console.WriteLine("Veuillez choisir l'image que vous souhaitez traiter : " + "\n" + "1) Lena" + "\n" + "2) Lac en montagnes");
                    int ImageRequest = int.Parse(Console.ReadLine());
                    switch(ImageRequest)
                    {
                        case 1:
                            name = "lena.bmp";
                                image = new ImageBitmap(name);
                                break;
                        case 2:
                                name = "lac_en_montagne.bmp";
                                image = new ImageBitmap(name);
                                break;
                    }  
                    Console.WriteLine("\n" + "------ MENU------ Entrez le numéro correspondant à ce que vous voulez vérifier :" + "\n" + "1) Test de conversion endian=>int et vice versa" + "\n" + "2) Rotation de l'image" + "\n" + "3) Création d'image avec initiale" + "\n" + "4) Matrice de convolution : renforcement des bords" + "\n" + "5) Innovation : filtre négatif" + "\n" + "6) Quitter le programme");
                    int request = int.Parse(Console.ReadLine());
                    switch (request)
                    {
                        case 1:
                            Console.WriteLine("\n" + "Choisissez une valeur entière à convertir en tableau de byte format little endian :");
                            int ConversionRequest = int.Parse(Console.ReadLine());
                            byte[] testTab = BitConverter.GetBytes(ConversionRequest); // test conversion d'un entier en 4 octets
                            Console.WriteLine("\n" + "Affichage du tableau de byte format little endian : ");
                            Console.Write("[");
                            for (int i = 0; i < testTab.Length; i++)
                            {
                                Console.Write(testTab[i]);
                                if (i != testTab.Length - 1)
                                {
                                    Console.Write(", ");
                                }
                            }
                            Console.Write("]");
                            Console.WriteLine("\n" + "Conversion dans le sens inverse pour vérifier qu'on retrouve la valeur");
                            Console.WriteLine(BitConverter.ToInt32(testTab,0)); // test conversion d'un tableau de 4 octets en entier
                            Console.ReadLine();
                            Console.Clear();

                            break;
                        case 2:
                            Console.WriteLine("Choisissez l'angle de rotation : " + "\n" + "1) 90°" + "\n" + "2) 180°" + "\n" + "3) 270°");
                            int RotationRequest = int.Parse(Console.ReadLine());
                            switch (RotationRequest)
                            {
                                case 1:
                                    int[,] rotatedBoard = image.Rotation90(image.Board);
                                    byte[] writtenTab = image.From_Board_To_Array(rotatedBoard); // passage de la matrice d'entier à un tableau de byte
                                    image.From_Image_To_File("90°Rotation.bmp", writtenTab); // écriture de l'instance image dans un fichier WrittenFile qui est écrasé si existant (consultable dans le dossier debug)
                                    Console.WriteLine("\n" + "Rotation effectuée : veuillez ouvrir 90°Rotation.bmp dans le dossier debug");
                                    break;
                                case 2:
                                    int[,] rotatedBoard2 = image.Rotation180(image.Board);
                                    byte[] writtenTab2 = image.From_Board_To_Array(rotatedBoard2);
                                    image.From_Image_To_File("180°Rotation.bmp", writtenTab2);
                                    Console.WriteLine("\n" + "Rotation effectuée : veuillez ouvrir 180°Rotation.bmp dans le dossier debug");
                                    break;
                                case 3:
                                    int[,] rotatedBoard3 = image.Rotation270(image.Board);
                                    byte[] writtenTab3 = image.From_Board_To_Array(rotatedBoard3);
                                    image.From_Image_To_File("270°Rotation.bmp", writtenTab3);
                                    Console.WriteLine("\n" + "Rotation effectuée : veuillez ouvrir 270°Rotation.bmp dans le dossier debug");
                                    break;
                            }
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 3:
                            byte[] writtenTab4 = image.From_Board_To_Array(image.CreateImage(400, 400));
                            image.From_Image_To_File("NewImage.bmp", writtenTab4);
                            Console.WriteLine("\n" + "Image créee : veuillez ouvrir NewImage.bmp dans le dossier debug");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 4:
                            int[,] RenforcedBoard = image.RenforcementDesBords(image.Board);
                            byte[] writtenTab5 = image.From_Board_To_Array(RenforcedBoard);
                            image.From_Image_To_File("Convolution.bmp", writtenTab5);
                            Console.WriteLine("\n" + "La matrice de convolution a bien été appliquée, veuillez ouvrir Convolution.bmp dans le dossier debug ");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 5:
                            int[,] Innovation = image.Innovation(image.Board);
                            byte[] writtenTab6 = image.From_Board_To_Array(Innovation);
                            image.From_Image_To_File("Innovation.bmp", writtenTab6);
                            Console.WriteLine("\n" + "Fait. Veuillez ouvrir Innovation.bmp dans le dossier debug");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 6:
                            menu = true;
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Valeur erronée");
                }
                
                  
            }
        }
    }
}
