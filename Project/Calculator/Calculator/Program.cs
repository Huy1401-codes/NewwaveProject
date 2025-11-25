using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();  
            Console.WriteLine("***** Máy Tính Mini *****");
            Console.WriteLine("Chọn phép toán:");
            Console.WriteLine("1. Cộng");
            Console.WriteLine("2. Trừ");
            Console.WriteLine("3. Nhân");
            Console.WriteLine("4. Chia");
            Console.WriteLine("5. Thoát");
            Console.Write("Lựa chọn của bạn (1/2/3/4/5): ");

            string choice = Console.ReadLine();  // Nhập từ bàn phím

            // tắt chương trình
            if (choice == "5")
            {
                Console.WriteLine("Cảm ơn đã sử dụng máy tính!");
                break;
            }

            double num1, num2, result;

            // Nhập 2 số
            Console.Write("Nhập số thứ nhất: ");
            while (!double.TryParse(Console.ReadLine(), out num1))
            {
                Console.Write("Vui lòng nhập một số hợp lệ cho số thứ nhất: ");
            }

            Console.Write("Nhập số thứ hai: ");
            while (!double.TryParse(Console.ReadLine(), out num2))
            {
                Console.Write("Vui lòng nhập một số hợp lệ cho số thứ hai: ");
            }

            
            switch (choice)
            {
                case "1":
                    result = num1 + num2;
                    Console.WriteLine($"Kết quả: {num1} + {num2} = {result}");
                    break;

                case "2":
                    result = num1 - num2;
                    Console.WriteLine($"Kết quả: {num1} - {num2} = {result}");
                    break;

                case "3":
                    result = num1 * num2;
                    Console.WriteLine($"Kết quả: {num1} * {num2} = {result}");
                    break;

                case "4":
                    // Kiểm tra chia cho 0
                    if (num2 == 0)
                    {
                        Console.WriteLine("Lỗi: Không thể chia cho 0!");
                    }
                    else
                    {
                        result = num1 / num2;
                        Console.WriteLine($"Kết quả: {num1} / {num2} = {result}");
                    }
                    break;

                default:
                    Console.WriteLine("Lựa chọn không hợp lệ! Vui lòng chọn lại.");
                    break;
            }

            
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}
