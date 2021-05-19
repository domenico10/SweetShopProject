using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ToDoListApp.Models
{
    public class Filters
    {
        public Filters(string filterString)
        {
            FilterString = filterString ?? "all-all-all"; //DEFAULT IS SET TO SHOW ALL ALL ALL 
            string[] filters = FilterString.Split('-'); //CREATE AN ARRAY OF STRING AND SPLIT THEM USING THE DASH
            CategoryId = filters[0];//ASSIGN PROPERTY TO THE STRING INDEX
            Due = filters[1];//ASSIGN PROPERTY TO THE STRING INDEX
            StatusId = filters[2];//ASSIGN PROPERTY TO THE STRING INDEX
        }

        public string FilterString { get; }//PROPERTY FOR THE FILTER STRING
        public string CategoryId { get; }//PROPERTY FOR THE CATEGORY IN THE FILTER
        public string  Due { get; }//PROPERTY FOR THE DUEDATE IN THE FILTER
        public string StatusId { get; }//PROPERTY FOR THE STATUS IN THE FILTER

        public bool HasStatus => StatusId.ToLower() != "all"; //TRUE/FALSE--CHECK IF STATUS FILTER IS NOT SET TO ALL(DEFAULT)
        public bool HasDue => Due.ToLower() != "all";//TRUE/FALSE--CHECK IF DUEDATE FILTER IS NOT SET TO ALL(DEFAULT)
        public bool HasCategory => CategoryId.ToLower() != "all";//TRUE/FALSE--CHECK IF CATEGORY FILTER IS NOT SET TO ALL(DEFAULT)

        public static Dictionary<string, string> DueFilterValues => //CREATE A DICTIONARY THAT TAKES THE KEY AND THE VALUE THAT HOLDS VALUES FOR THE DUE PROPERTY
            new Dictionary<string, string> //CREATE NEW INSTANCE OF DICTIONARY
            {
                {"future","Future" },  //KEY,VALUE
                {"past","Past" },  //KEY,VALUE
                {"today","Today"}  //KEY,VALUE
            };

        public bool IsPast => Due.ToLower() == "past";  //TRUE/FALSE--CHECK IF THE TASK IS PAST
        public bool IsFuture => Due.ToLower() == "future"; //TRUE/FALSE--CHECK IF THE TASK IS FUTURE
        public bool IsToday => Due.ToLower() == "today";//TRUE/FALSE--CHECK IF THE TASK IS TODAY

    }
}
