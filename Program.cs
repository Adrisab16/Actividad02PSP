// Actividad FuerzaBrutaAdrianSabinoPerez

using System.Security.Cryptography;
using System.Text;


class Ejercicio
{
    private static String password = "";
    private static bool find = false;

    static void Main()
    {
        var readlinestxt = File.ReadAllLines(@"E:\VisualStudio\Repositorios\Actividad02PSP\2151220-passwords.txt");

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
                var FoundedInThread = Sha256Finder(section, obj: hash); // Bucaremos el Sha256
                if (FoundedInThread) find = true;
                Console.WriteLine(!FoundedInThread ? "No se ha encontrado desde la entrada " + start + " a la entrada " + end : "Si se ha encontrado desde la entrada " + start + " a la entrada " + end);
            })
            );
        }

        var StartMoment = DateTime.Now;
        var allfinished = false;
        foreach (var hilo in threads){hilo.Start();}

        while (!allfinished)
        {
            allfinished = true;
            foreach (var hilo in threads){if (hilo.IsAlive){allfinished = false;}}
            if (find){break;}
        }

        Console.WriteLine("el hash coincide con el de la combinacion " + password);
        Console.WriteLine("Presione cualquier tecla para salir.");
        Console.ReadKey();

    }

    // Función con la finalidad de encontrar un Sha256 específico
    static bool Sha256Finder(string[] lines, string obj)
    {
        foreach (var line in lines)
        {
            var encrypted = Encryption(line);
            if (encrypted == obj)
            {password = line;return true;}

            if (find)
            {break;}
        }
        return false;
    }

    // Función para encriptar:
    static string Encryption(string cadena)
    {
        var resultado = string.Empty;
        var convert = SHA256.Create();

        // Crea el objeto de conversion:
        var hashValue = convert.ComputeHash(Encoding.UTF8.GetBytes(cadena));
        // Lo pasará de Byte a String y lo formateará:
        foreach (byte b in hashValue)
        {resultado += $"{b:X2}";}
        return resultado;
    }
}
