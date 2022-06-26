
namespace Lib{
  /// <summary>
  /// Each lot in ais_data.json can have one or more of characteristics.
  /// This object only has code numbers of a characteristic and its value.
  /// The descriptions of the characteristic and its value must be looked up as it is multilingual.
  /// </summary>
  public class Ais_Data_Characteristics{
    /// <summary>
    /// Refers to the main array entries in characteristics.json
    /// </summary>
    public string Code = "";

    /// <summary>
    /// Specifies the order in which these characteristics are to be displayed
    /// </summary>
    public int SortingRank = 0;

    /// <summary>
    /// Refers to the list of codes in the subarray under the main array entry in characteristics.json
    /// </summary>
    public string Value = "";
  }//end cl
}//end ns
