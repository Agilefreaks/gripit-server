using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GripitServer.Models;

namespace GripitServer.Services
{
    public class DataFrameProcessor : IDataFrameProcessor
    {
        private const char StateSeparator = ';';
        private const char ValueSeparator = ',';
        private const char IdSeparator = ':';
        private const string IdPattern = @"\w+";
        private const string ValuePattern = "[a-fA-F0-9]{1,3}";
        private static readonly string ValuesPattern = $"({ValuePattern}{ValueSeparator}){{3}}{ValuePattern}";
        private readonly Regex _stateDataRegex = new Regex($@"^({IdPattern}){IdSeparator}({ValuesPattern})$");


        public IList<ClimbingHoldState> GetHoldStates(DataFrame dataFrame)
        {
            var statesDataList = dataFrame.DataString.Split(StateSeparator);
            return statesDataList.Select(GetState).Where(state => state != null).ToList();
        }

        public ClimbingHoldState GetState(string stateData)
        {
            var match = _stateDataRegex.Match(stateData);
            if (!match.Success) return null;

            var idData = match.Groups[1].Value;
            var valuesData = match.Groups[2].Value.Split(ValueSeparator);
            var state = new ClimbingHoldState
            {
                Id = idData,
                UpValue = Convert.ToInt32(valuesData[0], 16),
                RightValue = Convert.ToInt32(valuesData[1], 16),
                DownValue = Convert.ToInt32(valuesData[2], 16),
                LeftValue = Convert.ToInt32(valuesData[3], 16)
            };

            return state;
        }
    }
}