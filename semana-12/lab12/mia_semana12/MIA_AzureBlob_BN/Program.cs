using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string? connectionString =
    Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING_LOCAL");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("No se encontró la Connection String.");
    Console.WriteLine("Configure la variable de entorno AZURE_STORAGE_CONNECTION_STRING.");
    return;
}

string containerName = "miaarchivos";

BlobServiceClient blobServiceClient =
    new BlobServiceClient(connectionString);

BlobContainerClient containerClient =
    blobServiceClient.GetBlobContainerClient(containerName);

await containerClient.CreateIfNotExistsAsync();

int opcion;

do
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine("     MIA - AZURE BLOB STORAGE");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Subir archivo");
    Console.WriteLine("2. Listar archivos");
    Console.WriteLine("3. Descargar archivo");
    Console.WriteLine("4. Eliminar archivo");
    Console.WriteLine("5. Salir");
    Console.WriteLine("=================================");
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        opcion = 0;
    }

    Console.Clear();

    switch (opcion)
    {
        case 1:
            await SubirArchivo();
            break;

        case 2:
            await ListarArchivos();
            break;

        case 3:
            await DescargarArchivo();
            break;

        case 4:
            await EliminarArchivo();
            break;

        case 5:
            Console.WriteLine("Programa finalizado.");
            break;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }

    if (opcion != 5)
    {
        Console.WriteLine();
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

} while (opcion != 5);


// SUBIR ARCHIVO
async Task SubirArchivo()
{
    Console.WriteLine("========== SUBIR ARCHIVO ==========");
    Console.Write("Ingrese la ruta del archivo: ");

    string? ruta = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(ruta))
    {
        Console.WriteLine("La ruta no puede estar vacía.");
        return;
    }

    if (!File.Exists(ruta))
    {
        Console.WriteLine("El archivo no existe.");
        return;
    }

    string nombreArchivo = Path.GetFileName(ruta);

    try
    {
        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        using FileStream stream = File.OpenRead(ruta);

        await blobClient.UploadAsync(
            stream,
            overwrite: true);

        Console.WriteLine();
        Console.WriteLine("Archivo subido correctamente.");
        Console.WriteLine("Nombre: " + nombreArchivo);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al subir el archivo.");
        Console.WriteLine(ex.Message);
    }
}


// LISTAR ARCHIVOS
async Task ListarArchivos()
{
    Console.WriteLine("========== LISTA DE ARCHIVOS ==========");

    try
    {
        Console.WriteLine();
        Console.WriteLine("{0,-30} {1,15}", "Nombre", "Tamaño");
        Console.WriteLine("-----------------------------------------------");

        bool hayArchivos = false;

        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            hayArchivos = true;

            long tamano = blobItem.Properties.ContentLength ?? 0;

            Console.WriteLine(
                "{0,-30} {1,10} bytes",
                blobItem.Name,
                tamano);
        }

        if (!hayArchivos)
        {
            Console.WriteLine("No hay archivos en el container.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al listar los archivos.");
        Console.WriteLine(ex.Message);
    }
}

// DESCARGAR ARCHIVO
async Task DescargarArchivo()
{
    Console.WriteLine("========== DESCARGAR ARCHIVO ==========");

    Console.Write("Ingrese el nombre del archivo: ");
    string? nombreArchivo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nombreArchivo))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        return;
    }

    try
    {
        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        if (!await blobClient.ExistsAsync())
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write("Ingrese la carpeta de destino: ");
        string? carpetaDestino = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(carpetaDestino))
        {
            Console.WriteLine("La carpeta no puede estar vacía.");
            return;
        }

        if (!Directory.Exists(carpetaDestino))
        {
            Directory.CreateDirectory(carpetaDestino);
        }

        string rutaDestino =
            Path.Combine(carpetaDestino, nombreArchivo);

        await blobClient.DownloadToAsync(rutaDestino);

        Console.WriteLine();
        Console.WriteLine("Archivo descargado correctamente.");
        Console.WriteLine("Ubicación: " + rutaDestino);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al descargar el archivo.");
        Console.WriteLine(ex.Message);
    }
}


// ELIMINAR ARCHIVO
async Task EliminarArchivo()
{
    Console.WriteLine("========== ELIMINAR ARCHIVO ==========");

    Console.Write("Ingrese el nombre del archivo: ");
    string? nombreArchivo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nombreArchivo))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        return;
    }

    try
    {
        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        if (!await blobClient.ExistsAsync())
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write(
            "¿Está seguro de eliminar el archivo? (S/N): ");

        string? confirmacion = Console.ReadLine();

        if (confirmacion?.ToUpper() != "S")
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        await blobClient.DeleteIfExistsAsync();

        Console.WriteLine();
        Console.WriteLine("Archivo eliminado correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al eliminar el archivo.");
        Console.WriteLine(ex.Message);
    }
}