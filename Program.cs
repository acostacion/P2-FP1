// Paula Sierra Luque
// José Tomás Gómez Becerra

using System.Drawing;
using System;
using System.Runtime.CompilerServices;

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

                // Scroll lateral + movimiento pájaro + colisiones + gestión de puntos.
                Avanza(suelo, techo, frame);

                // Renderizado.
                Render(suelo, techo, fil, frame, puntos, colision);

                Thread.Sleep(DELTA);
            }

        }

        static void Inicializa(out int[] suelo, out int[] techo, out int fil, out int ascenso, out int frame, out int puntos, ref bool colision) //done
        {
            suelo = new int[ANCHO];
            techo = new int[ANCHO];

            // Inicializamos sin obstáculos:
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
            // Limpia la consola en cada render.
            Console.Clear();

            // Color azul para pintar las paredes.
            Console.BackgroundColor = ConsoleColor.Blue;
            // Vamos recorriendo el ANCHO.
            for (int i = 0; i < ANCHO; ++i)
            {
                // TECHO. Recorremos el array de techo, desde techo[0] hasta techo[i],
                // pintando así cada coordenada (i*2,j), en cada vuelta. 
                for (int j = 0; j <= Convierte(techo[i]); j++)
                {
                    Console.SetCursorPosition(i * 2, j);
                    Console.Write("  ");
                }
                // SUELO. Recorremos el array de suelo, desde suelo[ALTO-1] hasta suelo[0] (hacia atrás),
                // pintando así cada coordenada (i*2,j), en cada vuelta. 
                for (int j = ALTO - 1; j >= Convierte(suelo[i]); j--)
                {
                    Console.SetCursorPosition(i * 2, j);
                    Console.Write("  ");
                }
            }
            // Devolvemos el color negro a la consola.
            Console.BackgroundColor = ConsoleColor.Black;

            #region Colision
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
            #endregion

            #region Debug
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
            #endregion
        }

        static void Avanza(int[] suelo, int[] techo, int frame)
        {
            // ---- 1º MOVEMOS EL ARRAY CADA POSICION A LA IZQUIERDA

            // Vamos moviendo techo hasta la PENultima posición del array.
            for(int i = 0; i < techo.Length - 1; i++)
            {
                techo[i] = techo[i + 1];
                suelo[i] = suelo[i + 1];
            }


            // ---- 2º COMPROBAMOS SI HAY QUE DAR NUEVO VALOR
            //Busqueda??¿ miramos 
            int cont = 0; //contador
            while (cont <= SEP_OBS)
            {
                //no pintamos obstaculo
                techo[techo.Length] = 7; 
                suelo[suelo.Length] = 0;
                
                if (cont == SEP_OBS) //Cuando lleguemos
                {
                    // ---- 3º DAMOS VALOR NUEVO
                    int s = rnd.Next(0, 4);
                    int t = HUECO - 1 + s;

                    suelo[suelo.Length] = s;
                    techo[techo.Length] = t;
                }
                cont++;
            }

            //Cada SEP_OBS frames se genera un nuevo obstáculo situado aleatoriamente dentro de esa columna. De
            //acuerdo a la lógica del juego se tiene 0 ≤ s < t ≤ ALTO − 1 y además el espacio de paso entre ambos
            //será HUECO, es decir: t − s = HUECO − 1.Esto puede conseguirse fácilmente generando aleatoriamente
            //el valor s en el rango apropiado y luego calculando t en función de s y HUECO.
            //Con este método ya puede hacerse una primera versión del bucle principal para mostrar el scroll lateral
            //con el suelo y el techo(y la generación de obstáculos)

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

    
