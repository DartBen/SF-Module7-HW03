using System;
using System.Net;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        DateOnly dateOnly = new DateOnly(2022, 6, 12);
        string str = "jjj";

        ShopDelivery shopDelivery = new ShopDelivery(str);

        Product a1 = new Product("A1", "B1");
        Product a2 = new Product("A2", "B2");

        Product[] products = { a1, a2 };

        Order<ShopDelivery> order = new Order<ShopDelivery>(shopDelivery, (555, "ЕЦ"), products, ("Pup", "pip", "999-555-888", "@mail.ru"));

        order.DisplayOrderInfo();
        order.DisplayAddress();
        order.DisplayProductList();
        order.DisplayUserInfo();

        Console.ReadKey();

    }
}

abstract class Delivery
{
    public string Address;

    public Delivery(string address)
    {
        Address = address;
    }
}

class HomeDelivery : Delivery
{

    /// <summary>
    /// Примерное время доставки
    /// </summary>
    public DateTime DeliveryDateTime { get; set; }

    public HomeDelivery(DateTime deliveryDateTime, string address) : base(address)
    {
        DeliveryDateTime = deliveryDateTime;
    }
}

class PickPointDelivery : Delivery
{
    /* ... */
    public DateOnly bookingDeadline { get; }
    public PickPointDelivery(string address) : base(address)
    {
        DateTime actualDateTime = DateTime.Now;
        DateOnly actualDate = new DateOnly(actualDateTime.Year, actualDateTime.Month, actualDateTime.Day);
        bookingDeadline = actualDate.AddDays(10);
    }
}

class ShopDelivery : Delivery
{
    /* ... */
    public DateOnly bookingDeadline { get; }
    public ShopDelivery(string address) : base(address)
    {
        DateTime actualDateTime = DateTime.Now;
        DateOnly actualDate = new DateOnly(actualDateTime.Year, actualDateTime.Month, actualDateTime.Day);
        bookingDeadline = actualDate.AddDays(10);
    }
}


static class DeliveryExtensions
{
    /// <summary>
    /// Проверка срока хранения
    /// </summary>
    /// <param name="delivery"></param>
    /// <returns></returns>
    public static bool DeliveryBookingExpired(this PickPointDelivery delivery)
    {
        DateTime actualDateTime = DateTime.Now;
        DateOnly actualDate = new DateOnly(actualDateTime.Year, actualDateTime.Month, actualDateTime.Day);
        bool result = actualDate > delivery.bookingDeadline;
        return result;
    }
    /// <summary>
    /// Перегрузка метода Проверки срока хранения
    /// </summary>
    /// <param name="delivery"></param>
    /// <returns></returns>
    public static bool DeliveryBookingExpired(this ShopDelivery delivery)
    {
        DateTime actualDateTime = DateTime.Now;
        DateOnly actualDate = new DateOnly(actualDateTime.Year, actualDateTime.Month, actualDateTime.Day);
        bool result = actualDate > delivery.bookingDeadline;
        return result;
    }
}


/// <summary>
/// Абстрактный класс информации
/// </summary>
abstract class Information<T1>
{
    public T1 Name { get; }

    public string Description { get; }

    public Information(T1 name, string description)
    {
        Name = name;
        Description = description;
    }
}

/// <summary>
/// Информация о предмете
/// </summary>
class Product : Information<string>
{
    public Product(string name, string description) : base(name, description)
    {
    }
}


/// <summary>
/// Список заказа
/// </summary>
class ProductList
{
    private protected Product[] Items;
    public Product this[int index]
    {
        get
        {
            if (index >= 0 && index < Items.Length)
            {
                return Items[index];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (index >= 0 && index < Items.Length)
            {
                Items[index] = value;
            }
        }
    }

    public int getCount()
    {
        return Items.Length;
    }

    public ProductList(Product[] products)
    {
        Items = products;
    }
}

/// <summary>
/// Информация о заказе-Идентификатор
/// </summary>
class OrderInformation : Information<int>
{
    public OrderInformation(int number, string description) : base(number, description)
    {
    }
}

/// <summary>
/// Информация о заказчике: Имя, Фамилия, телефон, электронная почта
/// </summary>
class CustomerInformation
{
    protected string firstName { get; }
    protected string lastName { get; }
    protected string phoneNumber { get; set; }
    protected string email { get; set; }

    public string GetCustomerFirstName()
    {
        return firstName;
    }

    public string GetCustomerLastName()
    {
        return lastName;
    }

    public string GetCustomerPhoneNumber()
    {
        return phoneNumber;
    }

    public string GetCustomerEmail()
    {
        return email;
    }

    public CustomerInformation(string firstName, string lastName, string phoneNumber, string email)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.phoneNumber = phoneNumber;
        this.email = email;
    }
}
class Order<TDelivery> where TDelivery : Delivery
{
    public TDelivery Delivery;

    //public int Number;

    //public string Description;

    public OrderInformation OrderInformation;

    public ProductList ProductList;

    public CustomerInformation Customer;



    public void DisplayAddress()
    {
        Console.WriteLine("Адрес: {0}", Delivery.Address);
    }

    public void DisplayUserInfo()
    {
        if (Customer != null)
        {
            Console.WriteLine("Информация о заказчике: \n\t Имя:{0} \n\t Фамилия:{1} \n\t Телефон:{2} \n\tЭлектронная почта:{3}",
            Customer.GetCustomerFirstName(), Customer.GetCustomerLastName(), Customer.GetCustomerPhoneNumber(), Customer.GetCustomerEmail());
        }
    }

    public void DisplayProductList()
    {
        if (ProductList != null)
        {
            Console.WriteLine("Список заказа:");
            for (int i = 0; i < ProductList.getCount(); i++)
            {
                Product product = ProductList[i];
                Console.WriteLine("{0}.\t {1}.", i + 1, product.Name);
            }
        }
        else { Console.WriteLine("Список заказа отсутствует"); }
    }

    public void DisplayOrderInfo()
    {
        if (OrderInformation != null)
        {
            Console.WriteLine("Номер заказа:\t{0}. \nОписание заказа: \t{1}", OrderInformation.Name, OrderInformation.Description);
        }
    }

    public Order(TDelivery delivery, (int number, string description) orderInformation, Product[] products,
        (string firstName, string lastName, string phoneNumber, string email) UserInformation)
    {
        Delivery = delivery;
        OrderInformation = new OrderInformation(orderInformation.number, orderInformation.description);
        ProductList = new ProductList(products);
        Customer = new CustomerInformation(UserInformation.firstName, UserInformation.lastName, UserInformation.phoneNumber, UserInformation.email);
    }

}