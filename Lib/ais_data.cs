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
          Lots.Add(AD);
        }//end foreach lot
        RetVal += $" Nr lot characteristics: {NrLotCharacteristics}.";
      }
      return RetVal;
    }

    /// <summary>
    /// List all lots. 
    /// Resolve characteric code and value code with the cleartext description.
    /// This method can easily be called from a console application,
    /// or from a GUI application, and has options to sort by auction, or to select one clock.
    /// </summary>
    /// <param name="TheCharacteristics"></param>
    /// <param name="ClockId"></param>
    /// <param name="OrderByAuction"></param>
    /// <returns></returns>
    public IEnumerable<string> EnumerateLots(Characteristics TheCharacteristics, string ClockId = "", bool OrderByAuction = false){
      // Prerequisite is that the JSON input files must already be read and parsed. Check this.
      if (Lots.Count == 0){
          throw new Exception($"The JSON files are not yet read and parsed. Call Parse first.");
      }
      // output may be optionally sorted by AuctioningSequence
      IEnumerable<Ais_Data> LotsForIteration = Lots;
      if (OrderByAuction){
        LotsForIteration = Lots.OrderBy(x => x.AuctioningSequence);
      }
      // scan all lots
      foreach(Ais_Data Lot in LotsForIteration){
        // output may only specify one particular auctioning clock
        if (ClockId != null && ClockId != "" && ClockId != Lot.ClockId){
          continue;
        }
        // compose the output text for the lot
        string LotText = $"{Lot.ProductName}, {Lot.SupplierName}, {Lot.PackagesPerCarrier}, {Lot.NrOfCarriers}, {Lot.QualityCode}";
        // get translations for the characteristics codes
        foreach(Ais_Data_Characteristics ADC in Lot.Lot_Characteristics.OrderBy(x => x.SortingRank)){
          // only SXX codes are stored. Probably ContainsKey is slow
          //if (TheCharacteristics.Characteristics_SXX_NL.ContainsKey(ADC.Code)){
          if (ADC.Code != null && ADC.Code != "" && ADC.Code[0] == 'S')
          {
            // characteristic S97 is in ais_data.json, but not in characteristics.json, so ContainsKey is needed
            if (TheCharacteristics.Characteristics_SXX_NL.ContainsKey(ADC.Code)){
              string CharDesc = TheCharacteristics.Characteristics_SXX_NL[ADC.Code];
              string CharValCode = $"{ADC.Code}{ADC.Value}";
              // characteristic 'S98' has value 'A2' but first lot has value 'A2 ', with extra space, strip that
              CharValCode = CharValCode.Trim();
              // char value S20 036 is not in char.json
              if (TheCharacteristics.Values.ContainsKey(CharValCode)){
                string ValueDesc = TheCharacteristics.Values[CharValCode];
                LotText += $", {CharDesc}: {ValueDesc}";
              }else{
                LotText += $", =====WARNING===== Characteristic value not found: {ADC.Code}{ADC.Value}";
              }
            }else{
              LotText += $", ====WARNING===== Characteristic not found: {ADC.Code}";
            }
          }
        }
        yield return LotText;
      }
    }
  }//end cl
}//end ns
