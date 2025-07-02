using CSharp.Models;

namespace CSharp.Helpers
{
    public static class HtmlGenerator
    {
        public static string GenerateTable(List<TimeEntry> employees)
        {
            var html = @"
            <html>
            <head>
              <style>
                body {
                  margin: 0;
                  padding: 0;
                  display: flex;
                  justify-content: center;
                  align-items: center;
                  height: 100vh;
                  font-family: Arial, sans-serif;
                }

                table {
                  border-collapse: collapse;
                  width: 80%;
                  max-width: 600px;
                }

                th, td {
                  border: 1px solid #aaa;
                  padding: 8px;
                  text-align: center;
                }

                tr:nth-child(even) {
                  background-color: #f2f2f2;
                }

                tr:nth-child(odd) {
                  background-color: #ffffff;
                }
              </style>
            </head>
            <body>
              <table>
                <tr><th>Name</th><th>Total Time Worked</th></tr>";


            foreach (var emp in employees)
            {
                string style = emp.TimeWorked < 100 ? " style='background-color:red;'" : "";
                html += $"<tr{style}><td>{emp.Employee}</td><td>{emp.TimeWorked}</td></tr>";
            }

            html += "</table></body></html>";
            return html;
        }
    }
}
