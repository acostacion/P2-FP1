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
        { 
            // Declaraciones anteriores.
            int[] suelo = { 0, 1, 0, 0, 2, 0, 0, 3, 0 }, techo = { 7, 5, 7, 7, 6, 7, 7, 7, 7 };
            int fil, ascenso, frame, puntos;
            bool colision = true;

            // Inicialización.
            Inicializa(out suelo, out techo, out fil, out ascenso, out frame, out puntos, ref colision);

            // Renderizado.
            Render(suelo, techo, fil, frame, puntos, colision);

            // Bucle principal.
            while(!colision)
            {
                // Lectura de input.
                LeeInput();

                // Si el juego continua.

                // Scrool lateral + movimiento pájaro + colisiones + gestión de puntos.

                // Renderizado.
                Render(suelo, techo, fil, frame, puntos, colision);

                Thread.Sleep(DELTA);
            }

        }

        static void Inicializa(out int[] suelo, out int[] techo, out int fil, out int ascenso, out int frame, out int puntos, ref bool colision) //done.
        {
            suelo = new int[ANCHO];
            techo = new int[ANCHO];
            for (int i = 0; i<ANCHO; i++)
            {
                suelo[i] = 0;
                techo[i]= 7;    
            }
            fil = ALTO / 2;
            ascenso = -1;
            frame = puntos = 0;
            colision = false;
        }

        static void Render(int[] suelo, int[] techo, int fil, int frame, int puntos, bool colision)
        {
            // Limpia la consola.
            Console.Clear();

            /*// Dibuja el techo y el suelo en la pantalla. El bucle va pasando de 0 a ANCHO y va moviendose por los huecos del array.
            for (int i = 0; i < ANCHO; i++)
            { 
                // La posición en X es i*2 para dar espacio a cada columna.
                // La posición en Y es el valor de techo[i] para dibujar el techo desde la parte superior de la consola.
                Console.SetCursorPosition(i * 2, techo[i]);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("  ");

                // La posición en X es i*2 para dar espacio a cada columna.
                // LO DE ALTO - suelo[i] no lo entiendo pero me lo ha sugerido una IA. INVESTIGARLO. ES QUE NO SABIA HACERLO FUNCIONAR EN ESTA PARTE Y HE 
                // HECHO TRAMPAS PERDON POLI :(.
                Console.SetCursorPosition(i * 2, ALTO - suelo[i]);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("  ");
            }*/
           

            // I ES PARA MOVER A TRAVÉS DE LAS COLUMNAS.
            // J PARA MOVER A TRAVÉS DE LAS FILAS.

            // La idea es poner el huevo e ir restando pa'trás y to eso. Colocas lo del suelo o techo y vas echando pa atrás con el for.
            // Necesitas el cambio de coords para hacerlo.
            for (int i = 0; i < ANCHO; i++)
            {
                for(int j = suelo[suelo.Length-1]; j > 0; j--)
                {
                    Console.SetCursorPosition(i, j);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("A");
                }

                for(int j = suelo.Length + 1; j < techo[0]; j++)
                {
                    /*Console.SetCursorPosition(i, suelo[j]);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("A");*/
                }

                for(int j = techo[0]; j < techo.Length; j++)
                {
                    Console.SetCursorPosition(i, techo[j]);
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("A");
                }
                

                /*Console.SetCursorPosition(i, )

                Console.SetCursorPosition(i, rnd.Next(HUECO + 1, techo.Length));
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");*/
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

        static char LeeInput()
        {
            char ch = ' ';
            if (Console.KeyAvailable)
            {
                string s = Console.ReadKey(true).Key.ToString();
                if (s == "X" || s == "Spacebar") ch = 'i'; // Impulso.                   
                else if (s == "P") ch = 'p'; // Pausa.					
                else if (s == "Q" || s == "Escape") ch = 'q'; // Salir.
                while (Console.KeyAvailable) Console.ReadKey();
            }
            return ch;
        }

        static int Convierte(int c)
        {
            return (ALTO - 1) - c;
        }
    }
}

    
