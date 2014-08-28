using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SLib.Util
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GridViewUtil
    {
        public const string GridExportTypeQueryStringKey = "gridexport";
        private GridViewUtil() { }
        #region Export to Excel
        public static bool InExportMode()
        {
            QueryStringUtil querystringUtil = new QueryStringUtil();
            if (querystringUtil[GridExportTypeQueryStringKey] != null && querystringUtil[GridExportTypeQueryStringKey].Trim() != string.Empty)
                return true;
            return false;
        }

        public static void Export(string fileName, GridView gridView)
        {
            Export(fileName, gridView, false, int.MaxValue,null);
        }
        public static void Export(string fileName, GridView gv, List<int> removeColumnIndice)
        {
            Export(fileName, gv, false, int.MaxValue, removeColumnIndice);
        }
        public static void Export(string fileName, GridView gv,bool allowPaging,int pageSize)
        {
            Export(fileName, gv, allowPaging, pageSize, null);
        }
        public static void Export(string fileName, GridView gv, bool allowPaging, int pageSize, List<int> removeColumnIndice)
        {
            if (gv == null)
                return;
            gv.AllowSorting = false;
            if (!allowPaging)
            {
                gv.AllowPaging = false;
                gv.PageIndex = 0;
                gv.PageSize = int.MaxValue;
                gv.DataBind();
            }
            else
                if (pageSize > 0)
                {
                    gv.AllowPaging = true;
                    gv.PageSize = pageSize;
                    gv.DataBind();
                }
            DoExport(fileName, gv, removeColumnIndice);
        }
        #region Stream one row per time
        //private static void DoExport(string fileName, GridView gv, List<int> removeColumnIndice)
        //{
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.AddHeader(
        //        "content-disposition", string.Format("attachment; filename={0}", fileName));
        //    HttpContext.Current.Response.ContentType = "application/ms-excel";
        //    HttpContext.Current.Server.ScriptTimeout = 28800;	//8 hours

        //    HttpContext.Current.Response.Write("<HTML>");
        //    HttpContext.Current.Response.Write("<HEAD><STYLE>.HDR { background-color:bisque;font-weight:bold }</STYLE></HEAD>");
        //    HttpContext.Current.Response.Write("<BODY><TABLE>");
        //    //  add the header row to the table
        //    if (gv.HeaderRow != null)
        //    {
        //        if (removeColumnIndice != null && removeColumnIndice.Count > 0)
        //        {
        //            removeColumnIndice.Sort();
        //            int removeCount = 0;
        //            foreach (int index in removeColumnIndice)
        //            {
        //                gv.HeaderRow.Cells.RemoveAt(index - removeCount);
        //                removeCount++;
        //            }
        //        }
        //        PrepareControlForExport(gv.HeaderRow, false);
        //        WriteRow(gv.HeaderRow);
        //    }

        //    //  add each of the data rows to the table
        //    foreach (GridViewRow row in gv.Rows)
        //    {
        //        if (removeColumnIndice != null && removeColumnIndice.Count > 0)
        //        {
        //            removeColumnIndice.Sort();
        //            int removeCount = 0;
        //            foreach (int index in removeColumnIndice)
        //            {
        //                row.Cells.RemoveAt(index - removeCount);
        //                removeCount++;
        //            }
        //        }
        //        PrepareControlForExport(row, true);
        //        WriteRow(row);
        //    }

        //    //  add the footer row to the table
        //    if (gv.FooterRow != null)
        //    {
        //        if (removeColumnIndice != null && removeColumnIndice.Count > 0)
        //        {
        //            removeColumnIndice.Sort();
        //            int removeCount = 0;
        //            foreach (int index in removeColumnIndice)
        //            {
        //                gv.FooterRow.Cells.RemoveAt(index - removeCount);
        //                removeCount++;
        //            }
        //        }
        //        PrepareControlForExport(gv.FooterRow, true);
        //        WriteRow(gv.FooterRow);
        //    }

        //    HttpContext.Current.Response.Write("</TABLE></BODY></HTML>");
        //    HttpContext.Current.Response.End(); 
        //}
        //private static void WriteRow(GridViewRow row)
        //{
        //    HttpContext.Current.Response.Write("<TR>");
        //    for (int i = 0; i < row.Cells.Count; i++)
        //    {
        //        using (StringWriter sw = new StringWriter())
        //        {
        //            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
        //            {
        //                row.Cells[i].RenderControl(htw);
        //                string result = FormatForExcel(sw.ToString());
        //                HttpContext.Current.Response.Write("<TD>");
        //                HttpContext.Current.Response.Write(result);
        //                HttpContext.Current.Response.Write("</TD>");
        //            }
        //        }
        //    }
        //    HttpContext.Current.Response.Write("</TR>");
        //    // send the data in the Response object to the client
        //    HttpContext.Current.Response.Flush();
        //}
        ///// <summary>
        ///// Formats the text so that Excel doesn't barf when rendering.
        ///// </summary>
        ///// <param name="text"></param>
        ///// <returns></returns>
        //public static string FormatForExcel(string text)
        //{
        //    string sReturn = System.Web.HttpUtility.HtmlEncode(text);
        //    while ((sReturn.Length > 0) && ((sReturn[0] == '-') || (sReturn[0] == '=')) || (sReturn[0] == '+'))
        //    {
        //        sReturn = sReturn.Remove(0, 1);
        //    }
        //    return sReturn;
        //}
        #endregion

        private static void DoExport(string fileName, GridView gv, List<int> removeColumnIndice)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Server.ScriptTimeout = 28800;	//8 hours
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a table to contain the grid
                    Table table = new Table();

                    //  include the gridline settings
                    table.GridLines = gv.GridLines;

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        if (removeColumnIndice != null && removeColumnIndice.Count > 0)
                        {
                            removeColumnIndice.Sort();
                            int removeCount = 0;
                            foreach (int index in removeColumnIndice)
                            {
                                gv.HeaderRow.Cells.RemoveAt(index - removeCount);
                                removeCount++;
                            }
                        }
                        PrepareControlForExport(gv.HeaderRow, false);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        if (removeColumnIndice != null && removeColumnIndice.Count > 0)
                        {
                            removeColumnIndice.Sort();
                            int removeCount = 0;
                            foreach (int index in removeColumnIndice)
                            {
                                row.Cells.RemoveAt(index - removeCount);
                                removeCount++;
                            }
                        }
                        PrepareControlForExport(row, true);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        if (removeColumnIndice != null && removeColumnIndice.Count > 0)
                        {
                            removeColumnIndice.Sort();
                            int removeCount = 0;
                            foreach (int index in removeColumnIndice)
                            {
                                gv.FooterRow.Cells.RemoveAt(index - removeCount);
                                removeCount++;
                            }
                        }
                        PrepareControlForExport(gv.FooterRow, true);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    //table.RenderControl(htw);
                    // Create a form to contain the grid

                    //HtmlForm frm = new HtmlForm();
                    //gv.Parent.Controls.Add(frm);
                    //frm.Attributes["runat"] = "server";
                    //frm.Controls.Add(table);
                    //frm.RenderControl(htw);
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }
        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control, bool renderImageText)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                HtmlGenericControl literalControl = null;
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    literalControl = new HtmlGenericControl();
                    literalControl.InnerText = (current as LinkButton).Text;
                    control.Controls.AddAt(i, literalControl);
                    foreach (Control children in current.Controls)
                        literalControl.Controls.Add(children);
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    //oParent.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                    literalControl = new HtmlGenericControl();
                    if (renderImageText)
                        literalControl.InnerText = (current as ImageButton).AlternateText;
                    control.Controls.AddAt(i, literalControl);
                    foreach (Control children in current.Controls)
                        literalControl.Controls.Add(children);
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    //oParent.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                    literalControl = new HtmlGenericControl();
                    literalControl.InnerText = (current as HyperLink).Text;
                    control.Controls.AddAt(i, literalControl);
                    foreach (Control children in current.Controls)
                        literalControl.Controls.Add(children);
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    //oParent.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                    literalControl = new HtmlGenericControl();
                    literalControl.InnerText = (current as DropDownList).SelectedItem.Text;
                    control.Controls.AddAt(i, literalControl);
                    foreach (Control children in current.Controls)
                        literalControl.Controls.Add(children);
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    //oParent.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                    literalControl = new HtmlGenericControl();
                    literalControl.InnerText = (current as CheckBox).Checked ? "True" : "False";
                    control.Controls.AddAt(i, literalControl);
                    foreach (Control children in current.Controls)
                        literalControl.Controls.Add(children);
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(control.Controls[i], renderImageText);
                }
            }
        }
        #endregion 

        #region Display Export Link
        private const string ExportLinkText = "Export";
        private const string ExportLinkToolTip = "Export to Excel";
        private const string DefaultExportLinkLeftPadding = "1em";

        /// <summary> 
        /// Call for each GridView_RowCreated 
        /// </summary> 
        public static void AddExportToGrid(GridView grid, GridViewRow gridRow, string cssClass, string navigateUrl,string imageFilePath)
        {
            // Ignore all GridView rows other than the pager. 
            //if (gridRow.RowType != DataControlRowType.Pager)
            //    return;
            if (navigateUrl == null || navigateUrl.Trim() == string.Empty)
                return;

            // Make sure the pager is visible even when the page size is longer than the actual number of rows. 
            //bool enoughRows = grid.Rows.Count > 0;
            //if (enoughRows)
            //{
            //    grid.PreRender += new EventHandler(MakeSurePagerIsVisible);
            //}

            TableCell newCell = CreateExportLink(navigateUrl, cssClass,imageFilePath);
            System.Web.UI.WebControls.TableRow tableRow = GetPagerTableRow(gridRow.Cells[0]);
            tableRow.Cells.AddAt(0, newCell);
        }

        private static TableCell CreateExportLink(string navigateUrl, string cssClass, string imageFilePath)
        {
            HyperLink lnkExport = new HyperLink();
            lnkExport.Text = ExportLinkText;
            lnkExport.ToolTip = ExportLinkToolTip;
            lnkExport.NavigateUrl = navigateUrl;
            lnkExport.Target = "_blank";
            lnkExport.ID = "lnkExport";

            if (string.IsNullOrEmpty(cssClass))
                lnkExport.Style.Add(HtmlTextWriterStyle.PaddingLeft, DefaultExportLinkLeftPadding);
            else
                lnkExport.CssClass = cssClass;

            System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
            image.AlternateText = ExportLinkToolTip;
            image.ImageUrl = imageFilePath;
            lnkExport.Controls.Add(image);

            Label label = new Label();
            label.Text = ExportLinkText;
            lnkExport.Controls.Add(label);

            TableCell newCell = new TableCell();
            newCell.HorizontalAlign = HorizontalAlign.Right;
            newCell.Controls.Add(lnkExport);
            return newCell;
        }
        #endregion

        #region Set Sorting Images
        public static void SetGridViewSortImages(object sender, GridViewRowEventArgs e, string sortExpression, string sortDirection, ImageClickEventHandler ascClickEventHandler, ImageClickEventHandler descClickEventHandler, string ascImageFilePath, string descImageFilePage, string headerTextCssClass)
        {
            if (e.Row != null && e.Row.RowType == DataControlRowType.Header)
            {
                int i = 0;
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls() || (tc.Text != string.Empty && tc.Text.Trim() != "&nbsp;"))
                    {
                        Table table = new Table(), imageTable = new Table();
                        table.CellPadding = 0;
                        table.CellSpacing = 0;
                        table.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                        table.Attributes.Add("summary", "Header cell structual table");
                        imageTable.CellSpacing = 0;
                        imageTable.CellPadding = 0;
                        imageTable.Attributes.Add("summary", "Sort images structual table");
                        table.BorderStyle = BorderStyle.None;
                        imageTable.BorderStyle = BorderStyle.None;
                        TableRow row = null, AscImageRow, DescImageRow;
                        TableCell AscImageCell, DescImageCell, headerTextCell, imagesCell;

                        AscImageRow = new TableRow();
                        AscImageRow.BackColor = Color.Transparent;
                        AscImageCell = new TableCell();
                        AscImageCell.BorderWidth = new Unit(0);
                        AscImageCell.Style.Add(HtmlTextWriterStyle.Padding, "0");
                        AscImageCell.Style.Add(HtmlTextWriterStyle.Margin, "0");
                        AscImageRow.Cells.Add(AscImageCell);

                        DescImageRow = new TableRow();
                        DescImageRow.BackColor = Color.Transparent;
                        DescImageCell = new TableCell();
                        DescImageCell.BorderWidth = new Unit(0);
                        DescImageCell.Style.Add(HtmlTextWriterStyle.Padding, "0");
                        DescImageCell.Style.Add(HtmlTextWriterStyle.Margin, "0");
                        DescImageRow.Cells.Add(DescImageCell);

                        imagesCell = new TableCell();
                        imagesCell.Width = new Unit(1);
                        imagesCell.BorderWidth = new Unit(0);
                        imagesCell.Style.Add(HtmlTextWriterStyle.Padding, "0");
                        imagesCell.Style.Add(HtmlTextWriterStyle.Margin, "0");
                        imagesCell.Controls.Add(imageTable);

                        row = new TableRow();
                        row.BackColor = Color.Transparent;

                        headerTextCell = new TableCell();
                        headerTextCell.BorderWidth = new Unit(0);
                        headerTextCell.Style.Add(HtmlTextWriterStyle.Padding, "0");
                        headerTextCell.Style.Add(HtmlTextWriterStyle.Margin, "0");
                        headerTextCell.HorizontalAlign = HorizontalAlign.Left;
                        Label HeaderLabel = new Label();
                        HeaderLabel.CssClass = headerTextCssClass;
                        headerTextCell.Controls.Add(HeaderLabel);

                        if (tc.HasControls())
                        {
                            // search for the header link
                            LinkButton lnk = tc.Controls[0] as LinkButton;
                            if (lnk != null)
                            {
                                ImageButton imgBtnUp = new ImageButton();
                                ImageButton imgBtnDown = new ImageButton();
                                imgBtnUp.ID = "imgAsc" + i.ToString();
                                imgBtnDown.ID = "imgDesc" + i.ToString();
                                i++;
                                imgBtnUp.Click += new ImageClickEventHandler(ascClickEventHandler);
                                imgBtnDown.Click += new ImageClickEventHandler(descClickEventHandler);
                                imgBtnUp.CommandName = lnk.CommandName;
                                imgBtnDown.CommandName = lnk.CommandName;
                                imgBtnUp.CommandArgument = lnk.CommandArgument;
                                imgBtnDown.CommandArgument = lnk.CommandArgument;

                                //--set the properties
                                HeaderLabel.Text = lnk.Text.Trim();

                                imgBtnUp.ImageUrl = ascImageFilePath;
                                imgBtnUp.AlternateText = "ascending";
                                imgBtnDown.ImageUrl = descImageFilePage;
                                imgBtnDown.AlternateText = "descending";

                                //// checking if the header link is the user's choice
                                if (sortExpression != lnk.CommandArgument || sortDirection == "DESC")
                                {
                                    AscImageCell.Controls.Add(imgBtnUp);
                                    imageTable.Rows.Add(AscImageRow);
                                }
                                if (sortExpression != lnk.CommandArgument || sortDirection == "ASC")
                                {
                                    DescImageCell.Controls.Add(imgBtnDown);
                                    imageTable.Rows.Add(DescImageRow);
                                }

                                //--this will remove the clickable column header link button that is automatically created
                                //--when adding sorting capabilities to a gridview
                                tc.Controls.RemoveAt(0);

                                if (!AscImageCell.HasControls())
                                {
                                    AscImageCell.Dispose();
                                    AscImageRow.Dispose();
                                }
                                if (!DescImageCell.HasControls())
                                {
                                    DescImageCell.Dispose();
                                    DescImageRow.Dispose();
                                }
                                row.Cells.Add(imagesCell);
                                row.Cells.Add(headerTextCell);
                                table.Rows.Add(row);
                                tc.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                                tc.Controls.Add(table);
                            }
                        }
                        else
                        {
                            imagesCell.Dispose();
                            HeaderLabel.Text = tc.Text;
                            headerTextCell.VerticalAlign = VerticalAlign.Middle;
                            row.Cells.Add(headerTextCell);
                            table.Rows.Add(row);
                            tc.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                            tc.Controls.Add(table);
                        }
                    }
                }
            }
        }

        [Obsolete]
        public static void SetGridViewSortImages(object sender, GridViewRowEventArgs e, string sortExpression, string sortDirection, ImageClickEventHandler ascClickEventHandler, ImageClickEventHandler descClickEventHandler, string ascImageFilePath, string descImageFilePage, string headerCellCssClass, string headerSortImagesLayoutCSSClass, string headerTextCSSClass)
        {
            SetGridViewSortImages(sender, e, sortExpression, sortDirection, ascClickEventHandler, descClickEventHandler, ascImageFilePath, descImageFilePage, headerTextCSSClass);
        }
        #endregion

        #region Add Page Size Selection DropDownList
        #region Fields
        private const string MinimalPageSize = "10";
        private const string MaximumPageSize = "300";//"500000";
        private const string DefaultPageSize = "25";
        private const string DisplayAllRecords="All";
        private static readonly List<string> PageSizeSelections = new List<string> { MinimalPageSize, "25", "50", "100", "200"};
        private const string PageSizeSelectorLabelText = "Page Size: ";
        private const string DefaultPageSizeSelectorLabelLeftPadding= "5em";
        /// <summary> 
        /// ID for the page length selector added to the pager 
        /// </summary> 
        private const string PageSizeSelectorId = "PageSizeSelector";
        //private const string PageSizeSelectorCookieName = PageSizeSelectorId;
        //private const string PageSizeCookieTag = "PageSize";
        //private const int PageSizeCookieLifespan = 365;
        #endregion

        #region Methods
        /// <summary> 
        /// Call from the first GridView_Load 
        /// </summary> 
        public static void SetPageSize(Page page, GridView grid)
        {

            // Get the size from the cookie or set to DEFAULT_PAGE_SIZE. 
            string selectedValue = RetrievePersistedSize(page, grid.ID);
            if (string.IsNullOrEmpty(selectedValue))
                selectedValue = grid.PageSize.ToString();
            if (selectedValue == DisplayAllRecords)
                grid.PageSize = Convert.ToInt32(MaximumPageSize);
            else
                grid.PageSize = Convert.ToInt32(selectedValue);

        }

        /// <summary> 
        /// Call for each GridView_RowCreated 
        /// </summary> 
        public static void AddSizerToGridPager(GridView grid, GridViewRow gridRow, string cssClass, EventHandler selectedIndexChangeExternal)
        {

            // Ignore all GridView rows other than the pager. 
            if (gridRow.RowType != DataControlRowType.Pager)
                return;

            // Make sure the pager is visible even when the page size is longer than the actual number of rows. 
            bool enoughRows = grid.Rows.Count >= Convert.ToInt32(MinimalPageSize);
            if (enoughRows)
            {
                grid.PreRender += new EventHandler(MakeSurePagerIsVisible);
            }

            // Insert the selector at the right-hand side of the pager table 
            string selectedValue = grid.PageSize.ToString();
            TableCell newCell = CreateSizer(selectedValue, cssClass, selectedIndexChangeExternal);
            System.Web.UI.WebControls.TableRow tableRow = GetPagerTableRow(gridRow.Cells[0]);
            tableRow.Cells.Add(newCell);

        }
        #endregion

        #region AddSizerToGridPager
        private static TableCell CreateSizer(string selectedValue,string cssClass, EventHandler selectedIndexChangeExternal)
        {

            DropDownList sel = new DropDownList();
            sel.ID = PageSizeSelectorId;
            if (selectedValue == MaximumPageSize)
                selectedValue = DisplayAllRecords;
            if (!PageSizeSelections.Contains(selectedValue))
            {
                sel.Items.Add(new ListItem(selectedValue));
            }
            foreach (string item in PageSizeSelections)
            {
                sel.Items.Add(new ListItem(item));
            }
            sel.SelectedValue = selectedValue;
            sel.AutoPostBack = true;
            sel.EnableViewState = true;
            sel.SelectedIndexChanged+= new EventHandler(PageSizeSelector_SelectedIndexChanged);
            sel.SelectedIndexChanged += selectedIndexChangeExternal;

            Label label = new Label();
            label.Text = PageSizeSelectorLabelText;

            Panel panPageSizer = new Panel();
            panPageSizer.Controls.Add(label);
            panPageSizer.Controls.Add(sel);
            if (string.IsNullOrEmpty(cssClass))
            {
                panPageSizer.Style.Add(HtmlTextWriterStyle.PaddingLeft, DefaultPageSizeSelectorLabelLeftPadding);
                panPageSizer.Style.Add(HtmlTextWriterStyle.Display, "inline");
            }
            else
                panPageSizer.CssClass = cssClass;

            TableCell newCell = new TableCell();
            newCell.HorizontalAlign = HorizontalAlign.Right;
            newCell.Controls.Add(panPageSizer);
            return newCell;

        }

        private static void MakeSurePagerIsVisible(object sender, System.EventArgs e)
        {

            GridView grid = sender as GridView;
            switch (grid.PagerSettings.Position)
            {
                case PagerPosition.Bottom:
                    grid.BottomPagerRow.Visible = true;
                    break;
                case PagerPosition.Top:
                    grid.TopPagerRow.Visible = true;
                    break;
                case PagerPosition.TopAndBottom:
                    grid.BottomPagerRow.Visible = true;
                    grid.TopPagerRow.Visible = true;
                    break;
            }

        }

        // event handler for the dropdown 
        private static void PageSizeSelector_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // set the grid PageSize and persist the selected value 
            // this would work even with multiple paged grids on the page 

            DropDownList ddl = sender as DropDownList;
            string requestedPageSize = ddl.SelectedValue;
            GridView grid = GetParentGrid(ddl);

            PersistSize(ddl.Page, grid.ID, requestedPageSize);
            if (requestedPageSize == DisplayAllRecords)
            {
                grid.PageIndex = 0;
                grid.PageSize = Convert.ToInt32(MaximumPageSize);
            }
            else
            {
                grid.PageIndex = 0;
                grid.PageSize = Convert.ToInt32(requestedPageSize);
            }

        }
        #endregion

        #region Page control hierarchy navigation
        // Drill in for the table row generated by the pager renderer. 
        private static TableRow GetPagerTableRow(Control gridRowCell)
        {
            // Warning: this will break if ms changes how the GridView pager is rendered. 
            // Another good reason to enhance this function is to handle templated pagers. 
            TableRow row = null;
            while (gridRowCell!=null)
            {
                row = gridRowCell as TableRow;
                if (row != null)
                {
                    break; // TODO: might not be correct. Was : Exit Do 
                }
                if(gridRowCell.Controls.Count > 0)
                    gridRowCell = gridRowCell.Controls[0];
            }
            return row;

        }

        // Climb up from the pagingDropDown to get the parent grid. 
        private static GridView GetParentGrid(Control pagingDropDown)
        {

            GridView grid = null;
            Control C = pagingDropDown.Parent;
            while (!((C) is Page))
            {
                grid = C as GridView;
                if (grid != null)
                {
                    break; // TODO: might not be correct. Was : Exit Do 
                }
                C = C.Parent;
            }
            return grid;

        }
        #endregion

        #region Persistance handling

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="gridId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Browser has limitation on the total number of cookies per domain and size of the cookie. 
        /// For example, IE 6 has 4k bytes size limit and 20 cookies/domain limit.
        /// </remarks>
        private static string RetrievePersistedSize(Page page, string gridId)
        {

            // Get the size from the cookie or set to the default size. 
            string selectedValue = null;// DefaultPageSize;

            //HttpCookieCollection cookies = page.Request.Cookies;
            //string cookieName = CreateCookieName(page.Request.FilePath, gridId);
            //for (int x = 0; x <= cookies.Count - 1; x++)
            //{
            //    if (cookies[x].Name == cookieName)
            //    {
            //        selectedValue = cookies[x][PageSizeCookieTag].Trim();
            //        break; // TODO: might not be correct. Was : Exit For 
            //    }
            //}

            string key = CreatePageSizeUniqueKeyName(page.Request.FilePath, gridId);
            selectedValue=PageUtil.GetFilterValue(page,CreatePageSizeUniqueKeyName(page.Request.FilePath,gridId));
            return selectedValue;

        }

        private static void PersistSize(Page page, string gridId, string gridSize)
        {

            //HttpCookie cookie = new HttpCookie(CreateCookieName(page.Request.FilePath, gridId));
            //cookie[PageSizeCookieTag] = gridSize;

            // No need to preserve the cookie with the default size. 
            //if (gridSize != DefaultPageSize)
            //{
            //    cookie.Expires = DateTime.Now.AddDays(PageSizeCookieLifespan);
            //}
            //else
            //{
            //    cookie.Expires = DateTime.Now.AddDays(-10);
            //}

            //page.Response.Cookies.Add(cookie);
            PageUtil.SaveSelectionInSession(page, CreatePageSizeUniqueKeyName(page.Request.FilePath, gridId), gridSize);

        }

        private static string CreatePageSizeUniqueKeyName(string pageName, string gridId)
        {
            //return PageSizeCookieTag + pageName.ToLower() + "_" + gridId;
            return PageSizeSelectorId + "_" + gridId;
        }
        #endregion 
        #endregion

        //#region Display Total Page Number
        ////private const string TotalPageNumberLabelText = "Pages: ";
        //private const string PageLabelText = "Page ";
        //private const string OfLabelText = " of ";
        //private const string DefaultTotalPageNumberLabelLeftPadding = "1em";
        //private const string DefaultTotalPageNumberLabelRightPadding = "5em";
        //private const string PageIndexSelectorId = "PageIndexSelector";
        ///// <summary> 
        ///// Call for each GridView_RowCreated 
        ///// </summary> 
        //public static void AddTotalPageNumberToGridPager(GridView grid, GridViewRow gridRow, string cssClass,int totalPageNumber)
        //{

        //    // Ignore all GridView rows other than the pager. 
        //    if (gridRow.RowType != DataControlRowType.Pager)
        //        return;

        //    // Make sure the pager is visible even when the page size is longer than the actual number of rows. 
        //    //bool enoughRows = grid.Rows.Count >0;
        //    //if (enoughRows)
        //    //{
        //    //    grid.PreRender += new EventHandler(MakeSurePagerIsVisible);
        //    //}

        //    // Insert the Total Page Number at the left-hand side of the pager table 
        //    TableCell newCell = CreateTotalPageNumber(totalPageNumber,grid.PageIndex,cssClass);
        //    System.Web.UI.WebControls.TableRow tableRow = GetPagerTableRow(gridRow.Cells[0]);
        //    tableRow.Cells.AddAt(0,newCell);

        //}

        //private static TableCell CreateTotalPageNumber(int totalPageNumber,int currentPageIndex,string cssClass)
        //{
        //    DropDownList ddlPages = new DropDownList();
        //    ddlPages.ID = PageIndexSelectorId;
        //    for (int i = 1; i <= totalPageNumber; i++)
        //    {

        //        ListItem lstItem = new ListItem(i.ToString());
        //        if (i == currentPageIndex + 1)
        //            lstItem.Selected = true;
        //        ddlPages.Items.Add(lstItem);
        //    }
        //    ddlPages.AutoPostBack = true;
        //    ddlPages.EnableViewState = true;
        //    ddlPages.SelectedIndexChanged += new EventHandler(PageIndexSelector_SelectedIndexChanged);

        //    Label lblTotalPageNumber = new Label(), lblPageLabel=new Label(), lblOfLabel=new Label();
        //    lblTotalPageNumber.Text = totalPageNumber.ToString();
        //    lblPageLabel.Text = PageLabelText;
        //    lblOfLabel.Text = OfLabelText;

        //    Panel panPages = new Panel();
        //    panPages.Controls.Add(lblPageLabel);
        //    panPages.Controls.Add(ddlPages);
        //    panPages.Controls.Add(lblOfLabel);
        //    panPages.Controls.Add(lblTotalPageNumber);
        //    if (string.IsNullOrEmpty(cssClass))
        //    {
        //        panPages.Style.Add(HtmlTextWriterStyle.PaddingLeft, DefaultTotalPageNumberLabelLeftPadding);
        //        panPages.Style.Add(HtmlTextWriterStyle.PaddingRight, DefaultTotalPageNumberLabelRightPadding);
        //        panPages.Style.Add(HtmlTextWriterStyle.Display, "inline");
        //    }
        //    else
        //        panPages.CssClass = cssClass;

        //    TableCell newCell = new TableCell();
        //    newCell.HorizontalAlign = HorizontalAlign.Right;
        //    newCell.Controls.Add(panPages);
        //    return newCell;

        //}

        //// event handler for the dropdown 
        //private static void PageIndexSelector_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    //Known Problem: Setting PageIndex in code doesn't trigger the PageIndexChanging/PageIndexChanged event
        //    // set the grid PageSize and persist the selected value 
        //    // this would work even with multiple paged grids on the page 

        //    DropDownList ddlPageIndex = sender as DropDownList;
        //    string requestedPageIndex = ddlPageIndex.SelectedValue;
        //    GridView grid = GetParentGrid(ddlPageIndex);
        //    grid.PageIndex = ddlPageIndex.SelectedIndex-1;
        //}
        //#endregion

        #region Display Total Row Number
        private const string TotalRowNumberLabelText = "Records: ";
        private const string DefaultTotalRowNumberLabelLeftPadding = "1em";

        /// <summary> 
        /// Call for each GridView_RowCreated 
        /// </summary> 
        public static void AddTotalRowNumberToGridPager(GridView grid, GridViewRow gridRow, string cssClass,int totalRowNumber)
        {
            // Ignore all GridView rows other than the pager. 
            //if (gridRow.RowType != DataControlRowType.Pager)
            //    return;

            // Make sure the pager is visible even when the page size is longer than the actual number of rows. 
            //bool enoughRows = grid.Rows.Count > 0;
            //if (enoughRows)
            //{
            //    grid.PreRender += new EventHandler(MakeSurePagerIsVisible);
            //}

            // Insert the Total Page Number at the left-hand side of the pager table 
            TableCell newCell = CreateTotalRowNumber(totalRowNumber,cssClass);
            System.Web.UI.WebControls.TableRow tableRow = GetPagerTableRow(gridRow.Cells[0]);
            tableRow.Cells.AddAt(0,newCell);
        }

        private static TableCell CreateTotalRowNumber(int totalRowNumber,string cssClass)
        {
            Label label = new Label();
            label.Text = TotalRowNumberLabelText+totalRowNumber;
            if (string.IsNullOrEmpty(cssClass))
                label.Style.Add(HtmlTextWriterStyle.PaddingLeft, DefaultTotalRowNumberLabelLeftPadding);
            else
                label.CssClass = cssClass;

            TableCell newCell = new TableCell();
            newCell.HorizontalAlign = HorizontalAlign.Right;
            newCell.Controls.Add(label);
            return newCell;

        }
        #endregion
    }
}


