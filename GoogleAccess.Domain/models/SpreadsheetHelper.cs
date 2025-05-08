using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Data = Google.Apis.Sheets.v4.Data;

namespace GoogleAccess.Domain.Models
{
    public class SpreadsheetHelper : IFetcherAsync<ImmutableDictionary<string, KZJobEntry>>
    {
        private static readonly string[] DefaultScopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private static readonly string[] DefaultRanges = { "A1:AF6", "A21:AF26" };

        private readonly SheetsService _sheetService;
        private readonly string _spreadsheetId;
        private readonly string[] _ranges;
        private ImmutableDictionary<string, KZJobEntry> _kzEntries;

        public SpreadsheetHelper(GoogleAuthConfig config, UserCredential credentials)
        {
            _spreadsheetId = config?.SpreadsheetId ?? throw new ArgumentNullException(nameof(config));
            _ranges = config?.Ranges ?? DefaultRanges;
            
            _sheetService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials ?? throw new ArgumentNullException(nameof(credentials)),
                ApplicationName = null ?? "Google Sheets Helper"
            });

            _kzEntries = ImmutableDictionary<string, KZJobEntry>.Empty;
        }

        public SpreadsheetHelper(GoogleAuthConfig config, string accessToken, string refreshToken)
        {
            _spreadsheetId = config?.SpreadsheetId ?? throw new ArgumentNullException(nameof(config));
            _ranges = config?.Ranges ?? DefaultRanges;

            var tokens = new TokenResponse
            {
                AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken)),
                RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken))
            };

            var credentials = new UserCredential(
                new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = config.ClientId,
                        ClientSecret = config.ClientSecret
                    },
                    Scopes = DefaultScopes,
                    DataStore = new FileDataStore(config.ClientSecretPath, true)
                }), 
                "user", 
                tokens);

            _sheetService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = null ?? "Google Sheets Helper"
            });

            _kzEntries = ImmutableDictionary<string, KZJobEntry>.Empty;
        }

        public async Task<ImmutableDictionary<string, KZJobEntry>> FetchDataAsync()
        {
            try
            {
                return await AccessAndReadSpreadsheetAsync();
            }
            catch (Exception ex)
            {
                throw new SpreadsheetHelperException("Failed to fetch spreadsheet data", ex);
            }
        }

        private async Task<ImmutableDictionary<string, KZJobEntry>> AccessAndReadSpreadsheetAsync()
        {
            var request = _sheetService.Spreadsheets.Get(_spreadsheetId);
            request.Ranges = _ranges;
            request.IncludeGridData = true;

            var response = await request.ExecuteAsync();
            return ProcessSpreadsheetResponseAsync(response);
        }

        private ImmutableDictionary<string, KZJobEntry> ProcessSpreadsheetResponseAsync(Data.Spreadsheet response)
        {
            var newEntries = new Dictionary<string, KZJobEntry>();

            foreach (var grid in response.Sheets[0].Data)
            {
                var entries = ParseGridData(grid);
                foreach (var entry in entries)
                {
                    newEntries[entry.PrepAreaName] = entry;
                }
            }

            _kzEntries = newEntries.ToImmutableDictionary();
            return _kzEntries;
        }

        private IEnumerable<KZJobEntry> ParseGridData(Data.GridData grid)
        {
            if (grid?.RowData == null || grid.RowData.Count < 4)
                return Enumerable.Empty<KZJobEntry>();

            var zones = grid.RowData[0].Values;
            var jobs = grid.RowData[3].Values;

            return zones
                .Zip(jobs, (zone, job) => new { Zone = zone, Job = job })
                .Where(x => x.Zone?.FormattedValue != null && x.Job?.FormattedValue != null)
                .Select(x => CreateKZJobEntry(x.Zone.FormattedValue, x.Job));
        }

        private KZJobEntry CreateKZJobEntry(string zoneName, Data.CellData jobCell)
        {
            var backgroundColor = jobCell.UserEnteredFormat?.BackgroundColor;
            var red = backgroundColor?.Red ?? 0;
            var green = backgroundColor?.Green ?? 0;
            var blue = backgroundColor?.Blue ?? 0;

            return new KZJobEntry
            {
                PrepAreaName = zoneName.ToLower(),
                JobName = jobCell.FormattedValue,
                ColorBackground = Color.FromArgb(
                    (int)(red * 255),
                    (int)(green * 255),
                    (int)(blue * 255))
            };
        }
    }

    public class SpreadsheetHelperException : Exception
    {
        public SpreadsheetHelperException(string message) : base(message) { }
        public SpreadsheetHelperException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}

