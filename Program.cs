using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

public interface IFormatConverter
{
    string Convert(string csvData);
}

public class XmlConverter : IFormatConverter
{
    public string Convert(string csvData)
    {
        var lines = csvData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        var headers = lines[0].Split(',');
        var personas = new XElement("personas");

        for (int i = 1; i < lines.Length; i++)
        {
            var fields = lines[i].Split(',');
            var persona = new XElement("persona");
            for (int j = 0; j < headers.Length; j++)
            {
                persona.Add(new XElement(headers[j], fields[j]));
            }
            personas.Add(persona);
        }

        return personas.ToString();
    }
}

public class JsonConverter : IFormatConverter
{
    public string Convert(string csvData)
    {
        // Lógica para convertir CSV a JSON (como en el ejemplo anterior)
        return csvData +  " json";
    }
}


class Program
{
    static void Main(string[] args)
    {

       // Console.WriteLine(Environment.CurrentDirectory + "\\data\\personas.csv");
       // Console.ReadKey();
        //Uno();
        Dos();

        
    }

    private static void Dos()
    {
        // Create the text file.
        string csvString = @"GREAL,Great Lakes Food Market,Howard Snyder,Marketing Manager,(503) 555-7555,2732 Baker Blvd.,Eugene,OR,97403,USA
HUNGC,Hungry Coyote Import Store,Yoshi Latimer,Sales Representative,(503) 555-6874,City Center Plaza 516 Main St.,Elgin,OR,97827,USA
LAZYK,Lazy K Kountry Store,John Steel,Marketing Manager,(509) 555-7969,12 Orchestra Terrace,Walla Walla,WA,99362,USA
LETSS,Let's Stop N Shop,Jaime Yorres,Owner,(415) 555-5938,87 Polk St. Suite 5,San Francisco,CA,94117,USA";
        File.WriteAllText("data\\cust.csv", csvString);

        // Read into an array of strings.
        string[] source = File.ReadAllLines("data\\cust.csv");
        XElement cust = new XElement("Root",
            from str in source
            let fields = str.Split(',')
            select new XElement("Customer",
                new XAttribute("CustomerID", fields[0]),
                new XElement("CompanyName", fields[1]),
                new XElement("ContactName", fields[2]),
                new XElement("ContactTitle", fields[3]),
                new XElement("Phone", fields[4]),
                new XElement("FullAddress",
                    new XElement("Address", fields[5]),
                    new XElement("City", fields[6]),
                    new XElement("Region", fields[7]),
                    new XElement("PostalCode", fields[8]),
                    new XElement("Country", fields[9])
                )
            )
        );
        Console.WriteLine(cust);
        File.WriteAllText("data\\cust.xml", cust.ToString());
        Console.ReadKey();

    }

    private static void Uno()
    {
        string csvData = File.ReadAllText(Environment.CurrentDirectory+"\\data\\personas.csv");

        // Convertir a XML
        IFormatConverter converter = new XmlConverter();
        string xmlData = converter.Convert(csvData);

        Console.WriteLine(xmlData);

        // Convertir a JSON
        converter = new JsonConverter();
        string jsonData = converter.Convert(csvData);
        Console.WriteLine(jsonData);

        Console.WriteLine("Conversión realizada correctamente.");
        Console.ReadKey();
    }
}

