// Paula Sierra Luque
// José Tomás Gómez Becerra

namespace P2_FP1
{
    class Program
    {
        static Random rnd = new Random(); // Generador de aleatorios para colocar los obstáculos verticales.

        #region Constantes
        const bool DEBUG = true; // Para sacar información de depuración en el Renderizado.

        const int ANCHO = 9, ALTO = 8, // Tamaño del área de juego.

        SEP_OBS = 3, // Separación horizontal entre obstáculos.

        HUECO = 3, // Hueco de los obstáculos (en vertical).

        COL_BIRD = ANCHO / 3, // Columna fija del pájaro.

        IMPULSO = 3, // Unidades de ascenso por aleteo.

        DELTA = 300; // Retardo entre frames (ms).
        #endregion

        static void Main()
        { // programa principal
            int[] suelo = { 0, 1, 0, 0, 2, 0, 0, 3, 0 }, techo = { 7, 5, 7, 7, 6, 7, 7, 7, 7 };
            int fil, ascenso, frame, puntos;
            bool colision = true;

            Inicializa(out suelo, out techo, out fil, out ascenso, out frame, out puntos, ref colision);
            Render(suelo, techo, fil, frame, puntos, colision);
            while(!colision)
            {

            }

        }

        static void Inicializa(out int[] suelo, out int[] techo, out int fil, out int ascenso, out int frame, out int puntos, ref bool colision) //done.
        {
            suelo = new int[ANCHO];
            techo = new int[ANCHO];
            fil = ALTO / 2;
            ascenso = -1;
            frame = puntos = 0;
            colision = false;
        }

        static void Render(int[] suelo, int[] techo, int fil, int frame, int puntos, bool colision)
        {
            Console.Clear();

            for (int i = 0; i < ANCHO; i++)
            {
                Console.SetCursorPosition(techo[i], 0);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(" ");

                Console.SetCursorPosition(suelo[i], ALTO);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
            }

            //if(!colision)
            //{
            //    Console.SetCursorPosition(fil, COL_BIRD);
            //    Console.BackgroundColor = ConsoleColor.Magenta;
            //    Console.Write("->");
            //}
            //else
            //{
            //    Console.SetCursorPosition(fil, COL_BIRD);
            //    Console.BackgroundColor = ConsoleColor.Red;
            //    Console.Write("**");
            //}

            //if (DEBUG)
            //{
            //    Console.SetCursorPosition(0, ALTO + 2);
            //    Console.WriteLine("Puntos: " + puntos);
            //    for (int i = 0; i < suelo.Length; i++)
            //    {
            //        Console.Write(" " + techo[i]);
            //        Console.WriteLine("");
            //        Console.Write(" " + suelo[i]);
            //        Console.WriteLine("");
            //    }
            //    Console.WriteLine("Pos bird: " + fil);
            //    Console.WriteLine("Frame:  " + frame);
            //}
            
            











            /*for(int i = 0; i < suelo.Length; i++)
            {
                suelo[i] = rnd.Next(0,4);
                techo[i] = suelo[i] + HUECO;                           
            }*/



        }
    }
}

    
