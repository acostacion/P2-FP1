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
            // Limpia la consola.
            Console.Clear();

            // Dibuja el techo y el suelo en la pantalla. El bucle va pasando de 0 a ANCHO y va moviendose por los huecos del array.
            for (int i = 0; i < ANCHO; i++)
            {
                // Dibuja un bloque (por cada iteración) en la posición correspondiente al techo.
                Console.SetCursorPosition(i * 2, ALTO - techo[i]);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("  ");

                // Dibuja un bloque (por cada iteración) en la posición correspondiente al suelo.
                Console.SetCursorPosition(i * 2, ALTO - suelo[i]);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("  ");
            }

            // Si no hay colisión...
            if (!colision)
            {
                // Dibuja el pájaro con fondo magenta.
                Console.SetCursorPosition(fil, COL_BIRD);
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("->");
            }
            // Si hay colisión...
            else
            {
                // Dibuja la muerte del pájaro en rojo.
                Console.SetCursorPosition(fil, COL_BIRD);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("**");
            }

            // Si está el debug activado...
            if (DEBUG)
            {
                // Ponemos el fondo negro para el debug.
                Console.BackgroundColor = ConsoleColor.Black;

                // Muestra los puntos.
                Console.SetCursorPosition(0, ALTO + 2);
                Console.WriteLine("Puntos: " + puntos);

                // Muestra los valores del techo.
                for (int i = 0; i < techo.Length; i++)
                {
                    Console.Write(" " + techo[i]);
                }
                Console.Write(" (techo)");
                Console.WriteLine("");

                // Muestra los valores del suelo.
                for(int i = 0; i < suelo.Length; i++)
                {
                    Console.Write(" " + suelo[i]);
                }
                Console.Write(" (suelo)");
                Console.WriteLine("");

                // Muestra la posición del pájaro.
                Console.Write("Pos bird: " + fil + "  ");

                // Muestra el valor de los frames.
                Console.WriteLine("Frame:  " + frame);
            }
        }

    }
}

    
