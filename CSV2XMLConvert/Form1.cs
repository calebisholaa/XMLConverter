using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;

namespace CSV2XMLConvert
{
    public partial class from1 : Form
    {

        string filePath = "";
        string fileContent = "";
        public from1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenAndReadFile();
            
        }


        private void OpenAndReadFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    this.filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            this.displayText.Text = fileContent;

            // Console.WriteLine(fileContent + "From Open");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SaveFile(fileContent);
            this.displayText.Text = SaveFile(fileContent);
        }

        private void Convert2XML(string fileContent)
        {
            var xml = new XElement("TopElement",
                fileContent.Select(line => new XElement("Item",
                fileContent.Split(',')
             .Select((column, index) => new XElement("Column" + index, column)))));

            // xml.Save(@"C:\xmlout.xml");
        }


        private string SaveFile(string file)
        {
            string result = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {


                //var header = File.ReadLines(filePath).First().Split(','); //Read header
                //var lines = File.ReadAllLines(filePath).Skip(1); //Skip header row        

                ////format the xml how you want
                //var xml = new XElement("EmpDetails",
                //    lines.Select(line => new XElement("Item",
                //        line.Split(',').Select((column, index) => new XElement(header[index], column)))));


                // Read all lines from the CSV file
                var fileContent = File.ReadAllLines(filePath);


                // Remove any white space from each line
                fileContent = fileContent.Select(line => Regex.Replace(line, @"\s", "")).ToArray();


                // Split the first line into headers, and replace any spaces with underscores
                string[] headers = fileContent[0].Split(',').Select(x => x.Replace(" ", "_").Replace("/", "_").Trim('\"')).ToArray();

                // Skip the first line (headers)
                fileContent = fileContent.Skip(1).ToArray();


                // Create a new XML document with a top-level element
                var xml = new XElement("TopElement");

                // Iterate over the rows in the CSV file
                foreach (string line in fileContent)
                {
                    
                    // Split the line into columns, and trim any double quotes
                    string[] columns = line.Split(',').Select(x => x.Trim('\"')).ToArray();
                  

                
                    // If the row has no data, create a row element with "null" values for each column
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        xml.Add(new XElement("Item", headers.Select(header => new XElement(header, "null"))));
                    }
                    else
                    {
                        // Otherwise, create a row element with values from each column
                        var item = new XElement("Item");
                        for (int i = 0; i < headers.Length && i < columns.Length; i++)
                        {
                            var value = string.IsNullOrWhiteSpace(columns[i]) ? "null" : columns[i];
                            item.Add(new XElement(headers[i], value));
                        }
                        xml.Add(item);
                    }
                }


                #region
                //// Read all lines from the CSV file
                //var fileContent = File.ReadAllLines(filePath);

                //// Split the first line into headers, and replace any spaces with underscores
                //string[] headers = fileContent[0].Split(',').Select(x => x.Replace(" ", "_").Replace("/", "_").Trim('\"')).ToArray();

                //// Skip the first line (headers)
                //fileContent = fileContent.Skip(1).ToArray();

                //// Create a new XML document with a top-level element
                //var xml = new XElement("TopElement");

                //// Create a variable to hold the current row
                //string currentRow = "";

                //// Iterate over the rows in the CSV file
                //foreach (string line in fileContent)
                //{
                //    // If the current row is not empty, add it to the XML document
                //    if (!string.IsNullOrWhiteSpace(currentRow))
                //    {
                //        // Split the row into columns, and trim any double quotes
                //        string[] columns = currentRow.Split(',').Select(x => x.Trim('\"')).ToArray();

                //        // Create a row element with values from each column
                //        var item = new XElement("Item");
                //        for (int i = 0; i < headers.Length && i < columns.Length; i++)
                //        {
                //            var value = string.IsNullOrWhiteSpace(columns[i]) ? "null" : columns[i];
                //            item.Add(new XElement(headers[i], value));
                //        }
                //        xml.Add(item);
                //    }

                //    // Set the current row to the next non-empty row in the CSV file
                //    currentRow = line.Trim();

                //    // If the current row is empty, continue to the next row
                //    if (string.IsNullOrWhiteSpace(currentRow))
                //    {
                //        continue;
                //    }

                //    // Check if the current row is a continuation of the previous row
                //    if (char.IsLetterOrDigit(currentRow[0]))
                //    {
                //        currentRow = "," + currentRow;
                //    }
                //    else
                //    {
                //        // If the current row is not a continuation of the previous row, add it to the XML document
                //        xml.Add(new XElement("Item", headers.Select(header => new XElement(header, "null"))));
                //    }
                //}

                //// Add the last row to the XML document
                //if (!string.IsNullOrWhiteSpace(currentRow))
                //{
                //    // Split the row into columns, and trim any double quotes
                //    string[] columns = currentRow.Split(',').Select(x => x.Trim('\"')).ToArray();

                //    // Create a row element with values from each column
                //    var item = new XElement("Item");
                //    for (int i = 0; i < headers.Length && i < columns.Length; i++)
                //    {
                //        var value = string.IsNullOrWhiteSpace(columns[i]) ? "null" : columns[i];
                //        item.Add(new XElement(headers[i], value));
                //    }
                //    xml.Add(item);
                //}
                #endregion

                xml.Save(saveFileDialog.FileName);
                result = xml.ToString();


                //var xml = new XElement("TopElement",
                //fileContent.Select(line => new XElement("Item",
                //    fileContent.Split(',')
                //    .Select((column, index) => new XElement("Column" +index, column)))));



            }

            return result;
        }
    }
}
