namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {

        private Form1 form;
        [TestInitialize]
        public void TestInitialize()
        {
            form = new Form1(new TestableApi());
            form.Show();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            form.Close();
            form.Dispose();
        }


        public void ReturnProducts()
        {

            form.GetProduct();

            Assert.IsNotNull(form.ordersDataGridView);
            Assert.IsNotNull(form.ordersDataGridView.DataSource, "ordersDataGridView.DataSource is null");
        }

        [TestMethod]
        public void deleteButton_DeleteProduct()
        {
            form.GetProduct();

            var rowcount1 = form.ordersDataGridView.Rows.Count;
            form.deleteOrderButton.PerformClick();
            var rowcount2 = form.ordersDataGridView.Rows.Count;

            Assert.AreEqual(rowcount1 - 1, rowcount2);
        }

        [TestMethod]

        public void createOrderButton_testforChange()
        {
            form.getProduct();
            var price1 = form.ordersDataGridView.SelectedRows[5];
            form.createOrderButton.PerformClick();
            var price2 = form.ordersDataGridView.SelectedRows[5];

            Assert.AreNotEqual(price1, price2);
        }

    }
}