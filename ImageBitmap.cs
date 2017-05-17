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
        /// <summary>
        /// Je n'ai pas mis les accents dans les commentaires destiné au fichier XML car certains editeurs de texte comme WordPad n'ont pas l'utf8..
        /// </summary>
        //champs
        int tailleFichier;
        int offset;
        int largeur;
        int hauteur;
        int byteNumberPerColor;
        int[,] board;
        byte[] header;
        const int WIDTH_INDEX = 18; // Index du tableau à partir desquels on convertit les valeurs nécessaires
        const int HEIGHT_INDEX = 22;
        const int BYTE_NUMBER_INDEX = 28;
        const int OFFSET_INDEX = 10;
        const int FILE_WEIGHT_INDEX = 2;

        //Constructeur(s)
        public ImageBitmap(string fileName)
        {
            byte[] fichier = File.ReadAllBytes(fileName);
            this.largeur = Convert_Endian_To_Int(fichier, WIDTH_INDEX,4); //conversion des couples du header en entier 
            this.hauteur = Convert_Endian_To_Int(fichier, HEIGHT_INDEX,4);
            this.byteNumberPerColor = Convert_Endian_To_Int(fichier, BYTE_NUMBER_INDEX,2);
            this.offset = fichier[OFFSET_INDEX];
            this.tailleFichier = Convert_Endian_To_Int(fichier, FILE_WEIGHT_INDEX,4);
            header = new byte[offset];
            for (int i = 0; i <offset;i++)
            {
                this.header[i] = fichier[i];
            }
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
        /// <summary>
        /// Ecriture de fichier
        /// </summary>
        /// <param name="fileName"> nom que l'on souhaite donner au fichier contenant l'image </param>
        /// <param name="fichier"> tableau de bytes contenant l'image a ecrire </param>
        public void From_Image_To_File(string fileName, byte[] fichier)
        {
            File.WriteAllBytes(fileName,fichier);

        }
        /// <summary>
        /// Conversion Endian => Int
        /// </summary>
        /// <param name="fichier"> tableau de bytes contenant l'image </param>
        /// <param name="source"> indice de depart dans le tableau </param>
        /// <param name="tailleCouple"> nombre de cases que l'ont souhaite prendre en compte a partir de l'indice de depart </param>
        /// <returns> Un entier a partir du tableau de bytes converti </returns>
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
        /// <summary>
        /// Conversion Int => Endian
        /// </summary>
        /// <param name="entier"> entier a convertir</param>
        /// <returns> tableau de bytes converti a partir de l'entier entre en parametre </returns>
        public byte[] Convert_Int_To_Endian(int entier)
        {
            byte[]byteArray = BitConverter.GetBytes(entier); // conversion de l'entier entré en paramètre en 4 octets stockés dans un tableau de bytes
            byte[] toReturnBytesArray = new byte[byteArray.Length-1];
            for(int i = 0; i < toReturnBytesArray.Length;i++)
            {
                toReturnBytesArray[i] = byteArray[i + 1]; // stockage dans un autre tableau pour supprimer le 0 car on utilisera cette conversion uniquement pour les triplets d'octets propre à l'image
            }

            return toReturnBytesArray;
        }
        /// <summary>
        /// Rotation d'angle 90 degres
        /// </summary>
        /// <param name="board"> matrice d'entiers contenant l'image </param>
        /// <returns> nouvelle matrice contenant l'image apres rotation </returns>
        public int[,] Rotation90 (int[,] board)
        {
            int[,] Clone = new int[largeur, hauteur]; //création matrice aux dimensions inversées
            for(int i = 0; i<hauteur;i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Clone[j, i] = board[i,j];
                }
            }
            return Clone;
        }
        /// <summary>
        /// Rotation d'angle 180 degres
        /// </summary>
        /// <param name="board"> matrice d'entiers contenant l'image </param>
        /// <returns> nouvelle matrice contenant l'image apres rotation </returns>
        public int[,] Rotation180(int[,] board)
        {
            int[,] Clone = new int[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Clone[hauteur-1-i,largeur-1-j] = board[i,j];
                }
            }
            return Clone;
        }
        /// <summary>
        /// Rotation d'angle 270 degres
        /// </summary>
        /// <param name="board"> matrice d'entiers contenant l'image </param>
        /// <returns> nouvelle matrice contenant l'image apres rotation </returns>
        public int[,] Rotation270(int[,] board)
        {
            int[,] Clone = new int[largeur, hauteur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Clone[j,i] = board[i,j];
                }
            }
            return Clone;
        }
        /// <summary>
        /// From Board To Array
        /// </summary>
        /// <param name="board"> Matrice contenant les bytes convertis </param>
        /// <returns> Tableau contenant les bytes convertis pret a etre lu </returns>
        public byte[] From_Board_To_Array (int[,]board) 
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
                array[i] = header[i]; // stockage du header et des infos
            }
            byte[] boardLength1 = BitConverter.GetBytes(board.GetLength(0)); //conversions des dimensions de la matrice
            byte[] boardLength2 = BitConverter.GetBytes(board.GetLength(1));

            for (int i = 0; i <4; i++ )
            {
                array[HEIGHT_INDEX + i] = boardLength1[i]; // remplacement par les nouvelles dimensions de la matrice
                array[WIDTH_INDEX + i] = boardLength2[i];
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
        /// <summary>
        /// Creation d'image et affichage : initiale du prenom
        /// </summary>
        /// <param name="hauteur"> dimension 0 de l'image </param>
        /// <param name="largeur"> dimension 1 de l'image </param>
        /// <returns> matrice d'entiers contenant la nouvelle image </returns>
        public int[,] CreateImage(int hauteur, int largeur) // En travaux
        {
            int[,] matrix = new int [hauteur, largeur];
            int j = hauteur / 2;
            int k = 1;
            for (int i = 3 * hauteur / 4; i > hauteur / 4; i--)
            {
                matrix[i, j - (k / 2)] = -256; //diagonale ascendante
                matrix[i, j + (k++ / 2)] = -256; // diagonale descendante                  
            }
            for(int i = 3*hauteur/8; i< 5*hauteur/8;i++)
            {
                matrix[hauteur / 2, i] = -256;// trait horizontal
            }
            return matrix;
        }
        /// <summary>
        /// Matrice de Convolution - Renforcement des Bords
        /// </summary>
        /// <param name="matrix"> Matrice d'entiers contenant l'image </param>
        /// <returns> matrice resultante de l'application du filtre </returns>
        public int[,] RenforcementDesBords(int[,] matrix)
        {
            int[,] ConvolutionMatrix = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];
            
            for(int i = 1; i<matrix.GetLength(0)-1;i++)
            {
                for(int j = 1; j < matrix.GetLength(1)-1;j++) // opération pour la case et les 8 voisins
                {
                    result[i, j] = matrix[i - 1, j - 1] * ConvolutionMatrix[0, 0] + matrix[i - 1, j] * ConvolutionMatrix[0, 1] + matrix[i - 1, j + 1] * ConvolutionMatrix[0, 2] + matrix[i, j - 1] * ConvolutionMatrix[1, 0] + matrix[i + 1, j - 1] * ConvolutionMatrix[2, 0] + matrix[i, j + 1] * ConvolutionMatrix[1, 2] + matrix[i + 1, j] * ConvolutionMatrix[2, 1] + matrix[i + 1, j + 1] * ConvolutionMatrix[2, 2] + matrix[i,j] * ConvolutionMatrix[1,1];
                }
            }
            return result;
            
        }
        /// <summary>
        /// Innovation : filtre negatif
        /// </summary>
        /// <param name="matrix"> Matrice d'entiers content l'image </param>
        /// <returns> matrice resultante de l'application du filtre </returns>
        public int[,] Innovation(int[,] matrix)
        {
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i=0; i<matrix.GetLength(0);i ++)
            {
                for(int j = 0; j<matrix.GetLength(1);j++)
                {
                    result[i, j] =  - matrix[i, j]; // valeur négative 
                }
            }
            return result;
        }
    }
}
