// See https://aka.ms/new-console-template for more information

using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Ejercicio
{
    private static String password = "";
    private static bool find = false;

    static void Main()
    {
        var readlinestxt = File.ReadAllLines(@".\2151220-passwords.txt");

        Console.WriteLine("Inserte el hash para la comparación: ");

        var hash = Console.ReadLine();

        Console.WriteLine("Inserte el numero de hilos en los que desea dividirlo: ");

        var ThreadsNumber = Convert.ToInt32(Console.ReadLine());

        var Threads = new List<Thread>();

        for (int n = 0; n < ThreadsNumber; n++) // n = numero
        {
            var fraccion = readlinestxt.Length / ThreadsNumber;
            var start = fraccion * n;
            var end = fraccion * (n + 1);

            var section = readlinestxt[new Range(start, end)];

            Threads.Add(new Thread(() =>
            {
                Console.WriteLine("nuevo hilo");
                var encontradoEnEsta = Sha256Finder(section, hash);
                if (encontradoEnEsta) find = true;
                Console.WriteLine(!encontradoEnEsta ? "no encontrado de entrada " + start + " a entrada " + end : "encontrado de entrada " + start + " a entrada " + end);
            })
            );
        }

        var StartMoment = DateTime.Now;
        var todosTerminados = false;
        foreach (var hilo in Threads){hilo.Start();}

        while (!todosTerminados)
        {
            todosTerminados = true;
            foreach (var hilo in Threads)
            {if (hilo.IsAlive){todosTerminados = false;}}

            if (find){break;}
        }

        Console.WriteLine("hemos tardado " + (DateTime.Now - StartMoment));

        Console.WriteLine("el hash coincide con el de la combinacion " + password);

        Console.WriteLine("Presione cualquier tecla para salir.");
        Console.ReadKey();

    }



    static bool Sha256Finder(string[] lines, string obj)
    {
        foreach (var line in lines)
        {
            var encriptado = Encryption(line);
            if (encriptado == obj)
            {password = line;return true;}

            if (find)
            {break;}
        }

        return false;
    }

    static string Encryption(string cadena)
    {
        var resultado = string.Empty;
        var convert = SHA256.Create();

        var hashValue = convert.ComputeHash(Encoding.UTF8.GetBytes(cadena));
        foreach (byte b in hashValue)
        {resultado += $"{b:X2}";}
        return resultado;
    }

}
