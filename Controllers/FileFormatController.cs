using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Railway.Interface;
using System.Data;
using System.Text;

namespace Railway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
        public class FileFormatController : ControllerBase
        {
            private readonly ITrain _trainRep;
            private readonly ISchedule _scheduleRep;
            private readonly IStation _stationRep;

            public FileFormatController(IStation stationRep, ISchedule scheduleRep, ITrain trainRep)
            {
                _stationRep = stationRep;
                _scheduleRep = scheduleRep;
                _trainRep = trainRep;
            }

            [Authorize(Roles ="Admin,Supervisor")]
            [HttpGet("Station/Excel")]
            public IActionResult StationExcel()
            {
                // This methood exports data in Excel
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[5]
                {
                new DataColumn("Train Number"),
                new DataColumn("Station"),
                new DataColumn("Drop off"),
                new DataColumn("Source"),
                new DataColumn("Destination")
                });

                var schedules = _stationRep.GetAll();

                foreach (var model in schedules)
                {
                    dt.Rows.Add(model.Train.TrainNumber, model.Name, model.ArrivalTime, model.Train.Source, model.Train.Destination);
                }



                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.ColumnWidth = 18.71;
                    wb.Worksheets.Add(dt);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Schedule list.xlsx");
                    }
                }
            }
        [AllowAnonymous]
        [HttpGet("Trains/Pdf")]
            public IActionResult TrainsPdf()
            {
                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");
                DateTime dTime = DateTime.Now;
                string strPDFFileName = string.Format("Trains.pdf");
                Document doc = new Document();
                doc.SetMargins(0, 0, 0, 0);
                PdfPTable tableLayout = new PdfPTable(4);
                doc.SetMargins(10, 10, 10, 0);
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fontInvoice = new Font(bf, 20, Font.NORMAL);
                Paragraph paragraph = new Paragraph("Trains List", fontInvoice);
                paragraph.Alignment = Element.ALIGN_CENTER;
                doc.Add(paragraph);
                Paragraph p3 = new Paragraph();
                p3.SpacingAfter = 8;
                doc.Add(p3);
                doc.Add(Add_Content_To_PDF(tableLayout, "Trains"));
                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return File(workStream, "application/pdf", strPDFFileName);
            }
        [Authorize(Roles = "Admin,Supervisor")]
        [HttpGet("Train/Excel")]
            public IActionResult TrainExcel()
            {
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[4]
                {
                new DataColumn("Train Number"),
                new DataColumn("Name"),
                new DataColumn("Source"),
                new DataColumn("Destination"),
                });


                var trains = _trainRep.GetTrains();


                foreach (var model in trains)
                {
                    dt.Rows.Add(model.TrainNumber, model.Name, model.Source, model.Destination);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Train List.xlsx");
                    }
                }
            }

        [Authorize(Roles = "Admin,Supervisor")]
        [HttpGet("Schedules/Excel")]
            public IActionResult ScheduleExcel()
            {
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[7]
                {
                new DataColumn("Train Number"),
                new DataColumn("Train Name"),
                new DataColumn("Source"),
                new DataColumn("Destination"),
                new DataColumn("Depature"),
                new DataColumn("Arrival"),
                new DataColumn("Duration"),
                });


                var schedules = _scheduleRep.GetAll();


                foreach (var model in schedules)
                {
                    dt.Rows.Add(model.Train.TrainNumber, model.Train.Name, model.Train.Source, model.Train.Destination, model.Depature, model.Arrival, model.Duration);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Schedules.xlsx");
                    }
                }
            }
        [AllowAnonymous]
            [HttpGet("Schedules/Pdf")]
            public IActionResult SchedulesPdf()
            {
                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");
                DateTime dTime = DateTime.Now;
                string strPDFFileName = string.Format("Schedule List" + ".pdf");
                Document doc = new Document();
                doc.SetMargins(0, 0, 0, 0);
                PdfPTable tableLayout = new PdfPTable(6);
                doc.SetMargins(10, 10, 10, 0);
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fontInvoice = new Font(bf, 20, Font.NORMAL);
                Paragraph paragraph = new Paragraph("Schedules List", fontInvoice);
                paragraph.Alignment = Element.ALIGN_CENTER;
                doc.Add(paragraph);
                Paragraph p3 = new Paragraph();
                p3.SpacingAfter = 8;
                doc.Add(p3);
                doc.Add(Add_Content_To_PDF(tableLayout, "Schedules"));
                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return File(workStream, "application/pdf", strPDFFileName);
            }
        [AllowAnonymous]
        [HttpGet("Stations/Pdf")]
            public IActionResult StationsPdf()
            {
                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");
                string strPDFFileName = string.Format("Stations" + ".pdf");
                Document doc = new Document();
                doc.SetMargins(0, 0, 0, 0);
                PdfPTable tableLayout = new PdfPTable(5);
                doc.SetMargins(10, 10, 10, 0);
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fontInvoice = new Font(bf, 20, Font.NORMAL);
                Paragraph paragraph = new Paragraph("Stations", fontInvoice);
                paragraph.Alignment = Element.ALIGN_CENTER;
                doc.Add(paragraph);
                Paragraph p3 = new Paragraph();
                p3.SpacingAfter = 8;
                doc.Add(p3);
                doc.Add(Add_Content_To_PDF(tableLayout, "Stations"));
                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return File(workStream, "application/pdf", strPDFFileName);
            }
            protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, string model)
            {
                float[] headers;
                if (model == "Trains")
                {
                    float[] TrainHeaders = { 26, 27, 29, 26 };
                    headers = TrainHeaders;
                }
                else if (model == "Schedules")
                {
                    float[] ScheduleHeadrers = { 26, 27, 29, 26, 34, 34 };
                    headers = ScheduleHeadrers;
                }
                else
                {
                    float[] StationHeaders = { 26, 27, 29, 26, 34 };
                    headers = StationHeaders;
                }

                tableLayout.SetWidths(headers); //Set the pdf headers  
                tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
                tableLayout.HeaderRows = 1;
                var count = 1;

                //Add header  

                if (model == "Trains")
                {
                    AddCellToHeader(tableLayout, "Train Number");
                    AddCellToHeader(tableLayout, "Name");
                    AddCellToHeader(tableLayout, "Source");
                    AddCellToHeader(tableLayout, "Destination");


                    foreach (var item in _trainRep.GetTrains())
                    {
                        if (count >= 1)
                        {
                            //Add body  
                            //AddCellToBody(tableLayout, stud.Id.ToString(), count);
                            AddCellToBody(tableLayout, item.TrainNumber.ToString(), count);
                            AddCellToBody(tableLayout, item.Name.ToString(), count);
                            AddCellToBody(tableLayout, item.Source.ToString(), count);
                            AddCellToBody(tableLayout, item.Destination.ToString(), count);


                            count++;
                        }
                    }
                }
                else if (model == "Schedules")
                {
                    AddCellToHeader(tableLayout, "Train Number");
                    AddCellToHeader(tableLayout, "Source");
                    AddCellToHeader(tableLayout, "Destination");
                    AddCellToHeader(tableLayout, "Depature");
                    AddCellToHeader(tableLayout, "Arrival");
                    AddCellToHeader(tableLayout, "Duration");


                    foreach (var item in _scheduleRep.GetAll())
                    {
                        if (count >= 1)
                        {
                            //Add body  

                            AddCellToBody(tableLayout, item.Train.TrainNumber.ToString(), count);
                            AddCellToBody(tableLayout, item.Train.Source.ToString(), count);
                            AddCellToBody(tableLayout, item.Train.Destination.ToString(), count);
                            AddCellToBody(tableLayout, item.Depature.ToString(), count);
                            AddCellToBody(tableLayout, item.Arrival.ToString(), count);
                            AddCellToBody(tableLayout, item.Duration.ToString(), count);



                            count++;
                        }
                    }
                }
                else if (model == "Stations")
                {

                    AddCellToHeader(tableLayout, "Train Number");
                    AddCellToHeader(tableLayout, "Station");
                    AddCellToHeader(tableLayout, "Drop Off");
                    AddCellToHeader(tableLayout, "Source");
                    AddCellToHeader(tableLayout, "Destination");

                    foreach (var stud in _stationRep.GetAll())
                    {
                        if (count >= 1)
                        {
                            //Add body  

                            AddCellToBody(tableLayout, stud.Train.TrainNumber.ToString(), count);
                            AddCellToBody(tableLayout, stud.Name.ToString(), count);
                            AddCellToBody(tableLayout, stud.ArrivalTime.ToString(), count);
                            AddCellToBody(tableLayout, stud.Train.Source.ToString(), count);
                            AddCellToBody(tableLayout, stud.Train.Destination.ToString(), count);
                            count++;
                        }
                    }
                }

                return tableLayout;
            }
            private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new BaseColor(255, 255, 255)
                });
            }
            private static void AddCellToBody(PdfPTable tableLayout, string cellText, int count)
            {
                if (count % 2 == 0)
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 8,
                        BackgroundColor = new BaseColor(255, 255, 255)
                    });
                }
                else
                {
                    tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 8,
                        BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211)
                    });
                }
            }
        }
    }

