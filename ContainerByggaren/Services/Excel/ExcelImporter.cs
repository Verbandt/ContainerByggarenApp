using ClosedXML.Excel;
using ContainerByggaren.Models;
using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerByggaren.Services.Excel
{
    public class ExcelImporter
    {
        public List<Article> ImportArticles(string filepath)
        {
            List<Article> articles = [];

            using var workbook = new XLWorkbook(filepath);
            var worksheet = workbook.Worksheet(1);

            var usedRange = worksheet.RangeUsed();
            if (usedRange == null)
                throw new Exception("The worksheet is empty."); // TODO: Fixa till ett bättre felmeddelande

            var rows = usedRange.RowsUsed().ToList();
            if (rows.Count < 2)
                throw new Exception("The worksheet must contain at least one data row."); // TODO: Fixa till ett bättre felmeddelande

            var headerRow = rows[0];

            Dictionary<string, int> columnMap = CreateColumnMap(headerRow);

            foreach (var row in rows.Skip(1))
            {
                if (row.IsEmpty())
                    continue;

                Article article = new(
                    articleNum: row.Cell(columnMap["ArticleNum"]).GetString(),
                    emb: row.Cell(columnMap["Emb"]).GetString(),
                    unitLoad: GetDouble(row, columnMap, "Unit load"),
                    partsPerTrain: GetDouble(row, columnMap, "Parts per train"),
                    lineProductivity: GetDouble(row, columnMap, "Line productivity"),
                    volumeM3: GetDouble(row, columnMap, "Volume m3"),
                    weight: GetDouble(row, columnMap, "Weight"),
                    embPerTrain: GetDouble(row, columnMap, "Emb per train"),
                    m2: GetDouble(row, columnMap, "M2"),
                    embLength: GetDouble(row, columnMap, "Emb length"),
                    embWidth: GetDouble(row, columnMap, "Emb width"),
                    embHeight: GetDouble(row, columnMap, "Emb height"),
                    presam: GetString(row, columnMap, "PRESAM"),
                    embColor: GetString(row, columnMap, "Emb color"),
                    steelEmb: GetString(row, columnMap, "Steel emb")
                    );
            }

            return articles;
        }

        private Dictionary<string, int> CreateColumnMap(IXLRangeRow headerRow)
        {
            var columnmap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var cell in headerRow.CellsUsed())
            {
                var headerValue = cell.GetString().Trim();
                if (!string.IsNullOrEmpty(headerValue))
                {
                    columnmap[headerValue] = cell.Address.ColumnNumber;
                }
            }

            return columnmap;
        }

        private string GetString(IXLRangeRow row, Dictionary<string, int> columnMap, string columnName)
        {
            if (!columnMap.TryGetValue(columnName, out int columnNumber))
                throw new Exception($"Missing Excel column: {columnName}"); // TODO: Fixa till ett bättre felmeddelande

            return row.Cell(columnNumber).GetFormattedString().Trim();
        }

        private double GetDouble(IXLRangeRow row, Dictionary<string, int> columnMap, string columnName)
        {
            string text = GetString(row, columnMap, columnName);

            if (string.IsNullOrWhiteSpace(text))
                return 0;

            text = text.Replace(",", ".");

            if (double.TryParse(
                    text,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double value))
            {
                return value;
            }

            throw new Exception($"Could not convert value '{text}' in column '{columnName}' to number.");  // TODO: Fixa till ett bättre felmeddelande
        }

    }
}
