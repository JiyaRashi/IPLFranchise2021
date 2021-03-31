using IPLFranchise2021.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.ViewModels
{
    public class ScoreDetailsViewModel :BindableBase
    {
        private List<Batsman> _batsmenDetails;



        public ScoreDetailsViewModel()
        {
            _batsmenDetails = GetAllBatsmen();
        }

        public List<Batsman> BatsmenDetails
        {
            get { return _batsmenDetails; }
            set { _batsmenDetails = value; }
        }

        public List<Batsman> GetAllBatsmen()
        {
            string path = "Data/sampleScore.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToBat();

            return query.ToList();
        }
    }
}
