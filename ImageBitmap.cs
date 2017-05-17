using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetInfo
{
    class ImageBitmap
    {
        //champs
        int tailleFichier;
        int offset;
        int largeur;
        int hauteur;
        int byteNumberPerColor;
        int[,] board;
        byte[] fichier;
        const int WIDTH_INDEX = 18; // Index du tableau à partir desquels on convertit les valeurs nécessaires
        const int HEIGHT_INDEX = 22;
        const int BYTE_NUMBER_INDEX = 28;
        const int OFFSET_INDEX = 10;
        const int FILE_WEIGHT_INDEX = 2;

        //Constructeur(s)
        public ImageBitmap(string fileName)
        {
            this.fichier = File.ReadAllBytes(fileName);
            this.largeur = Convert_Endian_To_Int(fichier, WIDTH_INDEX,4); //conversion des couples du header en entier 
            this.hauteur = Convert_Endian_To_Int(fichier, HEIGHT_INDEX,4);
            this.byteNumberPerColor = Convert_Endian_To_Int(fichier, BYTE_NUMBER_INDEX,2);
            this.offset = fichier[OFFSET_INDEX];
            this.tailleFichier = Convert_Endian_To_Int(fichier, FILE_WEIGHT_INDEX,4);
            board = new int[hauteur, largeur];
            int source = offset;
            for(int i = 0; i < hauteur;i++)
            {
                
                for (int j = 0; j < largeur; j++)
                {
                    board[i, j] = Convert_Endian_To_Int(fichier, source, 3); // on remplit la matrice par des int convertits à partir du tableau de bytes
                    source += 3; // on incrémente de 3 car on traite 3 octets par 3 octets
                }
            }

        }

        //Propriété(s)
        public byte[] Fichier
        {
            get { return this.fichier;}
            set { this.fichier = value; }
        }
        public int Largeur
        {
            get { return this.largeur; }
        }
        public int Hauteur
        {
            get { return this.hauteur; }
        }
        public int ByteNumberPerColor
        {
            get { return this.byteNumberPerColor; }
        }
        public int Offset
        {
            get { return this.offset; }
        }
        public int TailleFichier
        {
            get { return tailleFichier; }
        }
        public int[,] Board
        {
            get { return this.board; }
        }

        //Méthodes
        public void From_Image_To_File(string fileName, byte[] fichier)
        {
            File.WriteAllBytes(fileName,fichier);

        }
        public int Convert_Endian_To_Int(byte[] fichier,int source, int tailleCouple) // la source correspond au i-ème nombres de cases de taille tailleCouple que l'ont souhaite convertir
        {
            byte[] Bytes_Array = new byte[4]; //création d'un tableau qui contiendra les octets à convertir
            if(tailleCouple<Bytes_Array.Length) // on ajoute des 0 si le tableau ne contient pas 4 octets de manière à pouvoir utiliser int32
            {
                for(int i = 0; i < Bytes_Array.Length-tailleCouple; i++)
                {
                    Bytes_Array[i] = 0;
                }
            }
            for (int i = 0; i < tailleCouple; i++)
            {
                Bytes_Array[Bytes_Array.Length-tailleCouple + i] = fichier[source + i]; //Algo pour stocker les octets 
            }
            int entier = BitConverter.ToInt32(Bytes_Array, 0); //conversion du tableau en entier, sans inversion du tableau car BitConverter tient compte de l'endianess
            return entier;
        }

        public byte[] Convert_Int_To_Endian(int entier)
        {
            byte[]byteArray = BitConverter.GetBytes(entier); // conversion de l'entier entré en paramètre en 4 octets stockés dans un tableau de bytes
            byte[] toReturnBytesArray = new byte[byteArray.Length - 1];
            for(int i = 0; i < toReturnBytesArray.Length;i++)
            {
                toReturnBytesArray[i] = byteArray[i + 1]; // stockage dans un autre tableau pour supprimer le 0 car on utilisera cette conversion uniquement pour les triplets d'octets propre à l'image
            }

            return toReturnBytesArray;
        }
        public int[,] Rotation90 (int[,] board)
        {
            int[,] Clone = new int[largeur, hauteur];
            for(int i = 0; i<largeur;i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    Clone[i, j] = board[hauteur - i, largeur - j];//En travaux (Recherche de l'algorythme)
                }
            }
            return Clone;
        }
        public int[,] Rotation180(int[,] board)
        {
            int[,] Clone = new int[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Clone[i, j] = board[ - 1 - i,hauteur - 1 - j];//Toujours en travaux (Recherche de l'algorythme)
                }
            }
            return Clone;
        }
        public int[,] Rotation270(int[,] board)
        {
            int[,] Clone = new int[largeur, hauteur];
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    Clone[i, j] = board[hauteur - i, largeur - j];//Toujours en travaux (Recherche de l'algorythme)
                }
            }
            return Clone;
        }

        public byte[] From_Board_To_Array (int[,]board) // passage d'une matrice d'int à un tableau de bytes
        {
            byte[][] doubleArray = new byte[board.GetLength(1)* board.GetLength(0)][]; //création d'un double tableau
            for(int i = 0; i < board.GetLength(0);i++)
            {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    doubleArray[i * board.GetLength(1) + j] = Convert_Int_To_Endian(board[i, j]); // conversion des entiers en bytes et stockage dans un double tableau
                }
            }
            byte[] array = new byte[offset + 3 * board.GetLength(0) * board.GetLength(1)]; // taille 3 fois supérieure au double tableau de manière à obtenir l'espace nécessaire au passage d'un entier à 3 bytes
            for(int i = 0; i < offset;i++)
            {
                array[i] = fichier[i]; // stockage du header et des infos
            }
            for(int i = 0; i<doubleArray.Length;i++)
            {
                for(int j = 0; j<3;j++)
                {
                    array[offset + 3*i + j] = doubleArray[i][j]; // stockage du double tableau dans un seul tableau
                }
            }
            return array;
        }
    }
}
