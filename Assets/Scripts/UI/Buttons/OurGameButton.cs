using Enums;
using Io.AppMetrica;
using MetricsContent;
using UnityEngine;

namespace UI.Buttons
{
    public class OurGameButton : AbstractButton
    {
        [SerializeField] private string _link;
        [SerializeField]private OurGames _gameName;
        
        public override void OnClick()
        {
            // AppMetrica.ReportEvent("Our_Games/clickDownloads/" + _gameName.ToString());
            OurGamesMetrics.ReportClickDownload(_gameName.ToString());
            Application.OpenURL(_link);
        }
    }
}