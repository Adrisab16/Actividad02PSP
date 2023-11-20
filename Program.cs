// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;


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

        var threadsnumber = Convert.ToInt32(Console.ReadLine());

        var threads = new List<Thread>();

        for (int n = 0; n < threadsnumber; n++) // n = numero
        {
            var fraccion = readlinestxt.Length / threadsnumber;
            var start = fraccion * n;
            var end = fraccion * (n + 1);

            var section = readlinestxt[new Range(start, end)];

            threads.Add(new Thread(() =>
            {
                Console.WriteLine("New Thread");
                var FoundedInThread = Sha256Finder(section, hash); // Bucaremos el Sha256
                if (FoundedInThread) find = true;
                Console.WriteLine(!FoundedInThread ? "No se ha encontrado desde la entrada " + start + " a la entrada " + end : "Si se ha encontrado desde la entrada " + start + " a la entrada " + end);
            })
            );
        }

        var StartMoment = DateTime.Now;
        var todosTerminados = false;
        foreach (var hilo in threads){hilo.Start();}

        while (!todosTerminados)
        {
            todosTerminados = true;
            foreach (var hilo in threads)
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
