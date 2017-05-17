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
            ImageBitmap image = new ImageBitmap("lac_en_montagne.bmp");       //création d'une instance de la classe ImageBitmap 


            // Test Conversion

            /*
            Console.WriteLine(image.Convert_Endian_To_Int(image.Fichier,18,4)); // test et affichage conversion de la largeur de l'image 
            int valeur = image.Convert_Endian_To_Int(image.Fichier, image.Offset, 3);
            Console.WriteLine(valeur);
            Console.WriteLine(image.Fichier[image.Offset]);
            Console.WriteLine(image.Fichier[image.Offset+1]);
            Console.WriteLine(image.Fichier[image.Offset+2]);
            Console.ReadLine();
            byte[] testTab = image.Convert_Int_To_Endian(valeur); // test conversion d'un entier en 4 octets
            Affiche_Lecture(testab);
            Console.ReadLine();*/


            byte[] writtenTab = image.From_Board_To_Array(image.Board); // passage de la matrice d'entier à un tableau de byte
            image.From_Image_To_File("WrittenFile.bmp", writtenTab); // écriture de l'instance image dans un fichier WrittenFile qui est écrasé si existant (consultable dans le dossier debug)


            // Test comparaison par affichage : fichier lu et fichier écrit (plus lisible quand il s'agit du fichier Test003.bmp)

            /*Affiche_Lecture(image.Fichier);// Affichage du tableau d'octets contenu dans l'image 
            Affiche_Lecture(writtenTab);*/

            Console.ReadLine();

        }
    }
}
