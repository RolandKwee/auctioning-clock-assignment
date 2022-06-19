// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using Lib;// my lib, Ais_Data

namespace ACA{
  public static class Program{
      /// <summary>
      /// Entry point for the Console application
      /// </summary>
      /// <param name="args"></param>
      public static void Main(string[] args){
        try{
          Console.WriteLine("Hello from Auctioning Clock Assignment.");
          DateTime dtStart = DateTime.Now;
          Ais_Data AD = new Ais_Data();
          string RAD = AD.Parse(@"Data\ais-data.json");
          Console.WriteLine($"Parsed ais-data.json: {RAD}");
          //
          Characteristics CH = new Characteristics();
          string Rch = CH.Parse(@"Data\characteristics.json");
          Console.WriteLine($"Parsed characteristics.json: {Rch}"); 
          //
          int ms = (int)(0.5 + (DateTime.Now - dtStart).TotalMilliseconds);
          Console.WriteLine($"Program finished in {ms} ms.");
        }
        catch(Exception exc){
          Console.WriteLine($"Exception: {exc.Message}");
        }
        Console.Read();
        return;
      }
  }//end cl
}//end ns
