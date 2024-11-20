using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICustomerDataProvider
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);

        void CustomersToExcel();

        void AddCustomerFromConsole();
        void AddCustomer(Customer customer);

        void UpdateCustomerFromConsole();
        void UpdateCustomer(Customer customer);

        void DeleteCustomerFromConsole();
        void DeleteCustomer(Customer customer);

        void DeleteCustomers();
        void DeleteAllCustomer();
    }
}
