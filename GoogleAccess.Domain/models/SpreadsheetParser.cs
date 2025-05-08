using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Data = Google.Apis.Sheets.v4.Data;

namespace GoogleAccess.Domain.Models
{
    public class SpreadsheetParser
    {
        private readonly OAuth2Helper _oauthHelper;
        private readonly GoogleAuthConfig _config;
        private ImmutableDictionary<string, KZJobEntry> _kzEntries;

        public SpreadsheetParser(GoogleAuthConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _oauthHelper = new OAuth2Helper( "test", config);
            _kzEntries = ImmutableDictionary<string, KZJobEntry>.Empty;
        }

        public ImmutableDictionary<string, KZJobEntry> GetKzEntries()
        {
            return _kzEntries;
        }

        public void AccessAndReadSpreadsheet()
        {
            try
            {
//                //var sheetsService = _oauthHelper.GetService<SheetsService>(
//                //    _config.Username,
//                //    new[] { SheetsService.Scope.Spreadsheets, SheetsService.Scope.DriveFile });

////                var spreadsheet = GetSpreadsheetData(sheetsService);
//                ProcessSpreadsheetData(spreadsheet);
            }
            catch (Exception ex)
            {
                throw new SpreadsheetParserException("Failed to access and read spreadsheet", ex);
            }
        }

        private Data.Spreadsheet GetSpreadsheetData(SheetsService sheetsService)
        {
            var request = sheetsService.Spreadsheets.Get(_config.SpreadsheetId);
            request.Ranges = _config.Ranges;
            request.IncludeGridData = true;

            return request.Execute();
        }

        private void ProcessSpreadsheetData(Data.Spreadsheet spreadsheet)
        {
            var newEntries = new Dictionary<string, KZJobEntry>();

            foreach (var grid in spreadsheet.Sheets[0].Data)
            {
                var entries = ParseGridData(grid);
                foreach (var entry in entries)
                {
                    newEntries[entry.PrepAreaName] = entry;
                }
            }

            _kzEntries = newEntries.ToImmutableDictionary();
        }

        private IEnumerable<KZJobEntry> ParseGridData(Data.GridData grid)
        {
            var rowData = grid.RowData;
            if (rowData == null || rowData.Count < 4)
                return Enumerable.Empty<KZJobEntry>();

            return rowData[0].Values
                .Zip(rowData[3].Values, (zone, job) => new { Zone = zone, Job = job })
                .Where(x => x.Job?.FormattedValue != null && x.Zone?.FormattedValue != null)
                .Select(x => CreateKZJobEntry(x.Zone.FormattedValue, x.Job));
        }

        private KZJobEntry CreateKZJobEntry(string zone, Data.CellData jobCell)
        {
            var backgroundColor = jobCell.UserEnteredFormat?.BackgroundColor;
            var red = backgroundColor?.Red ?? 0;
            var green = backgroundColor?.Green ?? 0;
            var blue = backgroundColor?.Blue ?? 0;

            return new KZJobEntry
            {
                PrepAreaName = zone,
                JobName = jobCell.FormattedValue,
                ColorBackground = Color.FromArgb(
                    (int)(red * 255),
                    (int)(green * 255),
                    (int)(blue * 255))
            };
        }
    }

    public class SpreadsheetConfig
    {
        public string ClientSecretPath { get; set; }
        public string ApplicationName { get; set; }
        public string Username { get; set; }
        public string SpreadsheetId { get; set; }
        public string[] Ranges { get; set; }
    }

    public class SpreadsheetParserException : Exception
    {
        public SpreadsheetParserException(string message) : base(message) { }
        public SpreadsheetParserException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
