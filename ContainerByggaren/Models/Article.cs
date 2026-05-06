using System;
using System.Collections.Generic;

namespace ContainerByggaren.Models
{
    public class Article
    {
        public string ArticleNum { get; set; }
        public string Emb { get; set; }

        public double EmbLength { get; set; }
        public double EmbWidth { get; set; }
        public double EmbLengthFromExcelFile { get; set; }
        public double EmbWidthFromExcelFile { get; set; }
        public double OgEmbLength { get; set; }
        public double OgEmbWidth { get; set; }
        public double EmbHeight { get; set; }

        public double UnitLoad { get; set; }
        public double PartsPerTrain { get; set; }
        public double LineProductivity { get; set; }
        public double VolumeM3 { get; set; }
        public double Weight { get; set; }
        public double EmbPerTrain { get; set; }
        public double M2 { get; set; }

        public int CurrentTrainDeparture { get; set; }
        public double ArticlesToSendOnNextTrain { get; set; }

        public Dictionary<int, int> EmbSentPerDeparture { get; set; }

        public int HighestValue { get; set; }
        public double CurrentAmountOfEmbInStock { get; set; }

        public double CoverTimeH { get; set; }
        public double IntervalToSendEmbOn { get; set; }
        public double EmbPerQuarter { get; set; }

        public string Presam { get; set; }
        public string EmbColor { get; set; }
        public bool SteelEmb { get; set; }
        public double ReducedHeight { get; set; }

        public double TotalAmountOfEmbSent { get; set; }

        public int GetEmbSentForDeparture(int departure)
        {
            return EmbSentPerDeparture.TryGetValue(departure, out int value)
                ? value
                : 0;
        }

        public void AddEmbSentForDeparture(int departure, int amount)
        {
            if (!EmbSentPerDeparture.ContainsKey(departure))
                EmbSentPerDeparture[departure] = 0;

            EmbSentPerDeparture[departure] += amount;
        }

        public Article(
            object articleNum,
            object emb,
            double unitLoad,
            double partsPerTrain,
            double lineProductivity,
            double volumeM3,
            double weight,
            double embPerTrain,
            double m2,
            double embLength,
            double embWidth,
            double embHeight,
            string presam,
            string embColor,
            string steelEmb)
        {
            ArticleNum = articleNum.ToString() ?? "";
            Emb = emb.ToString() ?? "";

            EmbLength = embLength;
            EmbWidth = embWidth;
            EmbLengthFromExcelFile = embLength;
            EmbWidthFromExcelFile = embWidth;
            OgEmbLength = embLength;
            OgEmbWidth = embWidth;
            EmbHeight = embHeight;

            UnitLoad = unitLoad;
            PartsPerTrain = partsPerTrain;
            LineProductivity = lineProductivity;
            VolumeM3 = volumeM3;
            Weight = weight;
            EmbPerTrain = embPerTrain;
            M2 = m2;

            CurrentTrainDeparture = 0;
            ArticlesToSendOnNextTrain = 0;

            EmbSentPerDeparture = new Dictionary<int, int>();

            HighestValue = (int)Math.Ceiling(embPerTrain);
            CurrentAmountOfEmbInStock = 0;

            CoverTimeH = unitLoad / lineProductivity;
            IntervalToSendEmbOn = (unitLoad / lineProductivity) * 4;
            EmbPerQuarter = ((lineProductivity / 60) * 15) / unitLoad;

            Presam = presam;
            EmbColor = embColor;

            SteelEmb = steelEmb.ToLower() == "steel rack";
            ReducedHeight = SteelEmb ? 120 : 0;

            TotalAmountOfEmbSent = 0;


        }
    }
}
