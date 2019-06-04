using Eisk.BusinessEntities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Linq;
using System.IO;

namespace CL.App_Logic.Helpers
{

    public class Index
    {
        private global::System.String _RepName;
        public global::System.String RepName
        {
            get { return _RepName; }
            set { _RepName = value; }
        }

        private global::System.Int32 _RepID;
        public global::System.Int32 RepID
        {
            get { return _RepID; }
            set { _RepID = value; }
        }

        private global::System.Int32 _FirstPageNumber;
        public global::System.Int32 FirstPageNumber
        {
            get { return _FirstPageNumber; }
            set { _FirstPageNumber = value; }
        }

        private global::System.Int32 _LastPageNumber;
        public global::System.Int32 LastPageNumber
        {
            get { return _LastPageNumber; }
            set { _LastPageNumber = value; }
        }

        public Index(string RepName, int RepID, int FirstPageNumber, int LastPageNumber)
        {
            _RepName = RepName;
            _RepID = RepID;
            _FirstPageNumber = FirstPageNumber;
            _LastPageNumber = LastPageNumber;
        }
    }

    public static class Reports
    {
        private const int fullHeaderFirstLineNumber = 9;
        private const int shortHeaderFirstLineNumber = 6;

        #region Reports

        public static void Index(bool isFullHeader, int initialPageNum, int totalPages, string title, Eisk.BusinessEntities.Project project, UserInfo userInfo, List<Index> treeDetails, DateTime time, ExcelPackage pck)
        {
            isFullHeader = (isFullHeader || (!isFullHeader && initialPageNum == 0));
            int lineNumber = 0;
            bool firstRow = false;
            bool lastRow = false;
            bool lastRowAll = false;

            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();
            Eisk.BusinessEntities.ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.First();

            var ws = pck.Workbook.Worksheets.Add(title);

            int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

            ExcelColumn column1 = ws.Column(1);
            ExcelColumn column2 = ws.Column(2);
            ExcelColumn column3 = ws.Column(3);
            ExcelColumn column4 = ws.Column(4);

            column1.Width = 100D;
            column2.Width = 20D;
            column3.Width = 7.5D;
            column4.Width = 7.5D;

            // Set Font
            //
            ws.Cells["A1:D51"].Style.Font.Size = 8;
            ws.Cells["A1:D51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
            //

            // User Header & Project Header
            //
            CreateUserHeader(isFullHeader, false, userInfo, ws);
            CreateProjectHeader(isFullHeader, false, project, projectInfo, ws);
            // 

            // Header
            //
            ws.Cells["A" + (firstLineNumber - 3) + ":D" + (firstLineNumber - 3)].Merge = true;
            ws.Cells["A" + (firstLineNumber - 3)].Value = title;
            ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Bold = true;
            ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Size = 15;
            ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
            ws.Cells["A" + (firstLineNumber - 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A" + (firstLineNumber - 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //

            SetLine(ws, "C" + (firstLineNumber - 2), "D" + (firstLineNumber - 2), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Núm. de Página");

            for (int i = 0; i < treeDetails.Count; i++)
            {
                lineNumber = (firstLineNumber + i);
                firstRow = lineNumber == firstLineNumber;
                lastRow = lineNumber == firstLineNumber + treeDetails.Count - 1;
                lastRowAll = lastRow && i == (treeDetails.Count - 1);

                // --
                if (firstRow && lastRow)
                {
                    SetLineStyle(ws, "A" + (firstLineNumber - 1 + i), "D" + (firstLineNumber - 1 + i), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Top & Bottom Line
                }
                else if (firstRow && !lastRow)
                {
                    SetLineStyle(ws, "A" + (firstLineNumber - 1 + i), "D" + (firstLineNumber - 1 + i), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
                }
                else if (lastRow)
                {
                    SetLineStyle(ws, "A" + (firstLineNumber - 1 + i), "D" + (firstLineNumber - 1 + i), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
                }
                else
                {
                    SetLineStyle(ws, "A" + (firstLineNumber - 1 + i), "D" + (firstLineNumber - 1 + i), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
                }
                // --

                ws.Cells["A" + (firstLineNumber - 1 + i) + ":B" + (firstLineNumber - 1 + i)].Merge = true;
                ws.Cells["A" + (firstLineNumber - 1 + i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["A" + (firstLineNumber - 1 + i)].Style.Font.Bold = true;
                ws.Cells["A" + (firstLineNumber - 1 + i)].Value = treeDetails[i].RepName;

                ws.Cells["C" + (firstLineNumber - 1 + i) + ":D" + (firstLineNumber - 1 + i)].Merge = true;
                ws.Cells["C" + (firstLineNumber - 1 + i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C" + (firstLineNumber - 1 + i)].Value = treeDetails[i].FirstPageNumber + (treeDetails[i].FirstPageNumber == treeDetails[i].LastPageNumber ? "" : "-" + treeDetails[i].LastPageNumber.ToString());
            }

            CreateCreateTime(time, ws, firstLineNumber, treeDetails.Count - 2);
            SetPrinterSettings(ws, ws.Cells["A1:D51"], true, eOrientation.Portrait, 0.5M);
        }

        public static void ProjectInfo(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, Eisk.BusinessEntities.Project project, UserInfo userInfo, DateTime time, ExcelPackage pck)
        {
            isFullHeader = (isFullHeader || (!isFullHeader && !hasIndex && initialPageNum == 0));

            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();
            Eisk.BusinessEntities.ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.First();

            var ws = pck.Workbook.Worksheets.Add("Datos del Proyecto");

            int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

            ExcelRow row1 = ws.Row(firstLineNumber - 1);
            row1.Height = 20.25D;

            ExcelColumn column1 = ws.Column(1);
            ExcelColumn column2 = ws.Column(2);
            ExcelColumn column3 = ws.Column(3);
            ExcelColumn column4 = ws.Column(4);

            column1.Width = 35D;
            column2.Width = 35D;
            column3.Width = 35D;
            column4.Width = 35D;

            // Set Font
            //
            ws.Cells["A1:D51"].Style.Font.Size = 8;
            ws.Cells["A1:D51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
            //

            // User Header & Project Header
            //
            CreateUserHeader(isFullHeader, false, userInfo, ws);
            CreateProjectHeader(isFullHeader, false, project, projectInfo, ws);
            // 

            // Header
            //
            ws.Cells["A" + (firstLineNumber - 3) + ":D" + (firstLineNumber - 3)].Merge = true;
            ws.Cells["A" + (firstLineNumber - 3)].Value = "Datos del Proyecto";
            ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Bold = true;
            ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Size = 15;
            ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
            ws.Cells["A" + (firstLineNumber - 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A" + (firstLineNumber - 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //

            // Table Headers
            //
            SetLine(ws, "A" + (firstLineNumber - 2), "B" + (firstLineNumber - 2), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Nombre del Proyecto");
            SetLine(ws, "A" + (firstLineNumber - 1), "B" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Dirección Línea 1");
            SetLine(ws, "A" + (firstLineNumber + 0), "B" + (firstLineNumber + 0), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Dirección Línea 2");
            SetLine(ws, "A" + (firstLineNumber + 1), "B" + (firstLineNumber + 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Pueblo");
            SetLine(ws, "A" + (firstLineNumber + 2), "B" + (firstLineNumber + 2), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Localización (NAD-83) - Y");
            SetLine(ws, "A" + (firstLineNumber + 3), "B" + (firstLineNumber + 3), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Localización (NAD-83) - X");

            SetLine(ws, "A" + (firstLineNumber + 4), "B" + (firstLineNumber + 4), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Tipo de Desarrollo");
            SetLine(ws, "A" + (firstLineNumber + 5), "B" + (firstLineNumber + 5), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Proyecto de Interés Social");
            SetLine(ws, "A" + (firstLineNumber + 6), "B" + (firstLineNumber + 6), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Total de Cuerdas en el Proyecto");
            SetLine(ws, "A" + (firstLineNumber + 7), "B" + (firstLineNumber + 7), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Estacionamientos");
            SetLine(ws, "A" + (firstLineNumber + 8), "B" + (firstLineNumber + 8), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Mitigación por Perímetro o Solares");

            SetLine(ws, "A" + (firstLineNumber + 9), "B" + (firstLineNumber + 9), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Total de Solares en el Proyecto*");
            SetLine(ws, "A" + (firstLineNumber + 10), "B" + (firstLineNumber + 10), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Proyecto Previamente Impactado");
            SetLine(ws, "A" + (firstLineNumber + 11), "B" + (firstLineNumber + 11), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Distancia entre Árboles");

            SetLine(ws, "A" + (firstLineNumber + 13), "B" + (firstLineNumber + 13), true, ExcelBorderStyle.None, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "* - El Total de Solares en el Proyecto incluye aquellos que requieren mitigación y los que no");

            bool isLots = projectInfoTreeLocation.Mitigation <= 1;
            bool isSocialInterest = (project.ProjectInfoTreeLocations.First().SocialInterest.HasValue && project.ProjectInfoTreeLocations.First().SocialInterest.Value);
            bool isPreviouslyImpacted = (project.ProjectInfoTreeLocations.First().PreviouslyImpacted.HasValue && project.ProjectInfoTreeLocations.First().PreviouslyImpacted.Value);
            //  
            int lines = isLots ? 0 : 1;
            if (!isLots)
                lines += isPreviouslyImpacted ? 0 : 1;

            // Table Content
            //
            SetLine(ws, "C" + (firstLineNumber - 2), "D" + (firstLineNumber - 2), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfo.ProjectName);
            SetLine(ws, "C" + (firstLineNumber - 1), "D" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfo.Address1);
            SetLine(ws, "C" + (firstLineNumber + 0), "D" + (firstLineNumber + 0), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfo.Address2);
            SetLine(ws, "C" + (firstLineNumber + 1), "D" + (firstLineNumber + 1), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfo.City.CityName);
            SetLine(ws, "C" + (firstLineNumber + 2), "D" + (firstLineNumber + 2), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + String.Format("{0:0.####################################}", projectInfoTreeLocation.Y));
            SetLine(ws, "C" + (firstLineNumber + 3), "D" + (firstLineNumber + 3), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + String.Format("{0:0.####################################}", projectInfoTreeLocation.X));

            SetLine(ws, "C" + (firstLineNumber + 4), "D" + (firstLineNumber + 4), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + Eisk.BusinessLogicLayer.ProjectInfoTreeLocationBLL.GetTipoDesarrollo(projectInfoTreeLocation.Mitigation));
            SetLine(ws, "C" + (firstLineNumber + 5), "D" + (firstLineNumber + 5), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + (isSocialInterest ? "Sí" : "No"));
            SetLine(ws, "C" + (firstLineNumber + 6), "D" + (firstLineNumber + 6), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.Acres.ToString());
            SetLine(ws, "C" + (firstLineNumber + 7), "D" + (firstLineNumber + 7), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.Parkings.ToString());
            SetLine(ws, "C" + (firstLineNumber + 8), "D" + (firstLineNumber + 8), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + Eisk.BusinessLogicLayer.ProjectInfoTreeLocationBLL.GetMitigacion(projectInfoTreeLocation));

            SetLine(ws, "C" + (firstLineNumber + 9), "D" + (firstLineNumber + 9), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + (projectInfoTreeLocation.Lots0 + projectInfoTreeLocation.Lots1 + projectInfoTreeLocation.Lots2 + projectInfoTreeLocation.Lots3).ToString());
            SetLine(ws, "C" + (firstLineNumber + 10), "D" + (firstLineNumber + 10), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + (isPreviouslyImpacted ? "Sí" : "No"));
            SetLine(ws, "C" + (firstLineNumber + 11), "D" + (firstLineNumber + 11), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.DistanceBetweenTrees.ToString() + " pies");
            //
            // Table Borders Overight
            //
            SetLineStyle(ws, "A" + (firstLineNumber - 2), "D" + (firstLineNumber - 2), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
            SetLineStyle(ws, "A" + (firstLineNumber - 1), "D" + (firstLineNumber + 11), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
            SetLineStyle(ws, "A" + (firstLineNumber + 9 + lines), "D" + (firstLineNumber + 9 + lines), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
            //

            CreateCreateTime(time, ws, firstLineNumber, 13 + lines);
            CreatePageNumbers(false, totalPages, initialPageNum, ws, firstLineNumber, 13 + lines);
            SetPrinterSettings(ws, ws.Cells["A1:D21"], true, eOrientation.Portrait, 0.5M);

            if (isLots)
            {
                ws.DeleteRow((firstLineNumber + 11), 1);
                ws.DeleteRow((firstLineNumber + 10), 1);
            }
            else
            {
                ws.DeleteRow((firstLineNumber + 14), 1);
                ws.DeleteRow((firstLineNumber + 13), 1);
                ws.DeleteRow((firstLineNumber + 12), 1);
                if (isPreviouslyImpacted)
                    ws.DeleteRow((firstLineNumber + 11), 1);
                else
                    ws.DeleteRow((firstLineNumber + 12), 1);
                ws.DeleteRow((firstLineNumber + 9), 1);
            }
        }

        public static void TreeInventory(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, Eisk.BusinessEntities.Project project, UserInfo userInfo, List<TreeDetail> treeDetails, DateTime time, ExcelPackage pck, int maxLines)
        {
            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();

            int pageCount = (int)Math.Ceiling((double)treeDetails.Count / (double)maxLines);
            int crCnt = 0, prCnt = 0, trCnt = 0, pdCnt = 0;
            int lineNumber = 0;
            bool firstRow = false;
            bool lastRow = false;
            bool lastRowAll = false;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var ws = pck.Workbook.Worksheets.Add("Inventario de Árboles" + ((pageNumber == 0) ? "" : " - #" + (pageNumber + 1).ToString()));

                isFullHeader = (isFullHeader && pageNumber == 0) || ((pageNumber + initialPageNum) == 0 && !hasIndex);
                bool isWideHeader = true;
                int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

                ExcelRow row1 = ws.Row(firstLineNumber - 2);
                ExcelRow row2 = ws.Row(firstLineNumber - 1);

                row1.Height = 27D;
                row2.Height = 63.75D;

                ExcelColumn column1 = ws.Column(1);
                ExcelColumn column2 = ws.Column(2);
                ExcelColumn column3 = ws.Column(3);
                ExcelColumn column4 = ws.Column(4);
                ExcelColumn column5 = ws.Column(5);
                ExcelColumn column6 = ws.Column(6);
                ExcelColumn column7 = ws.Column(7);
                ExcelColumn column8 = ws.Column(8);
                ExcelColumn column9 = ws.Column(9);
                ExcelColumn column10 = ws.Column(10);
                ExcelColumn column11 = ws.Column(11);
                ExcelColumn column12 = ws.Column(12);
                ExcelColumn column13 = ws.Column(13);

                column1.Width = 8.43D;
                column2.Width = 26.71D;
                column3.Width = 26D;
                column4.Width = 12.71D;
                column5.Width = 11.57D;
                column6.Width = 4.43D;
                column7.Width = 4.14D;
                column8.Width = 4.43D;
                column9.Width = 73D;
                column10.Width = 5.86D;
                column11.Width = 4.86D;
                column12.Width = 4.71D;
                column13.Width = 3.71D;

                // Set Font
                //
                ws.Cells["A1:M51"].Style.Font.Size = 8;
                ws.Cells["A1:M51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
                //

                // User Header & Project Header
                //
                CreateUserHeader(isFullHeader, true, userInfo, ws);
                CreateProjectHeader(isFullHeader, true, project, projectInfo, ws);
                // 

                // Header
                //
                char endChar = (isWideHeader) ? 'M' : 'D';
                ws.Cells["A" + (firstLineNumber - 3) + ":" + endChar + (firstLineNumber - 3)].Merge = true;
                ws.Cells["A" + (firstLineNumber - 3)].Value = "Inventario de Árboles";
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Bold = true;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Size = 15;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
                ws.Cells["A" + (firstLineNumber - 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A" + (firstLineNumber - 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //

                // Table Headers
                //
                SetLine(ws, "A" + (firstLineNumber - 2), "A" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "NUM");
                SetLine(ws, "B" + (firstLineNumber - 2), "B" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Nombre Común");
                SetLine(ws, "C" + (firstLineNumber - 2), "C" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Nombre Científico");
                SetLine(ws, "D" + (firstLineNumber - 2), "D" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "DAP pulg.");
                SetLine(ws, "E" + (firstLineNumber - 2), "E" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Altura pies");
                SetLine(ws, "F" + (firstLineNumber - 2), "H" + (firstLineNumber - 2), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Condición");
                SetLine(ws, "F" + (firstLineNumber - 1), "F" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Excelente");
                SetLine(ws, "G" + (firstLineNumber - 1), "G" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Regular");
                SetLine(ws, "H" + (firstLineNumber - 1), "H" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Deficiente");
                SetLine(ws, "I" + (firstLineNumber - 2), "I" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Comentarios");
                SetLine(ws, "J" + (firstLineNumber - 2), "M" + (firstLineNumber - 2), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Acción Propuesta");
                SetLine(ws, "J" + (firstLineNumber - 1), "J" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Corte y Remoción");
                SetLine(ws, "K" + (firstLineNumber - 1), "K" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Protección");
                SetLine(ws, "L" + (firstLineNumber - 1), "L" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Transplantar");
                SetLine(ws, "M" + (firstLineNumber - 1), "M" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 90, "Poda");
                //

                // Format Columns
                //
                ws.Cells["A" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["B" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["C" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["D" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["E" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["F" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["G" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["H" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["I" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["J" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["K" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["L" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["M" + firstLineNumber + ":M" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //

                int linesInThisPage = (treeDetails.Count - (maxLines * pageNumber)) > maxLines ? maxLines : (treeDetails.Count - (maxLines * pageNumber));

                for (int i = (0 + (maxLines * pageNumber)); i < ((maxLines * pageNumber) + linesInThisPage); i++)
                {
                    lineNumber = (firstLineNumber + i - (maxLines * pageNumber));
                    firstRow = lineNumber == firstLineNumber;
                    lastRow = lineNumber == firstLineNumber + linesInThisPage - 1;
                    lastRowAll = lastRow && pageNumber == (pageCount - 1);

                    // --
                    if (firstRow && !lastRow)
                        SetLineStyle(ws, "A" + lineNumber, "M" + lineNumber, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
                    else if (lastRow)
                        SetLineStyle(ws, "A" + lineNumber, "M" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
                    else
                        SetLineStyle(ws, "A" + lineNumber, "M" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
                    // --

                    ws.Cells["A" + lineNumber].Value = i + 1 + " ";
                    ws.Cells["B" + lineNumber].Value = treeDetails[i].Project_Organisms.Organism.CommonName.CommonNameDesc;
                    ws.Cells["C" + lineNumber].Value = treeDetails[i].Project_Organisms.Organism.ScientificName.ScientificNameDesc;
                    ws.Cells["D" + lineNumber].Value = (treeDetails[i].Dap == 0) ? treeDetails[i].Varas + " varas" : String.Format("{0:0.####################################}", treeDetails[i].Dap) + "' ";
                    ws.Cells["E" + lineNumber].Value = String.Format("{0:0.####################################}", treeDetails[i].Height) + "'' ";
                    ws.Cells["F" + lineNumber].Value = (treeDetails[i].ConditionID == 1) ? "X" : "";
                    ws.Cells["G" + lineNumber].Value = (treeDetails[i].ConditionID == 2) ? "X" : "";
                    ws.Cells["H" + lineNumber].Value = (treeDetails[i].ConditionID == 3) ? "X" : "";

                    string newCommentary = (treeDetails[i].Dap_Counter > 1) ? "Ramificado a nivel de base " + treeDetails[i].Commentary.Trim() : treeDetails[i].Commentary.Trim(); // 72
                    ws.Cells["I" + lineNumber].Value = (newCommentary.Length > 100) ? "** Ver anejo de comentarios para ver el comentario completo" : newCommentary;
                    ws.Cells["I" + lineNumber].Style.Font.Italic = (newCommentary.Length > 100);

                    ws.Cells["J" + lineNumber].Value = (treeDetails[i].ActionProposedID == 1) ? "X" : "";
                    ws.Cells["K" + lineNumber].Value = (treeDetails[i].ActionProposedID == 2) ? "X" : "";
                    ws.Cells["L" + lineNumber].Value = (treeDetails[i].ActionProposedID == 3) ? "X" : "";
                    ws.Cells["M" + lineNumber].Value = (treeDetails[i].ActionProposedID == 4) ? "X" : "";

                    switch (treeDetails[i].ActionProposedID)
                    {
                        case 1:
                            crCnt++;
                            break;
                        case 2:
                            prCnt++;
                            break;
                        case 3:
                            trCnt++;
                            break;
                        case 4:
                            pdCnt++;
                            break;
                        default:
                            break;
                    }
                }

                if (lastRowAll)
                {
                    ws.Cells["I" + (firstLineNumber + linesInThisPage) + ":M" + (firstLineNumber + linesInThisPage)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    ws.Cells["I" + (firstLineNumber + linesInThisPage)].Value = "Total";
                    ws.Cells["J" + (firstLineNumber + linesInThisPage)].Value = crCnt;
                    ws.Cells["K" + (firstLineNumber + linesInThisPage)].Value = prCnt;
                    ws.Cells["L" + (firstLineNumber + linesInThisPage)].Value = trCnt;
                    ws.Cells["M" + (firstLineNumber + linesInThisPage)].Value = pdCnt;
                }

                CreateCreateTime(time, ws, firstLineNumber, linesInThisPage);
                CreatePageNumbers(isWideHeader, totalPages, pageNumber + initialPageNum, ws, firstLineNumber, linesInThisPage);
                SetPrinterSettings(ws, ws.Cells["A1:M51"], true, eOrientation.Landscape, 0.5M);
            }
        }

        public static void TreesSummary(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, Eisk.BusinessEntities.Project project, UserInfo userInfo, List<TreesSummary> treeDetails, DateTime time, ExcelPackage pck, int maxLines)
        {
            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();

            int pageCount = (int)Math.Ceiling((double)treeDetails.Count / (double)maxLines);
            int total = 0;
            int lineNumber = 0;
            bool firstRow = false;
            bool lastRow = false;
            bool lastRowAll = false;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var ws = pck.Workbook.Worksheets.Add("Resumen de Árboles" + ((pageNumber == 0) ? "" : " - #" + (pageNumber + 1).ToString()));

                isFullHeader = (isFullHeader && pageNumber == 0) || ((pageNumber + initialPageNum) == 0 && !hasIndex);
                bool isWideHeader = false;
                int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

                ExcelRow row1 = ws.Row(firstLineNumber - 2);
                ExcelRow row2 = ws.Row(firstLineNumber - 1);

                row1.Height = 27D;
                row2.Height = 12.75D;

                ExcelColumn column1 = ws.Column(1);
                ExcelColumn column2 = ws.Column(2);
                ExcelColumn column3 = ws.Column(3);
                ExcelColumn column4 = ws.Column(4);

                column1.Width = 8.43D;
                column2.Width = 55D;
                column3.Width = 55D;
                column4.Width = 13D;

                // Set Font
                //
                ws.Cells["A1:M51"].Style.Font.Size = 8;
                ws.Cells["A1:M51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
                //

                // User Header & Project Header
                //
                CreateUserHeader(isFullHeader, isWideHeader, userInfo, ws);
                CreateProjectHeader(isFullHeader, isWideHeader, project, projectInfo, ws);
                // 

                // Header
                //
                char endChar = (isWideHeader) ? 'M' : 'D';
                ws.Cells["A" + (firstLineNumber - 3) + ":" + endChar + (firstLineNumber - 3)].Merge = true;
                ws.Cells["A" + (firstLineNumber - 3)].Value = "Resumen de Árboles";
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Bold = true;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Size = 15;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
                ws.Cells["A" + (firstLineNumber - 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A" + (firstLineNumber - 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //

                // Table Headers
                //
                SetLine(ws, "A" + (firstLineNumber - 2), "A" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "NUM");
                SetLine(ws, "B" + (firstLineNumber - 2), "B" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Nombre Común");
                SetLine(ws, "C" + (firstLineNumber - 2), "C" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Nombre Científico");
                SetLine(ws, "D" + (firstLineNumber - 2), "D" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "CANTIDAD");
                //

                // Format Columns
                //
                ws.Cells["A" + firstLineNumber + ":D" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["B" + firstLineNumber + ":D" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["C" + firstLineNumber + ":D" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["D" + firstLineNumber + ":D" + (firstLineNumber + treeDetails.Count - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //

                int linesInThisPage = (treeDetails.Count - (maxLines * pageNumber)) > maxLines ? maxLines : (treeDetails.Count - (maxLines * pageNumber));

                for (int i = (0 + (maxLines * pageNumber)); i < ((maxLines * pageNumber) + linesInThisPage); i++)
                {
                    lineNumber = (firstLineNumber + i - (maxLines * pageNumber));
                    firstRow = lineNumber == firstLineNumber;
                    lastRow = lineNumber == firstLineNumber + linesInThisPage - 1;
                    lastRowAll = lastRow && pageNumber == (pageCount - 1);

                    // --
                    if (firstRow && !lastRow)
                        SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
                    else if (lastRow)
                        SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
                    else
                        SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
                    // --

                    ws.Cells["A" + lineNumber].Value = i + 1 + " ";
                    ws.Cells["B" + lineNumber].Value = treeDetails[i].CommonName;
                    ws.Cells["C" + lineNumber].Value = treeDetails[i].ScientificName;
                    ws.Cells["D" + lineNumber].Value = treeDetails[i].Count;

                    total += treeDetails[i].Count;
                }

                if (lastRowAll)
                {
                    ws.Cells["D" + (firstLineNumber + linesInThisPage)].Value = "Total  " + total + " ";
                    ws.Cells["D" + (firstLineNumber + linesInThisPage)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                CreateCreateTime(time, ws, firstLineNumber, linesInThisPage);
                CreatePageNumbers(isWideHeader, totalPages, pageNumber + initialPageNum, ws, firstLineNumber, linesInThisPage);
                SetPrinterSettings(ws, ws.Cells["A1:D51"], true, eOrientation.Portrait, 0.5M);
            }
        }

        public static void ActionProposedSummary(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, string title, Eisk.BusinessEntities.Project project, UserInfo userInfo, List<TreesSummary> treeDetails, DateTime time, ExcelPackage pck, int maxLines)
        {
            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();

            int pageCount = GetPageCountOrDefault(maxLines, treeDetails.Count); //(int)Math.Ceiling((double)treeDetails.Count / (double)maxLines);
            int total = 0;
            int lineNumber = 0;
            bool firstRow = false;
            bool lastRow = false;
            bool lastRowAll = false;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var ws = pck.Workbook.Worksheets.Add(title + ((pageNumber == 0) ? "" : " - #" + (pageNumber + 1).ToString()));

                isFullHeader = (isFullHeader && pageNumber == 0) || ((pageNumber + initialPageNum) == 0 && !hasIndex);
                bool isWideHeader = false;
                int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

                ExcelRow row1 = ws.Row(firstLineNumber - 2);
                ExcelRow row2 = ws.Row(firstLineNumber - 1);

                row1.Height = 27D;
                row2.Height = 12.75D;

                ExcelColumn column1 = ws.Column(1);
                ExcelColumn column2 = ws.Column(2);
                ExcelColumn column3 = ws.Column(3);
                ExcelColumn column4 = ws.Column(4);

                column1.Width = 12D;
                column2.Width = 40D;
                column3.Width = 10D;
                column4.Width = 50D;

                // Set Font
                //
                ws.Cells["A1:D51"].Style.Font.Size = 8;
                ws.Cells["A1:D51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
                //

                // User Header & Project Header
                //
                CreateUserHeader(isFullHeader, isWideHeader, userInfo, ws);
                CreateProjectHeader(isFullHeader, (isFullHeader ? true : false), project, projectInfo, ws);
                // 

                // Header
                //
                ws.Cells["A" + (firstLineNumber - 3) + ":D" + (firstLineNumber - 3)].Merge = true;
                ws.Cells["A" + (firstLineNumber - 3)].Value = title;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Bold = true;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Size = 15;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
                ws.Cells["A" + (firstLineNumber - 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A" + (firstLineNumber - 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //

                // Table Headers
                //
                SetLine(ws, "A" + (firstLineNumber - 2), "A" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "CANTIDAD");
                SetLine(ws, "B" + (firstLineNumber - 2), "C" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Nombre Común");
                SetLine(ws, "D" + (firstLineNumber - 2), "D" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Nombre Científico");
                //

                // Format Columns
                //
                ws.Cells["A" + firstLineNumber + ":A" + (firstLineNumber + maxLines - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["B" + firstLineNumber + ":B" + (firstLineNumber + maxLines - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["D" + firstLineNumber + ":D" + (firstLineNumber + maxLines - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //

                int linesInThisPage = (treeDetails.Count - (maxLines * pageNumber)) > maxLines ? maxLines : (treeDetails.Count - (maxLines * pageNumber));

                if (treeDetails.Count == 0)
                {
                    lineNumber = (firstLineNumber + (maxLines * pageNumber));
                    SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line

                    ws.Cells["A" + lineNumber].Value = "Ningún árbol aplicable".ToUpper();
                    ws.Cells["A" + lineNumber].Style.Font.Bold = true;
                    ws.Cells["A" + lineNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + lineNumber + ":D" + lineNumber].Merge = true;
                }
                else
                    for (int i = (0 + (maxLines * pageNumber)); i < ((maxLines * pageNumber) + linesInThisPage); i++)
                    {
                        lineNumber = (firstLineNumber + i - (maxLines * pageNumber));
                        firstRow = lineNumber == firstLineNumber;
                        lastRow = lineNumber == firstLineNumber + linesInThisPage - 1;
                        lastRowAll = lastRow && pageNumber == (pageCount - 1);

                        // --
                        if (firstRow && !lastRow)
                            SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
                        else if (lastRow)
                            SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
                        else
                            SetLineStyle(ws, "A" + lineNumber, "D" + lineNumber, ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
                        // --

                        ws.Cells["A" + lineNumber].Value = treeDetails[i].Count;
                        ws.Cells["B" + lineNumber + ":C" + lineNumber].Merge = true;
                        ws.Cells["B" + lineNumber].Value = treeDetails[i].CommonName;
                        ws.Cells["D" + lineNumber].Value = treeDetails[i].ScientificName;

                        total += treeDetails[i].Count;
                    }

                if (lastRowAll)
                {
                    ws.Cells["D" + (firstLineNumber + linesInThisPage)].Value = "Total  " + total + " ";
                    ws.Cells["D" + (firstLineNumber + linesInThisPage)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                CreateCreateTime(time, ws, firstLineNumber, linesInThisPage);
                CreatePageNumbers(isWideHeader, totalPages, pageNumber + initialPageNum, ws, firstLineNumber, linesInThisPage);
                SetPrinterSettings(ws, ws.Cells["A1:D51"], true, eOrientation.Portrait, 0.5M);
            }
        }

        public static void Comentaries(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, string title, Eisk.BusinessEntities.Project project, UserInfo userInfo, List<TreeDetail> treeDetails, DateTime time, ExcelPackage pck, int maxLines)
        {
            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();

            int totalTreeDetailsLines = 0;
            int pageCount = 0;
            foreach (var treeDetail in treeDetails)
            {
                int lines = (int)Math.Ceiling((double)treeDetail.Commentary.Length / 200D);
                if (totalTreeDetailsLines + lines > maxLines * pageCount)
                {
                    pageCount++;
                }

                totalTreeDetailsLines += lines;
            }

            // int pageCount = (int)Math.Ceiling((double)totalTreeDetailsLines / (double)maxLines);
            int lineNumber = 0;
            bool firstRow = false;
            bool lastRow = false;

            int currentTreeDetailsLines = 0;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var ws = pck.Workbook.Worksheets.Add(title + ((pageNumber == 0) ? "" : " - #" + (pageNumber + 1).ToString()));

                isFullHeader = (isFullHeader && pageNumber == 0) || ((pageNumber + initialPageNum) == 0 && !hasIndex);
                bool isWideHeader = false;
                int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

                ExcelRow row1 = ws.Row(firstLineNumber - 2);
                ExcelRow row2 = ws.Row(firstLineNumber - 1);

                row1.Height = 27D;
                row2.Height = 12.75D;

                ExcelColumn column1 = ws.Column(1);
                ExcelColumn column2 = ws.Column(2);
                ExcelColumn column3 = ws.Column(3);
                ExcelColumn column4 = ws.Column(4);

                column1.Width = 6.5D;
                column2.Width = 70D;
                column3.Width = 70D;
                column4.Width = 20D;

                // Set Font
                //
                ws.Cells["A1:D51"].Style.Font.Size = 8;
                ws.Cells["A1:D51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
                //

                // User Header & Project Header
                //
                CreateUserHeader(isFullHeader, isWideHeader, userInfo, ws);
                CreateProjectHeader(isFullHeader, (isFullHeader ? true : false), project, projectInfo, ws);
                // 

                // Header
                //
                ws.Cells["A" + (firstLineNumber - 3) + ":D" + (firstLineNumber - 3)].Merge = true;
                ws.Cells["A" + (firstLineNumber - 3)].Value = title;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Bold = true;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Size = 15;
                ws.Cells["A" + (firstLineNumber - 3)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
                ws.Cells["A" + (firstLineNumber - 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A" + (firstLineNumber - 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //

                // Table Headers
                //
                SetLine(ws, "A" + (firstLineNumber - 2), "A" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "NUM");
                SetLine(ws, "B" + (firstLineNumber - 2), "D" + (firstLineNumber - 1), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, "Comentarios");
                //

                // Format Columns
                //
                ws.Cells["A" + firstLineNumber + ":A" + (firstLineNumber + totalTreeDetailsLines - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["B" + firstLineNumber + ":D" + (firstLineNumber + totalTreeDetailsLines - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //

                int linesInThisPage = (totalTreeDetailsLines - (maxLines * pageNumber)) > maxLines ? maxLines : (totalTreeDetailsLines - (maxLines * pageNumber));

                for (int i = (0 + (maxLines * pageNumber)); i < ((maxLines * pageNumber) + maxLines) && (currentTreeDetailsLines != treeDetails.Count); i++)
                {
                    int lines = (int)Math.Ceiling((double)treeDetails[currentTreeDetailsLines].Commentary.Length / 200D);
                    if (i + lines - 1 > ((maxLines * pageNumber) + maxLines)) // Si al añadir las lineas se pasa, haz break y empieza en la prox pagina
                        break;

                    lineNumber = (firstLineNumber + i - (maxLines * pageNumber));
                    firstRow = currentTreeDetailsLines == 0;
                    lastRow = (currentTreeDetailsLines == (treeDetails.Count - 1));

                    ws.Cells["A" + lineNumber].Value = treeDetails[currentTreeDetailsLines].Number;
                    ws.Cells["A" + lineNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + lineNumber].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    ws.Cells["B" + lineNumber].Value = treeDetails[currentTreeDetailsLines].Commentary;
                    ws.Cells["B" + lineNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells["B" + lineNumber].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B" + lineNumber].Style.WrapText = true;

                    currentTreeDetailsLines++;
                    i += lines - 1;
                    lastRow = lastRow || (i + 1) >= (((maxLines * pageNumber) + linesInThisPage));

                    ws.Cells["B" + lineNumber + ":D" + (lineNumber + lines - 1)].Merge = true;
                    ws.Cells["A" + lineNumber + ":A" + (lineNumber + lines - 1)].Merge = true;

                    // --
                    if (firstRow && !lastRow)
                        SetLineStyle(ws, "A" + lineNumber, "D" + (lineNumber + lines - 1), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
                    else if (lastRow)
                        SetLineStyle(ws, "A" + lineNumber, "D" + (lineNumber + lines - 1), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
                    else
                        SetLineStyle(ws, "A" + lineNumber, "D" + (lineNumber + lines - 1), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
                    // --
                }

                CreateCreateTime(time, ws, firstLineNumber, linesInThisPage);
                CreatePageNumbers(isWideHeader, totalPages, pageNumber + initialPageNum, ws, firstLineNumber, linesInThisPage);
                SetPrinterSettings(ws, ws.Cells["A1:D51"], true, eOrientation.Portrait, 0.5M);
            }
        }

        public static void ProjectResults(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, List<Project_Organisms> project_Organisms, Eisk.BusinessEntities.Project project, UserInfo userInfo, DateTime time, ExcelPackage pck, ExcelWorksheet wsTemplate)
        {
            isFullHeader = (isFullHeader || (!isFullHeader && !hasIndex && initialPageNum == 0));

            Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();
            Eisk.BusinessEntities.ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.First();

            var ws = pck.Workbook.Worksheets.Add("Totales del Proyecto", wsTemplate);

            // User Header & Project Header
            //
            CreateUserHeader(isFullHeader, (isFullHeader ? 'A' : 'D'), 'G', userInfo, ws);
            CreateProjectHeader(isFullHeader, (isFullHeader ? 'G' : 'C'), project, projectInfo, ws);
            // 

            int littorals = project_Organisms.Where(instance => instance.TreeDetails.First().Littoral && instance.TreeDetails.First().ActionProposedID == 1).Count();// Litoral y Corte
            int maritimeZones = project_Organisms.Where(instance => instance.TreeDetails.First().MaritimeZone && instance.TreeDetails.First().ActionProposedID == 1).Count();// MaritimeZone y Corte
            bool socialInterest = (bool)project.ProjectInfoTreeLocations.First().SocialInterest;

            int treesToPlant = 0;
            int treesToPlantMitigation = 0;
            int treesToPlantMitigation4 = 0;
            int treesToPlantMitigation24 = 0;
            int treesToPlantMitigation40 = 0;
            int treesToPlantLittoral = 0;
            int treesToPlantMaritimeZone = 0;
            int treesToPlantParking = 0;
            int treesToPlantPerimeter = 0;
            int treesToPlantLot = 0;

            for (int i = 0; i < project_Organisms.Count; i++)
            {
                TreeDetail treeDetail = project_Organisms[i].TreeDetails.First();
                if (treeDetail.ActionProposedID == 1) // Corte y Remocion
                {
                    if (treeDetail.Dap == 0) // Cepa
                    {
                        treesToPlant += 1;
                        treesToPlantMitigation += 1;
                    }
                    else // Tree
                    {
                        // Sección 47.5.6 Árbol en la Servidumbre de Vigilancia de Litoral
                        if (treeDetail.Littoral)
                        {
                            treesToPlant += 3;
                            treesToPlantLittoral += 3;
                            treesToPlantMitigation += 3;
                        }
                        // ----------------------------------------------

                        // Sección 47.5.7 Árbol en la Zona Marítimo Terrestre 
                        // ----------------------------------------------
                        else if (treeDetail.MaritimeZone)
                        {
                            treesToPlant += 4;
                            treesToPlantMaritimeZone += 4;
                            treesToPlantMitigation += 4;
                        }
                        // ----------------------------------------------

                        // Sección 47.5.2 
                        // ----------------------------------------------
                        //Igual o mayor a 4”	2
                        else if (treeDetail.Dap < 24)
                        {
                            treesToPlant += 2;
                            treesToPlantMitigation += 2;
                            treesToPlantMitigation4 += 2;
                        }
                        //Igual o mayor a 24”	3
                        else if (treeDetail.Dap < 40)
                        {
                            treesToPlant += 3;
                            treesToPlantMitigation += 3;
                            treesToPlantMitigation24 += 3;
                        }
                        //Igual o mayor a 40”	4
                        else if (treeDetail.Dap >= 40)
                        {
                            treesToPlant += 4;
                            treesToPlantMitigation += 4;
                            treesToPlantMitigation40 += 4;
                        }
                        // ----------------------------------------------
                    }
                }
            }

            treesToPlantParking = (int)Math.Ceiling((double)projectInfoTreeLocation.Parkings / 4D);
            treesToPlant += treesToPlantParking;

            bool isLots = projectInfoTreeLocation.Mitigation <= 1;
            bool isSocialInterest = (project.ProjectInfoTreeLocations.First().SocialInterest.HasValue && project.ProjectInfoTreeLocations.First().SocialInterest.Value);
            bool isPreviouslyImpacted = (project.ProjectInfoTreeLocations.First().PreviouslyImpacted.HasValue && project.ProjectInfoTreeLocations.First().PreviouslyImpacted.Value);

            if (!isLots && !isPreviouslyImpacted)
            {
                if (projectInfoTreeLocation.Acres >= 1)
                    treesToPlantPerimeter = (int)Math.Ceiling((Math.Sqrt((double)projectInfoTreeLocation.Acres * 42306D) * 4D) / (double)projectInfoTreeLocation.DistanceBetweenTrees);
                treesToPlant += treesToPlantPerimeter;
            }
            else
            {
                treesToPlantLot = (int)projectInfoTreeLocation.Lots1 + ((int)projectInfoTreeLocation.Lots2 * 2) + ((int)projectInfoTreeLocation.Lots3 * 3);
                treesToPlant += treesToPlantLot;
            }

            ws.Cells["G" + 8].Value = (socialInterest ? "*" : "") + project_Organisms.Count.ToString();

            ws.Cells["G" + 10].Value = project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 1).Count();
            ws.Cells["G" + 11].Value = project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 2).Count();
            ws.Cells["G" + 12].Value = project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 3).Count();
            ws.Cells["G" + 13].Value = project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 4).Count();

            ws.Cells["G" + 16].Value = treesToPlant;
            ws.Cells["G" + 18].Value = treesToPlantMitigation;

            ws.Cells["F" + 19].Value = treesToPlantLittoral;
            ws.Cells["F" + 20].Value = treesToPlantMaritimeZone;
            ws.Cells["F" + 21].Value = treesToPlantMitigation4;
            ws.Cells["F" + 22].Value = treesToPlantMitigation24;
            ws.Cells["F" + 23].Value = treesToPlantMitigation40;

            ws.Cells["G" + 25].Value = treesToPlantParking;
            ws.Cells["F" + 26].Value = projectInfoTreeLocation.Parkings.ToString();

            ws.Cells["G" + 28].Value = treesToPlantPerimeter;
            ws.Cells["F" + 31].Value = projectInfoTreeLocation.Acres.ToString();
            ws.Cells["F" + 32].Value = projectInfoTreeLocation.DistanceBetweenTrees.ToString() + "'";
            ws.Cells["G" + 34].Value = treesToPlantLot;
            ws.Cells["F" + 35].Value = projectInfoTreeLocation.Lots1.ToString();
            ws.Cells["F" + 36].Value = projectInfoTreeLocation.Lots2.ToString();
            ws.Cells["F" + 37].Value = projectInfoTreeLocation.Lots3.ToString();

            //
            CreateCreateTime(time, ws, shortHeaderFirstLineNumber, 33);
            CreatePageNumbers(false, totalPages, "G", initialPageNum, ws, shortHeaderFirstLineNumber, 33);
            SetPrinterSettings(ws, ws.Cells["A1:G41"], true, eOrientation.Portrait, 0.5M);
            //

            if (!socialInterest)
                ws.DeleteRow(38, 2);
            if (projectInfoTreeLocation.Lots3 == 0)
                ws.DeleteRow(37, 1);
            if (projectInfoTreeLocation.Lots2 == 0)
                ws.DeleteRow(36, 1);
            if (projectInfoTreeLocation.Lots1 == 0)
                ws.DeleteRow(35, 1);
            if (!isLots)
            {
                ws.DeleteRow(34, 1);
                ws.DeleteRow(33, 1);
                if (isPreviouslyImpacted)
                {
                    ws.DeleteRow(32, 1);
                    ws.DeleteRow(31, 1);
                    ws.DeleteRow(30, 1);
                    ws.DeleteRow(28, 1);
                }
                else if (projectInfoTreeLocation.Acres < 1)
                {
                    ws.DeleteRow(32, 1);
                    ws.DeleteRow(31, 1);
                    ws.DeleteRow(29, 1);
                    ws.DeleteRow(28, 1);
                }
                else
                {
                    ws.DeleteRow(30, 1);
                    ws.DeleteRow(29, 1);
                }
            }
            else
            {
                ws.DeleteRow(32, 1);
                ws.DeleteRow(31, 1);
                ws.DeleteRow(30, 1);
                ws.DeleteRow(29, 1);
                ws.DeleteRow(28, 1);
                ws.DeleteRow(27, 1);
            }
            if (maritimeZones == 0)
                ws.DeleteRow(20, 1);
            if (littorals == 0)
                ws.DeleteRow(19, 1);
            if (!isFullHeader)
                ws.DeleteRow(3, 3);

        }

        //public static void ProjectResults(bool isFullHeader, bool hasIndex, int initialPageNum, int totalPages, List<Project_Organisms> project_Organisms, Eisk.BusinessEntities.Project project, UserInfo userInfo, DateTime time, ExcelPackage pck)
        //{
        //    using (ExcelPackage pck2 = new OfficeOpenXml.ExcelPackage())
        //    {
        //        isFullHeader = (isFullHeader || (!isFullHeader && !hasIndex && initialPageNum == 0));

        //        Eisk.BusinessEntities.ProjectInfo projectInfo = project.ProjectInfoes.First();
        //        Eisk.BusinessEntities.ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.First();

        //        ExcelWorksheet ws2 = null;
        //        pck2.Load(File.OpenRead(@"C:\Totales.xlsx"));
        //        ws2 = pck2.Workbook.Worksheets.First();

        //        //var ws = pck.Workbook.Worksheets.Add("Totales del Proyecto");
        //        var ws = pck.Workbook.Worksheets.Add("Totales del Proyecto", ws2);

        //        int firstLineNumber = isFullHeader ? fullHeaderFirstLineNumber : shortHeaderFirstLineNumber;

        //        ExcelRow row1 = ws.Row(firstLineNumber - 3);
        //        row1.Height = 20.25D;

        //        ExcelColumn column1 = ws.Column(1);
        //        ExcelColumn column2 = ws.Column(2);
        //        ExcelColumn column3 = ws.Column(3);
        //        ExcelColumn column4 = ws.Column(4);

        //        column1.Width = 35D;
        //        column2.Width = 35D;
        //        column3.Width = 35D;
        //        column4.Width = 35D;

        //        // Set Font
        //        //
        //        ws.Cells["A1:D51"].Style.Font.Size = 8;
        //        ws.Cells["A1:D51"].Style.Font.SetFromFont(new Font("Courier", 7.5F, FontStyle.Regular));
        //        //

        //        // User Header & Project Header
        //        //
        //        CreateUserHeader(isFullHeader, false, userInfo, ws);
        //        CreateProjectHeader(isFullHeader, false, project, projectInfo, ws);
        //        // 

        //        // Header
        //        //
        //        int start = -3;
        //        ws.Cells["A" + (firstLineNumber + start) + ":D" + (firstLineNumber - 3)].Merge = true;
        //        ws.Cells["A" + (firstLineNumber + start)].Value = "Totales del Proyecto";
        //        ws.Cells["A" + (firstLineNumber + start)].Style.Font.Bold = true;
        //        ws.Cells["A" + (firstLineNumber + start)].Style.Font.Size = 15;
        //        ws.Cells["A" + (firstLineNumber + start)].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
        //        ws.Cells["A" + (firstLineNumber + start)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ws.Cells["A" + (firstLineNumber + start)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        //

        //        // Table Headers
        //        //
        //        start++;
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.None, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Datos del Proyecto");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Cantidad de Estacionamientos");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Cantidad de Solares");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Cantidad de Cuerdas");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Distancia Entre Árboles");

        //        start++;

        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Total de Árboles en el Proyecto");

        //        start++;

        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.None, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Total de Árboles por Acción Propuesta");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Corte y Remoción");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Protección");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Poda");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Transplante");

        //        start++;

        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.None, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Totales a Plantarse");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Árboles a Plantarse por Mitigación");

        //        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        // Edgardo Ramos - 20130928
        //        int littorals = project_Organisms.Where(instance => instance.TreeDetails.First().Littoral && instance.TreeDetails.First().ActionProposedID == 1).Count();// Litoral y Corte
        //        if (littorals > 0)
        //        {
        //            SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Árboles a Plantarse por cortes en la Servidumbre de Vigilancia de Litoral ");
        //        }
        //        int maritimeZones = project_Organisms.Where(instance => instance.TreeDetails.First().MaritimeZone && instance.TreeDetails.First().ActionProposedID == 1).Count();// MaritimeZone y Corte
        //        if (maritimeZones > 0)
        //        {
        //            SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Árboles a Plantarse por cortes en la Zona Marítimo Terrestre ");
        //        }
        //        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Árboles a Plantarse por Cantidad de Estacionamientos");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Árboles a Plantarse por Perímetro");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Árboles a Plantarse por Cantidad de Solares");
        //        SetLine(ws, "A" + (firstLineNumber + ++start), "C" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, "Total de Árboles a Plantarse");
        //        //

        //        // Table Content
        //        //
        //        SetLine(ws, "D" + (firstLineNumber + 0), "D" + (firstLineNumber + 0), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.Parkings.ToString());
        //        SetLine(ws, "D" + (firstLineNumber + 1), "D" + (firstLineNumber + 1), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.Lots.ToString());
        //        SetLine(ws, "D" + (firstLineNumber + 2), "D" + (firstLineNumber + 2), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.Acres.ToString());
        //        SetLine(ws, "D" + (firstLineNumber + 3), "D" + (firstLineNumber + 3), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + projectInfoTreeLocation.DistanceBetweenTrees.ToString() + "'");

        //        SetLine(ws, "D" + (firstLineNumber + 5), "D" + (firstLineNumber + 5), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + project_Organisms.Count.ToString());

        //        SetLine(ws, "D" + (firstLineNumber + 8), "D" + (firstLineNumber + 8), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 1).Count());
        //        SetLine(ws, "D" + (firstLineNumber + 9), "D" + (firstLineNumber + 9), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 2).Count());
        //        SetLine(ws, "D" + (firstLineNumber + 10), "D" + (firstLineNumber + 10), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 3).Count());
        //        SetLine(ws, "D" + (firstLineNumber + 11), "D" + (firstLineNumber + 11), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + project_Organisms.Where(instance => instance.TreeDetails.First().ActionProposedID == 4).Count());

        //        int treesToPlant = 0;
        //        int treesToPlantMitigation = 0;
        //        int treesToPlantLittoral = 0;
        //        int treesToPlantMaritimeZone = 0;
        //        int treesToPlantParking = 0;
        //        int treesToPlantPerimeter = 0;
        //        int treesToPlantLot = 0;

        //        for (int i = 0; i < project_Organisms.Count; i++)
        //        {
        //            TreeDetail treeDetail = project_Organisms[i].TreeDetails.First();
        //            if (treeDetail.ActionProposedID == 1) // Corte y Remocion
        //            {
        //                if (treeDetail.Dap == 0) // Cepa
        //                {
        //                    treesToPlantMitigation += 1;
        //                    treesToPlant += 1;
        //                }
        //                else // Tree
        //                {
        //                    // Sección 47.5.6 Árbol en la Servidumbre de Vigilancia de Litoral
        //                    if (treeDetail.Littoral)
        //                    {
        //                        treesToPlant += 3;
        //                        treesToPlantLittoral += 3;
        //                    }
        //                    // ----------------------------------------------

        //                    // Sección 47.5.7 Árbol en la Zona Marítimo Terrestre 
        //                    // ----------------------------------------------
        //                    else if (treeDetail.MaritimeZone)
        //                    {
        //                        treesToPlant += 4;
        //                        treesToPlantMaritimeZone += 4;
        //                    }
        //                    // ----------------------------------------------

        //                    // Sección 47.5.2 
        //                    // ----------------------------------------------
        //                    //Igual o mayor a 4”	2
        //                    else if (treeDetail.Dap < 24)
        //                    {
        //                        treesToPlantMitigation += 2;
        //                        treesToPlant += 2;
        //                    }
        //                    //Igual o mayor a 24”	3
        //                    else if (treeDetail.Dap < 40)
        //                    {
        //                        treesToPlantMitigation += 3;
        //                        treesToPlant += 3;
        //                    }
        //                    //Igual o mayor a 40”	4
        //                    else if (treeDetail.Dap >= 40)
        //                    {
        //                        treesToPlantMitigation += 4;
        //                        treesToPlant += 4;
        //                    }
        //                    // ----------------------------------------------
        //                }
        //            }
        //        }

        //        treesToPlantParking = (int)Math.Ceiling((double)projectInfoTreeLocation.Parkings / 4D);
        //        treesToPlant += treesToPlantParking;

        //        treesToPlantPerimeter = (int)Math.Ceiling((Math.Sqrt((double)projectInfoTreeLocation.Acres * 42306D) * 4D) / (double)projectInfoTreeLocation.DistanceBetweenTrees);
        //        treesToPlant += treesToPlantPerimeter;

        //        treesToPlantLot = (int)projectInfoTreeLocation.Lots;
        //        treesToPlant += treesToPlantLot;

        //        start = 14;
        //        SetLine(ws, "D" + (firstLineNumber + start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlantMitigation);
        //        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        // Edgardo Ramos - 20130928
        //        if (littorals > 0)
        //        {
        //            SetLine(ws, "D" + (firstLineNumber + ++start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlantLittoral);
        //        }
        //        if (maritimeZones > 0)
        //        {
        //            SetLine(ws, "D" + (firstLineNumber + ++start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlantMaritimeZone);
        //        }
        //        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //        SetLine(ws, "D" + (firstLineNumber + ++start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlantParking);
        //        SetLine(ws, "D" + (firstLineNumber + ++start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlantPerimeter);
        //        SetLine(ws, "D" + (firstLineNumber + ++start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlantLot);
        //        SetLine(ws, "D" + (firstLineNumber + ++start), "D" + (firstLineNumber + start), true, ExcelBorderStyle.Thick, true, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, " " + treesToPlant);
        //        //

        //        // Table Borders Overight
        //        //
        //        SetLineStyle(ws, "A" + (firstLineNumber + 0), "D" + (firstLineNumber + 0), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
        //        SetLineStyle(ws, "A" + (firstLineNumber + 1), "D" + (firstLineNumber + 2), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
        //        SetLineStyle(ws, "A" + (firstLineNumber + 3), "D" + (firstLineNumber + 3), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line

        //        SetLineStyle(ws, "A" + (firstLineNumber + 5), "D" + (firstLineNumber + 5), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Top-Bottom Line

        //        SetLineStyle(ws, "A" + (firstLineNumber + 8), "D" + (firstLineNumber + 8), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
        //        SetLineStyle(ws, "A" + (firstLineNumber + 9), "D" + (firstLineNumber + 10), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
        //        SetLineStyle(ws, "A" + (firstLineNumber + 11), "D" + (firstLineNumber + 11), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line

        //        SetLineStyle(ws, "A" + (firstLineNumber + 14), "D" + (firstLineNumber + 14), ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Top Line
        //        int fix = 0;
        //        if (littorals > 0)
        //            fix++;
        //        if (maritimeZones > 0)
        //            fix++;
        //        SetLineStyle(ws, "A" + (firstLineNumber + 15), "D" + (firstLineNumber + 17 + fix), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thin); //- Middle Line
        //        SetLineStyle(ws, "A" + (firstLineNumber + 18 + fix), "D" + (firstLineNumber + 18 + fix), ExcelBorderStyle.Thin, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick, ExcelBorderStyle.Thick); //- Bottom Line
        //        //

        //        //
        //        CreateCreateTime(time, ws, firstLineNumber, 18 + fix);
        //        CreatePageNumbers(false, totalPages, initialPageNum, ws, firstLineNumber, 18 + fix);
        //        SetPrinterSettings(ws, ws.Cells["A1:D51"], true, eOrientation.Portrait, 0.5M);
        //    }
        //}


        #endregion

        private static T CastTo<T>(this Object value, T targetType)
        {
            // targetType above is just for compiler magic
            // to infer the type to cast x to
            return (T)value;
        }

        private static void SetPrinterSettings(ExcelWorksheet ws, ExcelRangeBase range, bool fitToPage, eOrientation orientation, decimal margin)
        {
            ws.PrinterSettings.PrintArea = range;
            ws.PrinterSettings.FitToPage = fitToPage;
            ws.PrinterSettings.Orientation = orientation;
            ws.PrinterSettings.BottomMargin = margin;
            ws.PrinterSettings.TopMargin = margin;
            ws.PrinterSettings.LeftMargin = margin;
            ws.PrinterSettings.RightMargin = margin;
        }

        private static void CreatePageNumbers(bool isWideHeader, int totalPages, int pageNumber, ExcelWorksheet ws, int firstLineNumber, int linesInThisPage)
        {
            CreatePageNumbers(isWideHeader, totalPages, "D", pageNumber, ws, firstLineNumber, linesInThisPage);
        }
        private static void CreatePageNumbers(bool isWideHeader, int totalPages, string firstChar, int pageNumber, ExcelWorksheet ws, int firstLineNumber, int linesInThisPage)
        {
            if (isWideHeader)
            {
                ws.Cells["J" + (firstLineNumber + linesInThisPage + 2)].Value = "Página";
                ws.Cells["J" + (firstLineNumber + linesInThisPage + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["K" + (firstLineNumber + linesInThisPage + 2)].Value = pageNumber + 1;
                //ws.Cells["K" + (firstLineNumber + linesInThisPage + 2)].Style.Font.Bold = true;
                ws.Cells["K" + (firstLineNumber + linesInThisPage + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["L" + (firstLineNumber + linesInThisPage + 2)].Value = "de";
                ws.Cells["L" + (firstLineNumber + linesInThisPage + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["M" + (firstLineNumber + linesInThisPage + 2)].Value = totalPages;
                ws.Cells["M" + (firstLineNumber + linesInThisPage + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else
            {
                if (!ws.Cells[firstChar + (firstLineNumber + linesInThisPage + 2)].Merge)
                    ws.Cells[firstChar + (firstLineNumber + linesInThisPage + 2)].Merge = true;
                ws.Cells[firstChar + (firstLineNumber + linesInThisPage + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[firstChar + (firstLineNumber + linesInThisPage + 2)].Value = "Página " + (pageNumber + 1) + "  de  " + totalPages + " ";
            }
        }

        private static void CreateCreateTime(DateTime time, ExcelWorksheet ws, int firstLineNumber, int linesInThisPage)
        {
            if (!ws.Cells["A" + (firstLineNumber + linesInThisPage + 2) + ":B" + (firstLineNumber + linesInThisPage + 2)].Merge)
                ws.Cells["A" + (firstLineNumber + linesInThisPage + 2) + ":B" + (firstLineNumber + linesInThisPage + 2)].Merge = true;
            ws.Cells["A" + (firstLineNumber + linesInThisPage + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells["A" + (firstLineNumber + linesInThisPage + 2)].Value = " Creado en: " + time.ToString("dd/MMM/yyyy hh:mm:ss tt", CultureInfo.CreateSpecificCulture("es-PR")).ToUpper();
        }

        public static void CreateUserHeader(bool isFullHeader, bool isWideHeader, Eisk.BusinessEntities.UserInfo userInfo, ExcelWorksheet ws)
        {
            char firstChar = (isFullHeader) ? 'A' : (isWideHeader ? 'I' : 'C');
            char endChar = (isWideHeader) ? 'M' : 'D';

            CreateUserHeader(isFullHeader, firstChar, endChar, userInfo, ws);
        }
        public static void CreateUserHeader(bool isFullHeader, char firstChar, char endChar, Eisk.BusinessEntities.UserInfo userInfo, ExcelWorksheet ws)
        {
            string value1 =
                (String.IsNullOrEmpty(userInfo.Title) ? String.Empty : userInfo.Title.ToUpper().Replace(".", "").Trim() + ". ") +
                (String.IsNullOrEmpty(userInfo.FirstName) ? String.Empty : userInfo.FirstName.ToUpper().Trim() + " ") +
                (String.IsNullOrEmpty(userInfo.MiddleName) ? String.Empty : userInfo.MiddleName.ToUpper().Trim() + " ") +
                (String.IsNullOrEmpty(userInfo.LastName) ? String.Empty : userInfo.LastName.ToUpper().Trim());
            SetLine(ws, firstChar + "1", endChar + "1", true, ExcelBorderStyle.None, true, isFullHeader ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, true, 12F, System.Drawing.Color.Black, value1);

            if (isFullHeader)
            {
                string value2 = (String.IsNullOrEmpty(userInfo.License) ? String.Empty : userInfo.License);
                SetLine(ws, firstChar + "2", endChar + "2", true, ExcelBorderStyle.None, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 0, false, 10F, System.Drawing.Color.Black, value2);
            }

            string value3 =

                    (String.IsNullOrEmpty(userInfo.CellPhone) ? String.Empty : (" Cel. " + String.Format("{0:(###) ###-####}", Convert.ToInt64(userInfo.CellPhone)) + " • ")) +
                    (!isFullHeader ? String.Empty :
                        (String.IsNullOrEmpty(userInfo.Phone) ? String.Empty : (" Tel. " + String.Format("{0:(###) ###-####}", Convert.ToInt64(userInfo.Phone)))) +
                        (String.IsNullOrEmpty(userInfo.PhoneExtension) ? String.Empty : (" Ext. " + userInfo.PhoneExtension + " • ")) +
                        (String.IsNullOrEmpty(userInfo.Phone) && String.IsNullOrEmpty(userInfo.PhoneExtension) ? String.Empty : " • ")) +
                    (!isFullHeader ? String.Empty : (String.IsNullOrEmpty(userInfo.Fax) ? String.Empty : (" Fax. " + String.Format("{0:(###) ###-####}", Convert.ToInt64(userInfo.Fax)) + " • "))) +
                //(String.IsNullOrEmpty(userInfo.Address1) ? String.Empty : (" " + userInfo.Address1)) +
                //(String.IsNullOrEmpty(userInfo.Address2) ? String.Empty : (" " + userInfo.Address2)) +
                //(String.IsNullOrEmpty(userInfo.City)     ? ", " : (", " + userInfo.City)) +
                //(String.IsNullOrEmpty(userInfo.State)    ? String.Empty : (" " + userInfo.State)) +
                //(String.IsNullOrEmpty(userInfo.ZipCode)  ? String.Empty : (" " + ((userInfo.ZipCode.Length <= 5) ? userInfo.ZipCode : userInfo.ZipCode.Substring(0, 5) + "-" + userInfo.ZipCode.Substring(5, userInfo.ZipCode.Length - 5)))) +
                   (userInfo.User.UserName)
                 ;
            SetLine(ws, firstChar + (isFullHeader ? "3" : "2"), endChar + (isFullHeader ? "3" : "2"), true, ExcelBorderStyle.None, true, isFullHeader ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, 0, false, 10F, System.Drawing.Color.Black, value3);
        }

        public static void CreateProjectHeader(bool isFullHeader, bool isWideHeader, Eisk.BusinessEntities.Project project, Eisk.BusinessEntities.ProjectInfo projectInfo, ExcelWorksheet ws)
        {
            char endChar = (isWideHeader) ? 'D' : 'B';
            CreateProjectHeader(isFullHeader, endChar, project, projectInfo, ws);
        }
        public static void CreateProjectHeader(bool isFullHeader, char endChar, Eisk.BusinessEntities.Project project, Eisk.BusinessEntities.ProjectInfo projectInfo, ExcelWorksheet ws)
        {

            string value4 = projectInfo.ProjectName;
            SetLine(ws, isFullHeader ? "A4" : "A1", endChar + (isFullHeader ? "4" : "1"), true, ExcelBorderStyle.None, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, true, 10, System.Drawing.Color.Black, value4);

            string value5 = projectInfo.City.CityName + ", Puerto Rico";
            SetLine(ws, isFullHeader ? "A5" : "A2", endChar + (isFullHeader ? "5" : "2"), true, ExcelBorderStyle.None, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 0, false, 10, System.Drawing.Color.Black, value5);
        }

        public static void SetLineStyle(ExcelWorksheet ws, string from, string to, ExcelBorderStyle topExcelBorderStyle, ExcelBorderStyle leftExcelBorderStyle, ExcelBorderStyle rightExcelBorderStyle, ExcelBorderStyle bottomExcelBorderStyle)
        {
            ws.Cells[from + ":" + to].Style.Border.Top.Style = topExcelBorderStyle;
            ws.Cells[from + ":" + to].Style.Border.Left.Style = leftExcelBorderStyle;
            ws.Cells[from + ":" + to].Style.Border.Right.Style = rightExcelBorderStyle;
            ws.Cells[from + ":" + to].Style.Border.Bottom.Style = bottomExcelBorderStyle;
        }

        public static void SetLine(ExcelWorksheet ws, string from, string to, bool merge, ExcelBorderStyle borderStyle, bool wrapText, bool bold, ExcelHorizontalAlignment horizontalAlignment, ExcelVerticalAlignment verticalAlignment, int textRotation, string value)
        {
            SetLine(ws, from, to, merge, borderStyle, wrapText, horizontalAlignment, verticalAlignment, textRotation, bold, 7.5F, System.Drawing.Color.Black, value);
        }

        public static void SetLine(ExcelWorksheet ws, string from, string to, bool merge, ExcelBorderStyle borderStyle, bool wrapText, ExcelHorizontalAlignment horizontalAlignment, ExcelVerticalAlignment verticalAlignment, int textRotation, bool bold, float size, System.Drawing.Color color, string value)
        {
            ws.Cells[from + ((from == to) ? "" : ":" + to)].Merge = merge;
            ws.Cells[from + ((from == to) ? "" : ":" + to)].Style.Border.BorderAround(borderStyle);
            ws.Cells[from + ((from == to) ? "" : ":" + to)].Style.WrapText = wrapText;
            ws.Cells[from].Style.HorizontalAlignment = horizontalAlignment;
            ws.Cells[from].Style.VerticalAlignment = verticalAlignment;
            ws.Cells[from].Style.TextRotation = textRotation;
            ws.Cells[from].Style.Font.Bold = bold;
            ws.Cells[from].Style.Font.Size = size;
            ws.Cells[from].Style.Font.Color.SetColor(color);
            ws.Cells[from].Value = value;
        }

        private static readonly Char[] ReplacementChars = new[] { 'á', 'é', 'í', 'ó', 'ü', 'ú', 'ñ', 'Á', 'É', 'Í', 'Ó', 'Ú', 'Ü', 'Ñ' };
        private static readonly Dictionary<Char, Char> ReplacementMappings = new Dictionary<Char, Char> 
                                                               { 
                                                                 { 'á', 'a'}, 
                                                                 { 'é', 'e'}, 
                                                                 { 'í', 'i'}, 
                                                                 { 'ó', 'o'}, 
                                                                 { 'ü', 'u'}, 
                                                                 { 'ú', 'u'}, 
                                                                 { 'ñ', 'n'}, 
                                                                 { 'Á', 'A'}, 
                                                                 { 'É', 'E'}, 
                                                                 { 'Í', 'I'}, 
                                                                 { 'Ó', 'O'}, 
                                                                 { 'Ü', 'U'}, 
                                                                 { 'Ú', 'U'}, 
                                                                 { 'Ñ', 'N'} 
                                                                };

        public static string Translate(String source)
        {
            var startIndex = 0;
            var currentIndex = 0;
            var result = new StringBuilder(source.Length);

            while ((currentIndex = source.IndexOfAny(ReplacementChars, startIndex)) != -1)
            {
                result.Append(source.Substring(startIndex, currentIndex - startIndex));
                result.Append(ReplacementMappings[source[currentIndex]]);

                startIndex = currentIndex + 1;
            }

            if (startIndex == 0)
                return source.ToString().Replace("-", "").Replace(@"\", "").Replace(@"/", "").Replace("?", "").Trim();

            result.Append(source.Substring(startIndex));

            return result.ToString().Replace("-", "").Replace(@"\", "").Replace(@"/", "").Replace("?", "").Trim();
        }

        public static int GetPageCountOrDefault(int maxLines, int listCount)
        {
            int count = (int)Math.Ceiling((double)listCount / (double)maxLines);
            int top = (
                        from i in (new int[2] { count, 1 })
                        orderby i descending
                        select i
                      ).First();
            return top;
        }

    }
}
