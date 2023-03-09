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


                // Read all lines from the CSV file
                var fileContent = File.ReadAllLines(filePath);


                // Remove any white space from each line
                fileContent = fileContent.Select(line => Regex.Replace(line, @"\s", "")).ToArray();


                // Split the first line into headers, and replace any spaces with underscores
                string[] headers = fileContent[0].Split(',').Select(x => x.Replace(" ", "_").Replace("/", "_").Trim('\"')).ToArray();

                #region
                for (int i= 0; i < headers.Length; i++)
                {
                    if (headers[i] == "ApplicationNumber")
                    {
                        headers[i] = "applicationId";
                    }
                    else if (headers[i] == "BannerID")
                    {
                        headers[i] = "studentId";
                    }
                    else if (headers[i] == "StudentID")
                    {
                        headers[i] = "globalId";
                    }
                    else if (headers[i] == "LastName")
                    {
                        headers[i] = "lastName";
                    }
                    else if (headers[i] == "FirstName")
                    {
                        headers[i] = "firstName";
                    }
                    else if (headers[i] == "MiddleName")
                    {
                        headers[i] = "middleName";
                    }
                    else if (headers[i] == "Suffix")
                    {
                        headers[i] = "suffix";
                    }
                    else if (headers[i] == "FormerLastName")
                    {
                        headers[i] = "maidenName";
                    }
                    else if (headers[i] == "MailingStreet")
                    {
                        headers[i] = "addressLine1";
                    }                                   
                    else if (headers[i] == "MailingCity")
                    {
                        headers[i] = "city";
                    }
                    else if (headers[i] == "Mailing_Sate_Province")
                    {
                        headers[i] = "state";
                    }
                    else if (headers[i] == "Mailing_Zip_Postal_Code")
                    {
                        headers[i] = "postalCode";
                    }
                    else if (headers[i] == "Home_Phone")
                    {
                        headers[i] = "homePhone";
                    }
                    else if (headers[i] == "Other_Phone")
                    {
                        headers[i] = "workPhone";
                    }
                    else if (headers[i] == "Mobile")
                    {
                        headers[i] = "cellPhone";
                    }
                    else if (headers[i] == "Email")
                    {
                        headers[i] = "email1";
                    }
                    else if (headers[i] == "Alt_Email")
                    {
                        headers[i] = "email2";
                    }
                    else if (headers[i] == "Citizenship")
                    {
                        headers[i] = "primaryCitizenship";
                    }
                    else if (headers[i] == "Application_Stage")
                    {
                        headers[i] = "applicationStatusCode";
                    }
                    else if (headers[i] == "Application_Staus")
                    {
                        headers[i] = "applicationStatusDesc";
                    }
                    else if (headers[i] == "Application_Submit_Date")
                    {
                        headers[i] = "appliedDate";
                    }

                    #endregion

                }
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
                        xml.Add(new XElement("Item", headers.Select(header => new XElement(header, " "))));
                    }
                    else
                    {
                        // Otherwise, create a row element with values from each column
                        var item = new XElement("Item");
                        for (int i = 0; i < headers.Length && i < columns.Length; i++)
                        {
                            var value = string.IsNullOrWhiteSpace(columns[i]) ? " " : columns[i];
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
