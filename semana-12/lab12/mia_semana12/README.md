# Laboratorio No. 2 – Cloud Storage

## Objetivo

Desarrollar una aplicación de consola en C# capaz de conectarse a Azure Blob Storage y realizar operaciones básicas de manejo de archivos en la nube mediante una Connection String.

La aplicación permite subir, listar, descargar y eliminar archivos almacenados en un container de Azure Blob Storage.

## Tecnologías utilizadas

* C#
* .NET
* Azure Blob Storage
* Azure.Storage.Blobs
* Visual Studio Code
* Azure Portal
* Git y GitHub

## Configuración de Azure

Para realizar el laboratorio se utilizó un Storage Account en Microsoft Azure y se creó un container llamado:

`miaarchivos`

El container fue configurado con nivel de acceso:

`Private`

La aplicación utiliza una Connection String para autenticarse y conectarse al Storage Account.

La Connection String no se almacena directamente en el código fuente. Se utiliza una variable de entorno llamada:

`AZURE_STORAGE_CONNECTION_STRING`

De esta manera, la credencial no se incluye dentro del repositorio de GitHub.

## Arquitectura de la solución

La aplicación utiliza las clases proporcionadas por el SDK de Azure Blob Storage.

La estructura principal de conexión es:

```text
BlobServiceClient
        |
        v
BlobContainerClient
        |
        v
BlobClient
```

`BlobServiceClient` permite establecer la conexión con el Storage Account.

`BlobContainerClient` permite trabajar con el container `miaarchivos`.

`BlobClient` permite realizar operaciones sobre un archivo específico almacenado como blob.

## Operaciones disponibles

### 1. Subir archivo

La aplicación solicita al usuario la ruta de un archivo local.

Primero verifica que el archivo exista. Después obtiene su nombre y lo utiliza para crear un BlobClient.

Finalmente, el archivo se carga al container mediante `UploadAsync()`.

### 2. Listar archivos

La aplicación consulta los blobs existentes en el container y muestra su nombre y tamaño.

Para realizar esta operación se utiliza `GetBlobsAsync()`.

Ejemplo:

```text
Nombre                                  Tamaño
-----------------------------------------------
MIA_Laboratorio_No.2_Semana_12.docx     189980 bytes
```

### 3. Descargar archivo

El usuario proporciona el nombre del archivo que desea descargar.

La aplicación verifica que el blob exista y solicita una carpeta de destino.

El archivo se descarga utilizando `DownloadToAsync()`.

### 4. Eliminar archivo

El usuario proporciona el nombre del blob que desea eliminar.

La aplicación verifica que el archivo exista y solicita confirmación antes de eliminarlo.

La eliminación se realiza mediante `DeleteIfExistsAsync()`.

## Manejo de errores

La aplicación realiza diferentes validaciones para evitar errores durante las operaciones.

Entre ellas:

* Verificación de que la ruta del archivo exista.
* Verificación de que el nombre del archivo no esté vacío.
* Verificación de que el blob exista antes de descargarlo.
* Verificación de que el blob exista antes de eliminarlo.
* Creación automática de la carpeta de destino cuando es necesario.
* Manejo de excepciones mediante bloques `try-catch`.
* Validación de la Connection String mediante una variable de entorno.

## Protección de la Connection String

La Connection String contiene información sensible, por lo que no se incluyó directamente dentro del código fuente que se sube a GitHub.

La aplicación obtiene la credencial mediante la variable de entorno:

```text
AZURE_STORAGE_CONNECTION_STRING
```

En PowerShell se configura antes de ejecutar el programa:

```powershell
$env:AZURE_STORAGE_CONNECTION_STRING="CONNECTION_STRING_REAL"
```

La credencial real no debe incluirse en el repositorio.

## Instrucciones para ejecutar el proyecto

### 1. Clonar o descargar el repositorio

Ubicarse en la carpeta del proyecto:

```powershell
cd MIA_AzureBlob_BN
```

### 2. Instalar las dependencias

El proyecto utiliza el paquete oficial:

```powershell
dotnet add package Azure.Storage.Blobs
```

### 3. Configurar la Connection String

En PowerShell:

```powershell
$env:AZURE_STORAGE_CONNECTION_STRING="CONNECTION_STRING_REAL"
```

### 4. Ejecutar la aplicación

```powershell
dotnet run
```

### 5. Utilizar el menú

La aplicación presenta las siguientes opciones:

```text
=================================
     MIA - AZURE BLOB STORAGE
=================================
1. Subir archivo
2. Listar archivos
3. Descargar archivo
4. Eliminar archivo
5. Salir
=================================
```

## Evidencias

El proyecto incluye capturas que muestran:

1. Storage Account configurado.
2. Container `miaarchivos` configurado como privado.
3. Aplicación ejecutándose.
4. Archivo antes de ser subido.
5. Archivo visible en Azure Portal después de subirlo.
6. Listado de archivos desde C#.
7. Archivo descargado localmente.
8. Archivo eliminado de Azure.

## Conclusión

Durante el laboratorio se desarrolló una aplicación de consola en C# capaz de conectarse a Azure Blob Storage y administrar archivos almacenados en la nube.

Se implementaron las operaciones principales de subida, listado, descarga y eliminación de blobs, además del manejo de errores y la protección de la Connection String mediante una variable de entorno.
