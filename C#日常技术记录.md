## 1、RDLC开发 72问

1. RDLC内容如何换行?

   换行符：System.Environment.NewLine

   "Hello"+System.Environment.NewLine+"World"

   

2. Cannot create a data reader for dataset 'DataSet'.

   一般常见的原因是因为数据源DataTable为NULL

   

3. RDLC导出Excel行高不会自动增长

   Reporting Services may fail in AutoGrow property in Excel Rendering with Merged Cells

   关键点：

   1、CanGrow and CanShrink set to false

   2、合并单元格高度不会自动增长、所以移除当前行的合并单元格

   

4. 343





参考：

1、[.rdlc Report - Cannot create a data reader for dataset 'DataSet1'](https://stackoverflow.com/questions/15365731/rdlc-report-cannot-create-a-data-reader-for-dataset-dataset1)

2、[Reporting Services may fail in AutoGrow property in Excel Rendering with Merged Cells (kodyaz.com)](https://www.kodyaz.com/articles/reporting-services-excel-rendering-autogrow.aspx)



## 2、Zebra开发 72问

1. 3234
2. 23
3. 3423
4. 234