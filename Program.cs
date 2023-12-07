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

        const int ANCHO = 22, ALTO = 14, // Tamaño del área de juego.

        SEP_OBS = 7, // Separación horizontal entre obstáculos.

        HUECO = 7, // Hueco de los obstáculos (en vertical).

        COL_BIRD = ANCHO / 3, // Columna fija del pájaro.

        IMPULSO = 3, // Unidades de ascenso por aleteo.

        DELTA = 300; // Retardo entre frames (ms).
        #endregion

        static void Main()
        { 
            // Declaraciones anteriores.
            int[] suelo, techo;
            int fil, ascenso, frame, puntos;
            bool colision;

            // Inicialización.
            Inicializa(out suelo, out techo, out fil, out ascenso, out frame, out puntos, out colision);

            // Renderizado inicial.
            Render(suelo, techo, fil, frame, puntos, colision);

            // Bucle principal.
            while(!colision)
            {
                // Lectura de input.
                char c = LeeInput();

                // Si el juego continua.
                if(c != 'q')
                {
                    // Scroll lateral + movimiento pájaro + colisiones + gestión de puntos.
                    Avanza(suelo, techo, frame);
                    Mueve(c, ref fil, ref ascenso);
                    Colision(suelo, techo, fil);
                    //Puntua(suelo, techo, ref puntos);

                    // Renderizado.
                    Render(suelo, techo, fil, frame, puntos, colision);

                    Thread.Sleep(DELTA);

                    frame++;
                }
            }
        }

        static void Inicializa(out int[] suelo, out int[] techo, out int fil, out int ascenso, out int frame, out int puntos, out bool colision) //done
        {
            suelo = new int[ANCHO];
            techo = new int[ANCHO];

            // Inicializamos sin obstáculos:
            for (int i = 0; i<ANCHO; i++)
            {
                // Suelo sin obstáculos.
                suelo[i] = 0;

                // Techo sin obstáculos.
                techo[i]= ALTO - 1;    
            }

            fil = ALTO / 2;
            ascenso = -1;
            frame = puntos = 0;
            colision = false;
        }

        static void Render(int[] suelo, int[] techo, int fil, int frame, int puntos, bool colision) // revisar
        {
            // Limpia la consola en cada render.
            Console.Clear();

            // Vamos recorriendo el ANCHO.
            for (int i = 0; i < ANCHO; i++)
            {
                // TECHO. Recorremos el array de techo, desde techo[0] hasta techo[i],
                // pintando así cada coordenada (i*2,j), en cada vuelta. 
                for (int j = 1; j <= Convierte(techo[i]); j++)
                {
                    Console.SetCursorPosition(i * 2, j);
                    // Color azul para pintar las paredes.
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("  ");
                    // Devolvemos el color negro a la consola.
                    Console.ResetColor();
                }

                // SUELO. Recorremos el array de suelo, desde suelo[ALTO-1] hasta suelo[0] (hacia atrás),
                // pintando así cada coordenada (i*2,j), en cada vuelta. 
                for (int j = Convierte(suelo[i]); j <= ALTO; j++)
                {
                    Console.SetCursorPosition(i * 2, j);
                    // Color azul para pintar las paredes.
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("  ");
                    // Devolvemos el color negro a la consola.
                    Console.ResetColor();
                }
            }

            #region Colision
            // Si no hay colisión...
            if (!colision)
            {
                // Dibuja el pájaro con fondo magenta.
                Console.SetCursorPosition(COL_BIRD * 2, ALTO - fil - 1);
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("->");
            }
            // Si hay colisión...
            else
            {
                // Dibuja la muerte del pájaro en rojo.
                Console.SetCursorPosition(COL_BIRD * 2, ALTO - fil - 1);
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

        static void Avanza(int[] suelo, int[] techo, int frame) // done (?)
        {
            // NOTA DE JT: la última posición del array es x.Length - 1, no techo.Length.

            // Desplazamos todos los elementos una posición a la izquierda.
            for (int i = 0; i < techo.Length - 1; i++)
            {
                techo[i] = techo[i + 1];
                suelo[i] = suelo[i + 1];
            }

            int s, t;

            // Comprobamos si debemos generar un nuevo obstáculo.
            if (frame % SEP_OBS == 0)
            {
                // Generamos "s" aleatoriamente dentro del rango.
                s = rnd.Next(0, ALTO - HUECO);

                // Generamos "t" basándonos en "s".
                t = HUECO - 1 + s;

                // Asignamos los nuevos valores a la última posición del array.
                suelo[ANCHO - 1] = s;
                techo[ANCHO - 1] = t;
            }
            else
            {
                // Si no es un frame de obstáculo, asignamos valores predeterminados.
                suelo[ANCHO - 1] = 0;
                techo[ANCHO - 1] = ALTO - 1;
            }
        }

        #region Métodos que controlan el movimiento del pájaro 
        static void Mueve(char ch, ref int fil, ref int ascenso) // done
        {
            //ascenso pasa por referencia para que los valores modificados salgan del método.

            // Si ch==’i’...
            if (ch == 'i')
            {
                // Ascenso toma el valor IMPULSO.
                ascenso = IMPULSO;
            }

            // Si ascenso ≥ 0... 
            if (ascenso >= 0)
            {
                // Se incrementa fil.
                fil++;

                // Se decrementa ascenso.
                ascenso--;
            }

            // En caso contrario...
            else
            {
                // Se decrementa fil.
                if(fil > 0)
                {
                    fil--;
                }
            }
        }

        static bool Colision(int[] suelo, int[] techo, int fil) // done.
        {
            // TECHO. Recorremos el array de techo.
            for (int i = 0; i <= techo.Length - 1; i++)
            {
                // Comprobamos colision con array de techo. Si coincide con 'fil', hay colisión.
                if (fil == techo[i])
                {
                    return true;
                }
            }

            // SUELO. Recorremos el array de suelo (para atrás).
            for (int i = suelo.Length - 1; i >= 0; i--)
            {
                // Comprobamos colision con array de suelo. Si coincide con 'fil', hay colisión.
                if (fil == suelo[i])
                {
                    return true;
                }
            }

            // No hay colisión.
            return false;
        }

        //static void Puntua(int[] suelo, int[] techo, ref int puntos)
        //{
        //    // puntos pasa por referencia porque se quiere que su valor salga del método.

        //    // No sé realmente por qué no se puede añadir el parámetro fil a Puntua, haría todo más sencillo. Dejo por aquí una posible implementación NO VÁLIDA:

        //    // Si no colisiona con las columnas...
        //    if (!Colision(suelo, techo, fil))
        //    {
        //        // Suma puntos.
        //        puntos++;
        //    }
        //}
        #endregion

        #region Guardar y cargar el juego.
        void GuardaJuego(string file, int[] suelo, int[] techo, int fil, int ascenso, int frame, int puntos)
        {

        }
        #endregion

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
            return ALTO - c;
        }
        
    }
}

    
