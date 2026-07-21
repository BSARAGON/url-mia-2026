// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Laboratorio_2_CSV_JSON;

class Program
{
    static void Main(string[] args)
    {
        string archivoCSV = "estudiantes.csv";
        string archivoJSON = "estudiantes.json";

        List<Estudiante> estudiantes = new List<Estudiante>();

        string[] lineas = File.ReadAllLines(archivoCSV);

        for (int i = 1; i < lineas.Length; i++)
        {
            string[] datos = lineas[i].Split(',');

            Estudiante estudiante = new Estudiante();

            estudiante.Id = int.Parse(datos[0]);
            estudiante.Nombre = datos[1];
            estudiante.Carrera = datos[2];

            estudiantes.Add(estudiante);
        }

        foreach (Estudiante estudiante in estudiantes)
        {
            Console.WriteLine(estudiante.Id + " - " + estudiante.Nombre + " - " + estudiante.Carrera);
        }

        string json = JsonSerializer.Serialize(estudiantes, new JsonSerializerOptions {WriteIndented = true});

        File.WriteAllText(archivoJSON, json);

        Console.WriteLine("Archivo estudiante.json creado correctamente");
    }
}