using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MarineTrafficDotNet
{
    /// <summary>
    /// Static class for calling into the MarineTraffic API
    /// </summary>
    public static class MarineTraffic
    {
        #region Fields & Properties

        static string _apiKey = System.Configuration.ConfigurationManager.AppSettings["MarineTrafficApiKey"];

        /// <summary>
        /// Gets the API Key provided in the configuration file of this application as the setting "MarineTrafficApiKey"
        /// </summary>
        public static string ApiKey { get { return _apiKey; } }

        #endregion

        #region API Methods

        /// <summary>
        /// Using the MMSI or IMO Number of the vessel, returns the image URL path parsed from the response
        /// </summary>
        /// <param name="MMSIorIMONumberOfTheVessel"></param>
        /// <returns></returns>
        public static string GetVesselPhoto(string MMSIorIMONumberOfTheVessel)
        {
            const string endpointUrl = "http://services.marinetraffic.com/api/exportvesselphoto/{0}/vessel_id:{1}";

            // Build the API call
            string callUrl = string.Format(endpointUrl, ApiKey, MMSIorIMONumberOfTheVessel);

            string apiKey = System.Configuration.ConfigurationManager.AppSettings["MarineTrafficApiKey"];

            HttpWebRequest request = HttpWebRequestExtensions.Create(callUrl, "GET");

            string response = request.GetResponseString();

            // Parse the response
            XmlSerializer serializer = new XmlSerializer(typeof(PHOTO));

            using (StringReader reader = new StringReader(response))
            {
                PHOTO photo = serializer.Deserialize(reader) as PHOTO;
                var val = photo.PHOTOURL.Rows[0] as PHOTO.PHOTOURLRow;
                return val.URL;
            }
        }

        #endregion
    }
}
