using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteInversaoDeDependencia
{
    class Program
    {
        static void Main(string[] args)
        {
            PayServiceDecoupled mg = new PayServiceDecoupled(new MGDeductionService());
            PayServiceDecoupled sp = new PayServiceDecoupled(new SPDeductionService());
            PayServiceDecoupled rj = new PayServiceDecoupled(new RJDeductionService());

            Console.WriteLine("Aqui Veremos a Utilização da Classe 'PayServive' sltamente acoplada:");

            Console.Write("\nDigite um valor para fazer os cálculos: ");
            double value = double.Parse(Console.ReadLine());
            EnterParaContinuar();

            #region Código Acoplado
            Console.WriteLine(
                "Para cada Estado que desejaríamos calcular a Taxa, seria necessário instanciar uma nova\n" +
                "Classe do DeductionService, conforme o Estado desejado, determinando a interface 'IDeductionService' \n" +
                "a ser utilizada. No caso, como o Default do PayService está como MGDeductionService,\n" +
                "para que pudessemos usar o RJDeductionService, por exemplo, teríamos que mudar\n" +
                "diretamente na classe. Entretanto, em uma aplicação que a Classe PayService é utilizada\n" +
                "mais de uma vez, mudar diretamente quebraria o código.\n\n" +
                "Quando instancimos a classe PayPal esse é o resultado:\n\n"+
                "RESULTADO: " + PayServiceCoupled.tax(value) + "\n\n" +
                "Mas, e se quiséssimos a taxa de Minas Gerais, São Paulo e Rio de Janeiro? Para isso, \n" +
                "desacoplamos a classe PayService, criando um construtor personalizado que passa uma \n" +
                "nova instância da classe DeductionService desejada, sem precisar modificar diretamente\n" +
                "na classe.\n\n" +
                "Quando instancimos a classe PayPal e passamos os Estados desejados como parâmetro, \n " +
                "esse é o resultado:\n\n" +
                "Minas Gerais: " + mg.tax(value) + "\n" +
                "São Paulo: " + sp.tax(value) + "\n" +
                "Rio de Janeiro: " + rj.tax(value)
                );
            #endregion

            Console.ReadLine();
        }


        public static void EnterParaContinuar()
        {
            Console.WriteLine("\nDigite enter para continuar!");
            Console.ReadLine();
        }

    }

    /// <summary>
    /// This interface is used to Decouple the class "PayService"  from classes of "DeductonService"
    /// </summary>
    interface IDeductionService
    {
        double Deduction(double amount);
    }

    /// <summary>
    /// This class has a high level of coupling between the DeductionService classes,
    /// as it depends directly on the instance of one of them. When it is necessary to
    /// change the DeductionService Class, it is necessary to go to the PayService
    /// class and change the instance.
    /// </summary>
    class PayServiceCoupled
    {
        //For change the DeductionService class "MGDeductionService" for 
        //"SPDeductionService" or "RJDeductionService" is necessary to chage
        //the instance, for example.
        private static IDeductionService deductionService = new MGDeductionService();

        /// <summary>
        ///  Returns a Double with the Tax
        /// </summary>
        /// <param name="amount">Specific amount of current instance</param>
        /// <returns></returns>
        public static double tax (double amount)
        {
            amount -= deductionService.Deduction(amount);
            return amount*0.2;
        }  
    }

    /// <summary>
    /// This class has a low coupling level because a constructor was created, in
    /// which the desired instance of the DeductionService class is passed, without
    /// having to change it directly here when necessary.
    /// </summary>
    class PayServiceDecoupled
    {
        //Interface variable
        private IDeductionService deductionService;

        /// <summary>
        /// Constructor that receive a new instance of  DeductionService class desired
        /// </summary>
        /// <param name="deductionService"></param>
        public PayServiceDecoupled(IDeductionService deductionService)
        {
            this.deductionService = deductionService;
        }

        /// <summary>
        ///  Returns a Double with the Tax
        /// </summary>
        /// <param name="amount">Specific amount of current instance</param>
        /// <returns></returns>
        public double tax(double amount)
        {
            amount -= deductionService.Deduction(amount);
            return amount*0.2;
        }
    }

    #region Diferent Deduction Services
    //Deduction Service from Minas Gerais
    class MGDeductionService : IDeductionService
    {
        /// <summary>
        ///  Calculates the amount according to the tax of Minas Gerais
        /// </summary>
        /// <param name="amount">Specific amount of Minas Gerais</param>
        /// <returns></returns>
        public double Deduction(double amount)
        {
           return amount*0.12;
        }
    }

    //Deduction Service from São Paulo
    class SPDeductionService : IDeductionService
    {
        /// <summary>
        ///  Calculates the amount according to the tax of São Paulo
        /// </summary>
        /// <param name="amount">Specific amount of São Paulo</param>
        /// <returns></returns>
        public double Deduction(double amount)
        {
            return amount * 0.10;
        }
    }

    //Deduction Service from Rio de Janeiro
    class RJDeductionService : IDeductionService
    {
        /// <summary>
        ///  Calculates the amount according to the tax of Rio de Janeiro
        /// </summary>
        /// <param name="amount">Specific amount of  Rio de Janeiro</param>
        /// <returns></returns>
        public double Deduction(double amount)
        {
            return amount * 0.15;
        }
    }
    #endregion
}
