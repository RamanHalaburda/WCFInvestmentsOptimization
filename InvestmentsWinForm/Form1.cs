using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InvestmentsWinForm.OptimizationService;

namespace InvestmentsWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //OptimizationServiceClient client = new OptimizationServiceClient();

            //double[] res = client.DoSimplex(double.Parse(textBox1.Text),
            //    double.Parse(textBox4.Text),
            //    double.Parse(textBox2.Text),
            //    double.Parse(textBox3.Text),
            //    double.Parse(textBox5.Text),
            //    double.Parse(textBox6.Text));

            Simplex s = new Simplex(double.Parse(textBox1.Text),
                double.Parse(textBox4.Text),
                double.Parse(textBox2.Text),
                double.Parse(textBox3.Text),
                double.Parse(textBox5.Text),
                double.Parse(textBox6.Text));

            double[] res = s.DoSimplex();

            label7.Text = res[0].ToString();
            label8.Text = res[1].ToString();
            label9.Text = res[2].ToString();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "400000";
            textBox4.Text = "3";
            textBox2.Text = "9";
            textBox3.Text = "11";
            textBox5.Text = "305000";
            textBox6.Text = "95000";
        }
    }

    public class Simplex
    {
        public double Sum { get; set; }
        public double Ratio { get; set; }
        public double DividentsA { get; set; }
        public double DividentsB { get; set; }
        public double LimitA { get; set; }
        public double LimitB { get; set; }

        public Simplex(double _sum, double _ratio, double _divA, double _divB, double _limA, double _limB)
        {
            this.Sum = _sum;
            this.Ratio = _ratio;
            this.DividentsA = _divA * 0.01;
            this.DividentsB = _divB * 0.01;
            this.LimitA = _limA;
            this.LimitB = _limB;
        }

        private double[,] fillMatrix()
        {
            double[,] mathModel = new double[4, 3]; // Выделяем память под входную матрицу

            try
            {
                mathModel[0, 0] = 1;
                mathModel[0, 1] = 1;
                mathModel[0, 2] = this.Sum;

                mathModel[1, 0] = 1;
                mathModel[1, 1] = -this.Ratio;
                mathModel[1, 2] = 0;

                mathModel[2, 0] = 0;
                mathModel[2, 1] = 1;
                mathModel[2, 2] = LimitA;
                
                mathModel[3, 0] = 0;
                mathModel[3, 1] = 1;
                mathModel[3, 2] = LimitB;

                mathModel[4, 0] = this.DividentsA;
                mathModel[4, 1] = this.DividentsB;
                //mathModel[3, 2] = max;
            }
            catch (Exception) { return null; }

            return mathModel;
        }

        private bool getF(double[,] InA, ref double[] A)
        {
            // --- Проверяем условия ---
            for (int i = 0; i < InA.GetLength(1) - 1; i++)
            {
                if (A[i] < 0)
                {
                    return false;
                }
            }
            for (int i = 0; i < InA.GetLength(0) - 1; i++)
            {
                double s = 0;
                for (int j = 0; j < InA.GetLength(1) - 1; j++)
                {
                    s += A[j] * InA[i, j];
                }
                if (s < InA[i, InA.GetLength(1) - 1])
                {
                    return false;
                }
            }
            // Считаем значение функции
            A[A.Length - 1] = 0;
            for (int i = 0; i < InA.GetLength(1) - 1; i++)
            {
                A[A.Length - 1] += A[i] * InA[InA.GetLength(0) - 1, i];
            }
            return true;
        }

        public double[] DoSimplex()
        {
            double[,] InA = fillMatrix();
            double[,] Mat = new double[InA.GetLength(0), InA.GetLength(1) + InA.GetLength(0)]; // Выделяем память для матрицы симплекс метода

            // --- Заполняем матрицу симплекс метода ---
            for (int i = 0; i < InA.GetLength(0); i++)
            {
                Mat[i, 0] = i + InA.GetLength(1); // Базис
                Mat[i, 1] = -InA[i, InA.GetLength(1) - 1]; // B
                for (int j = 0; j < InA.GetLength(1) - 1; j++)
                {
                    Mat[i, j + 2] = -InA[i, j]; // Переменные и коэффициенты целевой функции
                }
                for (int j = 0; j < InA.GetLength(0) - 1; j++) // Базисные иксы и коэффициенты целевой функции
                {
                    if (i == j)
                    {
                        Mat[i, j + 4] = 1;
                    }
                    else
                    {
                        Mat[i, j + 4] = 0;
                    }
                }
            }

            // Основной цикл симплекс метода
            do
            {
                // Среди отрицательных значений базисных переменных выбираем наибольший по модулю
                int indI = -1;
                for (int i = 0; i < Mat.GetLength(0); i++)
                {
                    if (Mat[i, 1] < 0)
                    {
                        if (indI == -1)
                        {
                            indI = i;
                        }
                        else
                        {
                            if (Mat[i, 1] < Mat[indI, 1])
                            {
                                indI = i;
                            }
                        }
                    }
                }

                int indJ = 0;
                if (indI != -1)
                {
                    // Находим тета
                    double[] Teta = new double[InA.GetLength(1) + InA.GetLength(0) - 2];
                    for (int j = 0; j < InA.GetLength(1) + InA.GetLength(0) - 2; j++)
                    {
                        if (Mat[indI, j + 2] != 0 && Mat[InA.GetLength(0) - 1, j + 2] != 0)
                        {
                            Teta[j] = Mat[InA.GetLength(0) - 1, j + 2] / Mat[indI, j + 2];
                        }
                        else
                        {
                            Teta[j] = double.MaxValue;
                        }
                    }

                    // Находим минимум тета
                    for (int j = 1; j < Teta.Length; j++)
                    {
                        if (Teta[j] < Teta[indJ])
                        {
                            indJ = j;
                        }
                    }
                    indJ += 2; // Смещаем на 2 элемента чтобы индексы соответствовали матрице Mat
                }

                // Завершаем цикл если все базисные переменные положительны
                if (indI == -1)
                {
                    break;
                }

                // Заменяем базис
                Mat[indI, 0] = indJ - 1; // Т.к. индексация базиса от 1

                // Выполняем преобразование методом Жордано-Гаусса
                double val = Mat[indI, indJ];
                for (int j = 1; j < Mat.GetLength(1); j++)
                {
                    Mat[indI, j] /= val;
                }

                for (int i = 0; i < Mat.GetLength(0); i++)
                {
                    if (i != indI)
                    {
                        double k = Mat[i, indJ] / Mat[indI, indJ]; // Коэффициент для текущей строки
                        for (int j = 1; j < Mat.GetLength(1); j++)
                        {
                            Mat[i, j] -= k * Mat[indI, j];
                        }
                    }
                }
            } while (true);

            double[] OutA = new double[InA.GetLength(1) + 1]; // Иксы, ответ, время

            for (int x = 1; x < InA.GetLength(1); x++) // Находим иксы
            {
                for (int i = 0; i < InA.GetLength(0) - 1; i++)
                {
                    if (Mat[i, 0] == x)
                    {
                        OutA[x - 1] = Mat[i, 1];
                    }
                }
            }

            OutA[InA.GetLength(1) - 1] = Mat[InA.GetLength(0) - 1, 1];
            //stopWatch.Stop();
            //OutA[InA.GetLength(1)] = 1000000.0 * stopWatch.ElapsedTicks / Stopwatch.Frequency;
            return OutA;
        }
    }
}
