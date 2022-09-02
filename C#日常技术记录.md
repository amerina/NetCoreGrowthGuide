## 1、RDLC开发

1. RDLC开发简介

   

2. RDLC开发流程

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

      

   5. 3434

3. 常见问题:RDLC内容如何换行?

   换行符：System.Environment.NewLine

   "Hello"+System.Environment.NewLine+"World"

   

4. 常见问题:Cannot create a data reader for dataset 'DataSet'.

   一般常见的原因是因为数据源DataTable为NULL

   

5. 常见问题:RDLC导出Excel行高不会自动增长

   Reporting Services may fail in AutoGrow property in Excel Rendering with Merged Cells

   关键点：

   1、CanGrow and CanShrink set to false

   2、合并单元格高度不会自动增长、所以移除当前行的合并单元格

   

6. 常见问题：RDLC父子报表-即报表嵌套

   

7. 分组

   

8. 合并单元格

   

9. 分页





参考：

1、[.rdlc Report - Cannot create a data reader for dataset 'DataSet1'](https://stackoverflow.com/questions/15365731/rdlc-report-cannot-create-a-data-reader-for-dataset-dataset1)

2、[Reporting Services may fail in AutoGrow property in Excel Rendering with Merged Cells (kodyaz.com)](https://www.kodyaz.com/articles/reporting-services-excel-rendering-autogrow.aspx)



## 2、Zebra开发

1. 3234
2. 23
3. 3423
4. 234



## 3、PDF拆分与合并

Split Merge Library