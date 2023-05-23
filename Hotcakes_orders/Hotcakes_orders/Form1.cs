using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Contacts;
using Hotcakes.CommerceDTO.v1.Orders;
using Newtonsoft.Json;

namespace Hotcakes_orders
{
    public partial class Form1 : Form
    {
        public IDictionary<string, string> kategoriak = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
            GetCategory();
            GetProduct();
        }

        public void SavedItem(string bvin)
        {
            string url = "http://20.234.113.211:8091/";
            string key = "1-36f61538-6409-4681-bce7-abd31e56fe80";

            Api proxy = new Api(url, key);

            var productDTO = proxy.ProductsFind(bvin).Content;

            int i;
            int.TryParse(textBox1.Text, out i);
            productDTO.SitePrice = i;

            // call the API to create the order
            ApiResponse<ProductDTO> response = proxy.ProductsUpdate(productDTO);

            GetProduct();
        }

        private void createOrderButton_Click(object sender, EventArgs e)
        {
            int k = ordersDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow dr = ordersDataGridView.Rows[k];
            string s = dr.Cells[0].Value.ToString();
            SavedItem(s);
            GetProduct();
        }

        private void deleteOrderButton_Click(object sender, EventArgs e)
        {
            string url = "http://20.234.113.211:8091/";
            string key = "1-36f61538-6409-4681-bce7-abd31e56fe80";

            Api proxy = new Api(url, key);

            // specify the order to delete
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            int columnIndex = ordersDataGridView.CurrentCell.ColumnIndex;

            var orderId = ordersDataGridView[columnIndex, rowIndex].Value.ToString();

            // call the API to delete the order
            ApiResponse<bool> response = proxy.OrdersDelete(orderId);

            GetCategory();
        }

        public void GetCategory()
        {
            string url = "http://20.234.113.211:8091/";
            string key = "1-36f61538-6409-4681-bce7-abd31e56fe80";

            Api proxy = new Api(url, key);

            // call the API to find all orders in the store
            ApiResponse<List<CategorySnapshotDTO>> response = proxy.CategoriesFindAll();
            string json = JsonConvert.SerializeObject(response);

            ApiResponse<List<CategorySnapshotDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<CategorySnapshotDTO>>>(json);

            DataTable dt = new DataTable();
            dt.Columns.Add("Bvin", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            foreach (CategorySnapshotDTO item in deserializedResponse.Content)
            {
                kategoriak.Add(item.Name, item.Bvin);
                dt.Rows.Add(item.Bvin, item.Name);
            }

            listBox1.DisplayMember = "Name";
            listBox1.DataSource = dt;
        }

        public void GetProduct()
        {
            string url = "http://20.234.113.211:8091/";
            string key = "1-36f61538-6409-4681-bce7-abd31e56fe80";
        
            Api proxy = new Api(url, key);

            CategoryDTO ujkat = new CategoryDTO();

            //CategoryDTO kat = listBox1.SelectedItem as CategoryDTO;

            ujkat.Name = listBox1.GetItemText(listBox1.SelectedItem);

            //Console.WriteLine(ujkat.Bvin);
        
            // specify a category to look for products in
            var katID = kategoriak[ujkat.Name];
        
            // specify the page number to retrieve
            var page = 1;
        
            // specify the number of results per page
            var pageSize = int.MaxValue; // if you want all products, then use int.MaxValue here
        
            
        
            // call the API to find all products in the store matching the category & page criteria
            ApiResponse<PageOfProducts> response = proxy.ProductsFindForCategory(katID, page, pageSize);
            string json = JsonConvert.SerializeObject(response);
            
            
            ordersDataGridView.DataSource = response.Content.Products.ToList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProduct();
        }

        private void ordersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int k = ordersDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow dr = ordersDataGridView.Rows[k];
            string s = dr.Cells[5].Value.ToString();
            textBox1.Text = s;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //
        //}
    }
}
