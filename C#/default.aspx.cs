using System;
using System.IO;
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        for (int i = 0; i < 2001; i++)
        {
            WebClient webClient = new WebClient();
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\" + i + "b.png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\" + i + "b.png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\blue " + i + "b.png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\blue " + i + "b.png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\blue " + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\blue " + i + ".png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Red\red " + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Red\red " + i + ".png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Orange\orange " + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Orange\orange " + i + ".png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Yellow\yellow" + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Yellow\yellow" + i + ".png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Green\green" + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Green\green" + i + ".png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Violet\violet" + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Violet\violet" + i + ".png");
            //if (File.Exists(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Gray\gray" + i + ".png"))
            //    File.Delete(@"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Gray\gray" + i + ".png");

            //Normal
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|3FCDFF|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|FF4A4A|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Red\" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|FF7F27|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Orange\" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|FFFF33|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Yellow\" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|33CC00|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Green\" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|DAA8DD|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Violet\" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|C9C9C9|11|_|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Gray\" + i + ".png");
            

            //Bold
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|3FCDFF|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Blue\b" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|FF4A4A|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Red\b" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|FF7F27|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Orange\b" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|FFFF33|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Yellow\b" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|33CC00|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Green\b" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|DAA8DD|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Violet\b" + i + ".png");
            //webClient.DownloadFile("https://chart.googleapis.com/chart?chst=d_map_spin&chld=1.0|0|C9C9C9|14|b|" + i, @"C:\Users\Galdo\Documents\Visual Studio 2013\Projects\Eisk\C#\App_Resources\images\icons\Gray\b" + i + ".png");
                                                                                                
        }

        Response.Redirect("~/secured/log-in/log-in.aspx");
    }
}
