using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Contacts;
using Hotcakes.CommerceDTO.v1.Orders;
using Newtonsoft.Json;


namespace Hotcakes_orders_data_reading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetOrders();           
        }

        public void ReadData()
        {
            string url = "http://www.dnndev.me";
            string key = "1-04ef5d8f-9490-4c54-b45a-a449865431cf";

            Api proxy = new Api(url, key);

            Random rnd = new Random();
         
            using (StreamReader sr = new StreamReader("orders/orders.csv", Encoding.UTF8))
            {
                var firstLine = sr.ReadLine();

                while (!sr.EndOfStream) {
                    
                    var line = sr.ReadLine();
                    var item = line.Split(';');

                    // create a new order object
                    var order = new OrderDTO();

                    // add billing information
                    order.BillingAddress = new AddressDTO
                    {
                        AddressType = AddressTypesDTO.Billing,
                        City = item[0],
                        CountryBvin = item[1],
                        FirstName = item[2],
                        LastName = item[3],
                        Line1 = "",
                        Line2 = "",
                        Phone = item[4],
                        PostalCode = item[5],
                        RegionBvin = item[6]
                    };

                    // add at least one line item
                    order.Items = new List<LineItemDTO>();
                    order.Items.Add(new LineItemDTO
                    {
                        ProductId = item[7],
                        Quantity = int.Parse(item[8])
                    });

                    // add the shipping address
                    order.ShippingAddress = new AddressDTO();
                    order.ShippingAddress = order.BillingAddress;
                    order.ShippingAddress.AddressType = AddressTypesDTO.Shipping;

                    // specify who is creating the order
                    order.UserEmail = "info@hotcakescommerce.com";
                    order.UserID = "1";

                    // call the API to create the order
                    ApiResponse<OrderDTO> response = proxy.OrdersCreate(order);

                    GetOrders();
                    Thread.Sleep(rnd.Next(3000, 7000));
                }
                
                sr.Close();
            }       
        }

        public void GetOrders()
        {
            string url = "http://www.dnndev.me";
            string key = "1-04ef5d8f-9490-4c54-b45a-a449865431cf";

            Api proxy = new Api(url, key);

            // call the API to find all orders in the store
            ApiResponse<List<OrderSnapshotDTO>> response = proxy.OrdersFindAll();
            string json = JsonConvert.SerializeObject(response);

            ApiResponse<List<OrderSnapshotDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<OrderSnapshotDTO>>>(json);

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("bvin", typeof(string));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("StoreId", typeof(long));

            foreach (OrderSnapshotDTO item in deserializedResponse.Content)
            {
                dt.Rows.Add(item.Id, item.bvin, item.BillingAddress.FirstName, item.BillingAddress.LastName, item.StoreId);
            }

            ordersDataGridView.DataSource = dt;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            ReadData();
        }
    }
}
