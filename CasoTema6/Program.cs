using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoTema6
{
    internal class Program
    {
        static int max_estudiantes = 40;
        static int max_cursos = 4;
        static string[] nombre_de_cursos = { "Matematicas", "Programacion", "Base de datos", "Fisica" };
        static string[] estudiantes = new string[max_estudiantes];
        static double[,] matrizNotas = new double[max_estudiantes, max_cursos];
        static int total_estudiantes = 0;
        static string rutaArchivo = "notas_curso.txt";

        public static void RegistrarEstudiantes()
        {
            if (total_estudiantes >= max_estudiantes)
            {
                Console.WriteLine("Capacidad máxima de estudiantes alcanzada ");
                return;
            }

            string nombre = Validador.LeerTexto(" Ingresar nombres y apellidos ").ToUpper();
            estudiantes[total_estudiantes ] = nombre;

            Console.WriteLine("Ingresar notas para cada curso (0 a 20):");
            for (int i = 0; i < max_cursos; i++)
            {
                matrizNotas[total_estudiantes, i] = Validador.LeerDecimal($"- {nombre_de_cursos[i]}: ");
            }
            total_estudiantes++;
            Console.WriteLine("¡Estudiante registrado con éxito!");
        }

        static void GuardarArchivo()
        {
            Console.WriteLine("Guardando archivo...");
            if (total_estudiantes == 0)
            {
                Console.WriteLine("No hay estudiantes registrados para guardar.");
                return;
            }

            using (StreamWriter escribir = new StreamWriter(rutaArchivo))
            {
                for (int i = 0; i < total_estudiantes; i++)
                {
                    string linea = estudiantes[i];
                    for (int j = 0; j < max_cursos; j++)
                    {
                        linea += $",{matrizNotas[i, j]}";
                    }
                    escribir.WriteLine(linea);
                }
            }
            Console.WriteLine("Registros guardados en el archivo.");
        }

        public static void CargarRegistros()
        {
            Console.WriteLine("Cargando registros desde el archivo...");
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("No se encontró el archivo de registros.");
                return;
            }

            total_estudiantes = 0;
            using (StreamReader lector = new StreamReader(rutaArchivo))
            {
                string linea;
                while ((linea = lector.ReadLine()) != null && total_estudiantes < max_estudiantes)
                {
                    string[] partes = linea.Split(',');
                    if (partes.Length == max_cursos + 1)
                    {
                        estudiantes[total_estudiantes] = partes[0];
                        for (int j = 0; j < max_cursos; j++)
                        {
                            if (double.TryParse(partes[j + 1], out double nota))
                            {
                                matrizNotas[total_estudiantes, j] = nota; 
                            }
                        }
                        total_estudiantes++;
                    }
                }
            }
            Console.WriteLine("Se cargaron " + total_estudiantes + " estudiantes con sus notas.");
        }

        public static void mostrarRegistros()
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("\t\t\t\tReporte de notas por cursos");
            Console.WriteLine(new string('-', 100));

            if (total_estudiantes == 0) 
            {
                Console.WriteLine("No hay registros.");
                return;
            }

            Console.Write($"{"Estudiante",-25}|");
            for (int j = 0; j < max_cursos; j++)
            {
                Console.Write($"{nombre_de_cursos[j],-12}|");
            }
            Console.WriteLine("Promedio    | Estado");
            Console.WriteLine(new string('-', 100));

            for (int i = 0; i < total_estudiantes; i++)
            {
                Console.Write($"{estudiantes[i],-25}|");
                double suma = 0;
                for (int j = 0; j < max_cursos; j++)
                {
                    Console.Write($"{matrizNotas[i, j],-12:F1}|");
                    suma += matrizNotas[i, j];
                }
                double promedio = suma / max_cursos;
                string estado = promedio >= 12 ? "Aprobado" : "Desaprobado"; 
                Console.WriteLine($"{promedio,-12:F2}| {estado}");
            }
            Console.WriteLine(new string('-', 100));
        }

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Registrar estudiante");
                Console.WriteLine("2. Guardar en archivo");
                Console.WriteLine("3. Cargar desde archivo");
                Console.WriteLine("4. Mostrar registros y promedios");
                Console.WriteLine("5. Salir");
                Console.Write("Elija una opción: ");

                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            RegistrarEstudiantes();
                            break;
                        case 2:
                            GuardarArchivo();
                            break;
                        case 3:
                            CargarRegistros();
                            break;
                        case 4:
                            mostrarRegistros();
                            break;
                        case 5:
                            Console.WriteLine("Muchos exitos para tu examen final");
                            break;
                    }
                    if (opcion != 5)
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
            } while (opcion != 5);
        }
    }
}