using System.IO;//File
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lib{
  /// <summary>
  /// Support for input file ais-data.json.
  /// The input data contains an array of lots, 
  /// where each lot represents a produce a grower wants to be auctioned.
  /// </summary>
  public class Ais_Data{

    public DateTime AuctionOccurrenceDate = DateTime.MinValue;

    public string AuctionSupplyID = "";

    /// <summary>
    /// the order in which to auction this particular lot
    /// e.g. 1_000001
    /// </summary>
    public string AuctioningSequence = "";

    public string BatchReference = "";

    /// <summary>
    /// the number of flowers in this lot
    /// </summary>
    public int CurrentNumberOfPieces = 0;

    /// <summary>
    /// minimum number of flowers that can be bought
    /// </summary>
    public int PiecesPerPackage = 0;

    /// <summary>
    /// a list containing one or more properties that describe the product
    /// </summary>
    public List<Ais_Data_Characteristics> Characteristics = new List<Ais_Data_Characteristics>();

    /// <summary>
    /// a guid specifying the clock
    /// on which this product is to be auctioned
    /// </summary>
    public string ClockId = "";

    /// <summary>
    /// Parse the json input file into a Json C# object
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public string Parse(string FilePath){
      string RetVal = "";
      FileInfo FI = new FileInfo(FilePath);
      if (FI.Exists){
        long FileSize = FI.Length;
        RetVal = $"File size is {FileSize}";
      } else{
        RetVal = $"File not found: {FilePath}";
        //throw new Exception($"File not found: {FilePath}");
      }
      using (StreamReader file = File.OpenText(FilePath))
      using (JsonTextReader reader = new JsonTextReader(file)){
        JArray JO = (JArray)JToken.ReadFrom(reader);
        RetVal += $". Array size: {JO.Count}";
        //yield return new Ais_Data();
      }
      return RetVal;
    }
  }//end cl
}//end ns
