// Paula Sierra Luque
// José Tomás Gómez Becerra

using System.Drawing;
using System;
using System.Runtime.CompilerServices;
using System.IO;

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
            // Declaraciones anteriores (está inicializado para que no de error en el cargajuego).
            int[] suelo = null, techo = null;
            int fil = 0, ascenso = 0, frame = 0, puntos = 0;
            bool colision = false;

            // Preguntar si se quiere cargar un juego guardado o iniciar uno nuevo.
            Console.WriteLine("¿Deseas cargar un juego guardado (c) o iniciar uno nuevo (n)?");
            char opcion = Console.ReadKey().KeyChar;

            if (opcion == 'c')
            {
                // Cargar juego.
                /*Console.WriteLine("Introduce el nombre del archivo de guardado:");
                string archivo = Console.ReadLine();
                CargaJuego(archivo, ref suelo, ref techo, ref fil, ref ascenso, ref frame, ref puntos, ref colision);*/
            }
            else
            {
                // Inicialización.
                Inicializa(out suelo, out techo, out fil, out ascenso, out frame, out puntos, out colision);
            }

            // Renderizado inicial.
            Render(suelo, techo, fil, frame, puntos, colision);

            bool pausar = false, salir = false;

            // Bucle principal.
            while(!colision && !salir)
            {
                if (!pausar)
                {
                    // Lectura de input.
                    char c = LeeInput();

                    if (c == 'p')
                    {
                        pausar = true;
                        Console.WriteLine("Juego pausado. Presiona cualquier tecla para continuar...");
                        Console.ReadLine();
                        pausar = false;
                    }
                    else if (c == 'q')
                    {
                        salir = true;
                    }
                    else
                    {
                        // Scroll lateral + movimiento pájaro + colisiones + gestión de puntos.
                        Avanza(suelo, techo, frame);
                        colision = Colision(suelo, techo, fil); // Comprueba colisión cada vez que se mueve para no atravesar paredes (en este caso escenario).
                        Mueve(c, ref fil, ref ascenso);
                        colision = Colision(suelo, techo, fil); // Comprueba colisión cada vez que se mueve para no atravesar paredes (en este caso pájaro).
                        Puntua(suelo, techo, ref puntos);

                        // Renderizado.
                        Render(suelo, techo, fil, frame, puntos, colision);

                        Thread.Sleep(DELTA);

                        frame++;
                    }
                }  
            }

            // Opción de guardar el juego después de salir del bucle ppal.
            Console.WriteLine("¿Deseas guardar el juego? (s/n)");
            opcion = Console.ReadKey().KeyChar;
            if (opcion == 's')
            {
                // (guardar juego).
            }
        }

        static void Inicializa(out int[] suelo, out int[] techo, out int fil, out int ascenso, out int frame, out int puntos, out bool colision) //done
        {
            suelo = new int[ANCHO];
            techo = new int[ANCHO];

            // Inicializamos sin obstáculos:
            for (int i = 0; i < ANCHO; i++)
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
                for (int j = 0; j <= Convierte(techo[i]) - 1; j++)
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

        static void Avanza(int[] suelo, int[] techo, int frame) // done
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

        static bool Colision(int[] suelo, int[] techo, int fil) // done
        { 
            return suelo[COL_BIRD] >= fil || techo[COL_BIRD] <= fil;
        }

        static void Puntua(int[] suelo, int[] techo, ref int puntos) // done.
        {
            // puntos pasa por referencia porque se quiere que su valor salga del método.

            // ESTO DE ABAJO ME LO HA SUGERIDO BING CHAT PORQUE YA NO SABÍA CÓMO HACERLO..

            // Asumiendo que el pájaro siempre está en la columna COL_BIRD
            if (suelo[COL_BIRD] != 0 || techo[COL_BIRD] != ALTO - 1)
            {
                // Si hay un obstáculo en la columna del pájaro, incrementa los puntos
                puntos++;
            }
        }
        #endregion

        #region Guardar y cargar el juego.
        static void GuardaJuego(string file, int[] suelo, int[] techo, int fil, int ascenso, int frame, int puntos)
        {
            // Declaración de flujo de salida, creación y asociación a un archivo concreto.
            StreamWriter salida = new StreamWriter("documento.txt");

            // Escribir las variables.
            salida.WriteLine(fil);
            salida.WriteLine(ascenso);
            salida.WriteLine(frame);
            salida.WriteLine(puntos);

            // Encontrar las posiciones de los obstáculos y escribirlas.
            for (int i = 0; i < suelo.Length; i = i + SEP_OBS)
            {
                // Suelo y techo sin obstáculos.
                if (suelo[i] != 0 || techo[i] != ALTO - 1)
                {
                    // Posición del array.
                    salida.WriteLine(i);

                    // Valores de suelo y techo para el obstáculo.
                    salida.WriteLine(suelo[i] + " " + techo[i]);
                }
            }
            // Cierre de flujo.
            salida.Close();
        }

        static void CargaJuego(string file, ref int[] suelo, ref int[] techo, ref int fil, ref int ascenso, ref int frame, ref int puntos, ref bool colision)
        {
            // Inicializar la variable colision a false.
            colision = false;

            // Declaración de flujo de entrada, creación de flujo y asociación al archivo.
            StreamReader entrada = new StreamReader("documento.txt");

            // Lectura de líneas de texto.
            fil = int.Parse(entrada.ReadLine());
            ascenso = int.Parse(entrada.ReadLine());
            frame = int.Parse(entrada.ReadLine());
            puntos = int.Parse(entrada.ReadLine());

            // Cierre de flujo
            entrada.Close();
            // FALTA LEER LO DE LOS ARRAYS.
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

    
