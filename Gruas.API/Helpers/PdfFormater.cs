using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
//string imagePath = "assets/background_invoice.png";


namespace Gruas.API.Helpers
{
    public class PdfFormater
    {
        public byte[] Formato_Invoice()
        {
            //String imageFile = "C:/logo_benavides.jpg";
            try
            {
                int fontSize = 11;
                using (MemoryStream stream = new MemoryStream())
                {


                    using (PdfWriter pdfWriter = new PdfWriter(stream))
                    using (PdfDocument pdfDocument = new PdfDocument(pdfWriter))
                    using (Document document = new Document(pdfDocument))
                    {


                        string imagePath = "assets/background_invoice.png";
                        ImageData imageData = ImageDataFactory.Create(imagePath);
                        Image image = new Image(imageData);

                        // Obtener el tamaño de la página
                        Rectangle pageSize = pdfDocument.GetDefaultPageSize();

                        // Ajustar la imagen al tamaño de la página
                        image.SetFixedPosition(0, 0);  // Establecer la posición en la página (esquina inferior izquierda)
                        image.ScaleToFit(pageSize.GetWidth(), pageSize.GetHeight()); // Escalar la imagen para ajustarse al tamaño de la página

                        // Agregar la imagen como fondo
                        document.Add(image);

                        Table tableFinal = new Table(10).UseAllAvailableWidth();
                        Table tableHeader = new Table(10).UseAllAvailableWidth();
                        Table tableFooter = new Table(10).UseAllAvailableWidth();

                        tableHeader.AddCell(new Cell(1, 8).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(fontSize)).SetHeight(100);
                        tableHeader.AddCell(new Cell(1, 2).Add(new Paragraph("#Orden")).SetTextAlignment(TextAlignment.CENTER).SetPaddingTop(43).SetBold().SetFontSize(fontSize)).SetHeight(100);

                        tableFinal.AddCell(new Cell(1, 10).Add(new Paragraph("Contenido")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(fontSize));

                        tableFooter.AddCell(new Cell(1, 8).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(fontSize));
                        tableFooter.AddCell(new Cell(1, 2).Add(new Paragraph("#Orden")).SetTextAlignment(TextAlignment.CENTER).SetPaddingTop(43).SetBold().SetFontSize(fontSize)).SetHeight(100);

                        float maxHeight = document.GetPdfDocument().GetDefaultPageSize().GetHeight() - document.GetTopMargin() - document.GetBottomMargin() - 170;
                        tableFinal.SetHeight(maxHeight);

                        // Forzar el diseño fijo
                        tableFinal.SetFixedLayout();
                        tableHeader.SetFixedLayout();
                        tableFooter.SetFixedLayout();

                        document.Add(tableHeader);
                        document.Add(tableFinal);
                        document.Add(tableFooter);

                        // Cerrar el documento
                        document.Close();

                        //Table tableFinal = new Table(10).UseAllAvailableWidth();
                        ////tableFinal.AddCell(new Cell(2, 5).Add(img).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(fuentePersonalizada).SetBorder(Border.NO_BORDER));
                        ////tableFinal.AddCell(new Cell(1, 5).Add(new Paragraph("Acuse de cita para el proveedor")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(fontSize).SetFont(fuentePersonalizada).SetFontColor(color).SetBorder(Border.NO_BORDER));

                        ////tableFinal.AddCell(new Cell(1, 5).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetFont(fuentePersonalizada));

                        //Paragraph pFechaHora = new Paragraph();
                        //pFechaHora.Add(new Text("Fecha y hora: ").SetFontSize(fontSize).SetBold());
                        //pFechaHora.Add(new Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).SetFontSize(fontSize));
                        //tableFinal.AddCell(new Cell(1, 2).Add(pFechaHora));

                        //// Agregar contenido al documento
                        //float maxHeight = document.GetPdfDocument().GetDefaultPageSize().GetHeight() - document.GetTopMargin() - document.GetBottomMargin();
                        //tableFinal.SetHeight(maxHeight);

                        //// Forzar el diseño fijo
                        //tableFinal.SetFixedLayout();

                        //document.Add(tableFinal);
                        ////document.SetFont(fuentePersonalizada);
                    }

                    // Convertir el MemoryStream a un arreglo de bytes
                    byte[] pdfBytes = stream.ToArray();
                    return pdfBytes;
                }





                //pdfDocument.SetDefaultPageSize




            }
            catch (Exception ioe)
            {
                return null;
            }

        }
    }
}
