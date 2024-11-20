using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICustomerDataProvider
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        void AddCustomer();
        void UpdateCustomer();
        void DeleteCustomer();

        void DeleteCustomers();
        void DeleteAllCustomer();
    }
}
