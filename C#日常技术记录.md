## 1、RDLC开发

1. RDLC开发流程

   1.  定义报表对象

      LocalReport rpt = new LocalReport();

   2. 填充报表数据

      rpt.ReportPath = "~/ReportFiles/HelloWorld.rdlc";
      rpt.EnableExternalImages = true;

      //报表参数

      ReportParameter[] rps = { pHello,pWorld };
      rpt.SetParameters(rps);

      //报表数据源

      ReportDataSource reportDataSource = new ReportDataSource();
      reportDataSource.Name = "DataTable";//数据源名称
      reportDataSource.Value = HelloDt;
      rpt.DataSources.Add(reportDataSource);

   3. 报表导出PDF

      ```c#
          public static string ExportReport(LocalReport rpt, string fileName)
          {
              string encoding = string.Empty;
              string mimeType = string.Empty;
              string extension = string.Empty;
              Warning[] warnings = null;
              string[] streamids = null;
      
              if (!string.IsNullOrEmpty(rpt.ReportPath))
              {
                  byte[] byteArray = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                  string FilePath = GetFolderPath() + fileName.Trim() + ".pdf";
                  if (File.Exists(FilePath))
                  {
                      FileHelper.Delete(FilePath);
                  }
                  FileHelper.WriteBytesToFile(byteArray, FilePath);
                  rpt.DataSources.Clear();
                  rpt.SetParameters(new List<ReportParameter>());
                  rpt.ReleaseSandboxAppDomain();
                  rpt.Dispose();
                  return FilePath;
              }
              else
                  return null;
          }
      ```

   4. 报表导出Excel

      

      

2. 常见问题:RDLC内容如何换行?

   换行符：System.Environment.NewLine

   "Hello"+System.Environment.NewLine+"World"

   

3. 常见问题:Cannot create a data reader for dataset 'DataSet'.

   一般常见的原因是因为数据源DataTable为NULL

   

4. 常见问题:RDLC导出Excel行高不会自动增长

   Reporting Services may fail in AutoGrow property in Excel Rendering with Merged Cells

   关键点：

   1、CanGrow and CanShrink set to false

   2、合并单元格高度不会自动增长、所以移除当前行的合并单元格

   

5. 常见问题：RDLC父子报表-即报表嵌套

   

6. 分组

   

7. 合并单元格

   

8. 分页





参考：

1、[.rdlc Report - Cannot create a data reader for dataset 'DataSet1'](https://stackoverflow.com/questions/15365731/rdlc-report-cannot-create-a-data-reader-for-dataset-dataset1)

2、[Reporting Services may fail in AutoGrow property in Excel Rendering with Merged Cells (kodyaz.com)](https://www.kodyaz.com/articles/reporting-services-excel-rendering-autogrow.aspx)

3、[Microsoft RDLC Report Designer - Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftRdlcReportDesignerforVisualStudio-18001)



## 2、Zebra开发

1. Sample Zebra

   ```ZPL II
   ^XA
   
   ^MUi
   ^COY,78
   ^CF,0,0,0^MD0^PW1200^POI^CI13^LH0,0
   
   ^FO0,1.175^GB4,.0175,.0175^FS
   ^FO0,2.25^GB4,.0175,.0175^FS
   ^FO0,3.36^GB4,.0175,.0175^FS
   ^FO0,4.5^GB4,.0175,.0175^FS
   
   ^FO1.65,.0175^GB.0175,1.175,.0175^FS
   ^FO2.4,1.175^GB.0175,1.075,.0175^FS
   ^FO2.4,3.36^GB.0175,1.15,.0175^FS
   
   
   ^FO.3,.1^A0N,.1267,.1267^FWN^FH^FDFrom^FS
   ^FO.3,.275^A0N,.1267,.1267^FWN^FH^FD[Ship From Name]^FS
   ^FO.3,.425^A0N,.1267,.1267^FWN^FH^FDdba JLAHOME^FS
   ^FO.3,.575^A0N,.1267,.1267^FB460,2,,L^FWN^FH^FD[Ship From Address1]^FS
   ^FO.3,.875^A0N,.1267,.1267^FWN^FH^FD[Ship From City]^FS
   ^FO.3,1.025^A0N,.1267,.1267^FWN^FH^FD[Ship From StateZipcode]^FS
   
   ^FO1.8,.1^A0N,.1267,.1267^FWN^FH^FDTo   [Ship To Name]^FS
   ^FO2.0,.3^A0N,.1267,.1267^FWN^FH^FDDC # [DCStoreNo]^FS
   ^FO2.0,.5^A0N,.1267,.1267^FB480,2,,L^FWN^FH^FD[Ship To Address1]^FS
   ^FO2.0,.7^A0N,.1267,.1267^FWN^FH^FD[Ship To Address2]^FS
   ^FO2.0,.925^A0N,.1267,.1267^FWN^FH^FD[Ship To CityState]^FS
   
   ^FO.3,1.225^A0N,.14,.14^FWN^FH^FDSHIP TO POST^FS
   ^FO1.225,1.25^A0N,.175,.175^FWN^FH^FD(420)  [ShipToPostalCode]^FS
   ^FO.33,1.45^BY.01,.05^BCN,.55,N,Y,Y,N^FWN^FD>:>8420[ShipToPostalCode]^FS
   ^FO2.475,1.225^A0N,.15,.15^FWN^FH^FDCARRIER:^FS
   ^FO2.475,1.65^A0N,.15,.15^FWN^FH^FDPRO:^FS
   
   ^FO.325,2.375^A0N,.2,.2^FWN^FH^FDPO: [CustomerPoNo]^FS
   ^FO.325,2.675^A0N,.2,.2^FWN^FH^FDDept #: [DeptNo]    Dept Name: [DeptName]^FS
   ^FO.3,3.225^A0N,.125,.125^FWN^FH^FD[MacolaOrderNo]^FS
   ^FO1.2,3.225^A0N,.125,.125^FWN^FH^FD[ItemNo]^FS
   ^FO2.15,3.225^A0N,.125,.125^FWN^FH^FDpack [Qty]^FS
   ^FO3.25,3.225^A0N,.125,.125^FWN^FH^FDcarton [CurrentIndex]^FS
   
   ^FO.3,3.41^A0N,.14,.14^FWN^FH^FDFOR^FS
   ^FO.875,3.435^A0N,.175,.175^FWN^FH^FD(91)  [MarkForStore-4]^FS
   ^FO.425,3.635^BY.015,.01^BCN,.825,N,Y,Y,N^FWN^FD>:>891[MarkForStore-4]^FS
   ^FO2.475,3.435^A0N,.15,.15^FWN^FH^FDLowe's^FS
   ^FO2.475,3.61^A0N,.15,.15^FWN^FH^FDStore [MarkForStore]^FS
   ^FO2.475,3.835^A0N,.15,.15^FB320,2,,L^FWN^FH^FD[MarkForAddress1]^FS
   ^FO2.475,4.01^A0N,.15,.15^FWN^FH^FD[MarkForAddress2]^FS
   ^FO2.475,4.185^A0N,.15,.15^FWN^FH^FD[MarkForCity]^FS
   ^FO2.475,4.36^A0N,.15,.15^FWN^FH^FD[MarkForStateZipcode]^FS
   
   ^FO.3,4.6^A0N,.14,.14^FWN^FH^FDSSCC-18:^FS
   ^FO1.28,4.69^A0N,.2,.2^FWN^FH^FD[UccCodeTilte]^FS
   ^FO.51,4.925^BY.02,1.3^BCN,1,N,Y,Y,N^FWN^FD>;>8[UccCode]^FS
   
   ^MUd
   
   ^XZ
   ```

   

2. Zebra开发是比较简单的主要就是编写ZPL码绘制主体

   然后代码替换变量

   通过RawPrinterHelper发送ZPL Code给斑马打印机

3. Zebra在线预览

   [Labelary Online ZPL Viewer](http://labelary.com/viewer.html)

4. 通过TCP打印

   ```c#
      public static void SendStringToPrintThroughTCP(string content, string ip, int port = 9100)
           {
               // Open connection
               var client = new System.Net.Sockets.TcpClient();
               client.Connect(ip, port);
   
               // Write content String to connection
               var writer = new System.IO.StreamWriter(client.GetStream());
               writer.Write(content);
               writer.Flush();
   
               // Close Connection
               writer.Close();
               client.Close();
           }
   ```

   

## 3、PDF拆分与合并

Split Merge Library



## 4、SharpZip



## 5、EPPLUS



## 6、EDI Reader



## 7、EDI Rules Creator Utility



## 8、BarcodeLib



## 9、Shopify开发









