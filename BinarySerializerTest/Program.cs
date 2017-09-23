using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinarySerializerTest
{
    class MainClass
    {
		public static void Main(string[] args)
		{
            InfoDict dataObj = new InfoDict();

            Console.WriteLine("Reading data...");

            dataObj.ReadCSV();

            Console.WriteLine("Saving data to binary...");

            dataObj.Save("data.bin");

            Console.WriteLine("Saving data to compressed binary...");

            dataObj.Compress("data.gz");

            Console.WriteLine("Decompressing data...");
            InfoDict rehydrated = InfoDict.Decompress("data.gz");

            Console.WriteLine("Finished!");
		}
    }

    [Serializable]
    public class InfoDict
    {
        public Dictionary<string, List<Info>> data = new Dictionary<string, List<Info>>();

        public void ReadCSV()
		{
			int count = 0;
            int goodCount = 0;
			foreach (string line in File.ReadAllLines(@"../../Aging_and_Disability_Services_-_Client_Level_Data_2016.csv"))
			{

				try
				{
					Info record = new Info(line.Split(',').ToArray());
                    string neighborhood;

                    if (record.GeographicLocation.Contains(':'))
                    {
                        neighborhood = record.GeographicLocation.Split(':')[1].Trim();
                        record.GeographicLocation = neighborhood;
                    }
                    else
                    {
                        neighborhood = record.GeographicLocation.Trim();
                    }

                    if (data.ContainsKey(neighborhood))
                    {
                        data[neighborhood].Add(record);
                    }
                    else
                    {
                        data[neighborhood] = new List<Info>() { record };
                    }
                    goodCount++;
                }
				catch
				{
					Console.WriteLine("Problem at {0}", count);
				}

				count++;
				if (count % 100 == 0)
				{
					Console.WriteLine("{0}", count);
				}
			}

            data.Remove(" ");
            data.Remove("N");
            data.Remove("");

			Console.WriteLine("Finished making dictionary!");

            foreach (string key in data.Keys)
            {
                Console.WriteLine("{0}:{1}", key, data[key].Count);
            }

		}

		public void Save(string sFileName)
		{
			using (FileStream oStream = new FileStream(sFileName, FileMode.Create, FileAccess.ReadWrite))
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(oStream, this);
				oStream.Flush();
			}
		}

		public static InfoDict Load(string sFileName)
		{
			FileInfo fi = null;
			InfoDict rehydrated = null;

			fi = new FileInfo(sFileName);
			if (fi.Exists)
			{
				using (FileStream oStream = new FileStream(sFileName, FileMode.Open, FileAccess.Read))
				{
					BinaryFormatter bf = new BinaryFormatter();
					oStream.Position = 0;
					rehydrated = (InfoDict)bf.Deserialize(oStream);
				}
			}

			return rehydrated;
		}

		public void Compress(string fileName)
		{

			using (FileStream compressedFileStream = File.Create(fileName))
			{
				using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(compressionStream, this);
					compressedFileStream.Flush();
				}
			}


		}

		public static InfoDict Decompress(string fileName)
		{
			FileInfo fi = null;
			InfoDict rehydrated = null;

			fi = new FileInfo(fileName);
            if (fi.Exists)
            {
                using (FileStream originalFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {

                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        originalFileStream.Position = 0;
                        rehydrated = (InfoDict)bf.Deserialize(decompressionStream);
                    }

                }
            }

            return rehydrated;
		}
    }


    [Serializable]
    public struct Info 
    {
        public Info(string[] lineArray)
        {
            Int32.TryParse(lineArray[0], out ActivityID);
            Int32.TryParse(lineArray[1], out ClientID);
		    GeographicLocation      = lineArray[2];
		    AgeRange                = lineArray[3];
		    Ethnicity               = lineArray[4];
            Int32.TryParse(lineArray[5], out Race);
            Int32.TryParse(lineArray[6], out Income);
		    LiveAlone               = lineArray[7];
		    LimitedEnglish          = lineArray[8];
		    Language                = lineArray[9];
		    NutritionalRisk         = lineArray[10];
		    SingleParent            = lineArray[11];
		    HouseholdWithChildren   = lineArray[12];
		    Homeless                = lineArray[13];
		    DisabilityStatus        = lineArray[14];
		    Unincorporated          = lineArray[15];
		    NumberofChildren        = lineArray[16];
		    RelationshipToRecipient = lineArray[17];
		    Kinship                 = lineArray[18];
		    Veteran                 = lineArray[19];
		    Eating                  = lineArray[20];
		    Toileting               = lineArray[21];
		    Walking                 = lineArray[22];
		    GettingPlaces           = lineArray[23];
		    Transferring            = lineArray[24];
		    Dressing                = lineArray[25];
		    Bathing                 = lineArray[26];
		    MedicalManagement       = lineArray[27];
		    Cooking                 = lineArray[28];
		    Shopping                = lineArray[29];
		    Chores                  = lineArray[30];
		    Driving                 = lineArray[31];
		    HeavyHousework          = lineArray[32];
		    Phoning                 = lineArray[33];
		    MoneyManagement         = lineArray[34];
            Int32.TryParse(lineArray[35], out DivisionID);
            Int32.TryParse(lineArray[36], out ServiceMonth);
            Int32.TryParse(lineArray[37], out Serviceyear);
            Int32.TryParse(lineArray[38], out AgencyID);
            Int32.TryParse(lineArray[39], out SiteID);
            Int32.TryParse(lineArray[40], out ServiceArea );
            Int32.TryParse(lineArray[41], out ServiceTypeID);
            Int32.TryParse(lineArray[42], out UnitsProvided);
		    UnitsProvidedType       = lineArray[43];
            Int32.TryParse(lineArray[44], out ContractID);
        }

        public int ActivityID;
        public int ClientID;
        public string GeographicLocation;
        public string AgeRange;
        public string Ethnicity;
        public int Race;
        public int Income;
        public string LiveAlone;
        public string LimitedEnglish;
        public string Language;
        public string NutritionalRisk;
        public string SingleParent;
        public string HouseholdWithChildren;
        public string Homeless;
        public string DisabilityStatus;
        public string Unincorporated;
        public string NumberofChildren;
        public string RelationshipToRecipient;
        public string Kinship;
        public string Veteran;
        public string Eating;
        public string Toileting;
        public string Walking;
        public string GettingPlaces;
        public string Transferring;
        public string Dressing;
        public string Bathing;
        public string MedicalManagement;
        public string Cooking;
        public string Shopping;
        public string Chores;
        public string Driving;
        public string HeavyHousework;
        public string Phoning;
        public string MoneyManagement;
        public int DivisionID;
        public int ServiceMonth;
        public int Serviceyear;
        public int AgencyID;
        public int SiteID;
        public int ServiceArea;
        public int ServiceTypeID;
        public int UnitsProvided;
        public string UnitsProvidedType;
        public int ContractID;
    }

}
