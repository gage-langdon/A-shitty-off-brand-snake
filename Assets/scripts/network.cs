using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

namespace Shitty.Networking
{
    public class Http : MonoBehaviour
    {

        public string data = "";
        public bool error = false;

        public string apiURI = "https://1nmquhg2il.execute-api.us-west-2.amazonaws.com/prod/snek";

        public IEnumerator Post(string url, string bodyJsonString)
        {
            var request = new UnityWebRequest(apiURI + url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log("Status Code: " + request.responseCode);
        }


        public IEnumerator Get(string url)
        {
            var http = new UnityWebRequest(apiURI + url, "GET");
            http.SetRequestHeader("Content-Type", "application/json");
            http.downloadHandler = new DownloadHandlerBuffer();
            yield return http.SendWebRequest();

            if (http.error != null)
            {
                error = true;
                yield return false;
            };
            data = http.downloadHandler.text;

            // Debug.Log(http.responseCode);
            // Debug.Log(http.error);
            // Debug.Log(http.downloadHandler.text);
        }
    }
}

