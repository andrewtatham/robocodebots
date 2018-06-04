using System;
using System.IO;
using AndrewTatham.BattleTests.Helpers;

namespace AndrewTatham.BattleTests.Reports
{
    public partial class ReportTemplate
    {
        public ReportTemplate(ScoreReportData scores)
        {
            _scores = scores;
        }

        private readonly ScoreReportData _scores;
    }

    public class ScoreReport
    {
        private static readonly string[] ReportFiles = {
            Path.Combine(Environment.CurrentDirectory, @"Reports\Scores.html"),
            Path.Combine(DropboxHelper.GetDropboxPath(), @"Code\Robocode\Scores.html")
        };

        private readonly string _html;

        public ScoreReport(ScoreReportData scores)
        {
            _html = new ReportTemplate(scores).TransformText();
        }

        public void Save()
        {
            foreach (var reportFile in ReportFiles)
            {
                Console.WriteLine("Saving report ({1} bytes) to: {0}", reportFile, _html.Length);
                Directory.CreateDirectory(Path.GetDirectoryName(reportFile));

                File.WriteAllText(reportFile, _html);
            }
        }
    }
}