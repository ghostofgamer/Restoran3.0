using Io.AppMetrica;
using Newtonsoft.Json;
using UnityEngine;

namespace MetricsContent
{
    public class OurGamesMetrics : MonoBehaviour
    {
        private struct ClickPicturesJson
        {
            public object clickPictures;  // используем object и null, чтобы AppMetrica понимала "папку"
        }
        
        public static void ReportClickPicture()
        {
            var payload = new ClickPicturesJson { clickPictures = null };
            string json = JsonConvert.SerializeObject(payload);
            AppMetrica.ReportEvent("Our_Games", json);
        }
        
        private struct ClickDownloadsJson
        {
            public object clickDownloads; // вложенный объект
        }
        
        public static void ReportClickDownload(string gameName)
        {
            // Приводим название игры к стабильному виду
            string sanitizedGame = SanitizeGameName(gameName);

            // Создаём вложенный объект: { clickDownloads: { GameName: null } }
            var nestedGame = new System.Collections.Generic.Dictionary<string, object>
            {
                { sanitizedGame, null }
            };

            var payload = new ClickDownloadsJson { clickDownloads = nestedGame };

            string json = JsonConvert.SerializeObject(payload);
            AppMetrica.ReportEvent("Our_Games", json);
        }

        // ------------------------
        // Приведение названия игры к безопасному виду
        // ------------------------
        private static string SanitizeGameName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "Unknown";
            char[] chars = name.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                bool ok = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                chars[i] = ok ? c : '_';
            }
            return new string(chars);
        }
    }
}