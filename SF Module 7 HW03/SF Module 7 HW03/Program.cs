using System;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        DateOnly dateOnly = new DateOnly(2022, 6, 12);
        string str = "jjj";

        ShopDelivery ShopDelivery = new ShopDelivery(dateOnly, str);

        Console.ReadKey();


    }
}

abstract class Delivery
{
    public string Address;
    public DateOnly Date;
    public Delivery(DateOnly date, string address)
    {
        Date = date;
        Address = address;
    }
}

class HomeDelivery : Delivery
{

    /// <summary>
    /// Примерное время доставки
    /// </summary>
    public TimeOnly Time { get; set; }

    public HomeDelivery(DateOnly date, TimeOnly time, string address) : base(date, address)
    {
        Time = time;
    }
}

class PickPointDelivery : Delivery
{
    /* ... */
    public PickPointDelivery(DateOnly date, string address) : base(date, address)
    { }
}

class ShopDelivery : Delivery
{
    /* ... */

    public ShopDelivery(DateOnly date, string address) : base(date, address)
    { }
}

/// <summary>
/// Абстрактный класс информации
/// </summary>
abstract class Information<T1>
{
    private T1 Name { get; }

    private string Description { get; }

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

class ProductList
{
    private Product[] Items;
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

    public ProductList(Product[] products)
    {
        if (products != null)
        {
            Items = products;
        }
    }
}

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
    private string firstName { get; }
    private string lastName { get; }
    private string phoneNumber { get; set; }
    private string email { get; set; }

    public CustomerInformation(string firstName, string lastName, string phoneNumber, string email)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.phoneNumber = phoneNumber;
        this.email = email;
    }
}
class Order<TDelivery, TStruct> where TDelivery : Delivery
{
    public TDelivery Delivery;

    //public int Number;

    //public string Description;

    public OrderInformation OrderInformation;

    public ProductList ProductList;

    public CustomerInformation Customer;


    public void DisplayAddress()
    {
        Console.WriteLine(Delivery.Address);
    }




    // ... Другие поля
}