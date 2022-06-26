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

    public string ProductName = "";

    public string SupplierName = "";

    /// <summary>
    /// Field: just try: packagesPerLoadCarrier
    /// </summary>
    public int PackagesPerCarrier = 0;

    /// <summary>
    /// Just try: currentNumberOfPieces divided by PackagesPerCarrier
    /// </summary>
    public int NrOfCarriers = 0;

    public int currentNumberOfPieces = 0;

    public string QualityCode = "";

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
    /// a guid specifying the clock
    /// on which this product is to be auctioned
    /// </summary>
    public string ClockId = "";

    /// <summary>
    /// Each lot can have one or more characteristics
    /// </summary>
    public List<Ais_Data_Characteristics> Lot_Characteristics = new List<Ais_Data_Characteristics>();

    /// <summary>
    /// each lot represents a produce a grower wants to be auctioned
    /// </summary>
    public List<Ais_Data> Lots = new List<Ais_Data>();

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
        RetVal = $"File size is {FileSize}.";
      } else{
        RetVal = $"File not found: {FilePath}.";
        //throw new Exception($"File not found: {FilePath}");
      }
      using (StreamReader file = File.OpenText(FilePath))
      using (JsonTextReader reader = new JsonTextReader(file)){
        JArray JLots = (JArray)JToken.ReadFrom(reader);
        RetVal += $" Lots array size: {JLots.Count}.";
        int NrLotCharacteristics = 0;
        foreach(JObject JLot in JLots){
          Ais_Data AD = new Ais_Data();
          // Readme: fields of interest
          AD.ClockId = (string?)JLot["clockId"]??"";
          AD.AuctioningSequence = (string?)JLot["auctioningSequence"]??"";
          AD.CurrentNumberOfPieces = (int?)JLot["currentNumberOfPieces"]??0;
          AD.PiecesPerPackage = (int?)JLot["piecesPerPackage"]??0;
          // Readme: fields for console output
          AD.ProductName = (string?)JLot["productName"]??"";
          AD.SupplierName = (string?)JLot["supplierName"]??"";
          AD.CurrentNumberOfPieces = (int?)JLot["currentNumberOfPieces"]??0;
          AD.PackagesPerCarrier = (int?)JLot["packagesPerLoadCarrier"]??0;
          AD.NrOfCarriers = 0;
          if (AD.CurrentNumberOfPieces > 0 && AD.PackagesPerCarrier > 0){
            AD.NrOfCarriers = AD.CurrentNumberOfPieces / AD.PackagesPerCarrier;
          }
          AD.QualityCode = (string?)JLot["qualityCode"]??"";
          // characteristics:
          AD.Lot_Characteristics = new List<Ais_Data_Characteristics>();
          JArray? JCharacteristics = (JArray?)JLot["characteristics"];
          if (JCharacteristics != null){
            foreach(JObject JCharacteristic in JCharacteristics){
              Ais_Data_Characteristics ADC = new Ais_Data_Characteristics();
              ADC.Code = (string?)JCharacteristic["code"]??"";
              ADC.SortingRank = (int?)JCharacteristic["sortingRank"]??0;
              ADC.Value = (string?)JCharacteristic["value"]??"";
              AD.Lot_Characteristics.Add(ADC);
              NrLotCharacteristics++;
            }
          }
        }//end foreach lot
        RetVal += $" Nr lot characteristics: {NrLotCharacteristics}.";
      }
      return RetVal;
    }
  }//end cl
}//end ns
