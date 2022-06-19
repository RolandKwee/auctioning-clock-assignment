using System.IO;//File
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lib
{
    /// <summary>
    /// Support for the input file characteristics.json
    /// </summary>
    public class Characteristics{
        /// <summary>
        /// The array of characteristics
        /// </summary>
        public JArray? CharacteristicTranslations = null;

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
                JObject JO = (JObject)JToken.ReadFrom(reader);
                CharacteristicTranslations = (JArray?)JO["characteristicTranslations"];
                if (CharacteristicTranslations == null){
                    RetVal += $". Failure parsing the characteristics array.";
                }else{
                    RetVal += $". Array size: {CharacteristicTranslations.Count}";
                }
            }
            return RetVal;
        }

    }//end cl
}//end ns
