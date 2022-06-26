using System.IO;//File
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lib
{
    /// <summary>
    /// Support for the input file characteristics.json.
    /// This JSON contains two tables. The main table is a list of characteristics,
    /// e.g. 
    /// Each characteristics has a list of values.
    /// Complex: the list of values depends on the characteristic.
    /// E.g. value code 020 may mean 'dubbelbloemig', or '20 cm'.
    /// Solution: value codes must be made unique by prefixing with the characteristic code. 
    /// </summary>
    public class Characteristics{
        /// <summary>
        /// Collection of all SXX characteristics and their NL translations.
        /// The Collection type is hash based for efficient searching.
        /// </summary>
        public Dictionary<string, string> Characteristics_SXX_NL = new Dictionary<string, string>();

        /// <summary>
        /// Collection of all values that appear in the sub arrays under main array entries.
        /// Assume that all values have a unique "code", then this collection serves
        /// to normalize the data in the JSON.
        /// </summary>
        public Dictionary<string, string> Values = new Dictionary<string, string>();

        /// <summary>
        /// Parse the json input file into a Json C# object.
        /// Then, select only SXX characteristics and the NL values
        /// in a C# List object, for easy searching.
        /// Although the JSON has "code"properties in the main array
        /// (as found out by simple search with NotePad++)
        /// as well as in the "value" arrays,
        /// there are only SXX characteristics in the main array. 
        /// Each of the main characteristics can have "value" in several languages, 
        /// in the sub arrays.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="LanguageCode">e.g. nl for Dutch</param>
        /// <returns></returns>
        public string Parse(string FilePath, string LanguageCode){
            // Initialize all output data
            Characteristics_SXX_NL = new Dictionary<string, string>();
            Values = new Dictionary<string, string>();
            string RetVal = "";
            // Look at the input argument
            FileInfo FI = new FileInfo(FilePath);
            // initial output just to see if the file can be found in the Data folder
            if (FI.Exists){
                long FileSize = FI.Length;
                RetVal = $"File size is {FileSize}.";
            } else{
                RetVal = $"File not found: {FilePath}.";
                throw new Exception($"File not found: {FilePath}");
            }
            // process the input file
            using (StreamReader file = File.OpenText(FilePath))
            using (JsonTextReader reader = new JsonTextReader(file)){
                JObject JO = (JObject)JToken.ReadFrom(reader);
                JArray? CharacteristicTranslations = (JArray?)JO["characteristicTranslations"];
                if (CharacteristicTranslations == null){
                    RetVal += $" Failure parsing the characteristics array.";
                }else{
                    RetVal += $" Array size: {CharacteristicTranslations.Count}.";
                    // only look in main array
                    foreach(JObject JCharacteristic in CharacteristicTranslations){
                        string? CharCode = (string?)JCharacteristic["code"];
                        // only use SXX characteristics
                        if (CharCode != null && CharCode.Length > 2 && CharCode[0] == 'S')
                        {
                            string? CharDesc = "";
                            JArray? JTranslations = (JArray?)JCharacteristic["translations"];
                            if (JTranslations != null)
                            {
                                foreach (JObject JTranslation in JTranslations)
                                {
                                    // only use NL translations
                                    if ((string?)JTranslation["language"] == LanguageCode)
                                    {
                                        CharDesc = (string?)JTranslation["value"];
                                        if (CharDesc != null)
                                        {
                                            JArray? JValues = (JArray?)JCharacteristic["values"];
                                            if (JValues != null)
                                            {
                                                foreach (JObject JValue in JValues)
                                                {
                                                    string ValueCode = (string?)JValue["code"] ?? "";
                                                    // combine with Characteristic code (poor man's composite key)
                                                    ValueCode = $"{CharCode}{ValueCode}";
                                                    string ValueDesc = "";
                                                    // test if the codes are unique
                                                    if (Values.ContainsKey(ValueCode))
                                                    {
                                                        // we could add more code to show which characteristics
                                                        // have this value, but with just the code, it is easy
                                                        // to look this up in a text editor (NotePad++)
                                                        // using Find All In Current Document. 
                                                        throw new Exception($"Duplicate Value Code: {ValueCode}");
                                                    }
                                                    // search the sub array of value translations
                                                    JArray? JValueTranslations = (JArray?)JValue["translations"];
                                                    if (JValueTranslations != null)
                                                    {
                                                        foreach (JObject JVal in JValueTranslations)
                                                        {
                                                            if ((string?)JTranslation["language"] == LanguageCode)
                                                            {
                                                                ValueDesc = (string?)JVal["value"] ?? "";
                                                                // keep simple, do not test for empty value
                                                                // Add the value to the Values collection
                                                                Values.Add(ValueCode, ValueDesc);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }//end foreach JValues
                                            }
                                            // characteristic data complete, add to List
                                            Characteristics_SXX_NL.Add(CharCode, CharDesc);
                                            break;
                                        }
                                    }
                                }//end foreach translation of a code
                            }
                        }
                    }//end foreach code
                    RetVal += $" Nr of SXX characteristics: {Characteristics_SXX_NL.Count}.";
                    RetVal += $" Nr of Values: {Values.Count}";
                }
            }//end using
            return RetVal;
        }

    }//end cl
}//end ns
