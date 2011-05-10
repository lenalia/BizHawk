﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizHawk.MultiClient
{
    class MovieHeader
    {
        //Required Header Params
        //Emulation - Core version, will be 1.0.0 until there is a versioning system
        //Movie -     Versioning for the Movie code itself, or perhaps this could be changed client version?
        //Platform -  Must know what platform we are making a movie on!
        //GameName -  Which game
        //TODO: GUID, checksum of game, other stuff

        Dictionary<string, string> HeaderParams = new Dictionary<string, string>(); //Platform specific options go here
        public List<string> Comments = new List<string>();
        
        public const string EMULATIONVERSION = "EmulationVersion";
        public const string MOVIEVERSION = "MovieVersion";
        public const string PLATFORM = "Platform";
        public const string GAMENAME = "GameName";
        public const string AUTHOR = "Author";

        public static string MovieVersion = "BizHawk v0.0.1";

        public MovieHeader() //All required fields will be set to default values
        {
            HeaderParams.Add(EMULATIONVERSION, "v1.0.0");
            HeaderParams.Add(MOVIEVERSION, "v1.0.0");
            HeaderParams.Add(PLATFORM, "");
            HeaderParams.Add(GAMENAME, "");
            HeaderParams.Add(AUTHOR, "");
        }

        public MovieHeader(string EmulatorVersion, string MovieVersion, string Platform, string GameName, string Author)
        {
            HeaderParams.Add("EmulationVersion", EmulatorVersion);
            HeaderParams.Add("MovieVersion", MovieVersion);
            HeaderParams.Add("Platform", Platform);
            HeaderParams.Add("GameName", GameName);
            HeaderParams.Add("Author", Author);
        }

        /// <summary>
        /// Adds the key value pair to header params.  If key already exists, value will be updated
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddHeaderLine(string key, string value) //TODO: check for redundancy and return bool?
        {
            string temp = value;
            
            if (!HeaderParams.TryGetValue(key, out temp)) //TODO: does a failed attempt mess with value?
                HeaderParams.Add(key, value);
        }

        public bool RemoveHeaderLine(string key)
        {
            return HeaderParams.Remove(key);
        }

        public void Clear()
        {
            HeaderParams.Clear();
        }

        public string GetHeaderLine(string key)
        {
            string value = "";
            HeaderParams.TryGetValue(key, out value);
            return value;
        }

        public Dictionary<string, string> GetHeaderInfo()
        {
            return HeaderParams;
        }
    }
}
