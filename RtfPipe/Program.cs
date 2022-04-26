// Very primitive RTF 2 HTML reader 
// Converts tiny subset of RTF (from VS IDE) into html.
// Author: Mike Stall (http://blogs.msdn.com/jmstall)
// Gets input RTF from clipboard.
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
// using RtfPipe;

namespace ClipBoard1
{
  class Program
  {
    [STAThread()]
    static void Main(string[] args)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      Console.WriteLine("Get RTF from the file.");
      string rtf = File.ReadAllText(@"D:\Users\Tetragramm\Downloads\Plane Construction Rules 10 (EXPERIMENTAL).rtf");

      var header = @"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">

<head>
	<meta charset=""utf-8"" />
	<meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
	<title>Flying Circus Plane Builder Rules</title>
	<link rel=""shortcut icon"" type=""image/x-icon"" href=""../favicon.ico"" />
</head>

<body class=""html_body"" dir=""ltr"">
";

      var html = RtfPipe.Rtf.ToHtml(rtf);

      html = Regex.Replace(html, @"data:([^>]*)>([^>]*)></p>", @"./Frame.png""></p>");
      html = Regex.Replace(html, @"<u>Weapons are located here.</u>", @"<a href=""https://tetragramm.github.io/PlaneBuilder/WeaponDisplay/weapons.html"">Weapons are located here.</a>");

      TextWriter tw = new StreamWriter("out.html");
      tw.Write(header);
      tw.Write(html);
      tw.Write("</body></html>");
      tw.Close();
    }
  }
}