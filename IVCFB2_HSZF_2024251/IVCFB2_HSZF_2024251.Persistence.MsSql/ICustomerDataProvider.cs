using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICustomerDataProvider
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);

        void CustomersToExcel();

        void AddCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void DeleteCustomer(Customer customer);

        void DeleteCustomers();
        void DeleteAllCustomer();
    }
}
