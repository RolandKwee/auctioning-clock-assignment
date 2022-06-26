// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using Lib;// my lib, Ais_Data

namespace ACA{
  public class Program{
      /// <summary>
      /// Entry point for the Console application
      /// </summary>
      /// <param name="args"></param>
      public static void Main(string[] args){
        try{
          // start the console app
          Console.WriteLine("Hello from Auctioning Clock Assignment. Version 20220626RK.");
          // the language code may be configurable, e.g. with command line arg, or menu
          string LanguageCode = "nl";
          DateTime dtStart = DateTime.Now;
          // read json input file 1
          Ais_Data AD = new Ais_Data();
          string RAD = AD.Parse(@"Data\ais-data.json");
          Console.WriteLine($"Parsed ais-data.json: {RAD}");
          // read json input file 2
          Characteristics CH = new Characteristics();
          string Rch = CH.Parse(@"Data\characteristics.json", LanguageCode);
          Console.WriteLine($"Parsed characteristics.json: {Rch}"); 
          int ms = (int)(0.5 + (DateTime.Now - dtStart).TotalMilliseconds);
          Console.WriteLine($"JSON input files read and parsed in {ms} ms.");
          // restart the timer for the listing
          dtStart = DateTime.Now;
          int LineNr = 0;
          string ClockId = "";
          bool SortByAuction = false;
          Console.WriteLine(string.Format("Listing of all lots with ClockId = {0}, sorted by auction: {1}",
            ClockId == "" ? "all" : ClockId,
            SortByAuction ? "sorted" : "unsorted"
          ));
          foreach(string LotsTxt in AD.EnumerateLots(CH, ClockId, SortByAuction)){
            LineNr++;
            Console.WriteLine($"{LineNr}. {LotsTxt}");
          }
          Console.WriteLine($"{LineNr} lots displayed in {ms} ms.");
        }
        catch(Exception exc){
          Console.WriteLine($"Exception: {exc.Message}, stack: {exc.StackTrace}");
        }
        Console.Read();
        return;
      }

  }//end cl
}//end ns
